using ChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace ChatAPI.Repositories
{
	public class MessageRepository : IMessageRepository
	{
		private static ConcurrentDictionary<int, Message> _allMessages =
			  new ConcurrentDictionary<int, Message>();

		private static ConcurrentDictionary<Chat, List<Message>> _chatMessages =
			  new ConcurrentDictionary<Chat, List<Message>>();

		public MessageRepository()
		{
		}
		public Message FindLastMessageByChat(Chat chat)
		{
			if (_chatMessages.ContainsKey(chat))
			{
				List<Message> messages = _chatMessages[chat];
				return messages[messages.Count - 1];
			}
			else
			{
				return null;
			}
		}
		public IEnumerable<Message> List(Chat chat)
		{
			return _chatMessages[chat];
		}
		
		public IEnumerable<int> ListAllKeys()
		{
			return _allMessages.Keys;
		}
		public RepositoryActionResult<Message> Create(Chat chat, Message message)
		{
			try
			{
				if (!_chatMessages.ContainsKey(chat))
					_chatMessages.TryAdd(chat, new List<Message>());
				if (!_allMessages.ContainsKey(chat.id))
					_allMessages.TryAdd(message.id, new Message());

				_allMessages[message.id] = message;

				List<Message> chatMessages = _chatMessages[chat];
				chatMessages.Add(message);
				_chatMessages[chat] = chatMessages;

				return new RepositoryActionResult<Message>(message, RepositoryActionStatus.Created);
			}
			catch (Exception ex)
			{
				return new RepositoryActionResult<Message>(null, RepositoryActionStatus.Error, ex);
			}
		}
		
	}
}
