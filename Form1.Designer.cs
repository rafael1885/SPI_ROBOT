namespace SPI_ROBOT
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lb_ROBOT = new System.Windows.Forms.Label();
            this.lb_on_off = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bt_Play = new System.Windows.Forms.Button();
            this.lb_RUN = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_ROBOT
            // 
            this.lb_ROBOT.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb_ROBOT.BackColor = System.Drawing.Color.Transparent;
            this.lb_ROBOT.Font = new System.Drawing.Font("Arial Narrow", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ROBOT.ForeColor = System.Drawing.Color.Yellow;
            this.lb_ROBOT.Location = new System.Drawing.Point(79, 9);
            this.lb_ROBOT.Name = "lb_ROBOT";
            this.lb_ROBOT.Size = new System.Drawing.Size(409, 83);
            this.lb_ROBOT.TabIndex = 98;
            this.lb_ROBOT.Text = "ROBOT_LOGS_SPI";
            // 
            // lb_on_off
            // 
            this.lb_on_off.AccessibleRole = System.Windows.Forms.AccessibleRole.Caret;
            this.lb_on_off.BackColor = System.Drawing.Color.Transparent;
            this.lb_on_off.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lb_on_off.ForeColor = System.Drawing.Color.Transparent;
            this.lb_on_off.Location = new System.Drawing.Point(172, 102);
            this.lb_on_off.Name = "lb_on_off";
            this.lb_on_off.Size = new System.Drawing.Size(226, 82);
            this.lb_on_off.TabIndex = 99;
            this.lb_on_off.Text = "lb_on_off";
            this.lb_on_off.UseCompatibleTextRendering = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // bt_Play
            // 
            this.bt_Play.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.bt_Play.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Play.ForeColor = System.Drawing.Color.White;
            this.bt_Play.Location = new System.Drawing.Point(172, 207);
            this.bt_Play.Name = "bt_Play";
            this.bt_Play.Size = new System.Drawing.Size(226, 47);
            this.bt_Play.TabIndex = 100;
            this.bt_Play.Text = "Play";
            this.bt_Play.UseVisualStyleBackColor = false;
            this.bt_Play.Click += new System.EventHandler(this.bt_Play_Click);
            // 
            // lb_RUN
            // 
            this.lb_RUN.AutoSize = true;
            this.lb_RUN.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.lb_RUN.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_RUN.ForeColor = System.Drawing.Color.White;
            this.lb_RUN.Location = new System.Drawing.Point(242, 267);
            this.lb_RUN.Name = "lb_RUN";
            this.lb_RUN.Size = new System.Drawing.Size(82, 24);
            this.lb_RUN.TabIndex = 101;
            this.lb_RUN.Text = "RUN...";
            this.lb_RUN.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(542, 329);
            this.Controls.Add(this.lb_RUN);
            this.Controls.Add(this.bt_Play);
            this.Controls.Add(this.lb_on_off);
            this.Controls.Add(this.lb_ROBOT);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SPI_ROBOTS_LOGS    Foxconn      By: Test Engineering          Version:1.2.3";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lb_ROBOT;
        public System.Windows.Forms.Label lb_on_off;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button bt_Play;
        private System.Windows.Forms.Label lb_RUN;
    }
}

