using AutoMapper;
using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.Blacklist;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/blacklist")]
    [Authorize(Policy = "SalesOnly")]
    public class BlacklistController : ControllerBase
    {
        private readonly IBlacklistService _service;
        private readonly IMapper _mapper;

        public BlacklistController(IBlacklistService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blacklists = await _service.GetAllAsync();

            var blacklistDtos = _mapper.Map<IEnumerable<BlacklistResponseDto>>(blacklists);

            return Ok(ApiResponse<IEnumerable<BlacklistResponseDto>>.SuccessResponse(
                blacklistDtos,
                "All blacklist entries fetched successfully"
            ));
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckEmailOrPhone([FromQuery] string email, [FromQuery] string? phone)
        {
            var blacklistItem = await _service.GetByEmailOrPhoneAsync(email, phone);

            return Ok(ApiResponse<bool>.SuccessResponse(
                blacklistItem != null,
                blacklistItem != null
                    ? "The email or phone is blacklisted"
                    : "The email or phone is not blacklisted"
            ));
        }
    }
}
