using System;
using System.Collections.Generic;
using System.Text;

namespace DragonMarkdown.DragonWordPressXml.Requests
{
    public class GetMediaItemRequest
    {
        public string RequestUrl => "wp.getMediaItem";
        public string Id;

        public GetMediaItemRequest(string id)
        {
            Id = id;
        }
    }
}
