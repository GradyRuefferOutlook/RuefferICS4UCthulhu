namespace SimonSays
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
            this.OctarineGenerator = new System.Windows.Forms.Timer(this.components);
            this.ColourGenerator = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // OctarineGenerator
            // 
            this.OctarineGenerator.Enabled = true;
            this.OctarineGenerator.Interval = 20;
            this.OctarineGenerator.Tick += new System.EventHandler(this.OctarineGenerator_Tick);
            // 
            // ColourGenerator
            // 
            this.ColourGenerator.Enabled = true;
            this.ColourGenerator.Interval = 20;
            this.ColourGenerator.Tick += new System.EventHandler(this.ColourGenerator_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(301, 300);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simon Says";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer ColourGenerator;
        public System.Windows.Forms.Timer OctarineGenerator;
    }
}

