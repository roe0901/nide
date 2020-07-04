using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public class Common
    {
        public static bool HttpGet(string url, out string reslut, int Type)
        {
            try
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(url.Trim()));
                httpReq.Method = "GET";
                if (Type == 1)
                {
                    httpReq.Headers.Add("auth", "token eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJPaFNoZW5naHVvIiwidXNhZ2UiOiJsb2dpbiIsInVzZXJfaWQiOjkzMzE1MywiZXhwIjoxNjA4Mjc4NjI5LjMyMTU4N30.4jApDIWDOk934Y73GDfGT3-peeMjZjZT58U5fKoNEDA");
                }
                else
                {
                    httpReq.Headers.Add("auth", "token eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJPaFNoZW5naHVvIiwidXNhZ2UiOiJsb2dpbiIsInVzZXJfaWQiOjkzMzEyMiwiZXhwIjoxNjA4Mjk2ODk2LjIwNjQ2M30.UJI-tM9FrawpHimS4eCEfCwLrGXUZW4D5uI_oUf0mbw");
                }
                WebResponse webResponse = httpReq.GetResponse();
                HttpWebResponse httpWebResponse = (HttpWebResponse)webResponse;
                Stream stream = httpWebResponse.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                reslut = reader.ReadToEnd();
                reader.Close();
                webResponse.Close();
                return true;
            }
            catch (Exception ex)
            {
                reslut = ex.Message;
                return false;
            }
        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        public string ParseToString(IDictionary<string, string> parameters)
        {
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }

        public static Dictionary<String, Object> ToMap(Object o)
        {
            Dictionary<String, Object> map = new Dictionary<string, object>();

            Type t = o.GetType();

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();

                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke(o, new Object[] { }));
                }
            }

            return map;

        }
    }
}
