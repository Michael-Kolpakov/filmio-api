using System.Linq.Expressions;
using Filmio.DAL.Helpers;
using Filmio.DAL.Persistence;
using Filmio.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace Filmio.DAL.Repositories.Realizations.Base;

public class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    private readonly FilmioDbContext _filmioDbContext;

    protected RepositoryBase(FilmioDbContext filmioDbContext)
    {
        _filmioDbContext = filmioDbContext;
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return await GetQueryable(predicate, include).ToListAsync();
    }

    public PaginationResponse<T> GetAllPaginated(
        ushort? pageNumber = null,
        ushort? pageSize = null,
        Expression<Func<T, T>>? selector = default,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, object>>? ascendingSortKeySelector = default,
        Expression<Func<T, object>>? descendingSortKeySelector = default)
    {
        var query = GetQueryable(
            predicate,
            include,
            selector,
            orderByAsc: ascendingSortKeySelector,
            orderByDesc: descendingSortKeySelector);
        return PaginationResponse<T>.Create(
            query,
            pageNumber,
            pageSize);
    }

    public async Task<T?> GetSingleOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return await GetQueryable(predicate, include).SingleOrDefaultAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return await GetQueryable(predicate, include).FirstOrDefaultAsync();
    }

    public T Create(T entity)
    {
        return _filmioDbContext.Set<T>().Add(entity).Entity;
    }

    public async Task<T> CreateAsync(T entity)
    {
        var tmp = await _filmioDbContext.Set<T>().AddAsync(entity);

        return tmp.Entity;
    }

    public Task CreateRangeAsync(IEnumerable<T> items)
    {
        return _filmioDbContext.Set<T>().AddRangeAsync(items);
    }

    public EntityEntry<T> Update(T entity)
    {
        return _filmioDbContext.Set<T>().Update(entity);
    }

    public void UpdateRange(IEnumerable<T> items)
    {
        _filmioDbContext.Set<T>().UpdateRange(items);
    }

    public void Delete(T entity)
    {
        _filmioDbContext.Set<T>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> items)
    {
        _filmioDbContext.Set<T>().RemoveRange(items);
    }

    private IQueryable<T> GetQueryable(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, T>>? selector = default,
        Expression<Func<T, object>>? orderByAsc = default,
        Expression<Func<T, object>>? orderByDesc = default,
        int? limit = null,
        int? offset = null)
    {
        var entityType = _filmioDbContext.Model.FindEntityType(typeof(T));
        var keyProperty = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault();
        IQueryable<T>? query;
        
        if (keyProperty != null)
        {
            query = _filmioDbContext
                .Set<T>()
                .AsNoTracking()
                .OrderBy(e => EF.Property<object>(e, keyProperty.Name));
        }
        else
        {
            query = _filmioDbContext.Set<T>().AsNoTracking();
        }

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (selector is not null)
        {
            query = query.Select(selector);
        }

        if (orderByAsc is not null)
        {
            query = query.OrderBy(orderByAsc);
        }

        if (orderByDesc is not null)
        {
            query = query.OrderByDescending(orderByDesc);
        }

        if (offset.HasValue && offset.Value >= 0)
        {
            query = query.Skip(offset.Value);
        }

        if (limit.HasValue && limit.Value > 0)
        {
            query = query.Take(limit.Value);
        }

        return query.AsNoTracking();
    }
}