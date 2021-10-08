﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Models
{
    public class RecentlyPlayedGameDTO
    {
        public class Rootobject
        {
            public Response response { get; set; }
        }

        public class Response
        {
            public int total_count { get; set; }
            public Game[] games { get; set; }
        }

        public class Game
        {
            public int appid { get; set; }
            public string name { get; set; }
            public int playtime_2weeks { get; set; }
            public int playtime_forever { get; set; }
            public string img_icon_url { get; set; }
            public string img_logo_url { get; set; }
            public int playtime_windows_forever { get; set; }
            public int playtime_mac_forever { get; set; }
            public int playtime_linux_forever { get; set; }
        }
    }


}
