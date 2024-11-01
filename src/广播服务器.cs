using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Net.Sockets;
using System.Net;

namespace CyberWin.Work.Qjcx.远程部署控制
{
    public partial class 广播服务器 : Form
    {

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        public 广播服务器()
        {
            InitializeComponent();
        }

        private void 广播服务器_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入广播信息";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(96, 200);
            this.button1.Name = "button1";
            this.button1.TabIndex = 2;
            this.button1.Text = "发送";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.AutoSize = false;
            this.textBox1.Location = new System.Drawing.Point(32, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(240, 112);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "textBox1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(128)), ((System.Byte)(255)), ((System.Byte)(255)));
            this.label2.Location = new System.Drawing.Point(24, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 40);
            this.label2.TabIndex = 4;
            this.label2.Text = "本程序由辽宁科技大学玄魂制作，QQ717532978";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }
        #endregion
        private void button1_Click(object sender, System.EventArgs e)
        {
            //只能用UDP协议发送广播，所以ProtocolType设置为UDP
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //让其自动提供子网中的IP地址
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, 19877);
            //设置broadcast值为1，允许套接字发送广播信息
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            //将发送内容转换为字节数组
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(this.textBox1.Text);
            //向子网发送信息
            socket.SendTo(bytes, iep);
            socket.Close();

        }
    }
}