using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;

using System.Web;
using System.Net.Sockets;
using System.Diagnostics;


namespace CyberWin.Work.Qjcx.远程部署控制
{
    public partial class frm_computer : Form
    {
        private string PATH_计算机配置信息MAC表;
        private string PATH_计算机配置信息MAC表_备份名称;



        private string vbox_路径;
        private string vbox_备份路径;
        private string vbox_运行路径;

        private string 配置路径=Application.StartupPath+"\\系统配置.ini";

        private DataSet ds = new DataSet();
        private DataSet ds2 = new DataSet();
        private DataSet ds_bak = new DataSet();

        private delegate void cyber_SetPos(int ipos, string vinfo,DataSet v_ds );//代理郭荣华
        private delegate void cyber_客户端加入(string 客户端IP);//代理郭荣华

        public frm_computer()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void frm_computer_Load(object sender, EventArgs e)
        {
            //加载虚拟机配置路径
           // StreamReader sr = new StreamReader(Application.StartupPath + "/vb_path.txt", Encoding.Default);
           // this.vbox_路径 = sr.ReadToEnd().Trim();

           // PATH_计算机配置信息MAC表 =// Application.StartupPath + "/computer.xml";
            //PATH_计算机配置信息MAC表_备份名称= //Application.StartupPath + "/computer_bak.xml";

            this.vbox_路径 = get配置路径("VBOX路径");
            PATH_计算机配置信息MAC表 = get配置路径("计算机保存路径");
            PATH_计算机配置信息MAC表_备份名称 = get配置路径("客户端备份路径");
            this.vbox_备份路径 = get配置路径("VBOX备份路径");
            this.vbox_运行路径 = get配置路径("VirtualBox_runpath");

            if(this.vbox_路径==""){
                加载();
            }else{
                加载();
                button6_Click_2(sender, e);
            }

            //MessageBox.Show("Dpan="+this.get介质("seconddriver") +"系统="+ this.get介质("systemdriver"));
         

            CyberThread = new Thread(new ThreadStart(cy_web));
           CyberThread.Start();

        }
        public string get配置路径(string key)
        {
            CyberSnow.VB.NET.My.Helper.InfoControl.iniFile ir = new CyberSnow.VB.NET.My.Helper.InfoControl.iniFile();
            return ir.GetINI("配置", key, "", this.配置路径).Trim().Replace("{apppath}",Application.StartupPath);
            //return "";
        }

        public string get介质(string key)
        {
            CyberSnow.VB.NET.My.Helper.InfoControl.iniFile ir = new CyberSnow.VB.NET.My.Helper.InfoControl.iniFile();
            return ir.GetINI("Storage", key, "{0}.vmdk", this.配置路径).Trim().Replace("{apppath}", Application.StartupPath);
            //return "";
        }

        public void 加载()
        {
         
            this.ds.ReadXml(PATH_计算机配置信息MAC表); 
            dataGridView1.DataSource = this.ds.Tables[0].DefaultView;
            this.ds_bak.ReadXml(PATH_计算机配置信息MAC表_备份名称);
        }
        public void 保存()
        {
           // DataSet ds;
           //、、 ds = (DataSet)dataGridView1.DataSource;
            ds.WriteXml(PATH_计算机配置信息MAC表);
           // ds.ReadXml(PATH_计算机配置信息MAC表);
           // dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            保存();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            //Application.Run(new Form1());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread fThread = new Thread(new ThreadStart(刷新客户端));
            fThread.Start();
            ds = ds2;
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ds.Tables[0].Clear();
        }

        private void 获取MACToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = this.dataGridView1.SelectedRows[0];
           // MessageBox.Show(dr.Cells[1].Value.ToString());

            cls_计算机远程 CC = new cls_计算机远程(dr.Cells[1].Value.ToString());

           // ds.Tables[0].Rows.Add(i.ToString(), CC.get计算机名称(), CC.getMac(), "aa", CC.get计算机状态());
           // MessageBox.Show(CC.getMac());
            this.dataGridView1.SelectedRows[0].Cells[3].Value = CC.getMac().Trim();

        }


        public void 刷新客户端()
        {
            //DataSet ds2 = new DataSet();
            this.ds2.ReadXml(PATH_计算机配置信息MAC表); 
            //ds2=ds;
            int i = 1;
            int min = 1;
            int max = 255;
            for (i = min; i <max; i++)
            {
               //System.Threading.Thread.Sleep(10);
                //SetTextMesssage(100 * i / 500, i.ToString() + "\r\n");
                //cls_计算机远程
                //this.dataGridView1.Rows.Add(i.ToString(),"test","ww-","aa","no");
                //ds.Tables[0].Rows.Add(i.ToString(), "test", "ww-", "aa", "no");
               // this.ds2.Tables[0].Rows.Add(i.ToString(), "192.168.1." + i.ToString(), "", "", "aa", "离线");
               // this.ds2.Tables[0].Rows.Add(i.ToString(), "192.168.1." + i.ToString(), "", "", "aa", "离线2");

                //cyber_SetTextMesssage(100 *( i-min+1) / (max - min), "当前：192.168.1." + i.ToString(),ds2);
                cls_计算机远程 CC = new cls_计算机远程("192.168.1." + i.ToString());
                if (CC.is远程服务开启() == true)
                {

                    this.ds2.Tables[0].Rows.Add(i.ToString(), "192.168.1." + i.ToString(), CC.get计算机名称(), CC.getMac(), "aa", CC.get计算机状态());
                   // cyber_SetTextMesssage(100 * (i - min) / (max - min), "当前：192.168.1." + i.ToString(), this.ds2);
                }
                else
                {
                    this.ds2.Tables[0].Rows.Add(i.ToString(), "192.168.1." + i.ToString(), "", "", "aa", "离线");
                   // cyber_SetTextMesssage(100 * (i - min + 1) / (max - min), "当前：192.168.1." + i.ToString(), this.ds2);
                }
                cyber_SetTextMesssage(100 * (i - min + 1) / (max - min), "当前：192.168.1." + i.ToString(), this.ds2);
               

            }
            //ds = ds2;
            //this.dataGridView1.DataSource = ds.Tables[0];
        }

        private void cyber_SetTextMesssage(int ipos, string vinfo, DataSet v_ds)
        {
            if (this.InvokeRequired)
            {
                cyber_SetPos setpos = new cyber_SetPos(cyber_SetTextMesssage);
                this.Invoke(setpos, new object[] { ipos, vinfo, v_ds });
            }
            else
            {
                //this.label1.Text = ipos.ToString() + "/1000";
                this.toolStripProgressBar1.Value = Convert.ToInt32(ipos);
               // this.textBox1.AppendText(vinfo);
                this.tip_ip.Text = vinfo;
               // ds = v_ds;
                //this.ds.Tables[0].Rows.Add("f", vinfo, "", "", "aa", "离线");
                this.dataGridView1.DataSource = this.ds2.Tables[0];
            }
        }
        private void SetTextMesssage(int ipos, string vinfo)
        {
           
     
                this.toolStripProgressBar1.Value = Convert.ToInt32(ipos);
        
                this.tip_ip.Text = vinfo;
     
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



        private void button6_Click(object sender, EventArgs e)
        {
            广播服务器 f = new 广播服务器();
            f.Show();
        }
//代理
        /*
        */
        private int Cyber_Port = 19876;
        private Thread CyberThread;
        private void cy_web()
        {
            using (HttpListener hl = new HttpListener())
            {
                hl.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                hl.Prefixes.Add("http://*:" + Cyber_Port + "/cyberwinsvr/");
                hl.Start();
                while (true)
                {
                    HttpListenerContext ctx = hl.GetContext();
                    ctx.Response.StatusCode = 200;
                    //ctx.Request.
                   // string l = ctx.Request.QueryString["like"];
                   // string m = ctx.Request.QueryString["m"];
                    string c = ctx.Request.QueryString["client"];
                    //string ip = ctx.Request.UserHostAddress.ToString();
                    //int pos = ip.IndexOf(":");
                   // add_客户端(ip.Substring(0,pos));
                    add_客户端(c);

                    if (c.Length > 4)
                    {
                  //  http://127.0.0.1:19876/cyberwinsvr?client=192.168.1.8
                        
                        //add_客户端(c);
                    }

                   // using (StreamWriter w = new StreamWriter(ctx.Response.OutputStream, Encoding.UTF8))
                  //  {

                       // string responseBody = cls_Cloud_Deployment_client.cyber_cmd(ctx.Request);
                       // w.WriteLine(responseBody);
                        //w.WriteLine("hello2222刘7777" + System.Web.HttpUtility.UrlDecode(l, Encoding.ASCII) + "郭荣华第二：" + System.Web.HttpUtility.UrlDecode(ctx.Request.Url.ToString(), Encoding.ASCII) + "<br>mm==" + m);


                     //   w.Flush();
                      //  w.Close();



                    //}
                }
            }
        }
        #region " 刷新2"
        public void 刷新2()
        {
            CyberThread = new Thread(new ThreadStart(cy_web));
            CyberThread.Start();
        }

        public void add_客户端(string 客户端IP)
        {
            if (this.InvokeRequired)
            {
                cyber_客户端加入 客户端加入 = new cyber_客户端加入(add_客户端);
                this.Invoke(客户端加入, new object[] { 客户端IP });
            }
            else
            {
                //this.label1.Text = ipos.ToString() + "/1000";
                //this.toolStripProgressBar1.Value = Convert.ToInt32(ipos);
                // this.textBox1.AppendText(vinfo);
                //this.tip_ip.Text = vinfo;
                // ds = v_ds;
                int pos = 客户端IP.LastIndexOf(".");

               

                //判断MAC是否存在如果存在则修改IP信息
                //     cls_计算机远程 CC = new cls_计算机远程(dr.Cells[1].Value.ToString());

                // ds.Tables[0].Rows.Add(i.ToString(), CC.get计算机名称(), CC.getMac(), "aa", CC.get计算机状态());
                // MessageBox.Show(CC.getMac());
              //  this.dataGridView1.SelectedRows[0].Cells[3].Value = CC.getMac().Trim();


                cls_计算机远程 CC = new cls_计算机远程(客户端IP);
                string 计算机远程_mac = CC.getMac().Trim();



               
                int ii, c1;
                bool 客户端已经存在 = false;

                 c1 = this.dataGridView1.Rows.Count;
                 for (ii = 0; ii < c1 - 1; ii++)
                 {
                     string dgv_mac=  this.dataGridView1.Rows[ii].Cells[3].Value.ToString().Trim();
                     if (计算机远程_mac.Equals(dgv_mac))
                     {
                         this.dataGridView1.Rows[ii].Cells[1].Value = 客户端IP;//ip
                         this.dataGridView1.Rows[ii].Cells[2].Value = CC.get计算机名称().Trim();//计算机名称
                         this.dataGridView1.Rows[ii].Cells[5].Value ="在线";//计算机名称
                         客户端已经存在 = true;

                     }

                 }
                 if (客户端已经存在 == false)
                 {

                     this.ds.Tables[0].Rows.Add(客户端IP.Substring(pos + 1, 客户端IP.Length - pos - 1), 客户端IP, "", "", "", "新客户机");
                 }


                this.dataGridView1.DataSource = this.ds.Tables[0];
            }
        }
        #endregion

  
        public void 郭荣华_广播(string 广播内容)
        {
            //this.add_客户端("ggggggggggggggggggggg");
            //只能用UDP协议发送广播，所以ProtocolType设置为UDP
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //让其自动提供子网中的IP地址
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, 19877);
            //设置broadcast值为1，允许套接字发送广播信息
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            //将发送内容转换为字节数组
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(广播内容);
            //向子网发送信息
            socket.SendTo(bytes, iep);
            socket.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CyberThread = new Thread(new ThreadStart(cy_web));
            CyberThread.Start();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //CyberThread = new Thread(new ThreadStart(cy_web));
            CyberThread.Abort();
            System.Environment.Exit(System.Environment.ExitCode);   
            Application.Exit();
            //CyberThread.s
        }

        private void button10_Click(object sender, EventArgs e)
        {

            广播服务器 F = new 广播服务器();
            F.Show();
      
          // MessageBox.Show(GetLocalIP());  
 
        }
        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名

                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);

                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {

                    //从IP地址列表中筛选出IPv4类型的IP地址

                    //AddressFamily.InterNetwork表示此IP为IPv4,

                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型

                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {

                        return IpEntry.AddressList[i].ToString();

                    }

                }

                return "";

            }

            catch (Exception ex)
            {

                MessageBox.Show("获取本机IP出错:" + ex.Message);

                return "";

            }

        }

        private void btn_广播客户机_Click(object sender, EventArgs e)
        {
            //ds.Tables[0].Clear();
            郭荣华_广播("刷新客户端");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (this.txt_rename_reg.Text.Trim() == "")
            {
                //MessageBox.Show
               // return;
                this.txt_rename_reg.Text = "qjyz-{0}";
            }
            int i, c;
            c = this.dataGridView1.Rows.Count;
            for (i = 0; i < c-1; i++)
            {
                 cls_计算机远程 CC = new cls_计算机远程(this.dataGridView1.Rows[i].Cells[1].Value.ToString());//ip地址
                 if (CC.is远程服务开启() == true)
                 {

                    // this.ds2.Tables[0].Rows.Add(i.ToString(), "192.168.1." + i.ToString(), CC.get计算机名称(), CC.getMac(), "aa", CC.get计算机状态());
                     // cyber_SetTextMesssage(100 * (i - min) / (max - min), "当前：192.168.1." + i.ToString(), this.ds2);
                     this.dataGridView1.Rows[i].Cells[2].Value = CC.get计算机名称().Trim();//计算机名称
                     this.dataGridView1.Rows[i].Cells[3].Value = CC.getMac().Trim();//计算机MAC

                     string s = 查找计算机备份名称(this.dataGridView1.Rows[i].Cells[3].Value.ToString()); ;//计算机配置名称
                     if (s == "")
                     {
                         s = string.Format(this.txt_rename_reg.Text.Trim(), this.dataGridView1.Rows[i].Cells[0].Value.ToString());
                     }

                     this.dataGridView1.Rows[i].Cells[4].Value = s;


                 }
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int i, c;
            c = this.dataGridView1.Rows.Count;
            for (i = 0; i < c - 1; i++)
            {
                SetTextMesssage(i * 100 / c, this.dataGridView1.Rows[i].Cells[1].Value.ToString());
                string ip = this.dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                if (ip == "")
                {
                    this.dataGridView1.Rows[i].Cells[5].Value = "（失去连接）";
                }
                else
                {
                    cls_计算机远程 CC = new cls_计算机远程(ip);//ip地址
                    if (CC.is远程服务开启() == true)
                    {


                        string newret = CC.set计算机名称(this.dataGridView1.Rows[i].Cells[4].Value.ToString());
                        this.dataGridView1.Rows[i].Cells[5].Value = "（在线）" + newret;



                    }
                }
            }
        }
        public string 查找计算机备份名称(string MAC)
        {
            this.ds_bak.ReadXml(PATH_计算机配置信息MAC表_备份名称);

            int i, c;
           // MessageBox.Show(MAC);
            
           // DataGridView dgv = new DataGridView();
            //dgv.DataSource = this.ds_bak.Tables[0];

           // c = dgv.Rows.Count;
            i = 0;
            c = this.ds_bak.Tables[0].Rows.Count;

            for (i = 0; i < c; i++)
            { 
                //mac 3
                //name 4
                DataRow dr = this.ds_bak.Tables[0].Rows[i];
               // dr.ItemArray[3];
               // string msc = this.ds_bak.Tables[0].Rows[i].ItemArray[3];
                //string name = this.ds_bak.Tables[0].Rows[i].ItemArray[4];
               // string i_ip = dgv.Rows[i].Cells[1].Value.ToString();
               // string i_mac = dgv.Rows[i].Cells[3].Value.ToString();
              //  string i_name = dgv.Rows[i].Cells[4].Value.ToString();
                string i_ip=dr["IP"].ToString();
                string i_mac = dr["MAC"].ToString();
                string i_name = dr["计算机原始名称"].ToString();

              //  MessageBox.Show(i_mac + i_name);
                if (MAC.Equals(i_mac))
                {
                    return i_name;
                }

                return "";

            }
            return "";
     
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.ds_bak = this.ds;
            this.ds_bak.WriteXml(PATH_计算机配置信息MAC表_备份名称);
        }

        private void btn_新建计算机信息_Click(object sender, EventArgs e)
        {
            int min, i, max;
            min = int.Parse(this.txt_ip_s.Text.Trim());
            max = int.Parse(this.txt_ip_e.Text.Trim());
            for(i=min;i<max;i++){
                string name = string.Format(this.txt_rename_reg.Text.Trim(), i.ToString());
                this.ds.Tables[0].Rows.Add(i.ToString(), this.txt_ip_pre.Text.Trim() + i.ToString(), "", "", name, "------");
            }
            
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void 查看备份名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.txt_rename_reg.Text.Trim() == "")
            {
                //MessageBox.Show
                // return;
                this.txt_rename_reg.Text = "qjyz-{0}";
            }
            //MessageBox.Show(查找计算机备份名称(this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString()));
            string s = 查找计算机备份名称(this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
            if (s == "")
            {
                s = string.Format(this.txt_rename_reg.Text.Trim(), this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            }
            this.dataGridView1.SelectedRows[0].Cells[4].Value = s;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = this.ds_bak.Tables[0].DefaultView;
            //this.ds_bak.ReadXml(PATH_计算机配置信息MAC表_备份名称);
        }

        private void 设置计算机名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //this.dataGridView1.SelectedRows[0].Cells[4].Value = 查找计算机备份名称(this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString());

                   cls_计算机远程 CC = new cls_计算机远程(this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString());//ip地址

                   string newret = CC.set计算机名称(this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
                    //this.dataGridView1.Rows[i].Cells[5].Value = "（在线）"+newret;
              this.dataGridView1.SelectedRows[0].Cells[5].Value = "（在线）"+newret;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = this.dataGridView1.SelectedRows[0];
            // MessageBox.Show(dr.Cells[1].Value.ToString());

            cls_计算机远程 CC = new cls_计算机远程(dr.Cells[1].Value.ToString());

            // ds.Tables[0].Rows.Add(i.ToString(), CC.get计算机名称(), CC.getMac(), "aa", CC.get计算机状态());
            // MessageBox.Show(CC.getMac());
            this.dataGridView1.SelectedRows[0].Cells[2].Value = CC.get计算机名称().Trim();
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            this.ds.Tables[0].Rows.Clear();
            string[] dir = Directory.GetDirectories(this.vbox_路径);
            int i, c;
            c = dir.Length;
            for (i = 0; i < c; i++)
            {
                //this.listBox1.Items.Add(Path.GetFileName(dir[i]));
               // this.listBox1.Items.Add(dir[i] + "\\" + Path.GetFileName(dir[i]) + ".vbox");
                string 虚拟机_配置路径 = dir[i] + "\\" + Path.GetFileName(dir[i]) + ".vbox";
                if (File.Exists(虚拟机_配置路径))
                {
                    //get虚拟机配置(dir[i] + "\\" + Path.GetFileName(dir[i]) + ".vbox");
                    cls_virtualbox cv = new cls_virtualbox(虚拟机_配置路径);
                    this.ds.Tables[0].Rows.Add(i.ToString(), "", "", cv.get虚拟机配置_MAC(), cv.get虚拟机配置_名称(), "--等待上线---", 虚拟机_配置路径);
                }
            }
           // this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
        }

        private void 保护系统盘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
               //cls_计算机远程 CC = new cls_计算机远程(this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString());//ip地址

               //    string newret = CC.set计算机名称(this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
                    //this.dataGridView1.Rows[i].Cells[5].Value = "（在线）"+newret;
             // this.dataGridView1.SelectedRows[0].Cells[5].Value = "（在线）"+newret;
          string path=  this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
          string path_des = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
           //备份
            //
            Directory.CreateDirectory(this.vbox_备份路径);


           File.Copy(path, this.vbox_备份路径 + "\\" +DateTime.Now.ToString("yyyyMMddHHmmss")+"_" + path_des + ".vbox", true);//（为true是覆盖同名文件）); //复制文件
          cls_virtualbox cv = new cls_virtualbox(path);
          cv.set虚拟机配置_系统盘("Immutable");

        
            // cls_virtualbox cv = new cls_virtualbox(vbox_路径+"\\"+""+".vbox");

        }

        void frm_computer_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //CyberThread = new Thread(new ThreadStart(cy_web));
            CyberThread.Abort();
            System.Environment.Exit(System.Environment.ExitCode);
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
              int i, c;
            c = this.dataGridView1.Rows.Count;
            for (i = 0; i < c - 1; i++)
            {
                SetTextMesssage(i * 100 / c, this.dataGridView1.Rows[i].Cells[1].Value.ToString());
                
                //cls_计算机远程 CC = new cls_计算机远程(this.dataGridView1.Rows[i].Cells[1].Value.ToString());//ip地址
                string path = this.dataGridView1.Rows[0].Cells[6].Value.ToString();
                string path_des = this.dataGridView1.Rows[0].Cells[4].Value.ToString();
                //备份
                //
                Directory.CreateDirectory(this.vbox_备份路径);


                File.Copy(path, this.vbox_备份路径 + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + path_des + ".vbox", true);//（为true是覆盖同名文件）); //复制文件
                cls_virtualbox cv = new cls_virtualbox(path);
                cv.set虚拟机配置_系统盘("Immutable");

            }
        }

        private void 远程重启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            cls_计算机远程 CC = new cls_计算机远程(this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString());//ip地址

            string newret = CC.计算机_重启();
    
            this.dataGridView1.SelectedRows[0].Cells[5].Value = "（重启）" + newret;
        }

        private void 远程关机ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            cls_计算机远程 CC = new cls_计算机远程(this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString());//ip地址

            string newret = CC.计算机_关机();

            this.dataGridView1.SelectedRows[0].Cells[5].Value = "（关机）" + newret;
        }

        private void 释放介质SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            //string path_des = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
             cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
            cv.high_释放磁盘_all_bycmd();

        }

        private void 附加D盘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString().Trim();
            string path_des = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
            //备份
            //
            Directory.CreateDirectory(this.vbox_备份路径);


            File.Copy(path, this.vbox_备份路径 + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss磁盘") + "_" + path_des + ".vbox", true);//（为true是覆盖同名文件）); //复制文件
            cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
            string id= this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim();
            string D = string.Format(this.get介质("seconddriver"), path_des);
            string path_FOLDER = System.IO.Path.GetDirectoryName(path);
            //MessageBox.Show(path_FOLDER);
           // cv.high_附加磁盘_D盘();
            //
            cv.high_附加磁盘_D盘(path_FOLDER+"\\"+D);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string path = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString().Trim();
            string path_des = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
            //备份
            //
            Directory.CreateDirectory(this.vbox_备份路径);


            File.Copy(path, this.vbox_备份路径 + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss磁盘") + "_" + path_des + ".vbox", true);//（为true是覆盖同名文件）); //复制文件
            cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim();
            string 系统盘 = string.Format(get介质("systemdriver"), path_des);
            string path_FOLDER = System.IO.Path.GetDirectoryName(path);
            //MessageBox.Show(path_FOLDER);
            // cv.high_附加磁盘_D盘();
            //
            //cv.init_SATA();
           // MessageBox.Show(this.get介质("systemdriver"));
            string 系统盘_附加路径 = path_FOLDER + "\\" + 系统盘;
          //  txt_cmd.Text =    string.Format(" storageattach \"{0}\" --storagectl \"{1}\" --port {2} --device 0 --type hdd --medium \"{3}\"", path_des, "SATA", 0, 系统盘_附加路径);

           cv.high_附加磁盘_系统盘(系统盘_附加路径);
        }

        private void 启动虚拟机XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString().Trim();
            string 虚拟机名称 = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
            //备份
            cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
           // cv.high_虚拟机_开机(虚拟机名称);
            cv.high_虚拟机_开机();

        }

        private void 用此模板克隆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString().Trim();
            string 虚拟机名称 = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
            //备份
            cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
           // cv.high_虚拟机_开机(虚拟机名称);
            int min, i, max;
            min = int.Parse(this.txt_ip_s.Text.Trim());
            max = int.Parse(this.txt_ip_e.Text.Trim());
            for (i = min; i < max; i++)
            {
                string name = string.Format(this.txt_rename_reg.Text.Trim(), i.ToString());
               // this.ds.Tables[0].Rows.Add(i.ToString(), this.txt_ip_pre.Text.Trim() + i.ToString(), "", "", name, "------");
                cv.high_虚拟机_克隆_with_注册(虚拟机名称, name);
            }


        }

        private void 写保护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString().Trim();
            string 虚拟机名称 = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
            //备份
            cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);

            string 系统盘 = string.Format(this.get介质("系统盘"), 虚拟机名称);
            string path_FOLDER = System.IO.Path.GetDirectoryName(path);
            //MessageBox.Show(path_FOLDER);
            // cv.high_附加磁盘_D盘();
            //
          //  cv.high_附加磁盘_D盘(path_FOLDER + "\\" + 系统盘);
//
            cv.high_虚拟机_写保护_byFilename(path_FOLDER + "\\" + 系统盘);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string path = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString().Trim();
            string path_des = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
            //备份
            //
            Directory.CreateDirectory(this.vbox_备份路径);


            File.Copy(path, this.vbox_备份路径 + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss添加stat控制器") + "_" + path_des + ".vbox", true);//（为true是覆盖同名文件）); //复制文件
            cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim();
            string 系统盘 = string.Format(this.get介质("系统盘"), path_des);
            string path_FOLDER = System.IO.Path.GetDirectoryName(path);
            //MessageBox.Show(path_FOLDER);
            // cv.high_附加磁盘_D盘();
            //
            //cv.init_SATA();
            cv.init_SATA_bycmd();
        }

        private void btn_clone_Click(object sender, EventArgs e)
        {
            try
            {
                string path = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString().Trim();
                string 虚拟机名称 = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
                //备份
                cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
                // cv.high_虚拟机_开机(虚拟机名称);
                int min, i, max;
                min = int.Parse(this.txt_ip_s.Text.Trim());
                max = int.Parse(this.txt_ip_e.Text.Trim());
                for (i = min; i < max; i++)
                {
                    string name = string.Format(this.txt_rename_reg.Text.Trim(), i.ToString());
                    // this.ds.Tables[0].Rows.Add(i.ToString(), this.txt_ip_pre.Text.Trim() + i.ToString(), "", "", name, "------");
                    cv.high_虚拟机_克隆_with_注册(虚拟机名称, name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择在左侧选择，克隆模板");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int i, c;
            c = this.dataGridView1.Rows.Count;
            for (i = 0; i < c - 1; i++)
            {
                SetTextMesssage(i * 100 / c, this.dataGridView1.Rows[i].Cells[1].Value.ToString());



                string path = this.dataGridView1.Rows[i].Cells[6].Value.ToString().Trim();
                string 虚拟机名称 = this.dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                //备份
                cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
                cv.high_释放磁盘_all_bycmd();


                this.dataGridView1.Rows[i].Cells[5].Value = "（释放介质）";
            }

          
        }

        private void button11_Click(object sender, EventArgs e)
        {

            string cmdtext = "taskkill /F /im VirtualBox.e*";
            Process MyProcess = new Process();
            //设定程序名 
            MyProcess.StartInfo.FileName = "cmd.exe";
            //关闭Shell的使用 
            MyProcess.StartInfo.UseShellExecute = false;
            //重定向标准输入 
            MyProcess.StartInfo.RedirectStandardInput = true;
            //重定向标准输出 
            MyProcess.StartInfo.RedirectStandardOutput = true;
            //重定向错误输出 
            MyProcess.StartInfo.RedirectStandardError = true;
            //设置不显示窗口 
            MyProcess.StartInfo.CreateNoWindow = true;
            //执行VER命令 
            MyProcess.Start();
            MyProcess.StandardInput.WriteLine(cmdtext);
            MyProcess.StandardInput.WriteLine("exit");
            //从输出流获取命令执行结果， 
            //string exepath = Application.StartupPath; 
            //把返回的DOS信息读出来 
            String StrInfo = MyProcess.StandardOutput.ReadToEnd();
          //  Console.WriteLine(StrInfo);
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
             int i, c;
            c = this.dataGridView1.Rows.Count;
            for (i = 0; i < c - 1; i++)
            {
                SetTextMesssage(i * 100 / c, this.dataGridView1.Rows[i].Cells[1].Value.ToString());
                string ip = this.dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                if (ip == "")
                {
                    this.dataGridView1.Rows[i].Cells[5].Value = "（失去连接）";
                }
                else
                {

                    cls_计算机远程 CC = new cls_计算机远程(ip);//ip地址

                    string newret = CC.计算机_关机();

                    this.dataGridView1.Rows[i].Cells[5].Value = "（关机中）";
                }
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            int i, c;
            c = this.dataGridView1.Rows.Count;
            for (i = 0; i < c - 1; i++)
            {
                SetTextMesssage(i * 100 / c, this.dataGridView1.Rows[i].Cells[1].Value.ToString());

             

                string path = this.dataGridView1.Rows[i].Cells[6].Value.ToString().Trim();
                string 虚拟机名称 = this.dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                //备份
                cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
                // cv.high_虚拟机_开机(虚拟机名称);
                cv.high_虚拟机_开机();


                this.dataGridView1.Rows[i].Cells[5].Value = "（开机中）";
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int i, c;
            c = this.dataGridView1.Rows.Count;
            for (i = 0; i < c - 1; i++)
            {
                SetTextMesssage(i * 100 / c, this.dataGridView1.Rows[i].Cells[1].Value.ToString());



                string path = this.dataGridView1.Rows[i].Cells[6].Value.ToString().Trim();
                string path_des = this.dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                //备份
                //
               // Directory.CreateDirectory(this.vbox_备份路径);


               // File.Copy(path, this.vbox_备份路径 + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss添加stat控制器") + "_" + path_des + ".vbox", true);//（为true是覆盖同名文件）); //复制文件
                cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
              
                string 系统盘 = string.Format(this.get介质("系统盘"), path_des);
                string path_FOLDER = System.IO.Path.GetDirectoryName(path);
                //MessageBox.Show(path_FOLDER);
                // cv.high_附加磁盘_D盘();
                //
                //cv.init_SATA();
                cv.init_SATA_bycmd();


                this.dataGridView1.Rows[i].Cells[5].Value = "（加载stat控制）";
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            int i, c;
            c = this.dataGridView1.Rows.Count;
            for (i = 0; i < c - 1; i++)
            {
                SetTextMesssage(i * 100 / c, this.dataGridView1.Rows[i].Cells[1].Value.ToString());



                string path = this.dataGridView1.Rows[i].Cells[6].Value.ToString().Trim();
                string path_des = this.dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                //备份
       
                cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
                string 系统盘 = string.Format(get介质("systemdriver"), path_des);
                string path_FOLDER = System.IO.Path.GetDirectoryName(path);
                 string 系统盘_附加路径 = path_FOLDER + "\\" + 系统盘;
             
                cv.high_附加磁盘_系统盘(系统盘_附加路径);

                this.dataGridView1.Rows[i].Cells[5].Value = "（挂载系统盘）";
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int i, c;
            c = this.dataGridView1.Rows.Count;
            for (i = 0; i < c - 1; i++)
            {
                SetTextMesssage(i * 100 / c, this.dataGridView1.Rows[i].Cells[1].Value.ToString());



                string path = this.dataGridView1.Rows[i].Cells[i].Value.ToString().Trim();
                string path_des = this.dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                //备份
                //
                    cls_virtualbox cv = new cls_virtualbox(path, this.vbox_运行路径);
              
                string D = string.Format(this.get介质("seconddriver"), path_des);
                string path_FOLDER = System.IO.Path.GetDirectoryName(path);
                //MessageBox.Show(path_FOLDER);
                // cv.high_附加磁盘_D盘();
                //
                cv.high_附加磁盘_D盘(path_FOLDER + "\\" + D);
                this.dataGridView1.Rows[i].Cells[5].Value = "（挂载D盘）";
            }
        }

 

    }




}