﻿using ChatNest.Core.Utilities.Security;
using ChatNest.Core.ViewModels.Auth;
using ChatNest.Data.Context;
using ChatNest.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Core.Services.Users
{
	public class UserService : BaseService, IUserService
	{
		public UserService(ApplicationDbContext context) : base(context)
		{
		}
		public async Task<bool> IsUserExist(string userName)
		{
			return await Table<User>().AnyAsync(u => u.UserName == userName.ToLower());
		}

		public async Task<bool> IsUserExist(long userId)
		{
			return await Table<User>().AnyAsync(u => u.Id == userId);
		}

		public async Task<bool> RegisterUser(RegisterViewModel registerModel)
		{

			if (await IsUserExist(registerModel.UserName))
				return false;

			if (registerModel.Password != registerModel.RePassword)
				return false;

			var password = registerModel.Password.EncodePasswordMd5();
			var user = new User()
			{
				Avatar = "Default.jpg",
				CreateDate = DateTime.Now,
				Password = password,
				UserName = registerModel.UserName.ToLower()
			};
			Insert(user);
			await Save();
			return true;
		}
		public async Task<User> LoginUser(LoginViewModel loginModel)
		{
			var user = await Table<User>().SingleOrDefaultAsync(u => u.UserName == loginModel.UserName.ToLower());
			if (user == null)
				return null;
			var password = loginModel.Password.EncodePasswordMd5();

			if (password != user.Password)
				return null;

			return user;
		}
	}
}
