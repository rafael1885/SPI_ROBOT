namespace SPI_ROBOT
{
    partial class HabilitarOperacaoOffline
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HabilitarOperacaoOffline));
            this.tb_suporte = new System.Windows.Forms.TextBox();
            this.lb_ID = new System.Windows.Forms.Label();
            this.bt_HabilitarOffline = new System.Windows.Forms.Button();
            this.Fechar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_suporte
            // 
            this.tb_suporte.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tb_suporte.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_suporte.ForeColor = System.Drawing.SystemColors.InfoText;
            this.tb_suporte.HideSelection = false;
            this.tb_suporte.Location = new System.Drawing.Point(152, 22);
            this.tb_suporte.MaxLength = 6;
            this.tb_suporte.Name = "tb_suporte";
            this.tb_suporte.Size = new System.Drawing.Size(142, 26);
            this.tb_suporte.TabIndex = 0;
            this.tb_suporte.TextChanged += new System.EventHandler(this.Tb_suporte_TextChanged);
            // 
            // lb_ID
            // 
            this.lb_ID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb_ID.AutoSize = true;
            this.lb_ID.BackColor = System.Drawing.Color.Transparent;
            this.lb_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ID.ForeColor = System.Drawing.Color.Chartreuse;
            this.lb_ID.Location = new System.Drawing.Point(24, 21);
            this.lb_ID.Name = "lb_ID";
            this.lb_ID.Size = new System.Drawing.Size(122, 25);
            this.lb_ID.TabIndex = 91;
            this.lb_ID.Text = "ID Suporte:";
            // 
            // bt_HabilitarOffline
            // 
            this.bt_HabilitarOffline.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bt_HabilitarOffline.BackColor = System.Drawing.Color.DarkSlateGray;
            this.bt_HabilitarOffline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_HabilitarOffline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_HabilitarOffline.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.bt_HabilitarOffline.ForeColor = System.Drawing.Color.White;
            this.bt_HabilitarOffline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bt_HabilitarOffline.Location = new System.Drawing.Point(45, 92);
            this.bt_HabilitarOffline.Name = "bt_HabilitarOffline";
            this.bt_HabilitarOffline.Size = new System.Drawing.Size(231, 68);
            this.bt_HabilitarOffline.TabIndex = 93;
            this.bt_HabilitarOffline.Text = "Habilitar Operação Offline";
            this.bt_HabilitarOffline.UseVisualStyleBackColor = false;
            this.bt_HabilitarOffline.Visible = false;
            this.bt_HabilitarOffline.Click += new System.EventHandler(this.Bt_HabilitarOffline_Click);
            // 
            // Fechar
            // 
            this.Fechar.BackColor = System.Drawing.Color.Red;
            this.Fechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Fechar.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Fechar.Location = new System.Drawing.Point(107, 179);
            this.Fechar.Name = "Fechar";
            this.Fechar.Size = new System.Drawing.Size(96, 33);
            this.Fechar.TabIndex = 97;
            this.Fechar.Text = "Fechar";
            this.Fechar.UseVisualStyleBackColor = false;
            this.Fechar.Click += new System.EventHandler(this.Fechar_Click);
            // 
            // HabilitarOperacaoOffline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(317, 224);
            this.ControlBox = false;
            this.Controls.Add(this.Fechar);
            this.Controls.Add(this.bt_HabilitarOffline);
            this.Controls.Add(this.tb_suporte);
            this.Controls.Add(this.lb_ID);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HabilitarOperacaoOffline";
            this.Text = "Habilitar Operacao Offline";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_suporte;
        private System.Windows.Forms.Label lb_ID;
        private System.Windows.Forms.Button bt_HabilitarOffline;
        private System.Windows.Forms.Button Fechar;
    }
}