using ProfileBook.API.DTOs.Group;

namespace ProfileBook.API.Services.Interfaces;

public interface IGroupService
{
    Task<GroupResponseDto> CreateGroup(GroupCreateDto request);

    Task<List<GroupResponseDto>> GetGroups();

    Task<bool> JoinGroup(GroupJoinDto request);

    Task<List<GroupResponseDto>> GetUserGroups(int userId);
}
