using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Hosting;
using EventServer.Core;
using EventServer.Core.Domain;

namespace EventServer.Infrastructure.Repositories
{
    using EventServer.Models;

    public class XmlRepository : IRepository
    {
        public T Get<T>(int id) where T : Entity
        {
            string path = GetPath<T>(id);
            return !File.Exists(path) ? null : Get<T>(path);
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            return Find<T>().SingleOrDefault(predicate);
        }

        public IQueryable<T> Find<T>() where T : Entity
        {
            return new DirectoryInfo(GetDirectoryName<T>())
                .GetFiles()
                .OrderBy(x => x.CreationTime)
                .Select(x => Get<T>(x.FullName))
                .ToArray()
                .AsQueryable();
        }

        public IQueryable<T> Find<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            return Find<T>().Where(predicate);
        }

        public T Save<T>(T entity) where T : Entity
        {
            File.WriteAllBytes(GetPath<T>(entity.Id), entity.SerializeXml());
            return entity;
        }

        public T Delete<T>(T entity) where T : Entity
        {
            string path = GetPath<T>(entity.Id);

            if (File.Exists(path))
                File.Delete(path);

            return entity;
        }

        /**********************************************************************/
        /**********************************************************************/

        private T Get<T>(string path)
        {
            return File.ReadAllBytes(path).DeserializeXml<T>();
        }

        private string GetPath<T>(int id)
        {
            var directoryName = GetDirectoryName<T>();
            var fileName = id.ToString();

            return directoryName + Path.DirectorySeparatorChar + fileName + ".xml";
        }

        private string GetDirectoryName<T>()
        {
            var directoryName = HostingEnvironment.MapPath("~/App_Data/") + typeof(T).Name;

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            return directoryName;
        }
    }
}