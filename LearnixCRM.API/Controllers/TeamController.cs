using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.Team;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/team")]
    [Authorize(Policy = "AdminOnly")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService  _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequestDto dto)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _teamService.CreateTeamAsync(dto, adminId);

            return Ok(ApiResponse<TeamResponseDto>.SuccessResponse(
                result,
                "Team created successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeams()
        {
            var result = await _teamService.GetAllTeamsAsync();

            return Ok(ApiResponse<List<TeamResponseDto>>.SuccessResponse(
                result,
                "Teams fetched successfully"));
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetTeamById(int teamId)
        {
            var result = await _teamService.GetTeamByIdAsync(teamId);

            return Ok(ApiResponse<TeamResponseDto>.SuccessResponse(
                result,
                "Team fetched successfully"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeam([FromBody] UpdateTeamRequestDto dto)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _teamService.UpdateTeamAsync(dto, adminId);

            return Ok(ApiResponse<TeamResponseDto>.SuccessResponse(
                result,
                "Team updated successfully"));
        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> DeleteTeam(int teamId)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _teamService.DeleteTeamAsync(teamId, adminId);

            return Ok(ApiResponse<object>.SuccessResponse(
                null,
                "Team deleted successfully"));
        }
        [HttpGet("{teamId}/members")]
        public async Task<IActionResult> GetTeamMembers(int teamId)
        {
            var result = await _teamService.GetTeamWithMembersAsync(teamId);

            return Ok(ApiResponse<TeamWithMembersResponseDto>.SuccessResponse(
                result,
                "Team members fetched successfully"));
        }
    }
}