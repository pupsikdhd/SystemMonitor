using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace TaskManager
{
    public partial class TaskManagerForm : Form
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;

        public TaskManagerForm()
        {
            InitializeComponent();
            InitializePerformanceCounters();
        }

        private void InitializePerformanceCounters()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
        }

        private void TaskManagerForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }


        private float GetTotalMemory()
        {
            return new ComputerInfo().TotalPhysicalMemory / (1024 * 1024);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            float cpuUsage = cpuCounter.NextValue();
            float ramUsage = ramCounter.NextValue();

            progressBarCPU.Value = (int)cpuUsage;
            progressBarRAM.Value = (int)((1 - (ramUsage / GetTotalMemory())) * 100);
            CPU.Text = $"CPU: {cpuUsage:F1}% ";
            RAM.Text = $"RAM: {GetTotalMemory() - ramUsage:F0} MB / {GetTotalMemory()} MB";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TaskManagerForm_Load_1(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void TaskManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TaskManagerMain taskManager = new TaskManagerMain(null);
            taskManager.Show();
        }
    }
}
