using ChatAPI.Models;
using ChatAPI.Repositories;
using System;

namespace ChatAPI.Factories
{
	class MessageFactory
	{
		public MessageFactory()
		{

		}
		public Message CreateMessage(MessageRequest message, Chat chat, User user, ref IMessageRepository messageRepo)
		{
			return new Message
			{
				id = NextAvailableID(ref messageRepo),
				user_id = chat.user_id,
				user_email = user.email,
				chat_id = chat.id,
				message = message.message,
				created = DateTime.Now,
			};
		}
		//public Message CreateMessage(Message message, Chat chat, ref IMessageRepository messageRepo)
		//{
		//	return new Message
		//	{
		//		id = NextAvailableID(ref messageRepo),
		//		user_id = chat.user_id,
		//		chat_id = chat.id,
		//		message = message.message,
		//		created = message.created,
		//	};
		//}
		public MessageResponse CreateMessageResponse(Message message, UserLite user)
		{
			return new MessageResponse
			{
				id = message.id,
				user_id = message.user_id,
				chat_id = message.chat_id,
				message = message.message,
				created = message.created,
				user = user
			};
		}
		private int NextAvailableID(ref IMessageRepository messageRepo)
		{
			int max = 0;
			foreach (int id in messageRepo.ListAllKeys())
			{
				if (id > max)
				{
					max = id;
				}
			}
			max++;
			return max;
		}
	}
}
