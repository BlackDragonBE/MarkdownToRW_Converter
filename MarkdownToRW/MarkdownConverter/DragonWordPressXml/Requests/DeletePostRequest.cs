using System;
using System.Collections.Generic;
using System.Text;

namespace DragonMarkdown.DragonWordPressXml.Requests
{
    /// <summary>
    /// Deletes a page, blog or file by id
    /// </summary>
    public class DeletePostRequest
    {
        public string RequestUrl => "wp.deletePost";
        public int PostId; // postid

        public DeletePostRequest(int id)
        {
            PostId = id;
        }
    }
}
