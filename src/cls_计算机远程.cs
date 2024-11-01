using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace CyberWin.Work.Qjcx.远程部署控制
{
    class cls_计算机远程
    {
        private  string cyber_remote_port = "19875";
        private string IP;
        private string remote_url;

        public cls_计算机远程(string IP)
        {
            this.IP = IP;
            this.remote_url = "http://" + this.IP + ":" + this.cyber_remote_port + "/cyberwinsvr";
        }

        public bool is远程服务开启()
        {
            IPAddress ip = IPAddress.Parse(this.IP);
            IPEndPoint point = new IPEndPoint(ip, int.Parse(this.cyber_remote_port));
            try
            {
                TcpClient tcp = new TcpClient();
                tcp.Connect(point);
                //MessageBox.Show("端口打开");
                return true; //"在线";
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;// "离线";
            }
        }

        private string buildAction(string actionname)
        {
            return this.remote_url + "?action=" + actionname;
        }
        public string 计算机_关机()
        {
            return OpenReadWithHttps(buildAction("shutdown"), "", "GB2312");
        }
        public string 计算机_重启()
        {
            return OpenReadWithHttps(buildAction("restart"), "", "GB2312");
        }
        ///<summary>

        //获取MAC
        //http://127.0.0.1:19875/cyberwinsvr?action=getComputerMAC

        ///</summary>

        ///<param name="URL">url地址</param>

        ///<param name="strPostdata">发送的数据</param>

        ///<returns></returns>
 
        public  string getMac()
        {
            return OpenReadWithHttps(buildAction("getComputerMAC"), "", "GB2312");
        }
        public string get计算机名称()
        {
            return OpenReadWithHttps(buildAction("getComputerName"), "", "GB2312");
        }

        public string set计算机名称(string 新计算机名称)
        {
            return OpenReadWithHttps(buildAction("setComputerName") + "&&param1=" + 新计算机名称, "", "GB2312");
        }

        public string get计算机状态()
        {
            return OpenReadWithHttps(buildAction("getLive"), "", "GB2312");
        }


        ///<summary>

        ///采用https协议访问网络

        ///</summary>

        ///<param name="URL">url地址</param>

        ///<param name="strPostdata">发送的数据</param>

        ///<returns></returns>

        private  static string OpenReadWithHttps(string URL, string strPostdata, string strEncoding)
        {
            try
            {
                Encoding encoding = Encoding.Default;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

                request.Method = "post";

                request.Accept = "text/html, application/xhtml+xml, */*";

                request.ContentType = "application/x-www-form-urlencoded";

                byte[] buffer = encoding.GetBytes(strPostdata);

                request.ContentLength = buffer.Length;

                request.GetRequestStream().Write(buffer, 0, buffer.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(strEncoding)))
                {

                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "cyber_empty";
            }
        }

    }
}
