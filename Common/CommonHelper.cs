using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public class CommonHelper
    {
        private static byte[] _Key = { 0x73, 0x75, 0x6e, 0x68, 0x65, 0x61, 0x72, 0x74 };
        private static byte[] _IV = { 0x73, 0x75, 0x6e, 0x68, 0x65, 0x61, 0x72, 0x74 };

        /// <summary>
        /// diary专用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="reslut"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static bool HttpGet(string url, out string reslut, int Type)
        {
            try
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(url.Trim()));
                httpReq.Method = "GET";
                if (Type == 1)
                {
                    httpReq.Headers.Add("auth", "token eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJPaFNoZW5naHVvIiwidXNhZ2UiOiJsb2dpbiIsInVzZXJfaWQiOjkzMzE1MywiZXhwIjoxNjIzNjM0NzA5LjE4ODA4Nn0.jIhLug3GT77P5EqAdb_XKqImAZNqTzzIKbx_uS8g754");
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

        

        #region DES加密解密
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt(string input)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(input);
            des.Key = _Key;
            des.IV = _IV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder result = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                result.AppendFormat("{0:X2}", b);
            }

            return result.ToString();
        }
       
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decrypt(string input)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len = input.Length / 2;
            byte[] inputByteArray = new byte[len];
            for (int i = 0; i < len; i++)
            {
                inputByteArray[i] = (byte)Convert.ToInt32(input.Substring(i * 2, 2), 16);
            }
            des.Key = _Key;
            des.IV = _IV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region RSA 加密解密
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string publickey, string content)
        {

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSADecrypt(string privatekey, string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
        }
        #endregion

        #region 格式转换


        /// <summary>
        /// Unicode转String
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public static string DtToJson(DataTable table)
        {
            try
            {
                string jsonString = "[";
                DataRowCollection drc = table.Rows;
                for (int i = 0; i < drc.Count; i++)
                {
                    jsonString += "{";
                    foreach (DataColumn column in table.Columns)
                    {
                        jsonString += "\"" + column.ColumnName + "\":";
                        if (column.DataType == typeof(DateTime) || column.DataType == typeof(string) || column.DataType == typeof(bool))
                        {
                            jsonString += "\"" + drc[i][column.ColumnName].ToString().Replace("\r", "").Replace("\n", "").Replace("\"", "'") + "\",";
                        }
                        else
                        {
                            jsonString += "\"" + drc[i][column.ColumnName].ToString().Replace("\r", "").Replace("\n", "").Replace("\"", "'") + "\",";
                        }
                    }

                    jsonString = DeleteLast(jsonString) + "},";
                }

                return DeleteLast(jsonString) + "]";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>   
        /// 删除结尾字符   
        /// </summary>   
        /// <param name="str">需要删除的字符</param>   
        /// <returns>完成后的字符串</returns>   
        private static string DeleteLast(string str)
        {
            return str.Length > 1 ? str.Substring(0, str.Length - 1) : str;
        }

        /// <summary>
        /// Json转成DataTable
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>DataTable</returns>
        public static DataTable JsonConvertDataTable(string json)
        {
            try
            {
                if (json.IndexOf("{") == -1) return null;
                return JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 判断是否为中文
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsChinaTxt(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;
            char[] c = text.ToCharArray();
            bool result = true;
            for (int i = 0; i < c.Length; i++)
                if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                {
                    continue;
                }
                else
                {
                    //Response.Write("不是汉字");
                    result = false;
                    break;
                }
            return result;
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumberic(string str)
        {
            if (str == null || str.Length == 0)
                return false;
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] bytestr = ascii.GetBytes(str);
            foreach (byte c in bytestr)
            {
                if (c < 48 || c > 57)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
