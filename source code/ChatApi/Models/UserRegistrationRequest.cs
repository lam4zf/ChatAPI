using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Models
{
	public class UserRegistrationRequest
	{
		public string email { get; set; }
		public string name { get; set; }
		public string password { get; set; }
		public string confirm { get; set; }

		public bool RequiredFieldsAreNullOrBlank
		{
			get
			{
				if ((email.Length == 0 || email == null)
					|| (name.Length == 0 || name == null)
					|| (password.Length == 0 || password == null))
					return true;
				return false;
			}
		}

		public bool PasswordsDoNotMatch
		{
			get
			{
				if (password != confirm)
					return true;
				return false;
			}
		}
	}
}
