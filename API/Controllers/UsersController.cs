using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using API.DTOs;
using API.Interfaces;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
  private readonly IUserRepository _userRepository;
  private readonly IMapper _mapper;

  public UsersController(IUserRepository userRepository, IMapper mapper)
  {
    _userRepository = userRepository;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
  {
    return Ok(await _userRepository.GetMembersAsync());
  }

  [HttpGet("{email}")] // /api/users/email@email.com
  public async Task<ActionResult<MemberDto>> GetUser(string email)
  {
    return Ok(await _userRepository.GetMemberByEmailAsync(email));
  }

  [HttpPut]
  public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
  {
    var email = User.FindFirst(ClaimTypes.Email)?.Value;
    var user = await _userRepository.GetUserByEmailAsync(email);

    if (user == null) return NotFound();

    _mapper.Map(memberUpdateDto, user);

    if (await _userRepository.SaveAllAsync()) return NoContent();

    return BadRequest("Failed to update user");
  }

}
