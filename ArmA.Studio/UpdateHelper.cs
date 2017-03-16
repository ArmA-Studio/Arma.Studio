using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArmA.Studio
{
    public static class UpdateHelper
    {
        //ToDo: Update webpage to provide new API with changelog etc.
        public const string CONST_UPDATEURL = "http://x39.io/api.php?action=projects&project=ArmA.Studio";


        public struct DownloadInfo
        {
            public bool available;
            public string name;
            public string link;
            public Version version;
        }

        public static async Task<DownloadInfo> GetDownloadInfo()
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(CONST_UPDATEURL))
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var conv = new ExpandoObjectConverter();
                dynamic responseObject = JsonConvert.DeserializeObject<ExpandoObject>(responseString, conv);

                if (responseObject.success)
                {
                    var version = responseObject.content.version as string;
                    var ver = Version.Parse(version);
                    var downloads = responseObject.content.download as IEnumerable<dynamic>;
                    if (downloads.Any())
                    {
                        var downloadNode = downloads.First();

                        return new DownloadInfo() { version = ver, available = ver > App.CurrentVersion, link = downloadNode.link, name = downloadNode.name };
                    }
                    else
                    {
                        return new DownloadInfo() { version = ver, available = false };
                    }
                }
                else
                {
                    return default(DownloadInfo);
                }
            }
        }
        public static async Task<string> DownloadFile(DownloadInfo info, IProgress<Tuple<long, long>> prog)
        {
            return await Task.Run(async () =>
            {
                using (var client = new HttpClient())
                using (var response = await client.GetAsync(info.link, HttpCompletionOption.ResponseHeadersRead))
                using (var responseContent = response.Content)
                {
                    var tmpFile = Path.Combine(App.TempPath, info.name);
                    if (!Directory.Exists(Path.GetDirectoryName(tmpFile)))
                        Directory.CreateDirectory(Path.GetDirectoryName(tmpFile));
                    long curLen = 0;
                    using (var fStream = File.Create(tmpFile))
                    using (var hStream = await responseContent.ReadAsStreamAsync())
                    {
                        while (true)
                        {
                            byte[] buffer = new byte[256];
                            var readBytes = hStream.Read(buffer, 0, buffer.Length);
                            if (readBytes == 0)
                                break;
                            fStream.Write(buffer, 0, readBytes);
                            curLen += readBytes;
                            prog.Report(new Tuple<long, long>(curLen, responseContent.Headers.ContentLength.Value));
                        }
                    }
                    return tmpFile;
                }
            });
        }
    }
}