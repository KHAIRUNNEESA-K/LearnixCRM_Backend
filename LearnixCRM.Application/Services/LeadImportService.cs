using AutoMapper;
using ClosedXML.Excel;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

public class LeadImportService : ILeadImportService
{
    private readonly ISalesLeadRepository _repository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public LeadImportService(
        ISalesLeadRepository repository,
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _repository = repository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<LeadImportResultDto> ImportLeadsAsync(
        Stream stream,
        int currentUserId)
    {
        using var workbook = new XLWorkbook(stream);
        Console.WriteLine($"Stream Length: {stream.Length}");

        // Always take first worksheet
        var worksheet = workbook.Worksheets.First();

        var result = new LeadImportResultDto
        {
            Total = 0,
            Imported = 0,
            Skipped = 0,
            Errors = new List<string>()
        };

        var existingEmails = await _repository.GetAllEmailsAsync();
        var emailSet = new HashSet<string>(existingEmails, StringComparer.OrdinalIgnoreCase);

        var courses = await _courseRepository.GetAllAsync();
        var courseDict = courses
            .Where(c => c.IsActive)
            .ToDictionary(c => c.Name.ToLower().Trim(), c => c.CourseId);

        int rowNumber = 2;

        while (true)
        {
            var row = worksheet.Row(rowNumber);

            var fullName = row.Cell(1).GetString().Trim();
            var email = row.Cell(2).GetString().Trim();
            var phone = row.Cell(3).GetString().Trim();
            var courseText = row.Cell(4).GetString().Trim();
            var sourceText = row.Cell(5).GetString().Trim();

            // Stop when completely empty row found
            if (string.IsNullOrWhiteSpace(fullName) &&
                string.IsNullOrWhiteSpace(email) &&
                string.IsNullOrWhiteSpace(courseText))
            {
                break;
            }

            result.Total++;

            try
            {
                if (string.IsNullOrWhiteSpace(fullName) ||
                    string.IsNullOrWhiteSpace(email))
                {
                    result.Skipped++;
                    rowNumber++;
                    continue;
                }

                if (emailSet.Contains(email))
                {
                    result.Skipped++;
                    rowNumber++;
                    continue;
                }

                if (!courseDict.TryGetValue(courseText.ToLower().Trim(), out int courseId))
                {
                    result.Skipped++;
                    result.Errors.Add($"Row {rowNumber}: Invalid Course '{courseText}'.");
                    rowNumber++;
                    continue;
                }

                if (!Enum.TryParse(sourceText.Trim(), true, out LeadSource source))
                {
                    result.Skipped++;
                    result.Errors.Add($"Row {rowNumber}: Invalid LeadSource '{sourceText}'.");
                    rowNumber++;
                    continue;
                }

                var lead = new Lead(
                    fullName,
                    email,
                    phone,
                    courseId,
                    source,
                    currentUserId,
                    currentUserId
                );

                await _repository.AddAsync(lead);

                emailSet.Add(email);
                result.Imported++;
            }
            catch (Exception ex)
            {
                result.Skipped++;
                result.Errors.Add($"Row {rowNumber}: {ex.Message}");
            }

            rowNumber++;
        }

        if (result.Imported == 0)
        {
            result.Errors.Add("All rows were skipped. No new leads imported.");
        }

        return result;
    }
}


