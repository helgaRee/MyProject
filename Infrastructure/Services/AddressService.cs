﻿using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AddressService(DataContext context)
{
    private readonly DataContext _context = context;





    public async Task<AddressEntity> GetAddressAsync(string UserId)
    {
        var addressEntity = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == UserId);

        return addressEntity!;


    }


    public async Task<AddressEntity> CreateAddressAsync(AddressEntity entity)
    {
        _context.Addresses.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }


    public async Task<bool> UpdateAddressAsync(AddressEntity entity)
    {
        var existingAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == entity.UserId);
        _context.Addresses.Add(entity);
        await _context.SaveChangesAsync();
        return true;
    }


}
