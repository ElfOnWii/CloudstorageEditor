using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace CloudstorageEditorLauncher
{
    class Listener
    {
        private static int _port = 5595;
        private static bool _isInUse = false;

        public static void Listen(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.Contains("port"))
                {
                    var port = arg.Split('=')[1];
                    _port = int.Parse(port);
                }
            }

            try
            {
                using (var client = new TcpClient("127.0.0.1", _port))
                    _isInUse = true;
            }
            catch { }

            if (_isInUse)
            {
                Console.WriteLine($"Port is in use!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            var server = new HttpListener();

            try
            {
                server.Prefixes.Add($"http://127.0.0.1:{_port}/");
                server.Start();
                Console.WriteLine($"Listening On Port {_port}");
            }
            catch
            {
                Console.WriteLine($"Run program as admin!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            while (true)
            {
                var context = server.GetContext();

                if (context.Request.Url.PathAndQuery == "/fortnite/api/cloudstorage/system")
                {
                    var data = JsonConvert.SerializeObject(new
                    {
                        uniqueFilename = "3460cbe1c57d4a838ace32951a4d7171",
                        filename = "DefaultGame.ini",
                        hash = "603E6907398C7E74E25C0AE8EC3A03FFAC7C9BB4",
                        hash256 = "973124FFC4A03E66D6A4458E587D5D6146F71FC57F359C8D516E0B12A50AB0D9",
                        length = File.ReadAllText("DefaultGame.ini").Length,
                        contentType = "application/octet-stream",
                        uploaded = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        storageType = "S3",
                        doNotCache = false
                    });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }

                if (context.Request.Url.PathAndQuery == "/fortnite/api/cloudstorage/system/config")
                {
                    var data = JsonConvert.SerializeObject(new
                    {
                    });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/cloudstorage/user"))
                {
                    var data = JsonConvert.SerializeObject(new
                    {
                    });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }

                if (context.Request.Url.PathAndQuery == "/fortnite/api/cloudstorage/system/3460cbe1c57d4a838ace32951a4d7171")
                {
                    var defaultGame = File.ReadAllBytes("DefaultGame.ini");

                    context.Response.ContentType = "application/octet-stream";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = defaultGame.Length;
                    context.Response.OutputStream.Write(defaultGame, 0, defaultGame.Length);
                }
            }
        }
    }
}
