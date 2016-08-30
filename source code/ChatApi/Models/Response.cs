using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Models
{
	public class PackagedUserLoginResponse
	{
		public bool success { get; set; }
		public UserLoginResponse data { get; set; }
	}
	public class PackagedUserResponse
	{
		public bool success { get; set; }
		public User data { get; set; }
	}
	public class PackagedChatResponses
	{
		public bool success { get; set; }
		public List<ChatResponse> data { get; set; }
	}
	public class PackagedChatResponse
	{
		public bool success { get; set; }
		public ChatResponse data { get; set; }
	}
	public class PackagedMessageResponse
	{
		public bool success { get; set; }
		public MessageResponse data { get; set; }
	}
	public class PackagedMessageResponses
	{
		public bool success { get; set; }
		public List<MessageResponse> data { get; set; }
	}
}
