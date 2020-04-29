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

        public static string SendGridApiKey { get; set; } = Environment.GetEnvironmentVariable("SendGridAp");
            public static string EmailConfirmationTemplateId { get; set; } = "d-12425b91466e46149de263bc746efef4";
            public static string PasswordResetEmailTemplateId { get; set; } = "d-ebcd2abece194f8ca0bfce98b822e5f2";
            public static string PaswordSendTemplate { get; set; } = "d-d63068bab910495ea6b93191b14e45f1";
            public static string SenderEmailAddress { get; set; } = "aurimas.valauskas@ktu.edu";
            public static string SenderEmailAddressName { get; set; } = "Aurimas";
           

    }
    
}
