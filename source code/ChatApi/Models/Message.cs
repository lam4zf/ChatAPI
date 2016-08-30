using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Models
{
	public class Message
	{
		public int id { get; set; }
		public int user_id { get; set; }
		public string user_email { get; set; }
		public int chat_id { get; set; }
		public string message { get; set; }
		public DateTime created { get; set; }

	}
}
