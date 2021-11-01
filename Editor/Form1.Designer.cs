namespace Editor
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.gLControl = new OpenTK.GLControl();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gLControl
            // 
            //this.gLControl.AccessibleDescription = "ControlGl";
            //this.gLControl.AccessibleName = "ControlGl";
            //this.gLControl.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            //this.gLControl.BackColor = System.Drawing.Color.Black;
            //this.gLControl.Location = new System.Drawing.Point(287, 124);
            //this.gLControl.Name = "gLControl";
            //this.gLControl.Size = new System.Drawing.Size(205, 190);
            //this.gLControl.TabIndex = 0;
            //this.gLControl.VSync = false;
            this.gLControl.Load += new System.EventHandler(this.GLControl_Load);
            this.gLControl.Resize += new System.EventHandler(this.GLControl_Resize);
            this.gLControl.Paint += new System.Windows.Forms.PaintEventHandler(this.GLControl_Paint);
            this.gLControl.Location = new System.Drawing.Point(-2, 0);
            this.gLControl.Name = "gLControl";
            this.gLControl.Size = new System.Drawing.Size(500, 300);
            this.gLControl.TabIndex = 0;
            this.gLControl.VSync = false;
            this.Controls.Add(this.gLControl);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(369, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 462);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gLControl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl gLControl;
        private System.Windows.Forms.Label label1;
    }
}

