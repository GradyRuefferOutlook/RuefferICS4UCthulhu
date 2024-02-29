using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace SimonSays
{
    //This was scrapped although reverse remain if you read my form1 note. Evidence of these still remain in a few locations of the code.
    /*
     * Buttons do:
     * Scroll type
     * Scroll Speed
     * Play type
     * Difficulty
     * Forward or Reverse
     * Ramping Speed
     * Instructions
     * Play Game (Middle Button)
     */
    public partial class MenuScreen : UserControl
    {
        //Define the lovecraft display
        Rectangle lovecraft = new Rectangle();
        PointF[] povCircle;

        //Holds the lovecraft animation frames (Unused as they looked odd, they were AI generated :D)
        Image[] lovecraftNorm = new Image[5];
        Image[] lovecraftLight = new Image[5];
        Image[] lovecraftDark = new Image[5];

        //Define the instruction screen
        public static Rectangle InstructionScreen = new Rectangle();
        public static Boolean showInstructions = true;

        int screenFade = 255;
        bool isOpening = true;
        public MenuScreen()
        {
            InitializeComponent();
            lovecraft = new Rectangle(23, 10, 50, 75);

            Form1.UserInput();

            DrawLovecraft();

            Form1.DrawUserInput(Form1.radius, this);

            InstructionScreen = new Rectangle(this.Height / 2 - 75, -(this.Height - 100), this.Height - 150, this.Height - 100);

            Refresh();
        }

        /// <summary>
        /// Draws the lovecraft pov display
        /// </summary>
        void DrawLovecraft()
        {
            Form1.DrawCircle(lovecraft.Height / 2 + 10);
            povCircle = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                povCircle[i] = new PointF(Form1.circList[i].X + lovecraft.Height / 2 + 10, Form1.circList[i].Y + lovecraft.Height / 2 + 10);
            }
        }




        private void MenuScreen_Paint(object sender, PaintEventArgs e)
        {
            //Draw the lovecraft display
            e.Graphics.FillPolygon(new SolidBrush(Color.LightYellow), povCircle);
            e.Graphics.DrawImage(Properties.Resources.LoveCraftBase__1_, lovecraft);

            //Draw the input
            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), Form1.circlePointsBor);
            e.Graphics.DrawPolygon(new Pen(Color.Gray), Form1.circlePointsBor);
            e.Graphics.DrawPolygon(new Pen(Color.Gray), Form1.circlePointsOuts);
            e.Graphics.DrawPolygon(new Pen(Color.Gray), Form1.circlePointsInsBor);
            e.Graphics.DrawPolygon(new Pen(Color.Gray), Form1.circlePointsIns);

            //White
            if (Form1.circlePointsIns.Length > 0)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, Form1.whiteRGB, Form1.whiteRGB, Form1.whiteRGB)), Form1.circlePointsIns);
            }

            //Red
            if (Form1.widgetRed.Length > 0)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, Form1.redR, 0, 0)), Form1.widgetRed);
            }

            //Orange
            if (Form1.widgetOrange.Length > 0)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, Form1.orangeRG, Form1.orangeRG - 125, 0)), Form1.widgetOrange);
            }

            //Yellow
            if (Form1.widgetYellow.Length > 0)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, Form1.yellowRG, Form1.yellowRG, 0)), Form1.widgetYellow);
            }

            //Green
            if (Form1.widgetGreen.Length > 0)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, 0, Form1.greenG, 0)), Form1.widgetGreen);
            }


            //Blue
            if (Form1.widgetBlue.Length > 0)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, 0, 0, Form1.blueB)), Form1.widgetBlue);
            }

            //Pink 
            if (Form1.widgetPink.Length > 0)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, 255, Form1.pinkRG, Form1.pinkRG)), Form1.widgetPink);
            }

            //Purple
            if (Form1.widgetPurple.Length > 0)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, Form1.purpleRB / 2, 0, Form1.purpleRB)), Form1.widgetPurple);
            }

            //Octarine
            if (Form1.widgetOctarine.Length > 0)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, 125, Form1.octarineG, Form1.octarineRB)), Form1.widgetOctarine);
            }

            //Print the title
            if (Form1.reverse)
            {
                e.Graphics.DrawString("tfarcevoL", Form1.font, new SolidBrush(Color.Wheat), new Point(85, 5));
            }

            else
            {
                e.Graphics.DrawString("Lovecraft", Form1.font, new SolidBrush(Color.Wheat), new Point(85, 5));
            }

            //Print instructions
            if (showInstructions)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Beige), InstructionScreen);
                e.Graphics.DrawString("INSTRUCTIONS", Form1.fancyFont, new SolidBrush(Color.Black), InstructionScreen.X, InstructionScreen.Y + 2);
                e.Graphics.DrawString("Space or W:", Form1.fancyFont, new SolidBrush(Color.Black), InstructionScreen.X, InstructionScreen.Y + 25);
                e.Graphics.DrawString("      To Select", Form1.fancyFont, new SolidBrush(Color.Black), InstructionScreen.X, InstructionScreen.Y + 50);
                e.Graphics.DrawString("S:", Form1.fancyFont, new SolidBrush(Color.Black), InstructionScreen.X, InstructionScreen.Y + 75);
                e.Graphics.DrawString("      To Start", Form1.fancyFont, new SolidBrush(Color.Black), InstructionScreen.X, InstructionScreen.Y + 100);
                e.Graphics.DrawString("A and D:", Form1.fancyFont, new SolidBrush(Color.Black), InstructionScreen.X, InstructionScreen.Y + 125);
                e.Graphics.DrawString("      To Rotate", Form1.fancyFont, new SolidBrush(Color.Black), InstructionScreen.X, InstructionScreen.Y + 150);
                e.Graphics.DrawString("Escape to Close", Form1.fancyFont, new SolidBrush(Color.Black), InstructionScreen.X, InstructionScreen.Y + 175);
            }

            //Draw the screenfade if fading in or out
            if (ScreenFader.Enabled)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(screenFade, 0, 0, 0)), new Rectangle(0, 0, this.Width, this.Height));
            }
        }

        private void MenuOp_Tick(object sender, EventArgs e)
        {
            //Check for input
            Form1.UserInput();

            //Draw the background elements, initially supposed to be animated but it looked very odd
            DrawLovecraft();

            //Draw the input
            Form1.DrawUserInput(Form1.radius, this);

            //Pull out the instructions
            if (showInstructions)
            {
                InstructionScreen.Y += 25;
                if (InstructionScreen.Y + InstructionScreen.Height > InstructionScreen.Height + 50)
                {
                    InstructionScreen.Y = 50;
                }
            }

            //Alter based on input
            if (Form1.press)
            {
                Form1.reverse = !Form1.reverse;
                Form1.press = false;
            }

            //Start the game
            if (Form1.prot)
            {
                Form1.prot = false;
                ScreenFader.Enabled = true;
                MenuOp.Enabled = false;
                LoadingScreen.sentFromMenu = true;
                return;
            }

            Refresh();
        }

        private void ScreenFader_Tick(object sender, EventArgs e)
        {
            //determine whether the screen is opening or closing
            if (isOpening)
            {
                //Increment the fade until the screen is fully visible
                screenFade -= 5;
                if (screenFade < 0)
                {
                    //Turn on the screen operations
                    isOpening = false;
                    MenuOp.Enabled = true;
                    ScreenFader.Enabled = false;
                    Form1.prot = false;
                    return;
                }
            }

            else
            {
                screenFade += 5;
                if (screenFade > 255)
                {
                    //Turn off the screen operations, Switch screens, play the switching sound
                    isOpening = true;
                    Form1.ScreenChanger(new LoadingScreen(), this);
                    Form1.continueSound.Open(new Uri(Application.StartupPath + "\\Resources\\ghost-whispers-6030 (mp3cut.net).wav"));
                    Form1.continueSound.Play();
                    showInstructions = false;
                    screenFade = 0;
                    ScreenFader.Enabled = false;
                    return;
                }
            }
            Refresh();
        }

        private void MenuScreen_Load(object sender, EventArgs e)
        {
            //Turn on the screen fade in
            ScreenFader.Enabled = true;
        }
    }
}
