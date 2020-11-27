using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LitJson;

namespace Memo.tools
{
    public class JsonHelper
    {
        public static readonly Encoding Encoding = new UTF8Encoding(false);
        private static string g_tempPath = string.Empty;
        private static JsonData g_tempData;

        public static JsonData ReadJson(string fullPath)
        {
            if (g_tempPath.Equals(fullPath))
            {
                return g_tempData;
            }

            if (!File.Exists(fullPath))
            {
                return null;
            }

            g_tempPath = fullPath;
            string json = File.ReadAllText(fullPath, Encoding);
            g_tempData = JsonMapper.ToObject(json);

            return g_tempData;
        }

        public static T ReadJson<T>(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return default;
            }

            g_tempPath = fullPath;
            string json = File.ReadAllText(fullPath, Encoding);
            return JsonMapper.ToObject<T>(json);
        }

        public static void WriteJson(string path, object obj)
        {
            if (null == obj)
            {
                return;
            }

            var sb = new StringBuilder();
            var jw = new JsonWriter(sb) {PrettyPrint = true};
            JsonMapper.ToJson(obj, jw);
            WriteStringToFile(path, Regex.Unescape(sb.ToString()));
        }

        public static void WriteJson(string path, JsonData objData)
        {
            if (null == objData)
            {
                return;
            }

            var sb = new StringBuilder();
            var jw = new JsonWriter(sb) {PrettyPrint = true};
            objData.ToJson(jw);
            WriteStringToFile(path, Regex.Unescape(sb.ToString()));
        }

        public static bool WriteStringToFile(string path, string text)
        {
            var strFullPath = "";
            try
            {
                strFullPath = Path.GetFullPath(path);

                //如果所在目录不存在，那么创建它
                string strDir = Path.GetDirectoryName(strFullPath);
                if (string.IsNullOrEmpty(strDir))
                {
                    return false;
                }

                if (!Directory.Exists(strDir))
                {
                    Directory.CreateDirectory(strDir);
                }

                var stream = new FileStream(path, FileMode.Create);
                var writer = new StreamWriter(stream, Encoding);
                writer.Write(text);
                writer.Close();
                stream.Close();
                return true;
            }
            catch (Exception exception)
            {
                var strMsg = $"WriteStringToFile fail, file=[{path}]->[{strFullPath}], Exception msg:[{exception}]";
                Console.WriteLine(strMsg);
                return false;
            }
        }
    }
}