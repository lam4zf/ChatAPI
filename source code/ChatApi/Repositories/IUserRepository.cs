using System;
using System.Collections.Generic;
using ChatAPI.Models;
using System.IdentityModel.Tokens;

namespace ChatAPI.Repositories
{
	public interface IUserRepository
	{
		RepositoryActionResult<UserLoginResponse> Register(User user);
		IEnumerable<User> GetAll();
		User FindUserByEmail(string email);
		User FindUserByToken(JwtSecurityToken token);
		RepositoryActionResult<UserLoginResponse> Login(UserLoginRequest user);
		RepositoryActionResult<User> Update(User item);

	}
}
