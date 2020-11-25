using LitJson;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Memo.tools
{
    public class JsonHelper
    {
        public static readonly Encoding Encoding = new UTF8Encoding(false);
        private static string s_TempPath = string.Empty;
        private static JsonData s_TempData;

        public static JsonData ReadJson(string fullPath)
        {
            if (s_TempPath.Equals(fullPath))
            {
                return s_TempData;
            }

            if (!File.Exists(fullPath))
            {
                return null;
            }

            s_TempPath = fullPath;
            string json = File.ReadAllText(fullPath, Encoding);
            s_TempData = JsonMapper.ToObject(json);

            return s_TempData;
        }

        public static T ReadJson<T>(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return default;
            }

            s_TempPath = fullPath;
            string json = File.ReadAllText(fullPath, Encoding);
            return JsonMapper.ToObject<T>(json);
        }

        public static void WriteJson(string path, object obj)
        {
            if (null == obj)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            JsonWriter jw = new JsonWriter(sb) { PrettyPrint = true };
            JsonMapper.ToJson(obj, jw);
            WriteStringToFile(path, Regex.Unescape(sb.ToString()));
        }

        public static void WriteJson(string path, JsonData objData)
        {
            if (null == objData)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            JsonWriter jw = new JsonWriter(sb) { PrettyPrint = true };
            objData.ToJson(jw);
            WriteStringToFile(path, Regex.Unescape(sb.ToString()));
        }

        public static bool WriteStringToFile(string path, string text)
        {
            string strFullPath = "";
            try
            {
                strFullPath = Path.GetFullPath(path);

                //如果所在目录不存在，那么创建它
                string strDir = Path.GetDirectoryName(strFullPath);
                if (!Directory.Exists(strDir))
                {
                    Directory.CreateDirectory(strDir);
                }

                FileStream stream = new FileStream(path, FileMode.Create);
                StreamWriter writer = new StreamWriter(stream, Encoding);
                writer.Write(text);
                writer.Close();
                stream.Close();
                return true;
            }
            catch (Exception exception)
            {
                string strMsg = string.Format("WriteStringToFile fail, file=[{0}]->[{1}], Exception msg:[{2}]"
                    , path
                    , strFullPath
                    , exception.ToString());

                Console.WriteLine(strMsg);
                return false;
            }
        }
    }
}
