using DataAccessLibrary.Data;

namespace DataAccessLibrary.Repositories;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public abstract class BaseRepository<TEntity>(BankAppDataContext dbContext)
    where TEntity : class
{
    public virtual async Task<TEntity?> CreateAsync(TEntity entity)
    {
        try
        {
            dbContext.Set<TEntity>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null!;
        }
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            var result = await dbContext.Set<TEntity>().FirstOrDefaultAsync(expression);
            return result ?? null!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null!;
        }
    }

    public virtual IQueryable<TEntity> GetAllAsync()
    {
        try
        {
            var result = dbContext.Set<TEntity>().AsQueryable();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual async Task<TEntity?> UpdateAsync(
        Expression<Func<TEntity, bool>> expression,
        TEntity entity
    )
    {
        try
        {
            var result = await dbContext.Set<TEntity>().FirstOrDefaultAsync(expression);
            if (result == null)
                return result!;

            dbContext.Entry(result).CurrentValues.SetValues(entity);
            await dbContext.SaveChangesAsync();

            return result!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null!;
    }

    /// <summary>
    /// Hard deletes the chosen entity. No going back!
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public virtual async Task<bool?> DeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            var result = await dbContext.Set<TEntity>().FirstOrDefaultAsync(expression);
            if (result == null)
                return null;

            dbContext.Set<TEntity>().Remove(result);
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            var result = await dbContext.Set<TEntity>().AnyAsync(expression);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
