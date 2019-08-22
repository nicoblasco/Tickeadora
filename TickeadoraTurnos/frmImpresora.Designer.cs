namespace TickeadoraTurnos
{
    partial class frmImpresora
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboImpresora = new System.Windows.Forms.ComboBox();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ckImagen = new System.Windows.Forms.CheckBox();
            this.txtAnteultimaLinea = new System.Windows.Forms.TextBox();
            this.txtUltimaLinea = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Impresora";
            // 
            // cboImpresora
            // 
            this.cboImpresora.FormattingEnabled = true;
            this.cboImpresora.Location = new System.Drawing.Point(107, 13);
            this.cboImpresora.Name = "cboImpresora";
            this.cboImpresora.Size = new System.Drawing.Size(398, 21);
            this.cboImpresora.TabIndex = 1;
            // 
            // btnGrabar
            // 
            this.btnGrabar.Location = new System.Drawing.Point(430, 132);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(75, 23);
            this.btnGrabar.TabIndex = 2;
            this.btnGrabar.Text = "Grabar";
            this.btnGrabar.UseVisualStyleBackColor = true;
            this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(36, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ante Última Linea";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(36, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Última Linea";
            // 
            // ckImagen
            // 
            this.ckImagen.AutoSize = true;
            this.ckImagen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ckImagen.Location = new System.Drawing.Point(39, 96);
            this.ckImagen.Name = "ckImagen";
            this.ckImagen.Size = new System.Drawing.Size(112, 17);
            this.ckImagen.TabIndex = 5;
            this.ckImagen.Text = "Incluye Imagen";
            this.ckImagen.UseVisualStyleBackColor = true;
            // 
            // txtAnteultimaLinea
            // 
            this.txtAnteultimaLinea.Location = new System.Drawing.Point(150, 41);
            this.txtAnteultimaLinea.Name = "txtAnteultimaLinea";
            this.txtAnteultimaLinea.Size = new System.Drawing.Size(355, 20);
            this.txtAnteultimaLinea.TabIndex = 6;
            // 
            // txtUltimaLinea
            // 
            this.txtUltimaLinea.Location = new System.Drawing.Point(150, 69);
            this.txtUltimaLinea.Name = "txtUltimaLinea";
            this.txtUltimaLinea.Size = new System.Drawing.Size(355, 20);
            this.txtUltimaLinea.TabIndex = 7;
            // 
            // frmImpresora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 167);
            this.Controls.Add(this.txtUltimaLinea);
            this.Controls.Add(this.txtAnteultimaLinea);
            this.Controls.Add(this.ckImagen);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGrabar);
            this.Controls.Add(this.cboImpresora);
            this.Controls.Add(this.label1);
            this.Name = "frmImpresora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impresora";
            this.Load += new System.EventHandler(this.frmImpresora_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboImpresora;
        private System.Windows.Forms.Button btnGrabar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ckImagen;
        private System.Windows.Forms.TextBox txtAnteultimaLinea;
        private System.Windows.Forms.TextBox txtUltimaLinea;
    }
}