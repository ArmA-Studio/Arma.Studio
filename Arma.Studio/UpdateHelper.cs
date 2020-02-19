using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Arma.Studio
{
    public static class UpdateHelper
    {
        public const string CONST_RELEASESURL = "https://api.github.com/repos/ArmA-Studio/Arma.Studio/releases";


        public struct DownloadInfo
        {
            public bool available;
            public string name;
            public string link;
        }

        public static async Task<DownloadInfo> GetDownloadInfoAsync()
        {
            return await GetInDevDownloadInfoAsync();
            //if (ConfigHost.App.UseInDevBuild)
            //{
            //    return await GetInDevDownloadInfoAsync();
            //}
            //else
            //{
            //    return await GetReleaseDownloadInfoAsync();
            //}
        }
        public static async Task<string> DownloadFileAsync(DownloadInfo info, Action<long, long> prog)
        {
            return await Task.Run(async () =>
            {
                using (var client = new HttpClient())
                using (var response = await client.GetAsync(info.link, HttpCompletionOption.ResponseHeadersRead))
                using (var responseContent = response.Content)
                {
                    string tmpFile = Path.Combine(App.TempPath, info.name);
                    if (!Directory.Exists(Path.GetDirectoryName(tmpFile)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(tmpFile));
                    }

                    long curLen = 0;
                    using (var fStream = File.Create(tmpFile))
                    using (var hStream = await responseContent.ReadAsStreamAsync())
                    {
                        while (true)
                        {
                            byte[] buffer = new byte[1024 * 16];
                            int readBytes = hStream.Read(buffer, 0, buffer.Length);
                            if (readBytes == 0)
                            {
                                break;
                            }

                            fStream.Write(buffer, 0, readBytes);
                            curLen += readBytes;
                            prog(curLen, responseContent.Headers.ContentLength.Value);
                        }
                    }
                    return tmpFile;
                }
            });
        }

        public static async Task<DownloadInfo> GetInDevDownloadInfoAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"arma.studio/{typeof(App).Assembly.GetName().Version.Major}.{typeof(App).Assembly.GetName().Version.Minor}.{typeof(App).Assembly.GetName().Version.Revision}+{App.GitCommitId}");
                // Get latest release infos
                string commitid = null;
                string asset = null;
                using (var response = await client.GetAsync(CONST_RELEASESURL))
                using (var content = response.Content)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        String error = "Update failed for unknown reason.";
                        try
                        {
                            error = await content.ReadAsStringAsync();
                        }
#pragma warning disable CA1031 // Do not catch general exception types
                        catch { } // Ingored as we just do not know if we get anything back at all. Thus just try to parse as string and throw it into the exception.
#pragma warning restore CA1031 // Do not catch general exception types
                        throw new Exception(error.Length > 1024 * 8 ? error.Substring(0, 1025 * 8) : error);
                    }
                    var responseString = await content.ReadAsStringAsync();
                    dynamic obj = JToken.Parse(responseString);
                    commitid = obj[0].target_commitish;
                    foreach(var it in obj[0].assets)
                    {
                        if (it.name == "bin.zip")
                        {
                            asset = it.browser_download_url;
                            break;
                        }
                    }
                }
                if (String.IsNullOrWhiteSpace(commitid) || String.IsNullOrWhiteSpace(asset))
                {
                    return default;
                }

                if (commitid == App.GitCommitId)
                {
                    return default(DownloadInfo);
                }
                return new DownloadInfo()
                {
                    available = true,
                    link = asset,
                    name = commitid
                };
            }
        }
    }
}
