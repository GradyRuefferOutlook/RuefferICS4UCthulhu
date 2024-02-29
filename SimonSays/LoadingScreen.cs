using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

//This is not an actual loading menu, it runs the same time all the time just for fun
namespace SimonSays
{
    public partial class LoadingScreen : UserControl
    {
        //Initalize loading display
        string loadingDisplay = "Loading";
        int loadOp = 0;

        //Store run cycle
        Image[] lovecraftRun = new Image[15];
        int runOp = 0;
        int runCount = 0;

        //Operator for moving between the menu and game screen
        public static bool sentFromMenu = true;
        public LoadingScreen()
        {
            InitializeComponent();

            //Store the images
            lovecraftRun[0] = Properties.Resources.LoveRun__1_;
            lovecraftRun[1] = Properties.Resources.LoveRun__2_;
            lovecraftRun[2] = Properties.Resources.LoveRun__3_;
            lovecraftRun[3] = Properties.Resources.LoveRun__4_;
            lovecraftRun[4] = Properties.Resources.LoveRun__5_;
            lovecraftRun[5] = Properties.Resources.LoveRun__6_;
            lovecraftRun[6] = Properties.Resources.LoveRun__7_;
            lovecraftRun[7] = Properties.Resources.LoveRun__8_;
            lovecraftRun[8] = Properties.Resources.LoveRun__9_;
            lovecraftRun[9] = Properties.Resources.LoveRun__10_;
            lovecraftRun[10] = Properties.Resources.LoveRun__11_;
            lovecraftRun[11] = Properties.Resources.LoveRun__12_;
            lovecraftRun[12] = Properties.Resources.LoveRun__13_;
            lovecraftRun[13] = Properties.Resources.LoveRun__14_;
            lovecraftRun[14] = Properties.Resources.LoveRun__15_;
        }

        private void LoadingOp_Tick(object sender, EventArgs e)
        {
            //Increment loading dots
            loadOp++;
            if (loadOp > 5)
            {
                loadOp = 1;
            }

            loadingDisplay = "Loading";

            //Display loading dots
            for (int i = 0; i < loadOp; i++)
            {
                loadingDisplay += ".";
            }

            //Increment run animation
            runOp++;
            if (runOp > 14)
            {
                runOp = 0;
                runCount++;
            }

            //If the run cycle has been completed a given number of times, change the screen
            if (runCount == 2)
            {
                //Sent from menu, go to game
                if (sentFromMenu)
                {
                    Form1.ScreenChanger(new GameScreen(), this);
                    Form1.inGame = true;
                }

                //Sent from game, go to menu
                else
                {
                    Form1.ScreenChanger(new MenuScreen(), this);
                    Form1.continueSound.Open(new Uri(Application.StartupPath + "\\Resources\\ghost-whispers-6030 (mp3cut.net).wav"));
                    Form1.continueSound.Play();
                    Form1.inGame = false;
                }

                //Reset values for reuse
                runCount = 0;
                runOp = 0;
                LoadingOp.Enabled = false;

                return;
            }

            Refresh();
        }

        private void LoadingScreen_Paint(object sender, PaintEventArgs e)
        {
            //Draw the loading string and run cycle
            e.Graphics.DrawString(loadingDisplay, Form1.font, new SolidBrush(Color.Wheat), new Point(10, this.Height - 50));
            e.Graphics.DrawImage(lovecraftRun[runOp], new Rectangle(this.Width - 80, this.Height - 105, 75, 100));
        }

        private void LoadingScreen_Load(object sender, EventArgs e)
        {
            //Start the operator
            LoadingOp.Enabled = true;
        }
    }
}
