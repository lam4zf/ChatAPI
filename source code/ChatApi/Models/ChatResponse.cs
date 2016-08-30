using System;

namespace ChatAPI.Models
{
	public class ChatResponse
	{
		public int id { get; set; }
		public int user_id { get; set; }
		public string name { get; set; }
		public DateTime created { get; set; }
		public UserLite user { get; set; }
		public Message last_message { get; set; }
	}
}
