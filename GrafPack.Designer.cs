namespace GrafPack
{
    partial class GrafPack
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
            this.SuspendLayout();
            // 
            // GrafPack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 494);
            this.Name = "GrafPack";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.GrafPack_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GrafPack_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GrafPack_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GrafPack_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}