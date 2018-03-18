using System;
using System.Collections.Generic;
using System.Text;

namespace DragonMarkdown.DragonWordPressXml.Requests
{
    // https://codex.wordpress.org/XML-RPC_WordPress_API/Users

    public class GetProfileRequest
    {
        public string RequestUrl => "wp.getProfile";
    }
}
