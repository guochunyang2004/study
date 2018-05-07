using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace App.Core
{
    public class WebHander
    {

        private Encoding _Encoding = null;

        public WebHander()
        {

        }
        public WebHander(Encoding encode)
        {
            _Encoding = encode;
        }

        #region  读取页面详细信息
        /**/
        ///   <summary>    ///  读取页面详细信息    
        ///   </summary>      
        /// <param name="Url"> 需要读取的地址 </param>     
        ///   <param name="encoding"> 读取的编码方式 </param>      
        ///   <returns></returns>      
        public string GetPage(string Url, System.Text.Encoding encoding)
        {
            if (Url.Equals("about:blank")) return null; ;
            if (!Url.StartsWith("http://") && !Url.StartsWith("https://")) { Url = "http://" + Url; }
            int dialCount = 0;

        loop:
            StreamReader sreader = null;
            string result = string.Empty;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                // httpWebRequest.Timeout = 20;   
                #region  关键参数，否则会取不到内容　Important Parameters,else get nothing.
                httpWebRequest.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                httpWebRequest.Accept = "*/*";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                #endregion
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    sreader = new StreamReader(httpWebResponse.GetResponseStream(), encoding);
                    char[] cCont = new char[256];
                    int count = sreader.Read(cCont, 0, 256);
                    while (count > 0)
                    {  //  Dumps the 256 characters on a string and displays the string to the console.    
                        String str = new String(cCont, 0, count);
                        result += str;
                        count = sreader.Read(cCont, 0, 256);
                    }
                }
                if (null != httpWebResponse) { httpWebResponse.Close(); }
                return result;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure) { dialCount++;// ReDial(); 
                }
                if (dialCount < 5) { goto loop; }
                return null;
            }
            finally { if (sreader != null) { sreader.Close(); } }
        }

        public string GetPage(string Url)
        {
            return GetPage(Url, _Encoding);
        }
        #endregion


        

        public string PostPage(string url, string postdate, Encoding encode)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                //request.UserAgent = "Baiduspider(http://www.baidu.com/search/spider.htm)";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.01; Windows NT 5.0)";
                request.Timeout = 10000;

                #region 填充要post的内容

                if (postdate.Length > 0)
                {
                    request.ContentType = "application/x-www-form-urlencoded";

                    request.Method = "Post";
                    byte[] data = encode.GetBytes(postdate);
                    request.ContentLength = data.Length;

                    Stream requestStream = request.GetRequestStream();

                    requestStream.Write(data, 0, data.Length);

                    requestStream.Close();
                }
                #endregion

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;


                Stream stream;
                if (response.ContentEncoding == "gzip") // 注意内容编码
                {
                    stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    stream = response.GetResponseStream();
                }


                StreamReader reader = new StreamReader(stream, encode);
                string text = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return text;

            }
            catch //(Exception ex)
            {
                //LogHelper.WriteException(string.Format(" 采集过程出现异常，追踪地址：{0}?{1}", url, postdate), ex);
            }
            return string.Empty;
        }

        public string PostPage(string url, string pastData)
        {
            return PostPage(url, pastData, _Encoding);
        }

    }
}
