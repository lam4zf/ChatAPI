using ChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Factories
{
	public class ResponseFactory
	{
		public PackagedUserLoginResponse CreateResponse(UserLoginResponse user)
		{
			PackagedUserLoginResponse r = new PackagedUserLoginResponse();
			r.success = true;
			r.data = user;
			return r;
		}
		public PackagedUserResponse CreateResponse(User user)
		{
			PackagedUserResponse r = new PackagedUserResponse();
			r.success = true;
			r.data = user;
			return r;
		}
		public PackagedChatResponse CreateResponse(ChatResponse chat)
		{
			PackagedChatResponse r = new PackagedChatResponse();
			r.success = true;
			r.data = chat;
			return r;
		}
		public PackagedChatResponses CreateResponse(List<ChatResponse> chats)
		{
			PackagedChatResponses r = new PackagedChatResponses();
			r.success = true;
			r.data = chats;
			return r;
		}	
		public PackagedMessageResponse CreateResponse(MessageResponse message)
		{
			PackagedMessageResponse r = new PackagedMessageResponse();
			r.success = true;
			r.data = message;
			return r;
		}
		public PackagedMessageResponses CreateResponse(List<MessageResponse> messages)
		{
			PackagedMessageResponses r = new PackagedMessageResponses();
			r.success = true;
			r.data = messages;
			return r;
		}
	}
}
