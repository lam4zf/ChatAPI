using ChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChatAPI.Repositories
{
	public interface IChatRepository
	{
		RepositoryActionResult<Chat> Create(User user, Chat chat);
		RepositoryActionResult<Chat> FindChatById(int id, User user);
		IEnumerable<Chat> List(User user);
		IEnumerable<int> ListAllKeys();
	}
}
