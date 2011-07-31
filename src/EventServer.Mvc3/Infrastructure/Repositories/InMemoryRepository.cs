using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EventServer.Core;
using EventServer.Core.Domain;

namespace EventServer.Infrastructure.Repositories
{
    using EventServer.Models;

    public class InMemoryRepository : IRepository
    {
        public InMemoryRepository()
        {
        }

        public InMemoryRepository(bool clear)
        {
            if (clear)
                _lists.Clear();
        }

        public T Get<T>(int id) where T : Entity
        {
            return Get<T>(x => x.Id == id);
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            return Find<T>().SingleOrDefault(predicate);
        }

        public IQueryable<T> Find<T>() where T : Entity
        {
            return ListFor<T>().AsQueryable();
        }

        public IQueryable<T> Find<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            return Find<T>().Where(predicate);
        }

        public T Save<T>(T entity) where T : Entity
        {
            ListFor<T>().Add(entity);
            return entity;
        }

        public T Delete<T>(T entity) where T : Entity
        {
            ListFor<T>().Remove(entity);
            return entity;
        }

        /**********************************************************************/
        /**********************************************************************/

        private static readonly IDictionary<Type, object> _lists = new Dictionary<Type, object>();
        private static readonly object _lockObject = new object();

        private static IList<T> ListFor<T>()
        {
            var type = typeof(T);

            if (!_lists.ContainsKey(type))
            {
                lock (_lockObject)
                {
                    if (!_lists.ContainsKey(type))
                    {
                        Type listType = typeof(List<>).MakeGenericType(type);
                        _lists[type] = Activator.CreateInstance(listType);
                    }
                }
            }

            return (IList<T>)_lists[type];
        }
    }
}