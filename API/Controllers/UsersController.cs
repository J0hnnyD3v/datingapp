using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using API.Data;
using API.Entities;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
  private readonly DataContext _context;

  public UsersController(DataContext context)
  {
    _context = context;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
  {
    var users = await _context.Users.ToListAsync();
    return Ok(users);
  }

  [HttpGet("{id}")] // /api/users/1
  public async Task<ActionResult<AppUser>> GetUser(int id)
  {
    var user = await _context.Users.FindAsync(id);
    return Ok(user);
  }

}
