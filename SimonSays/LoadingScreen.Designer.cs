namespace SimonSays
{
    partial class LoadingScreen
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LoadingOp = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // LoadingOp
            // 
            this.LoadingOp.Tick += new System.EventHandler(this.LoadingOp_Tick);
            // 
            // LoadingScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.DoubleBuffered = true;
            this.Name = "LoadingScreen";
            this.Size = new System.Drawing.Size(401, 369);
            this.Load += new System.EventHandler(this.LoadingScreen_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.LoadingScreen_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer LoadingOp;
    }
}
