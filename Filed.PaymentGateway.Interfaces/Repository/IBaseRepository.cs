using System;
using System.Collections.Generic;
using System.Text;

namespace Filed.PaymentGateway.Interfaces.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Insert(T obj);
        T InsertAndGet(T obj);
        void Update(T obj);
        T UpdateAndGet(T obj);
        void Delete(int id);
        void Save();
    }
}
