using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace LearnixCRM.API.Requests
{
    public class UploadExcelRequest
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
