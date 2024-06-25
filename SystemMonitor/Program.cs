using System;
using System.Windows.Forms;
using TaskManager;

namespace SystemMonitor
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TaskManagerMain(args));
        }
    }
}
