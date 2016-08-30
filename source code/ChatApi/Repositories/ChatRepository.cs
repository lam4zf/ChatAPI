using ChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ChatAPI.Repositories
{
	public class ChatRepository : IChatRepository
	{

		private static ConcurrentDictionary<int,Chat> _allChats =
			  new ConcurrentDictionary<int,Chat>();
		private static ConcurrentDictionary<User, List<Chat>> _userChats =
			  new ConcurrentDictionary<User, List<Chat>>();

		public ChatRepository()
		{
		}

		public IEnumerable<Chat> List(User user)
		{
			return _userChats[user];
		}
		public IEnumerable<int> ListAllKeys()
		{
			return _allChats.Keys;
		}

		/// <summary>
		/// Given an ID, it returns a chat for the authorized user, granted it exists
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public RepositoryActionResult<Chat> FindChatById(int id, User user)
		{
			try
			{
				List<Chat> chats = _userChats[user];
				foreach (Chat c in chats) {
					if(c.id == id)
						return new RepositoryActionResult<Chat>(c, RepositoryActionStatus.Ok);
				}
				return new RepositoryActionResult<Chat>(null, RepositoryActionStatus.NotFound);
			}
			catch(Exception ex)
			{
				return new RepositoryActionResult<Chat>(null, RepositoryActionStatus.Error, ex);
			}
			
		}

		public RepositoryActionResult<Chat> Create(User user, Chat chat)
		{
			try
			{
				if (!_userChats.ContainsKey(user))
					_userChats.TryAdd(user,new List<Chat>());
				if (!_allChats.ContainsKey(chat.id))
					_allChats.TryAdd(chat.id, new Chat());

				List<Chat> userChats = _userChats[user];
				userChats.Add(chat);
				_userChats[user] = userChats;
				_allChats[chat.id] = chat;
				return new RepositoryActionResult<Chat>(chat, RepositoryActionStatus.Created);
			}
			catch(Exception ex)
			{
				return new RepositoryActionResult<Chat>(null, RepositoryActionStatus.Error, ex);
			}
		}
	}
}
