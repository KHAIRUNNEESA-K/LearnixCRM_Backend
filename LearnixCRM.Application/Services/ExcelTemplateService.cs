using ClosedXML.Excel;
using System.IO;

namespace LearnixCRM.Application.Services
{
    public class ExcelTemplateService : IExcelTemplateService
    {
        public byte[] GenerateLeadTemplate()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Lead Template");

            // Header
            worksheet.Cell("A1").Value = "FullName";
            worksheet.Cell("B1").Value = "Email";
            worksheet.Cell("C1").Value = "Phone";
            worksheet.Cell("D1").Value = "CourseId";
            worksheet.Cell("E1").Value = "Source";

            var headerRange = worksheet.Range("A1:E1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.SheetView.FreezeRows(1);

            // 🔹 Expand columns
            worksheet.Column("A").Width = 25;
            worksheet.Column("B").Width = 30;
            worksheet.Column("C").Width = 18;
            worksheet.Column("D").Width = 25;
            worksheet.Column("E").Width = 20;

            // 🔹 Expand rows
            worksheet.Rows("2:1000").Height = 22;

            // 🔹 Enable wrap text
            worksheet.Range("A2:E1000").Style.Alignment.WrapText = true;

            // Unlock data area
            worksheet.Range("A2:E1000").Style.Protection.Locked = false;

            // Lock header
            worksheet.Range("A1:E1").Style.Protection.Locked = true;

            // Protect sheet
            worksheet.Protect();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
