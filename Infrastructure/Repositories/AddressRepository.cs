using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AddressRepository(DataContext context) : Repo<AddressEntity>(context)
{
    private readonly DataContext _context = context;


    public async Task<AddressEntity> GetAddressAsync(string userId)
    {
        var addressEntity = await _context.Addresses
                                    .Include(a => a.Users)
                                    .FirstOrDefaultAsync(a => a.Users.Any(u => u.Id == userId));

        return addressEntity!;
    }
}
