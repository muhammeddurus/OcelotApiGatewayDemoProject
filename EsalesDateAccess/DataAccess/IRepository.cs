using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsalesDateAccess.DataAccess
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Insert(T item);
        void Update(T item);
        void Delete(int id);
    }
}
