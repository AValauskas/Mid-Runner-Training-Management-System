using CodeMash.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class Settings
    {
            public static Guid ProjectId { get; set; } = Guid.Parse("813cac02-a70f-48c7-899c-67fc84676b64");
            public static string ApiKey { get; set; } = "KC6GsNe9v7g3U1jQPIqHCOfWWyplQB6A";
            public static CodeMashClient Client { get; set; } = new CodeMashClient(ApiKey, ProjectId);



       
    }
}
