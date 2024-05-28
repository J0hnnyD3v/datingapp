using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository : IUserRepository
{
  private readonly DataContext _context;
  public UserRepository(DataContext context)
  {
    _context = context;
  }

  public async Task<AppUser> GetUserByEmailAsync(string email)
  {
    return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
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
