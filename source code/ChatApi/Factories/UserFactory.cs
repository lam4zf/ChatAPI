using ChatAPI.Models;
using ChatAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChatAPI.Factories
{
	public class UserFactory
	{

		public UserFactory()
		{

		}

		public User CreateUser(User user)
		{
			return new User
			{
				id = user.id,
				name = user.name,
				email = user.email,
				password = user.password,
				token = user.token
			};
		}

		public User CreateUser(UserRegistrationRequest user, ref IUserRepository userRepo)
		{
			return new User
			{
				id = NextAvailableID(ref userRepo),
				name = user.name,
				email = user.email,
				password = user.password,
				token = null
			};
		}

		public UserLoginResponse CreateAuthenticatedUser(UserLoginResponse user)
		{
			return new UserLoginResponse
			{
				id = user.id,
				token = user.token,
				email = user.email,
				name = user.name,
			};

		}

		private int NextAvailableID(ref IUserRepository userRepo)
		{
			int max = 0;
			foreach (User u in userRepo.GetAll())
			{
				if(u.id > max)
				{
					max = u.id;
				}
			}
			max++;
			return max;
		}
	}
}
