﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyTwitter.JsonTypes
{

    public class Description
    {		
        [JsonProperty("urls")]
		public List<Url> Urls { get; set; }
    }

}
