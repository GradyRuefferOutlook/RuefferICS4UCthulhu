namespace SimonSays
{
    partial class GameScreen
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
            this.ScreenOpener = new System.Windows.Forms.Timer(this.components);
            this.LovecraftOperator = new System.Windows.Forms.Timer(this.components);
            this.CompTurn = new System.Windows.Forms.Timer(this.components);
            this.YourTurn = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ScreenOpener
            // 
            this.ScreenOpener.Interval = 20;
            this.ScreenOpener.Tick += new System.EventHandler(this.ScreenOpener_Tick);
            // 
            // LovecraftOperator
            // 
            this.LovecraftOperator.Interval = 20;
            this.LovecraftOperator.Tick += new System.EventHandler(this.LovecraftOperator_Tick);
            // 
            // CompTurn
            // 
            this.CompTurn.Interval = 20;
            this.CompTurn.Tick += new System.EventHandler(this.CompTurn_Tick);
            // 
            // YourTurn
            // 
            this.YourTurn.Interval = 20;
            this.YourTurn.Tick += new System.EventHandler(this.YourTurn_Tick);
            // 
            // GameScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::SimonSays.Properties.Resources.CthulhuBackdrop;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GameScreen";
            this.Size = new System.Drawing.Size(401, 369);
            this.Load += new System.EventHandler(this.GameScreen_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameScreen_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer ScreenOpener;
        private System.Windows.Forms.Timer LovecraftOperator;
        private System.Windows.Forms.Timer CompTurn;
        private System.Windows.Forms.Timer YourTurn;
    }
}
