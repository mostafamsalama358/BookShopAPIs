using Bl.UnitOfWork;
using Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Repos.Generics
{
    public class Generic<T> : IGeneric<T> where T : class
    {
         BookShopContextAPIs _context;
        public Generic(BookShopContextAPIs _context)
        {
            this._context = _context;
        }

        public void Add(T entity)
        {
           _context.Set<T>().Add(entity);
        }

        public bool Delete(int id)
        {
            try
            {
                T? entity = _context.Set<T>().Find(id);
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _context.Set<T>().ToList(); // If any null values are encountered, EF will handle it
            }
            catch (Exception ex)
            {
                // Log or handle exceptions here
                Console.WriteLine($"Error occurred in GetAll(): {ex.Message}");
                throw;
            }
        }

        public async Task <T?> GetById(int id)
        {
            var entity =  _context.Set<T>().FindAsync(id);
            return await entity;
        }

        public bool Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return true;

        }
    }
}
