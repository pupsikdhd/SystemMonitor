using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using SystemMonitor;

namespace TaskManager
{
    public partial class TaskManagerMain : Form
    {
        // Модификаторы клавиш
        private const int MOD_ALT = 0x1;
        private const int MOD_CONTROL = 0x2;
        private const int MOD_SHIFT = 0x4;
        private const int MOD_WIN = 0x8;

        // ID для горячей клавиши
        private const int HOTKEY_ID = 1;

        private PerformanceCounter cpuCounter;
        private bool isTray;
        private PerformanceCounter ramCounter;

        public TaskManagerMain(string[] args)
        {
            InitializeComponent();
            InitializePerformanceCounters();
            PopulateProcessList();


            RegisterHotKey(Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, (int)Keys.T);
        }

        // Импортируем функцию из User32.dll для регистрации горячих клавиш
        [DllImport("User32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("User32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        private void InitializePerformanceCounters()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
        }

        private void PopulateProcessList()
        {
            listViewProcesses.Columns.Add("Name", 120, HorizontalAlignment.Left);
            listViewProcesses.Columns.Add("ID", 70, HorizontalAlignment.Left);
            listViewProcesses.Columns.Add("CPU", 70, HorizontalAlignment.Left);
            listViewProcesses.Columns.Add("Memory (MB)", 100, HorizontalAlignment.Left);

            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                var item = new ListViewItem(process.ProcessName);
                item.SubItems.Add(process.Id.ToString());
                item.SubItems.Add("0");
                item.SubItems.Add("0");
                listViewProcesses.Items.Add(item);
            }
        }

        private void TaskManagerForm_Load(object sender, EventArgs e)
        {
            isTray = false;
            timer1.Start();

            notifyIcon1.BalloonTipTitle = "App";
            notifyIcon1.BalloonTipText = "Application minimized to tray";
            notifyIcon1.Text = "System Monitor";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var cpuUsage = cpuCounter.NextValue();
            var ramUsage = ramCounter.NextValue();

            progressBarCPU.Value = (int)cpuUsage;
            progressBarRAM.Value = (int)((GetTotalMemory() - ramUsage) / GetTotalMemory() * 100);

            labelCPU.Text = $"CPU using: {cpuUsage:F1}%";
            labelRAM.Text = $"RAM using: {GetTotalMemory() - ramUsage:F0} MB / {GetTotalMemory()} MB";

            if (checkBox1.Checked)
                UpdateProcessList();
        }

        private void UpdateProcessList()
        {
            listViewProcesses.Items.Clear();

            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                float cpuUsage = 0;
                float ramUsage = 0;

                try
                {
                    cpuUsage = (float)Math.Round(
                        process.TotalProcessorTime.TotalMilliseconds / Environment.ProcessorCount / 10, 1);
                    ramUsage = process.WorkingSet64 / (1024 * 1024);
                }
                catch
                {
                    continue;
                }

                var item = new ListViewItem(process.ProcessName);

                item.SubItems.Add(process.Id.ToString());

                item.SubItems.Add($"{cpuUsage:F1}%");
                item.SubItems.Add($"{ramUsage:F1}");

                listViewProcesses.Items.Add(item);
            }

            listViewProcesses.Sort();
        }


        private float GetTotalMemory()
        {
            return new ComputerInfo().TotalPhysicalMemory / (1024 * 1024);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsRunningAsAdmin())
            {
                var RestartRunAs = MessageBox.Show
                (
                    "No administrator permision.\n Restart with Administrator permision?",
                    "Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );
                if (RestartRunAs == DialogResult.Yes) RestartAsAdmin();
                return;
            }

            if (listViewProcesses.SelectedItems.Count > 0)
            {
                var processId = int.Parse(listViewProcesses.SelectedItems[0].SubItems[1].Text);
                try
                {
                    Process.GetProcessById(processId).Kill();
                }
                catch (Exception ex)
                {
                    MessageBox.Show
                    (
                        $"Failed to kill process: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }

                listViewProcesses.Items.Remove(listViewProcesses.SelectedItems[0]);
                UpdateProcessList();
            }
            else
            {
                MessageBox.Show
                (
                    "Please select a process to kill.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateProcessList();
        }

        private bool IsRunningAsAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void RestartAsAdmin()
        {
            var name = Process.GetCurrentProcess().MainModule.FileName;
            var startInfo = new ProcessStartInfo(name) { Verb = "runas" };
            try
            {
                Process.Start(startInfo);
                isTray = true;
                Application.Exit();
            }
            catch (Exception ex)
            {
                var dr = MessageBox.Show
                (
                    $"An error has occurred.\n Details:\n{ex} \n Try again?",
                    "Error.",
                    MessageBoxButtons.YesNo
                );

                if (dr == DialogResult.Yes)
                    RestartAsAdmin();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isTray = true;
            Application.Exit();
        }

        private void createTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var createTask = new CreateTask();
            createTask.ShowDialog();
        }

        private void TaskManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isTray)
            {
                Application.Exit();
            }

            else
            {
                e.Cancel = true;
                Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
        }





        private void restartAsAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsRunningAsAdmin())
            {
                MessageBox.Show(
                    "The program is already running as administrator",
                    "Error"
                );
                return;
            }

            RestartAsAdmin();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(1000);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            isTray = true;
            Application.Exit();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(null, null);
        }


        private void createTaskToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var createTask = new CreateTask();
            createTask.Show();
        }

        private void widgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var taskManagerForm = new TaskManagerForm();
            taskManagerForm.Show();
            isTray = true;
            Hide();
        }

        private void takeown()
        {
            var resourceName = "InstallTakeOwnership.reg";
            var tempFilePath = Path.GetTempFileName() + ".reg";

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new Exception("Resource not found: " + resourceName);

                using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
            }

            var process = new Process();
            process.StartInfo.FileName = "regedit.exe";
            process.StartInfo.Arguments = "/s " + tempFilePath;
            process.Start();
            process.WaitForExit();
            File.Delete(tempFilePath);
        }

        private void takeOwnershipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsRunningAsAdmin())
            {
                takeown();
                return;
            }

            var RestartRunAs = MessageBox.Show
            (
                "No administrator permision.\n Restart with Administrator permision?",
                "Error",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );
            if (RestartRunAs == DialogResult.Yes) RestartAsAdmin();
        }

        private void restartAsAdminToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            restartAsAdminToolStripMenuItem.PerformClick();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID) createTaskToolStripMenuItem.PerformClick();
            base.WndProc(ref m);
        }
    }
}