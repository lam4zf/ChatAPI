using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
[assembly: OwinStartup(typeof(ChatAPI.Startup))]

namespace ChatAPI
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseWebApi(WebApiConfig.Register());

		}
	}
}
