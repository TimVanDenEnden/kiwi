﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lisa.Kiwi.WebApi
{
    public class AuthRepository : IDisposable
    {
        private readonly KiwiContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new KiwiContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }

        public async Task<IdentityResult> AddUser(AddUserModel userModel)
        {
            var user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IEnumerable<IdentityUser>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            return users;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityUser> FindUser(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public bool HasRole(IdentityUser user, string roleName)
        {
            var role = _ctx.Roles.First(r => r.Name == roleName);
            return user.Roles.Any(r => r.RoleId == role.Id);
        }

        public async Task<string> GetRole(IdentityUser user)
        {
            var role = await _userManager.GetRolesAsync(user.Id);
            return role.FirstOrDefault();
        }

        public async Task UpdatePassword(string id, string password)
        {
            await _userManager.RemovePasswordAsync(id);
            await _userManager.AddPasswordAsync(id, password);
        }
    }
}