using System;
using System.Windows.Forms;

namespace CleanProApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //bool CanCreateNewApp = false;
            //Mutex mutex = new Mutex(true, Process.GetCurrentProcess().ProcessName, out CanCreateNewApp);
            //// Prevent repeat starts ------Method-01
            //if (!CanCreateNewApp)
            //{
            //    MessageBox.Show("程序已经在运行", "警告", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    Environment.Exit(1);
            //}
            //// Prevent repeat starts ------Method-01

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormRoot());
        }
    }
}