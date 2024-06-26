﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
  private readonly IUserRepository _userRepository;
  private readonly IMapper _mapper;
  private readonly IPhotoService _photoService;

  public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
  {
    _userRepository = userRepository;
    _mapper = mapper;
    _photoService = photoService;
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
    var user = await _userRepository.GetUserByEmailAsync(User.GetEmail());

    if (user == null) return NotFound();

    _mapper.Map(memberUpdateDto, user);

    if (await _userRepository.SaveAllAsync()) return NoContent();

    return BadRequest("Failed to update user");
  }

  [HttpPost("add-photo")]
  public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
  {
    var user = await _userRepository.GetUserByEmailAsync(User.GetEmail());

    if (user == null) return NotFound();

    var result = await _photoService.AddPhotoAsync(file);

    if (result.Error != null) return BadRequest(result.Error.Message);

    var photo = new Photo
    {
      Url = result.SecureUrl.AbsoluteUri,
      PublicId = result.PublicId
    };

    if (user.Photos.Count == 0) photo.IsMain = true;
    user.Photos.Add(photo);

    // if (user.Photos.Count == 0)
    // {
    //   photo.IsMain = true;
    //   user.Photos.Add(photo);
    // }
    // else
    // {
    //   user.Photos[0] = photo;
    // }

    if (await _userRepository.SaveAllAsync()) return Ok(_mapper.Map<PhotoDto>(photo));

    return BadRequest("Problem saving the photo");
  }
}
