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
        var worksheet = workbook.Worksheet(1);

        var result = new LeadImportResultDto
        {
            Total = 0,
            Imported = 0,
            Skipped = 0,
            Errors = new List<string>()
        };

        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;

        
        if (lastRow < 1)
        {
            result.Errors.Add("Excel file contains no data rows.");
            return result;
        }

        
        var existingEmails = await _repository.GetAllEmailsAsync();
        var emailSet = new HashSet<string>(existingEmails, StringComparer.OrdinalIgnoreCase);

        for (int rowNumber = 2; rowNumber <= lastRow; rowNumber++)
        {
            var row = worksheet.Row(rowNumber);
            result.Total++;

            try
            {
                var fullName = row.Cell(1).GetString().Trim();
                var email = row.Cell(2).GetString().Trim();
                var phone = row.Cell(3).GetString().Trim();
                var courseText = row.Cell(4).GetString().Trim();
                var sourceText = row.Cell(5).GetString().Trim();

                var courses = await _courseRepository.GetAllAsync();
                var courseDict = courses
                    .Where(c => c.IsActive)
                    .ToDictionary(c => c.Name.ToLower(), c => c.CourseId);


                if (string.IsNullOrWhiteSpace(fullName) ||
                    string.IsNullOrWhiteSpace(email))
                {
                    result.Skipped++;
                    result.Errors.Add($"Row {rowNumber}: FullName or Email is empty.");
                    continue;
                }

                
                if (emailSet.Contains(email))
                {
                    result.Skipped++;
                    result.Errors.Add($"Row {rowNumber}: Email already exists.");
                    continue;
                }


                if (!courseDict.TryGetValue(courseText.ToLower(), out int courseId))
                {
                    result.Skipped++;
                    result.Errors.Add($"Row {rowNumber}: Invalid Course '{courseText}'.");
                    continue;
                }

                if (!Enum.TryParse(sourceText, true, out LeadSource source))
                {
                    result.Skipped++;
                    result.Errors.Add($"Row {rowNumber}: Invalid LeadSource '{sourceText}'.");
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
        }

        
        if (result.Imported == 0)
        {
            result.Errors.Add("All rows were skipped. No new leads imported.");
        }

        return result;
    }
}
