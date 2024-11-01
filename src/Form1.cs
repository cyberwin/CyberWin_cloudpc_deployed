using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;

namespace CyberWin.Work.Qjcx.远程部署控制
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public static string GetUrltoHtml(string Url, string type)
        {

            try
            {

                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);

                // Get the response instance.

                System.Net.WebResponse wResp = wReq.GetResponse();

                System.IO.Stream respStream = wResp.GetResponseStream();

                // Dim reader As StreamReader = New StreamReader(respStream)

                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                {
                    return reader.ReadToEnd();
                }

            }

            catch (System.Exception ex)
            {

                //errorMsg = ex.Message;

            }

            return "";

        }
        ///<summary>

        ///采用https协议访问网络

        ///</summary>

        ///<param name="URL">url地址</param>

        ///<param name="strPostdata">发送的数据</param>

        ///<returns></returns>

        public string OpenReadWithHttps(string URL, string strPostdata, string strEncoding)
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = OpenReadWithHttps(this.textBox1.Text,this.textBox2.Text,"GB2312");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = OpenReadWithHttps(this.textBox1.Text, this.textBox2.Text, "GB2312");
        }
    }
}