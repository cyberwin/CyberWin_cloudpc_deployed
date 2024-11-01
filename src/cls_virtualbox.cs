using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Windows.Forms;

namespace CyberWin.Work.Qjcx.远程部署控制
{
    class cls_virtualbox
    {
        private string 配置文件;
        private string vbox_运行环境_路径;
        private  string vbox_manger_path;
        public cls_virtualbox(string 配置文件)
        {
            this.配置文件 = 配置文件;
        }

        public cls_virtualbox(string 配置文件, string vbox_运行环境_路径)
        {
            this.配置文件 = 配置文件;
            this.vbox_运行环境_路径 = vbox_运行环境_路径;
            this.vbox_manger_path = vbox_运行环境_路径 + "\\VBoxManage.exe";

        }

        

        public bool cyber_call_高级命令(string cmd) {
            System.Diagnostics.Process 命令主体 = new Process();
            System.Diagnostics.ProcessStartInfo 命令附属信息 = new ProcessStartInfo(this.vbox_manger_path);
            命令附属信息.Arguments = " "+cmd;
            命令附属信息.WorkingDirectory = this.vbox_运行环境_路径+"\\";
            System.Diagnostics.Process.Start(命令附属信息).WaitForExit();

            return true;

           // System.Diagnostics.Process.Start().WaitForExit();

        }
        public bool init_SATA_bycmd()
        {
            return this.cyber_call_高级命令(" storagectl \""+this.get虚拟机配置_名称()+"\" --name \"SATA\" --add ide --controller PIIX4");
        }
        public bool init_SATA()
        {
             XmlDocument doc = new XmlDocument();
            doc.Load(this.配置文件);    //加载Xml文件  

            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlNodeList personNodes = rootElem.GetElementsByTagName("Machine"); //获取


            string strName = ((XmlElement)personNodes[0]).GetAttribute("name");   //获取name属性值  

            //mac
            //获取硬件层
            XmlNodeList 存储控制_s = ((XmlElement)personNodes[0]).GetElementsByTagName("StorageControllers");  //获取硬件层
            //
           //  <StorageController name="SATA" type="AHCI" PortCount="3" useHostIOCache="false" Bootable="true" IDE0MasterEmulationPort="0" IDE0SlaveEmulationPort="1" IDE1MasterEmulationPort="2" IDE1SlaveEmulationPort="3">
           XmlNode xn= doc.CreateElement("StorageControlle");


           XmlAttribute xa_n = doc.CreateAttribute("name");
           xa_n.Value = "SATA";

           XmlAttribute xa_p = doc.CreateAttribute("PortCount");
           xa_p.Value = "0";

           XmlAttribute xa_t = doc.CreateAttribute("type");
           xa_t.Value = "AHCI";

           XmlAttribute xa_useHostIOCache = doc.CreateAttribute("useHostIOCache");
           xa_useHostIOCache.Value = "false";

           XmlAttribute xa_Bootable = doc.CreateAttribute("Bootable");
           xa_Bootable.Value = "true";

           XmlAttribute xa_IDE0MasterEmulationPort = doc.CreateAttribute("IDE0MasterEmulationPort");
           xa_IDE0MasterEmulationPort.Value = "0";

           XmlAttribute xa_IDE0SlaveEmulationPort = doc.CreateAttribute("IDE0SlaveEmulationPort");
           xa_IDE0SlaveEmulationPort.Value = "1";

           XmlAttribute xa_IDE1MasterEmulationPort = doc.CreateAttribute("IDE1MasterEmulationPort");
           xa_IDE1MasterEmulationPort.Value = "2";

           XmlAttribute xa_IDE1SlaveEmulationPort = doc.CreateAttribute("IDE1SlaveEmulationPort");
           xa_IDE1SlaveEmulationPort.Value = "3";

           xn.Attributes.Append(xa_n);
           xn.Attributes.Append(xa_p);
           xn.Attributes.Append(xa_t);
           xn.Attributes.Append(xa_useHostIOCache);
           xn.Attributes.Append(xa_Bootable);
           xn.Attributes.Append(xa_IDE0MasterEmulationPort);
           xn.Attributes.Append(xa_IDE0SlaveEmulationPort);
           xn.Attributes.Append(xa_IDE1MasterEmulationPort);
           xn.Attributes.Append(xa_IDE1SlaveEmulationPort);

           ((XmlElement)存储控制_s[0]).AppendChild(xn);

           doc.Save(this.配置文件);

        return true;
 
        }

        public bool  high_虚拟机_克隆(string src_虚拟机, string des_虚拟机, string otherinfo)
        {
            return this.cyber_call_高级命令(" clonevm " + src_虚拟机 + " --name " + des_虚拟机 + " " + otherinfo);
        }
        public bool high_虚拟机_克隆_with_注册(string src_虚拟机, string des_虚拟机)
        {
            //clonevm qjyz-2 --name qjyz-f --register
            return this.high_虚拟机_克隆(src_虚拟机, des_虚拟机, " --register");

        }
        //
        public bool high_附加磁盘_系统盘(string 系统盘)
        {
            return this.high_附加磁盘_with_port(系统盘, "0");
          

        }
        public bool high_附加磁盘_D盘(string 磁盘)
        {
            return this.high_附加磁盘_with_port(磁盘, "1");


        }
        public bool high_附加磁盘_with_port(string 磁盘, string port)
        {
            return this.high_附加磁盘_with_portController(磁盘, port, "SATA");

        }
        public bool high_附加磁盘_with_portController(string 磁盘, string port, string controller)
        {
            //添加HD
            //VBoxManage  storageattach "qjyz-1" --storagectl "SATA" --port 1 --device 0 --type hdd --medium "F:/VM/qjyz-1/qjyz-3.vdi"
            string cmd = string.Format(" storageattach \"{0}\" --storagectl \"{1}\" --port {2} --device 0 --type hdd --medium \"{3}\"", this.get虚拟机配置_名称(), controller, port, 磁盘);
          //  MessageBox.Show(cmd);
          //  return true;
            return this.cyber_call_高级命令(cmd);
      

        }
        //2015年1月9日
        //

        //storagectl qjyz-2 --name "SATA" --remove
        public bool high_释放磁盘_all_bycmd()
        {
            return this.cyber_call_高级命令(" storagectl \""+this.get虚拟机配置_名称()+"\" --name \"SATA\" --remove");
        }

        public bool high_释放磁盘_all()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.配置文件);    //加载Xml文件  

            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlNodeList personNodes = rootElem.GetElementsByTagName("Machine"); //获取


            string strName = ((XmlElement)personNodes[0]).GetAttribute("name");   //获取name属性值  

            //mac
            //获取硬件层
            XmlNodeList 存储控制_s = ((XmlElement)personNodes[0]).GetElementsByTagName("StorageControllers");  //获取硬件层
            XmlNodeList 存储控制 = ((XmlElement)存储控制_s[0]).GetElementsByTagName("StorageController");  //获取网卡
           // XmlNodeList 存储 = ((XmlElement)存储控制[0]).GetElementsByTagName("AttachedDevice");  //获取网卡第一张网卡
            ((XmlElement)存储控制[0]).SetAttribute("test", "sssssssssssssssss" + strName+this.get虚拟机配置_MAC());
            foreach (XmlNode xn in 存储控制)
            {
                XmlElement xel = (XmlElement)xn;
                XmlNodeList node = xel.GetElementsByTagName("AttachedDevice");
                if (node.Count > 0)
                {
                  
                   // xel.SetAttribute("aa","111111111111111");
                        xel.RemoveAll();
                  //  break;
                }
            }
            ///////////////////////////////////////////////////////////////////////
            foreach (XmlNode xn in 存储控制_s)
            {
                XmlElement xel = (XmlElement)xn;
                XmlNodeList node = xel.GetElementsByTagName("StorageController");
                if (node.Count > 0)
                {

                    // xel.SetAttribute("aa","111111111111111");
                    xel.RemoveAll();
                    //  break;
                }
            }
            //////////////////////////////////////////////////////
            //存储.
            //((XmlElement)存储).RemoveAll();
            //MACAddress
           // XmlElement xeRoot = doc.DocumentElement;
           // XmlDeclaration xdl = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");
           // doc.InsertBefore(xdl, xeRoot);
            doc.Save(this.配置文件);
            return true;
        }
        public void high_附加磁盘_byUUID(string uuid)
        {
        }

        public void high_附加磁盘_byFilename(string file)
        {

        }

        public bool high_虚拟机_写保护_byUUID(string uuid)
        {
            return this.cyber_call_高级命令(" modifyhd " + uuid + " --type immutable");
        }

        public bool high_虚拟机_写保护_byFilename(string file)
        {
            return this.cyber_call_高级命令(" modifyhd " + file + " --type immutable");
        }

        public void high_虚拟机_关机()
        {
        }

        public bool high_虚拟机_重启(string 虚拟机)
        {
            return this.cyber_call_高级命令(" startvm " + 虚拟机);
     
        }

        public bool high_虚拟机_开机(string 虚拟机)
        {
            return this.cyber_call_高级命令(" startvm " + 虚拟机);

        }

        public bool high_虚拟机_开机()
        {
            return this.cyber_call_高级命令(" startvm " + this.get虚拟机配置_名称());

        }




        //写入保护
        public void set虚拟机配置_系统盘(string type)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.配置文件);    //加载Xml文件  

            XmlElement rootElem = doc.DocumentElement;   //获取根节点  

            XmlNodeList personNodes = rootElem.GetElementsByTagName("Machine"); //获取

            string strName = ((XmlElement)personNodes[0]).GetAttribute("name");   //获取name属性值  


            //mac
            //获取硬件层
            XmlNodeList hardware = ((XmlElement)personNodes[0]).GetElementsByTagName("MediaRegistry");  //获取硬件层
            XmlNodeList HardDisks = ((XmlElement)hardware[0]).GetElementsByTagName("HardDisks");  //获取网卡
           // XmlNodeList HardDisks_1 = ((XmlElement)HardDisks[0]).GetElementsByTagName("HardDisk");  //获取网卡
           // ((XmlElement)HardDisks_1[0]).SetAttribute("type", type);// GetAttribute("MACAddress");   //获取name属性值 
            XmlNode hd1= HardDisks.Item(0);
            //hd1.Attributes.SetNamedItem(
            ((XmlElement)HardDisks[0].ChildNodes[0]).SetAttribute("type", type);
            doc.Save(this.配置文件);

            
            //XmlNodeList network_ap_0 = ((XmlElement)network_ap[0]).GetElementsByTagName("Adapter");  //获取网卡第一张网卡
            //MACAddress
            /*
            string strName_mac1 = ((XmlElement)network_ap_0[0]).GetAttribute("MACAddress");   //获取name属性值 

            // this.listBox1.Items.Add("计算机：" + strName);
            string s = "1030"; s = s.Insert(2, ":");
            strName_mac1 = strName_mac1.Insert(2, ":");
            strName_mac1 = strName_mac1.Insert(5, ":");
            strName_mac1 = strName_mac1.Insert(8, ":");
            strName_mac1 = strName_mac1.Insert(11, ":");
            strName_mac1 = strName_mac1.Insert(14, ":");
             * */
           // return strName;

            // this.listBox2.Items.Add("计算机：" + strName+"mac：" + strName_mac1);
        }
        public  string get虚拟机配置_名称()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.配置文件);    //加载Xml文件  

            XmlElement rootElem = doc.DocumentElement;   //获取根节点  

            XmlNodeList personNodes = rootElem.GetElementsByTagName("Machine"); //获取

            string strName = ((XmlElement)personNodes[0]).GetAttribute("name");   //获取name属性值  


            //mac
            //获取硬件层
            XmlNodeList hardware = ((XmlElement)personNodes[0]).GetElementsByTagName("Hardware");  //获取硬件层
            XmlNodeList network_ap = ((XmlElement)hardware[0]).GetElementsByTagName("Network");  //获取网卡
            XmlNodeList network_ap_0 = ((XmlElement)network_ap[0]).GetElementsByTagName("Adapter");  //获取网卡第一张网卡
            //MACAddress
            string strName_mac1 = ((XmlElement)network_ap_0[0]).GetAttribute("MACAddress");   //获取name属性值 

            // this.listBox1.Items.Add("计算机：" + strName);
            string s = "1030"; s = s.Insert(2, ":");
            strName_mac1 = strName_mac1.Insert(2, ":");
            strName_mac1 = strName_mac1.Insert(5, ":");
            strName_mac1 = strName_mac1.Insert(8, ":");
            strName_mac1 = strName_mac1.Insert(11, ":");
            strName_mac1 = strName_mac1.Insert(14, ":");
            return strName.Trim();

            // this.listBox2.Items.Add("计算机：" + strName+"mac：" + strName_mac1);
        }
        public  string get虚拟机配置_MAC()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.配置文件);    //加载Xml文件  

            XmlElement rootElem = doc.DocumentElement;   //获取根节点  

            XmlNodeList personNodes = rootElem.GetElementsByTagName("Machine"); //获取

            string strName = ((XmlElement)personNodes[0]).GetAttribute("name");   //获取name属性值  


            //mac
            //获取硬件层
            XmlNodeList hardware = ((XmlElement)personNodes[0]).GetElementsByTagName("Hardware");  //获取硬件层
            XmlNodeList network_ap = ((XmlElement)hardware[0]).GetElementsByTagName("Network");  //获取网卡
            XmlNodeList network_ap_0 = ((XmlElement)network_ap[0]).GetElementsByTagName("Adapter");  //获取网卡第一张网卡
            //MACAddress
            string strName_mac1 = ((XmlElement)network_ap_0[0]).GetAttribute("MACAddress");   //获取name属性值 

            // this.listBox1.Items.Add("计算机：" + strName);
            string s = "1030"; s = s.Insert(2, ":");
            strName_mac1 = strName_mac1.Insert(2, ":");
            strName_mac1 = strName_mac1.Insert(5, ":");
            strName_mac1 = strName_mac1.Insert(8, ":");
            strName_mac1 = strName_mac1.Insert(11, ":");
            strName_mac1 = strName_mac1.Insert(14, ":");
            return strName_mac1;
        }

         public void get虚拟机配置(string 配置文件)
        {
            
            XmlDocument doc = new XmlDocument();
            doc.Load(配置文件);    //加载Xml文件  

            XmlElement rootElem = doc.DocumentElement;   //获取根节点  

            XmlNodeList personNodes = rootElem.GetElementsByTagName("Machine"); //获取

            string strName = ((XmlElement)personNodes[0]).GetAttribute("name");   //获取name属性值  
         

            //mac
            //获取硬件层
            XmlNodeList hardware = ((XmlElement)personNodes[0]).GetElementsByTagName("Hardware");  //获取硬件层
            XmlNodeList network_ap = ((XmlElement)hardware[0]).GetElementsByTagName("Network");  //获取网卡
            XmlNodeList network_ap_0 = ((XmlElement)network_ap[0]).GetElementsByTagName("Adapter");  //获取网卡第一张网卡
            //MACAddress
            string strName_mac1 = ((XmlElement)network_ap_0[0]).GetAttribute("MACAddress");   //获取name属性值 

           // this.listBox1.Items.Add("计算机：" + strName);
            string s = "1030"; s = s.Insert(2, ":");
            strName_mac1 = strName_mac1.Insert(2, ":");
            strName_mac1 = strName_mac1.Insert(5, ":");
            strName_mac1 = strName_mac1.Insert(8, ":");
            strName_mac1 = strName_mac1.Insert(11, ":");
            strName_mac1 = strName_mac1.Insert(14, ":");

           // this.listBox2.Items.Add("计算机：" + strName+"mac：" + strName_mac1);


        }
    
    }
}
