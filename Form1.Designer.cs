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
            this.SuspendLayout();
            // 
            // lb_ROBOT
            // 
            this.lb_ROBOT.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb_ROBOT.BackColor = System.Drawing.Color.Transparent;
            this.lb_ROBOT.Font = new System.Drawing.Font("Arial Narrow", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ROBOT.ForeColor = System.Drawing.Color.Yellow;
            this.lb_ROBOT.Location = new System.Drawing.Point(80, 19);
            this.lb_ROBOT.Name = "lb_ROBOT";
            this.lb_ROBOT.Size = new System.Drawing.Size(409, 83);
            this.lb_ROBOT.TabIndex = 98;
            this.lb_ROBOT.Text = "ROBOT_LOGS_SPI";
            this.lb_ROBOT.Click += new System.EventHandler(this.lb_ROBOT_Click);
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
            this.timer1.Interval = 8000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(542, 275);
            this.Controls.Add(this.lb_on_off);
            this.Controls.Add(this.lb_ROBOT);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SPI_ROBOTS_LOGS    Foxconn      By: Test Engineering          Version:1.2.3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lb_ROBOT;
        public System.Windows.Forms.Label lb_on_off;
        private System.Windows.Forms.Timer timer1;
    }
}

