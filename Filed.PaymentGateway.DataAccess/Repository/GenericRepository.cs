using Filed.PaymentGateway.DataAccess.Common;
using Filed.PaymentGateway.Interfaces.Gateway;
using Filed.PaymentGateway.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filed.PaymentGateway.DataAccess.Repository
{
    public class GenericRepository<T> : IBaseRepository<T> where T : class
    {
        private DbSet<T> table = null;
        private readonly PaymentContext _PaymentContext;

        public GenericRepository(IPaymentGateway paymentGateway, PaymentContext paymentContext){
            _PaymentContext = paymentContext;
            table = table = _PaymentContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();

        }

        public T GetById(int id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public T InsertAndGet(T obj)
        {
            table.Add(obj);
            Save();
            return obj;
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _PaymentContext.Entry(obj).State = EntityState.Modified;
        }

        public T UpdateAndGet(T obj)
        {
            table.Attach(obj);
            _PaymentContext.Entry(obj).State = EntityState.Modified;
            Save();
            return obj;
        }

        public void Delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public void Save()
        {
            _PaymentContext.SaveChanges();
        }
    }
}
