
namespace ChatAPI.Models
{
	public class UserLoginRequest
	{		
		public string email { get; set; }
		public string password { get; set; }

		public bool RequiredFieldsAreNullOrBlank
		{
			get
			{
				if ((email.Length == 0 || email == null)
					|| (password.Length == 0 || password == null))
					return true;
				return false;
			}
		}
	}
}
