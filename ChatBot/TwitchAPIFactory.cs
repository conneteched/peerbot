﻿ using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Api;

namespace ChatBot
{
    public class TwitchAPIFactory
    {
        public static TwitchAPI GetAPI()
        {
            var api = new TwitchAPI();
            api.Settings.ClientId = Config.ClientID;
            api.Settings.Secret = Config.Secret; // App Secret is not an Accesstoken

            return api;
        }
    }
}
