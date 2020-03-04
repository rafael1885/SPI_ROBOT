namespace SPI_ROBOT
{
    partial class VisualizarUsuario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualizarUsuario));
            this.lb_usuario = new System.Windows.Forms.Label();
            this.bt_voltar = new System.Windows.Forms.Button();
            this.pb_foto = new System.Windows.Forms.PictureBox();
            this.ModoOperacao = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_foto)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_usuario
            // 
            resources.ApplyResources(this.lb_usuario, "lb_usuario");
            this.lb_usuario.ForeColor = System.Drawing.Color.Navy;
            this.lb_usuario.Name = "lb_usuario";
            // 
            // bt_voltar
            // 
            this.bt_voltar.BackColor = System.Drawing.SystemColors.ButtonShadow;
            resources.ApplyResources(this.bt_voltar, "bt_voltar");
            this.bt_voltar.ForeColor = System.Drawing.Color.Blue;
            this.bt_voltar.Name = "bt_voltar";
            this.bt_voltar.UseVisualStyleBackColor = false;
            this.bt_voltar.Click += new System.EventHandler(this.Bt_voltar_Click);
            // 
            // pb_foto
            // 
            this.pb_foto.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pb_foto, "pb_foto");
            this.pb_foto.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pb_foto.Name = "pb_foto";
            this.pb_foto.TabStop = false;
            // 
            // ModoOperacao
            // 
            resources.ApplyResources(this.ModoOperacao, "ModoOperacao");
            this.ModoOperacao.BackColor = System.Drawing.Color.Blue;
            this.ModoOperacao.ForeColor = System.Drawing.Color.White;
            this.ModoOperacao.Name = "ModoOperacao";
            // 
            // VisualizarUsuario
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ModoOperacao);
            this.Controls.Add(this.pb_foto);
            this.Controls.Add(this.bt_voltar);
            this.Controls.Add(this.lb_usuario);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisualizarUsuario";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pb_foto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lb_usuario;
        private System.Windows.Forms.PictureBox pb_foto;
        private System.Windows.Forms.Label ModoOperacao;
        public System.Windows.Forms.Button bt_voltar;
    }
}