using System;
using System.Linq;
using System.Linq.Expressions;
using EventServer.Core.Domain;

namespace EventServer.Core
{
   public interface IRepository
   {
      T Get<T>(int id) where T : Entity;
      T Get<T>(Expression<Func<T, bool>> predicate) where T : Entity;

      IQueryable<T> Find<T>() where T : Entity;
      IQueryable<T> Find<T>(Expression<Func<T, bool>> predicate) where T : Entity;

      T Save<T>(T entity) where T : Entity;
      T Delete<T>(T entity) where T : Entity;
   }
}