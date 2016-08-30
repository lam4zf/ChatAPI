using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ChatAPI.Factories;
using ChatAPI.Models;
using ChatAPI.Repositories;
using Thinktecture.IdentityModel.Tokens;
using System.IdentityModel.Tokens;

namespace ChatAPI.Controllers
{	
    public class UserController : ApiController
    {
		IUserRepository _users;
		UserFactory _userFactory = new UserFactory();

		IChatRepository _chats;
		ChatFactory _chatFactory = new ChatFactory();

		IMessageRepository _messages;
		MessageFactory _messageFactory = new MessageFactory();

		ResponseFactory _responseFactory = new ResponseFactory();
		/// <summary>
		/// Default constructor
		/// </summary>
		public UserController()
		{
			_users = new UserRepository();			
			
			_chats = new ChatRepository();

			_messages = new MessageRepository();
		}

		/// <summary>
		/// Loads some test data since all objects are in-memory
		/// </summary>
		/// <returns></returns>
		[Route("api/loaddata")]
		[HttpGet]
		public IHttpActionResult LoadData()
		{
			User user1 = new User();
			user1.id = 1;
			user1.name = "Andre";
			user1.email = "andre@orainteractive.com";
			user1.password = "test123";
			_users.Register(user1);
			User user2 = new User();
			user2.id = 2;
			user2.name = "Dan";
			user2.email = "dan@orainteractive.com";
			user2.password = "test123";
			_users.Register(user2);
			Chat chat1 = new Chat();
			chat1.id = 1;
			chat1.user_id = 1;
			chat1.name = "A chat";
			chat1.created = new DateTime(2016, 07, 12, 4, 30, 21);
			_chats.Create(user1, chat1);
			Chat chat2 = new Chat();
			chat2.id = 1;
			chat2.user_id = 1;
			chat2.name = "A chat 2";
			chat2.created = new DateTime(2016, 07, 14, 12, 30, 21);
			_chats.Create(user2, chat2);
			Message message1 = new Message();
			message1.id = 1;
			message1.chat_id = 1;
			message1.user_id = 1;
			message1.message = "Hey there!";
			message1.created = new DateTime(2016, 07, 12, 04, 32, 21);
			_messages.Create(chat1, message1);
			Message message2 = new Message();
			message2.id = 2;
			message2.chat_id = 1;
			message2.user_id = 2;
			message2.message = "Oh hey!";
			message2.created = new DateTime(2016, 07, 12, 06, 30, 21);
			_messages.Create(chat1, message2);
			return Ok();
		}


		#region Login/Register Endpoints
		[Route("api/users/login")]
		[HttpPost]
		public IHttpActionResult Login([FromBody]UserLoginRequest user)
		{
			try
			{
				if (user == null || user.RequiredFieldsAreNullOrBlank)
				{
					return BadRequest("Some or all of the data required for user registration is blank or missing. Please try again.");
				}
				var result = _users.Login(user);

				if (result.Status == RepositoryActionStatus.Updated)
				{
					var authenticatedUser = _userFactory.CreateAuthenticatedUser(result.Entity);
					var response = _responseFactory.CreateResponse(authenticatedUser);
					return Ok(response);
				}
				else if (result.Status == RepositoryActionStatus.NotFound)
				{
					return NotFound(); //user was not found or passwords did not match
				}

				return BadRequest();
			}
			catch (Exception ex)
			{
				return InternalServerError();
			}
		}

		[Route("api/users/register")]
		[HttpPost]
		public IHttpActionResult Register([FromBody] UserRegistrationRequest user)
		{
			try
			{
				if (user == null || user.RequiredFieldsAreNullOrBlank)
				{
					return BadRequest("Some or all of the data required for user registration is blank or missing.");
				}
				if (user.PasswordsDoNotMatch)
				{
					return BadRequest("Your passwords do not match. Please try again.");
				}
				if (_users.FindUserByEmail(user.email) != null)
				{
					return BadRequest("A user with this email already exists. Please try again.");
				}

				var u = _userFactory.CreateUser(user, ref _users);
				var result = _users.Register(u);

				if (result.Status == RepositoryActionStatus.Created)
				{
					var newUser = _userFactory.CreateAuthenticatedUser(result.Entity);
					var response = _responseFactory.CreateResponse(newUser);
					return Ok(response);

				}

				return BadRequest();
			}
			catch (Exception ex)
			{
				return InternalServerError();
			}
		}
		#endregion

		#region User Profile Endpoints
		[Route("api/users/me")]
		[HttpGet]
		public IHttpActionResult View()
		{
			if (Request.Headers.Contains("Authorization"))
			{
				try
				{
					string token = Request.Headers.GetValues("Authorization").First();

					JwtSecurityToken jwt = new JwtSecurityToken(token);

					User authorizedUser = _users.FindUserByToken(jwt);

					if (authorizedUser == null)
					{
						return Unauthorized();
					}
					else
					{
						var response = _responseFactory.CreateResponse(authorizedUser);
						return Ok(response);			
					}
				}
				catch (Exception ex)
				{
					return InternalServerError();
				}
			}
			return Unauthorized();
		}

		[Route("api/users/me")]
		[HttpPut]
		public IHttpActionResult Edit() 
		{
			if (Request.Headers.Contains("Authorization"))
			{
				string token = Request.Headers.GetValues("Authorization").First();

				JwtSecurityToken jwt = new JwtSecurityToken(token);
				
				User authorizedUser = _users.FindUserByToken(jwt);
				try
				{
					if (authorizedUser == null)
					{
						return Unauthorized();
					}

					var user = _userFactory.CreateUser(authorizedUser);

					var result = _users.Update(user);
					if (result.Status == RepositoryActionStatus.Updated)
					{
						var response = _responseFactory.CreateResponse(result.Entity);
						return Ok(response);	
					}
					else if (result.Status == RepositoryActionStatus.NotFound)
					{
						return NotFound();
					}


					return BadRequest();
				}
				catch (Exception ex)
				{
					return InternalServerError();
				}
			}
			return Unauthorized();
		}
		#endregion
		
		#region Chat Endpoints
		/// <summary>
		/// Lists all chats for a given user based on authorization token
		/// </summary>
		/// <todo>Add pagination requirements</todo>
		/// <returns></returns>
		[Route("api/chats")]
		[HttpGet]
		public IHttpActionResult ListChats(string q = "", string page = "", Nullable<int> limit = null)
		{
			if (Request.Headers.Contains("Authorization"))
			{
				try
				{
					string token = Request.Headers.GetValues("Authorization").First();

					JwtSecurityToken jwt = new JwtSecurityToken(token);
					User authorizedUser = _users.FindUserByToken(jwt);
					if (authorizedUser == null)
					{
						return Unauthorized();
					}
					else
					{
						var chats = _chats.List(authorizedUser);
						List<ChatResponse> chatResponses = new List<ChatResponse>();
						Message lastMessage;
						User otherChatter;
						//get the user and last message for every chat and create a response
						foreach (Chat c in chats)
						{
							otherChatter = _users.FindUserByEmail(c.user_email);
							lastMessage = _messages.FindLastMessageByChat(c);
							chatResponses.Add(new ChatResponse
							{
								id = c.id,
								user_id = c.user_id,
								name = c.name,
								created = c.created,
								user = new UserLite { id=otherChatter.id,name=otherChatter.name },
								last_message = lastMessage
							});
						}
						return Ok(_responseFactory.CreateResponse(chatResponses));
						//return Ok(chats.ToList().Select(c => _chatFactory.CreateChat(c)));
					}
				}
				catch (Exception)
				{
					return InternalServerError();
				}
			}
			return Unauthorized();
		}
		/// <summary>
		/// Inserts a single chat into the repository
		/// </summary>
		/// <param name="chat"></param>
		/// <returns></returns>
		[Route("api/chats/")]
		[HttpPost]
		public IHttpActionResult Create([FromBody] ChatRequest chat, string q="", string page="", Nullable<int> limit=null) //needs authorization token to tie into user
		{
			if (Request.Headers.Contains("Authorization"))
			{
				try
				{
					string token = Request.Headers.GetValues("Authorization").First();

					JwtSecurityToken jwt = new JwtSecurityToken(token);
					User authorizedUser = _users.FindUserByToken(jwt);
					if (authorizedUser == null)
					{
						return Unauthorized();
					}

					var c = _chatFactory.CreateChat(chat, authorizedUser, ref _chats);
					var result = _chats.Create(authorizedUser, c);

					if (result.Status == RepositoryActionStatus.Created)
					{
						var newChat = _chatFactory.CreateChat(result.Entity);
						var chatResponse = new ChatResponse
						{
							id = c.id,
							user_id = c.user_id,
							name = c.name,
							created = c.created,
							user = new UserLite { id = authorizedUser.id, name = authorizedUser.name },
							last_message = null
						};
						return Ok(_responseFactory.CreateResponse(chatResponse));
					}
					
					return BadRequest();
				}
				catch (Exception ex)
				{
					return InternalServerError();
				}
			}
			return Unauthorized();
		}
		#endregion

		#region Message Endpoints
		/// <summary>
		/// Lists all the messages for a given chat for an authenticated user.
		/// </summary>
		/// <returns></returns>
		[Route("")]
		[Route("api/chats/{chat_id}/messages/{page?}/{limit?}")]
		[HttpGet]
		public IHttpActionResult List(int chat_id, string page = "1", int limit = 10)//needs to respect pagination
		{
			if (Request.Headers.Contains("Authorization"))
			{
				try
				{
					string token = Request.Headers.GetValues("Authorization").First();

					JwtSecurityToken jwt = new JwtSecurityToken(token);
					User authorizedUser = _users.FindUserByToken(jwt);
					if (authorizedUser == null)
					{
						return Unauthorized();
					}
					else
					{
						var messages = _messages.List(_chats.FindChatById(chat_id,authorizedUser).Entity);
						List<MessageResponse> messageResponses = new List<MessageResponse>();
						User otherChatter;
						//get the user and last message for every chat and create a response
						foreach (Message m in messages)
						{
							otherChatter = _users.FindUserByEmail(m.user_email);
							messageResponses.Add(new MessageResponse
							{
								id = m.id,
								chat_id = m.chat_id,
								user_id = m.user_id,
								message = m.message,
								created = m.created,
								user = new UserLite { id = otherChatter.id, name = otherChatter.name },
							});
						}
						return Ok(_responseFactory.CreateResponse(messageResponses));
					}
				}
				catch (Exception ex)
				{
					return InternalServerError();
				}
			}
			return Unauthorized();
		}

		[Route("api/chats/{chat_id}/messages")]
		[HttpPost]
		public IHttpActionResult Create(int chat_id,[FromBody] MessageRequest message) //needs to respect pagination
		{
			if (Request.Headers.Contains("Authorization"))
			{
				try
				{
					string token = Request.Headers.GetValues("Authorization").First();

					JwtSecurityToken jwt = new JwtSecurityToken(token);
					User authorizedUser = _users.FindUserByToken(jwt);
					if(authorizedUser == null)
					{
						return Unauthorized();
					}
					if (message == null)
					{
						return BadRequest("Your message is empty.  Please try again.");
					}
					var associatedChat = _chats.FindChatById(chat_id,authorizedUser); //verify it belongs to the user
					if (associatedChat.Entity != null)
					{
						var m = _messageFactory.CreateMessage(message, associatedChat.Entity, authorizedUser, ref _messages);

						var result = _messages.Create(associatedChat.Entity, m);

						if (result.Status == RepositoryActionStatus.Created)
						{
							var liteAuthorizedUser = new UserLite { id = authorizedUser.id, name=authorizedUser.name };
							var newMessageResponse = _messageFactory.CreateMessageResponse(result.Entity, liteAuthorizedUser);
							var response = _responseFactory.CreateResponse(newMessageResponse);
							return Ok(response);
						}
					}

					return BadRequest("You do not have access to create a message for the specified chat_id");
				}
				catch (Exception ex)
				{
					return InternalServerError();
				}
			}
			return Unauthorized();
		}
		#endregion

	}
}
