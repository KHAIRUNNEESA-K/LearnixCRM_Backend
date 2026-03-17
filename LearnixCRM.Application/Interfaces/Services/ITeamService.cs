using LearnixCRM.Application.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface ITeamService
    {
        Task<TeamResponseDto> CreateTeamAsync(CreateTeamRequestDto dto, int adminId);
        Task<List<TeamResponseDto>> GetAllTeamsAsync();
        Task<TeamResponseDto> GetTeamByIdAsync(int teamId);
        Task<TeamResponseDto> UpdateTeamAsync(UpdateTeamRequestDto dto, int adminId);
         Task DeleteTeamAsync(int teamId, int adminId);
        Task<TeamWithMembersResponseDto> GetTeamWithMembersAsync(int teamId);
    }
}
