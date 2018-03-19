using System;
using System.Text;
using RestSharp;
using RestSharp.Serializers;
using DragonMarkdown.DragonWordPressXml.Requests;
using DragonMarkdown.DragonWordPressXml.Responses;

namespace DragonMarkdown.DragonWordPressXml
{
    public class DragonWordPressClient
    {
        private readonly WordPressConfig config;

        public DragonWordPressClient(WordPressConfig config)
        {
            this.config = config;
        }

        public GetMediaItemResponse SendMediaItemRequest(GetMediaItemRequest request)
        {
            GetMediaItemResponse response = new GetMediaItemResponse();

            var rpcClient = new XmlRpcRestClient(config.RequestUrl);
            var rpcRequest = new XmlRpcRestRequest(request.RequestUrl) {Method = Method.POST};

            // Add request parameters
            rpcRequest.AddXmlRpcBody(config.BlogID, config.Username, config.Password, request.Id);

            try
            {
                // Get response
                var rpcResponse = rpcClient.Execute<RpcResponseValue<string>>(rpcRequest);

                // Find and fill members in
                XmlMemberSearcher searcher = new XmlMemberSearcher(rpcResponse.Content);
                response.Link = searcher.GetValueOfMember("link");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return response;
        }

        public UploadFileResponse SendUploadFileRequest(UploadFileRequest request)
        {
            UploadFileResponse response = new UploadFileResponse();

            var rpcClient = new XmlRpcRestClient(config.RequestUrl);
            var rpcRequest = new XmlRpcRestRequest(request.RequestUrl) {Method = Method.POST};

            // Add request parameters
            //rpcRequest.XmlSerializer = new XmlRpcSerializer("",false);
            rpcRequest.AddXmlRpcBody(config.BlogID, config.Username, config.Password, request.FileRequestStruct,"true","false");

            try
            {
                // Get response
                var rpcResponse = rpcClient.Execute<RpcResponseValue<string>>(rpcRequest);

                // Find and fill members in
                XmlMemberSearcher searcher = new XmlMemberSearcher(rpcResponse.Content);
                response.FileResponseStruct.Id = Convert.ToInt32(searcher.GetValueOfMember("id"));
                response.FileResponseStruct.Url = searcher.GetValueOfMember("url");
                response.FileResponseStruct.File = searcher.GetValueOfMember("file");
                response.FileResponseStruct.FileType = searcher.GetValueOfMember("type");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return response;

        }

        public DeletePostResponse SendDeletePostRequest(DeletePostRequest request)
        {
            DeletePostResponse response = new DeletePostResponse();

            var rpcClient = new XmlRpcRestClient(config.RequestUrl);
            var rpcRequest = new XmlRpcRestRequest(request.RequestUrl) {Method = Method.POST};

            // Add request parameters
            rpcRequest.AddXmlRpcBody(config.BlogID, config.Username, config.Password, request.PostId);

            try
            {
                // Get response
                var rpcResponse = rpcClient.Execute<RpcResponseValue<string>>(rpcRequest);

                // Find and fill members in
                response.Success = rpcResponse.Data.Value != "false";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return response;
        }

        public GetProfileResponse SendGetProfileRequest(GetProfileRequest request)
        {
            GetProfileResponse response = new GetProfileResponse();

            var rpcClient = new XmlRpcRestClient(config.RequestUrl);
            var rpcRequest = new XmlRpcRestRequest(request.RequestUrl) {Method = Method.POST};

            // Add request parameters
            rpcRequest.AddXmlRpcBody(config.BlogID, config.Username, config.Password);

            try
            {
                // Get response
                var rpcResponse = rpcClient.Execute<RpcResponseValue<string>>(rpcRequest);

                // Find and fill members in
                XmlMemberSearcher searcher = new XmlMemberSearcher(rpcResponse.Content);
                response.Bio = searcher.GetValueOfMember("bio");
                response.DisplayName = searcher.GetValueOfMember("display_name");
                response.Email = searcher.GetValueOfMember("email");
                response.FirstName = searcher.GetValueOfMember("first_name");
                response.LastName = searcher.GetValueOfMember("last_name");
                response.NiceName = searcher.GetValueOfMember("nicename");
                response.Nickname = searcher.GetValueOfMember("nickname");
                response.Url = searcher.GetValueOfMember("url");
                response.UserId = searcher.GetValueOfMember("user_id");
                response.Username = searcher.GetValueOfMember("username");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return response;
        }

        /// <summary>
        /// Prints the content of the rpc request for debugging purposes
        /// </summary>
        /// <param name="rpcRequest"></param>
        private static void PrintRpcRequest(XmlRpcRestRequest rpcRequest)
        {
            var sb = new StringBuilder();
            foreach (var param in rpcRequest.Parameters)
            {
                sb.AppendFormat("{0}: {1}\r\n", param.Name, param.Value);
            }

            Console.Write(sb.ToString());
            Console.Read();
        }

    }
}