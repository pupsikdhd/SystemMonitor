using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SystemMonitor
{
    public partial class CreateTask : Form
    {
        public CreateTask()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("cmd", "/c " + textBox1.Text);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) => this.Close();

        private void CreateTask_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void CreateTask_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == (char)Keys.Enter)
            {
                button2.PerformClick();
            }
        }
    }
}
