using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

using API.Controllers;
using API.Data;
using API.Entities;
using API.DTOs;
using API.Interfaces;

namespace API;

public class AccountController : BaseApiController
{
  private readonly DataContext _context;
  private readonly ITokenService _tokenService;

  public AccountController(DataContext context, ITokenService tokenService)
  {
    _context = context;
    _tokenService = tokenService;
  }

  [HttpPost("register")] // POST /api/account/register
  public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
  {
    if (await UserExists(registerDto.email.ToLower()))
    {
      return BadRequest("Email is taken");
    }

    using var hmac = new HMACSHA512();

    var user = new AppUser
    {
      FirstName = registerDto.firstName,
      LastName = registerDto.lastName,
      Email = registerDto.email.ToLower(),
      PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
      PasswordSalt = hmac.Key,
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    return Ok(new UserDto
    {
      email = user.Email,
      token = _tokenService.CreateToken(user),
    }
    );
  }

  [HttpPost("login")] // POST /api/account/login
  public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
  {
    var user = await _context.Users.SingleOrDefaultAsync(x => x.Email.Equals(loginDto.email.ToLower()));
    if (user == null)
    {
      return Unauthorized("Invalid credentials");
    }

    using var hmac = new HMACSHA512(user.PasswordSalt);
    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
    for (int i = 0; i < computedHash.Length; i++)
    {
      if (computedHash[i] != user.PasswordHash[i])
      {
        return Unauthorized("Invalid credentials");
      }
    }

    return Ok(new UserDto
    {
      email = user.Email,
      token = _tokenService.CreateToken(user),
    }
    );
  }

  private async Task<bool> UserExists(string email)
  {
    return await _context.Users.AnyAsync(x => x.Email.Equals(email.ToLower()));
  }
}
