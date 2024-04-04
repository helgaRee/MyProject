using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AddressService(DataContext context)
{
    private readonly DataContext _context = context;





    public async Task<AddressEntity> GetAddressAsync(int? id)
    {
        var addressEntity = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

        return addressEntity!;


    }

    public async Task<AddressEntity> GetAddressAsync(AddressEntity entity)
    {
        var addressEntity = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == entity.Id);

        return addressEntity!;


    }


    public async Task<bool> CreateAddressAsync(AddressEntity entity)
    {
        _context.Addresses.Add(entity);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> UpdateAddressAsync(AddressEntity entity)
    {
        var existingAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == entity.Id);

        if (existingAddress != null)
        {
            _context.Entry(existingAddress).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;

        }

        return false;
    }


}
