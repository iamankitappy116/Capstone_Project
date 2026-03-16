using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs.Auth;
using ProfileBook.API.DTOs.User;
using ProfileBook.API.Models;
using ProfileBook.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProfileBook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(DataContext context, IConfiguration configuration, IUserService userService)
    {
        _context = context;
        _configuration = configuration;
        _userService = userService;
    }

    [HttpPost("register")]
  public async Task<IActionResult> Register(UserRegisterDto request)
  {
    try
    {
      if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        return BadRequest("User already exists.");

       CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

       var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            Console.WriteLine($"[AUTH] Registered user: {user.Username} with role: {user.Role}");

            return Ok(new ProfileBook.API.DTOs.Auth.UserResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            });
        }
    catch (Exception ex)
    {
      return StatusCode(500, $"Server error: {ex.Message}");
    }
  }
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLoginDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
        {
            Console.WriteLine($"[AUTH] Login failed: User '{request.Username}' not found.");
            return BadRequest("User not found.");
        }

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            Console.WriteLine($"[AUTH] Login failed: Invalid password for user '{request.Username}'.");
            return BadRequest("Wrong password.");
        }

        Console.WriteLine($"[AUTH] Login successful for user: {user.Username}");

        var token = CreateToken(user);

        return Ok(new
        {
            token = token,
            userId = user.UserId,
            username = user.Username,
            role = user.Role
        });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<ProfileBook.API.DTOs.User.UserResponseDto>> GetProfile()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var user = await _userService.GetUserById(int.Parse(userId));

        if (user == null)
            return NotFound();

        return Ok(user);
    }



    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["JwtSettings:Token"]
            )
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "ProfileBook",
            audience: "ProfileBookUsers",
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

