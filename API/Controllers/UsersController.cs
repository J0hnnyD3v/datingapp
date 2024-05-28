using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using API.Data;
using API.Entities;
using API.Interfaces;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
  private readonly IUserRepository _userRepository;

  public UsersController(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
  {
    var users = await _userRepository.GetUsersAsync();
    return Ok(users);
  }

  [HttpGet("{email}")] // /api/users/email@email.com
  public async Task<ActionResult<AppUser>> GetUser(string email)
  {
    var user = await _userRepository.GetUserByEmailAsync(email);
    return Ok(user);
  }

}
