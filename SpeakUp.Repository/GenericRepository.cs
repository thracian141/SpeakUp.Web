using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Repository
{
	public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		internal DbSet<T> dbSet;

		public GenericRepository(ApplicationDbContext context)
		{
			_context = context;
			dbSet = _context.Set<T>();
		}

		private bool disposed = false;
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Add(T entity)
		{
			dbSet.Add(entity);
		}
		public async Task<T> AddAsync(T entity)
		{
			await dbSet.AddAsync(entity);
			return entity;
		}

		public void Update(T entity)
		{
			dbSet.Update(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}
		public async Task<T> UpdateAsync(T entity)
		{
			dbSet.Update(entity);
			_context.Entry(entity).State = EntityState.Modified;
			return entity;
		}

		public void Delete(T entity)
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				dbSet.Attach(entity);
			}
			dbSet.Remove(entity);
		}
		public async Task<T> DeleteAsync(T entity)
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				dbSet.Attach(entity);
			}
			dbSet.Remove(entity);
			return entity;
		}

		public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool disabledTracking = true)
		{
			IQueryable<T> query = dbSet;
			if (disabledTracking)
			{
				query = query.AsNoTracking();
			}
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (include != null)
			{
				query = include(query);
			}
			if (orderBy != null)
			{
				return orderBy(query).ToList();
			}
			else
			{
				return query.ToList();
			}
		}

		public T GetById(object id)
		{
			return dbSet.Find(id);
		}

		public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool disabledTracking = true)
		{
			IQueryable<T> query = dbSet;
			if (disabledTracking)
			{
				query = query.AsNoTracking();
			}
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (include != null)
			{
				query = include(query);
			}

			return await query.FirstOrDefaultAsync();
		}

		public T GetByCompositeId(params object[] keyValues)
		{
			return dbSet.Find(keyValues);
		}

		public async Task<T> GetByCompositeIdAsync(Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
			bool disabledTracking = true)
		{
			IQueryable<T> query = dbSet;
			if (disabledTracking)
			{
				query = query.AsNoTracking();
			}
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (include != null)
			{
				query = include(query);
			}

			return await query.FirstOrDefaultAsync();
		}

		public async Task<T> GetUsingCompositeIdAsync(params object[] keyValues) {
			return await dbSet.FindAsync(keyValues);
		}
	}
}
