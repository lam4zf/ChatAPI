using ChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Repositories
{
	public interface IMessageRepository
	{
		Message FindLastMessageByChat(Chat chat);
		IEnumerable<Message> List(Chat chat);
		IEnumerable<int> ListAllKeys();
		RepositoryActionResult<Message> Create(Chat chat, Message message);
	}
}
