using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace ArmA.Studio
{
    public static class UpdateHelper
    {
        public const string CONST_NORMAL_UPDATEURL = "https://x39.io/api.php?action=projects&project=ArmA.Studio";
        public const string CONST_INDEV_PROJECTURL = "https://ci.appveyor.com/api/projects/X39/arma-studio";
        public const string CONST_INDEV_ARTIFACTSURL = "https://ci.appveyor.com/api/buildjobs/{0}/artifacts";


        public struct DownloadInfo
        {
            public bool available;
            public string name;
            public string link;
            public Version version;
        }

        public static async Task<DownloadInfo> GetDownloadInfoAsync()
        {
            if (ConfigHost.App.UseInDevBuild)
            {
                return await GetInDevDownloadInfoAsync();
            }
            else
            {
                return await GetReleaseDownloadInfoAsync();
            }
        }

        public static async Task<DownloadInfo> GetReleaseDownloadInfoAsync()
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(CONST_NORMAL_UPDATEURL))
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
        public static async Task<string> DownloadFileAsync(DownloadInfo info, IProgress<Tuple<long, long>> prog)
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

        public static async Task<DownloadInfo> GetInDevDownloadInfoAsync()
        {
            using (var client = new HttpClient())
            {
                string jobid;
                string commitId;
                DateTime date;
                using (var response = await client.GetAsync(CONST_INDEV_PROJECTURL))
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic doc = JObject.Parse(responseString);
                    jobid = Convert.ToString(doc.build.jobs[0].jobId);
                    commitId = Convert.ToString(doc.build.commitId);
                    date = Convert.ToDateTime(doc.build.committed);
                }

                if (date <= App.BuildDateTime)
                {
                    return default(DownloadInfo);
                }

                string fileName;
                using (var response = await client.GetAsync(string.Format(CONST_INDEV_ARTIFACTSURL, jobid)))
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic doc = JArray.Parse(responseString);
                    fileName = Convert.ToString(doc[0].fileName);
                }
                var tmpDate = new DateTime(2000, 1, 1);
                var tmpDate2 = new DateTime(date.Year, date.Month, date.Day);
                return new DownloadInfo()
                {
                    available = true,
                    link = string.Concat(string.Format(CONST_INDEV_ARTIFACTSURL, jobid), '/', fileName),
                    name = commitId,
                    version = new Version(App.CurrentVersion.Major, App.CurrentVersion.Minor, (int)(date - tmpDate).TotalDays, (int)(date - tmpDate2).TotalSeconds)
                };
            }
        }

    }
}