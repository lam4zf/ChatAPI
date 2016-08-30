
namespace ChatAPI.Models
{
	public class AuthorizedUser
	{
		public int id { get; }
		public string token { get; }
		public string email { get; }
		public string name { get; }
	}
}
