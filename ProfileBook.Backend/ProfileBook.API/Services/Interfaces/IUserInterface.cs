using ProfileBook.API.DTOs.User;

namespace ProfileBook.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllUsers();

        Task<UserResponseDto?> GetUserById(int id);

        Task<UserResponseDto?> UpdateUser(int id, UserUpdateDto request);
        
        Task<UserResponseDto> CreateUser(UserCreateDto request);

        Task<bool> DeleteUser(int id);
    }
}
