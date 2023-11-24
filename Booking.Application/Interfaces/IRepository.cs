using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity); 
        T GetValue(Expression<Func<T, bool>>? filter=null/*,bool tracked=false*/);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null/*, bool tracked = false*/);
        bool Any(Expression<Func<T, bool>>? filter);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);     
       
    }
}
