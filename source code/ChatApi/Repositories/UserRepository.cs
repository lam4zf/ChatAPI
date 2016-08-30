using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using ChatAPI.Models;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Constants;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens;


namespace ChatAPI.Repositories
{
	public class UserRepository : IUserRepository
	{
		private static ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();

		public UserRepository()
		{

		}

		public IEnumerable<User> GetAll()
		{
			return _users.Values;
		}

		public RepositoryActionResult<UserLoginResponse> Register(User user)
		{
			try {
				_users[user.email] = user;
				string token = CreateJsonWebToken(user.email);
				user.token = token;
				UserLoginResponse authenticatedUser = new UserLoginResponse
				{
					id = user.id,
					email = user.email,
					name = user.name,
					token = user.token
				};

				return new RepositoryActionResult<UserLoginResponse>(authenticatedUser, RepositoryActionStatus.Created);
			}
			catch (Exception ex)
			{
				return new RepositoryActionResult<UserLoginResponse>(null, RepositoryActionStatus.Error, ex);
			}
		}

		public User FindUserByEmail(string email) 
		{
			User user;
			_users.TryGetValue(email, out user);
			return user;
		}

		public User FindUserByToken(JwtSecurityToken token)
		{
			string email = "";
			foreach(Claim c in token.Claims)
			{
				if (c.Type == ClaimTypes.NameIdentifier)
				{
					email = c.Value;
				}

			}
			User user;
			_users.TryGetValue(email, out user);
			return user;

		}

		public RepositoryActionResult<UserLoginResponse> Login(UserLoginRequest user)
		{
			try
			{
				var userRecord = _users[user.email];
				UserLoginResponse userResponse = new UserLoginResponse();

				if (userRecord == null )
				{
					return new RepositoryActionResult<UserLoginResponse>(userResponse, RepositoryActionStatus.NotFound);
				}
				if(userRecord.password != user.password)
				{
					return new RepositoryActionResult<UserLoginResponse>(userResponse, RepositoryActionStatus.NotFound);
				}
				string token = CreateJsonWebToken(user.email);
				_users[user.email].token = token;
				userResponse.id = userRecord.id;
				userResponse.email = userRecord.email; 
				userResponse.name = userRecord.name;
				userResponse.token = token;
				return new RepositoryActionResult<UserLoginResponse>(userResponse, RepositoryActionStatus.Updated);
			}
			catch(Exception ex)
			{
				return new RepositoryActionResult<UserLoginResponse>(null, RepositoryActionStatus.Error, ex);
			}

		}

		public RepositoryActionResult<User> Update(User user)
		{
			try {
				if(_users[user.email] == null)
				{
					return new RepositoryActionResult<User>(user, RepositoryActionStatus.NotFound);
				}
				_users[user.email] = user;
				return new RepositoryActionResult<User>(user, RepositoryActionStatus.Updated);
			}
			catch(Exception ex)
			{
				return new RepositoryActionResult<User>(null, RepositoryActionStatus.Error, ex);
			}
		}

		
		/// <summary>
		/// Creates the authorization token for requests made by users who have logged in.
		/// </summary>
		/// <param name="email">The email/username of the user which will be embedded in the token, and later retrieved for validation</param>
		/// <returns></returns>
		private string CreateJsonWebToken(string email)
		{
			try {
				JsonWebToken token = new JsonWebToken();
				token.Header = new Thinktecture.IdentityModel.Tokens.JwtHeader
				{
					SignatureAlgorithm = Thinktecture.IdentityModel.Constants.JwtConstants.SignatureAlgorithms.HMACSHA256,
					SigningCredentials = new HmacSigningCredentials("Dc9Mpi3jbooUpBQpB/4R7XtUsa3D/ALSjTVvk8IUZbg=")
				};
				
				token.Issuer = "https://chatapiidsvr3/embedded";
				token.Audience = new Uri("https://chatapi");
				token.Claims = new Dictionary<string, string>
				{
					{ ClaimTypes.NameIdentifier, email},
					{ ClaimTypes.Expiration, DateTime.UtcNow.AddDays(2).ToString()}
				};


				JsonWebTokenHandler handler = new JsonWebTokenHandler();
				
				string result = handler.WriteToken(token);
				return result;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
