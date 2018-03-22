using System;
using System.Windows.Forms;
using BurnInUI.Forms;

namespace BurnInUI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// 
        public static bool BOnlyOneInstance = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GeneralUi());
        }
    }
}
