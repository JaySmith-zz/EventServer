using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Security;

namespace EventServer.Infrastructure.Providers
{
    public class AppRoleProvider : RoleProvider
    {
        private string _rolesPath;

        public string RolesPath
        {
            get { return _rolesPath ?? (_rolesPath = HostingEnvironment.MapPath("~/App_Data/roles.xml")); }
        }
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return GetRoles()
                .Single(x => x.Name == roleName)
                .Users
                .Any(x => string.Equals(x, username, StringComparison.OrdinalIgnoreCase));
        }

        public override string[] GetRolesForUser(string username)
        {
            return GetRoles()
                .Where(x => x.Users.Any(user => string.Equals(user, username, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.Name)
                .ToArray();
        }

        public override void CreateRole(string roleName)
        {
            var newRolesList = GetRoles()
                .Concat(new[] {new AppRole(roleName)})
                .OrderBy(x => x.Name);

            SaveRoles(newRolesList);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            var roles = GetRoles();

            if (throwOnPopulatedRole && roles.SingleOrDefault(x => x.Name == roleName).Users.Count > 0)
                throw new ApplicationException("Cannot delete '{0}' (contains users)".F(roleName));

            SaveRoles(roles.Where(x => x.Name != roleName));

            return true;
        }

        public override bool RoleExists(string roleName)
        {
            return GetRoles().Any(x => x.Name == roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var roles = GetRoles();

            roleNames
                .Select(x => roles.Single(role => role.Name == x))
                .SelectMany(role => usernames, (role, username) => new {role, username})
                .Each(x => x.role.Users.Add(x.username));

            SaveRoles(roles);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var roles = GetRoles();

            roleNames
                .Select(x => roles.Single(role => role.Name == x))
                .SelectMany(role => usernames, (role, username) => new {role, username})
                .Each(x => x.role.Users.Remove(x.username));

            SaveRoles(roles);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return GetRoles()
                .Single(x => x.Name == roleName)
                .Users
                .ToArray();
        }

        public override string[] GetAllRoles()
        {
            return GetRoles().Select(x => x.Name).ToArray();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        private AppRole[] GetRoles()
        {
            if (!File.Exists(RolesPath))
                return new[] {new AppRole("Admin", "dev@dev.com")};

            return File.ReadAllBytes(RolesPath).DeserializeXml<AppRoles>().Roles;
        }

        private void SaveRoles(IEnumerable<AppRole> roles)
        {
            File.WriteAllBytes(RolesPath, new AppRoles {Roles = roles.ToArray()}.SerializeXml());
        }

        public class AppRoles
        {
            public AppRole[] Roles { get; set; }
        }

        public class AppRole
        {
            public AppRole() : this(null)
            {
            }

            public AppRole(string name)
            {
                Name = name;
                Users = new List<string>();
            }

            public AppRole(string name, params string[] users)
            {
                Name = name;
                Users = new List<string>(users);
            }

            public string Name { get; set; }
            public List<string> Users { get; set; }
        }
    }
}