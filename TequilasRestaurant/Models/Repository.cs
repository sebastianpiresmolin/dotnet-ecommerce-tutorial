﻿using Microsoft.EntityFrameworkCore;
using TequilasRestaurant.Data;

namespace TequilasRestaurant.Models;

public class Repository<T> : IRepository<T> where T : class
{
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    protected ApplicationDbContext _context { get; set; }
    private DbSet<T> _dbSet { get; }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id, QueryOptions<T> options)
    {
        IQueryable<T> query = _dbSet;
        if (options.HasWhere) query = query.Where(options.Where);

        if (options.HasOrderBy) query = query.OrderBy(options.OrderBy);

        foreach (var include in options.GetIncludes) query = query.Include(include);

        var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
        var primaryKeyName = key?.Name;
        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}