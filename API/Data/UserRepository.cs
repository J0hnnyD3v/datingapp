﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Data;

public class UserRepository : IUserRepository
{
  private readonly DataContext _context;
  private readonly IMapper _mapper;

  public UserRepository(DataContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<MemberDto> GetMemberByEmailAsync(string email)
  {
    return await _context.Users
        .Where(user => user.Email == email)
        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
        .SingleOrDefaultAsync();
  }

  public async Task<IEnumerable<MemberDto>> GetMembersAsync()
  {
    return await _context.Users
        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
        .ToListAsync();
  }

  public async Task<AppUser> GetUserByEmailAsync(string email)
  {
    return await _context.Users.Include(u => u.Photos).SingleOrDefaultAsync(u => u.Email == email);
  }

  public async Task<AppUser> GetUserByIdAsync(int id)
  {
    return await _context.Users.FindAsync(id);
  }

  public async Task<IEnumerable<AppUser>> GetUsersAsync()
  {
    var users = await _context.Users.Include(u => u.Photos).ToListAsync();
    return users;
  }

  public async Task<bool> SaveAllAsync()
  {
    return await _context.SaveChangesAsync() > 0;
  }

  public void Update(AppUser user)
  {
    _context.Entry(user).State = EntityState.Modified;
  }
}
