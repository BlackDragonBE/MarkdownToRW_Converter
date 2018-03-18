using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DragonMarkdown;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace DragonMarkdown.DragonWordPressXml.Requests
{
    // https://codex.wordpress.org/XML-RPC_WordPress_API/Media

    [SerializeAs(NameStyle = NameStyle.LowerCase, Content = false)]
    public class UploadFileRequestStruct
    {
        public string Name { get; set; } // File name
        public string Type { get; set; }// Mime type
        public byte[] Bits { get; set; } // Base64 bits
        public bool Overwrite { get; set; } // Overwrite original

    }

    public class UploadFileRequest
    {
        //private UploadFileRequestStruct FileRequestStruct;

        public string RequestUrl => "wp.uploadFile";
        public UploadFileRequestStruct FileRequestStruct;

        public UploadFileRequest(string filePath)
        {
            FileRequestStruct = new UploadFileRequestStruct();
            FileRequestStruct.Name = Path.GetFileName(filePath);

            // TODO: replace with mime dictionary entry for generic file uploading
            // FileRequestStruct.Type = "image/" + Path.GetExtension(filePath).Replace(".","");
            FileRequestStruct.Type = MimeMapper.MimeMap[Path.GetExtension(filePath).ToLower()];

            FileRequestStruct.Bits = File.ReadAllBytes(filePath);
            FileRequestStruct.Overwrite = false;
        }

    }
}
