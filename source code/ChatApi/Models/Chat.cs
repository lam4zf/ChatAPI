using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Models
{
	public class Chat
	{
		public int id { get; set; }
		public int user_id { get; set; }
		public string user_email { get; set; }
		public string name { get; set; }
		public DateTime created { get; set; }
		
	}
}
