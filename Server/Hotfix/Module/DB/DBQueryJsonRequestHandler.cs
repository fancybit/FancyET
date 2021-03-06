﻿using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.DB)]
	public class DBQueryJsonRequestHandler : AMRpcHandler<DBQueryJsonRequest, DBQueryJsonResponse>
	{
		protected override async void Run(Session session, DBQueryJsonRequest message, Action<DBQueryJsonResponse> reply)
		{
			DBQueryJsonResponse response = new DBQueryJsonResponse();
			try
			{
				DBCacheComponent dbCacheComponent = Game.Scene.GetComponent<DBCacheComponent>();
				List<Component> components = await dbCacheComponent.GetJson(message.CollectionName, message.Json);

				response.Components = components;

				if (message.NeedCache)
				{
					foreach (Component component in components)
					{
						dbCacheComponent.AddToCache(component, message.CollectionName);
					}
				}

				reply(response);
			}
			catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}