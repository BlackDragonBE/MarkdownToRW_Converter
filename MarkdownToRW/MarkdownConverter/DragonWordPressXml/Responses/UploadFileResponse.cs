using System;
using System.Collections.Generic;
using System.Text;

namespace DragonMarkdown.DragonWordPressXml.Responses
{
    // https://codex.wordpress.org/XML-RPC_WordPress_API/Media

    public struct UploadFileResponseStruct
    {
        public int Id; //id
        public string File; //file (name)
        public string Url; //url
        public string FileType; //type
    }
    
    public class UploadFileResponse
    {
        public UploadFileResponseStruct FileResponseStruct;
    }
}
