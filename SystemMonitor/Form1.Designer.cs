namespace TaskManager
{
    partial class TaskManagerForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskManagerForm));
            this.CPU = new System.Windows.Forms.Label();
            this.RAM = new System.Windows.Forms.Label();
            this.progressBarRAM = new System.Windows.Forms.ProgressBar();
            this.progressBarCPU = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CPU
            // 
            this.CPU.AutoSize = true;
            this.CPU.Location = new System.Drawing.Point(11, 110);
            this.CPU.Name = "CPU";
            this.CPU.Size = new System.Drawing.Size(82, 13);
            this.CPU.TabIndex = 0;
            this.CPU.Text = "CPU - checking";
            // 
            // RAM
            // 
            this.RAM.AutoSize = true;
            this.RAM.Location = new System.Drawing.Point(9, 68);
            this.RAM.Name = "RAM";
            this.RAM.Size = new System.Drawing.Size(84, 13);
            this.RAM.TabIndex = 1;
            this.RAM.Text = "RAM - checking";
            // 
            // progressBarRAM
            // 
            this.progressBarRAM.Location = new System.Drawing.Point(12, 42);
            this.progressBarRAM.Name = "progressBarRAM";
            this.progressBarRAM.Size = new System.Drawing.Size(498, 23);
            this.progressBarRAM.TabIndex = 2;
            // 
            // progressBarCPU
            // 
            this.progressBarCPU.Location = new System.Drawing.Point(12, 84);
            this.progressBarCPU.Name = "progressBarCPU";
            this.progressBarCPU.Size = new System.Drawing.Size(498, 23);
            this.progressBarCPU.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "TaskManager";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TaskManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 124);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBarCPU);
            this.Controls.Add(this.progressBarRAM);
            this.Controls.Add(this.RAM);
            this.Controls.Add(this.CPU);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(552, 163);
            this.MinimumSize = new System.Drawing.Size(530, 163);
            this.Name = "TaskManagerForm";
            this.Text = "Widget";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskManagerForm_FormClosing);
            this.Load += new System.EventHandler(this.TaskManagerForm_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CPU;
        private System.Windows.Forms.Label RAM;
        private System.Windows.Forms.ProgressBar progressBarRAM;
        private System.Windows.Forms.ProgressBar progressBarCPU;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
    }
}

