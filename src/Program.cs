using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CyberWin.Work.Qjcx.远程部署控制
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frm_computer());
        }
    }
}