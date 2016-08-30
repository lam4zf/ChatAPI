using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Models
{
	public class MessageRequest
	{
		public DateTime created { get; set; }
		public string message { get; set; }
	}
}
