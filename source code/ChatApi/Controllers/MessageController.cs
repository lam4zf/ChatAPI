using ChatAPI.Factories;
using ChatAPI.Models;
using ChatAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatAPI.Controllers
{
	public class MessageController : ApiController
	{
		IMessageRepository _messages;
		MessageFactory _messageFactory = new MessageFactory();

		/// <summary>
		/// Default constructor
		/// </summary>
		public MessageController()
		{
			_messages = new MessageRepository(); //the default constructor only makes chat right now
		}

		/// <summary>
		/// Override constructor to operate on new users
		/// </summary>
		/// <param name="repository"></param>
		public MessageController(IMessageRepository repository)
		{
			_messages = repository;
		}

		

	}
}
