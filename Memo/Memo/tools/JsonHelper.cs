using LitJson;
using System;
using System.IO;
using System.Linq;
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
                return default(T);
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

            string json = JsonMapper.ToJson(obj);
            json = JsonTree(json);
            WriteStringToFile(path, json);
        }

        public static void WriteJson(string path, JsonData objData)
        {
            if (null == objData)
            {
                return;
            }

            string json = objData.ToJson();
            json = JsonTree(json);
            WriteStringToFile(path, json);
        }

        /// <summary>
        /// JSON字符串格式化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonTree(string json)
        {
            json = Regex.Unescape(json);
            int level = 0;
            var jsonArr = json.ToArray();  // Using System.Linq;
            string jsonTree = string.Empty;
            for (int i = 0; i < json.Length; i++)
            {
                char c = jsonArr[i];
                if (level > 0 && '\n' == jsonTree.ToArray()[jsonTree.Length - 1])
                {
                    jsonTree += TreeLevel(level);
                }
                switch (c)
                {
                    case '{':
                    case '[':
                        jsonTree += c + "\n";
                        level++;
                        break;
                    case ',':
                        jsonTree += c + "\n";
                        break;
                    case '}':
                    case ']':
                        jsonTree += "\n";
                        level--;
                        jsonTree += TreeLevel(level);
                        jsonTree += c;
                        break;
                    default:
                        jsonTree += c;
                        break;
                }
            }
            return jsonTree;
        }
        /// <summary>
        /// 树等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static string TreeLevel(int level)
        {
            string leaf = string.Empty;
            for (int t = 0; t < level; t++)
            {
                leaf += "\t";
            }
            return leaf;
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
