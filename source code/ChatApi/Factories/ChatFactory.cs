using ChatAPI.Models;
using ChatAPI.Repositories;
using System;

namespace ChatAPI.Factories
{
	public class ChatFactory
	{
		public ChatFactory()
		{

		}
		public Chat CreateChat(ChatRequest chat, User user, ref IChatRepository chatRepo)
		{
			return new Chat
			{
				id = NextAvailableID(ref chatRepo),
				user_id = user.id,
				user_email = user.email,
				name = chat.name,
				created = DateTime.Now,
			};
		}
		public Chat CreateChat(Chat chat,  ref IChatRepository chatRepo)
		{
			return new Chat
			{
				id = NextAvailableID(ref chatRepo),
				user_id = chat.user_id,
				name = chat.name,
				created = chat.created,
			};
		}
		public Chat CreateChat(Chat chat)
		{
			return new Chat
			{
				id = chat.id,
				user_id = chat.user_id,
				name = chat.name,
				created = chat.created,
			};
		}
		public ChatResponse CreateChatResponse(Chat chat, UserLite user, Message message)
		{
			return new ChatResponse
			{
				id = chat.id,
				user_id = chat.user_id,
				name = chat.name,
				created = chat.created,
				user = user,
				last_message = message
			};
		}
		private int NextAvailableID(ref IChatRepository chatRepo)
		{
			int max = 0;
			foreach (int id in chatRepo.ListAllKeys())
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
