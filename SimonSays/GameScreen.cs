using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Security.Policy;
using System.IO;

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        int screenFade = 255;
        Rectangle CthulhuHead;
        PointF[] cthulhuLeftEye;
        PointF[] cthulhuRightEye;

        bool hisTurn = true;
        int eyeTracker = 0;

        int turnTracker = 0;
        bool inc = true;

        public static bool correctInc = false;
        int correctAlpha = 0;

        bool isOpening = true;

        public static int flashRate = 15;

        public static System.Windows.Media.MediaPlayer redSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer orangeSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer yellowSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer greenSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer blueSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer pinkSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer purpleSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer octarineSound = new System.Windows.Media.MediaPlayer();

        public GameScreen()
        {
            InitializeComponent();
            CthulhuHead = new Rectangle(50, -10, this.Width - 100, this.Height - 100);
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            ScreenOpener.Enabled = true;
            Form1.cthulhuPattern.Clear();
            Form1.cthulhuPattern.Add(Form1.rndGen.Next(0, 8));
            //TODO: clear the pattern list from form1
            //TODO: refresh
            //TODO: pause for a bit
            //TODO: run ComputerTurn()
        }

        private void ComputerTurn()
        {
            //TODO: get rand num between 0 and 4 (0, 1, 2, 3) and add to pattern list. Each number represents a button. For example, 0 may be green, 1 may be blue, etc.

            //TODO: create a for loop that shows each value in the pattern by lighting up approriate button

            //TODO: set guess value back to 0
        }

        //TODO: create one of these event methods for each button
        private void greenButton_Click(object sender, EventArgs e)
        {
            //TODO: is the value in the pattern list at index [guess] equal to a green?
            // change button color
            // play sound
            // refresh
            // pause
            // set button colour back to original
            // add one to the guess variable

            //TODO:check to see if we are at the end of the pattern, (guess is the same as pattern list count).
            // call ComputerTurn() method
            // else call GameOver method
        }

        public void GameOver()
        {
            ScreenOpener.Enabled = true;
            return;
        }

        void DrawEyes()
        {
            Form1.DrawCircle(15);
            cthulhuLeftEye = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                cthulhuLeftEye[i] = new PointF(Form1.circList[i].X + 120, Form1.circList[i].Y + 75);
            }
            cthulhuRightEye = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                cthulhuRightEye[i] = new PointF(Form1.circList[i].X + 175, Form1.circList[i].Y + 75);
            }
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            DrawEyes();
            e.Graphics.FillPolygon(new SolidBrush(Color.Black), cthulhuLeftEye);
            e.Graphics.FillPolygon(new SolidBrush(Color.Black), cthulhuRightEye);
            if (hisTurn)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(Form1.eyeColour[eyeTracker].alpha, Form1.eyeColour[eyeTracker].red, Form1.eyeColour[eyeTracker].green, Form1.eyeColour[eyeTracker].blue)), cthulhuLeftEye);
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(Form1.eyeColour[eyeTracker].alpha, Form1.eyeColour[eyeTracker].red, Form1.eyeColour[eyeTracker].green, Form1.eyeColour[eyeTracker].blue)), cthulhuRightEye);
            }
            else if (!hisTurn)
            {
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, correctAlpha, correctAlpha, correctAlpha)), cthulhuLeftEye);
                e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, correctAlpha, correctAlpha, correctAlpha)), cthulhuRightEye);
            }

            e.Graphics.DrawImage(Properties.Resources.CthulhuHead, CthulhuHead);

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

            if (ScreenOpener.Enabled)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(screenFade, 0, 0, 0)), new Rectangle(0, 0, this.Width, this.Height));
            }

            if (MenuScreen.showInstructions)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Beige), MenuScreen.InstructionScreen);
                e.Graphics.DrawString("INSTRUCTIONS", Form1.fancyFont, new SolidBrush(Color.Black), MenuScreen.InstructionScreen.X, MenuScreen.InstructionScreen.Y + 2);
                e.Graphics.DrawString("Space or W:", Form1.fancyFont, new SolidBrush(Color.Black), MenuScreen.InstructionScreen.X, MenuScreen.InstructionScreen.Y + 25);
                e.Graphics.DrawString("      To Select", Form1.fancyFont, new SolidBrush(Color.Black), MenuScreen.InstructionScreen.X, MenuScreen.InstructionScreen.Y + 50);
                e.Graphics.DrawString("S:", Form1.fancyFont, new SolidBrush(Color.Black), MenuScreen.InstructionScreen.X, MenuScreen.InstructionScreen.Y + 75);
                e.Graphics.DrawString("      To Start", Form1.fancyFont, new SolidBrush(Color.Black), MenuScreen.InstructionScreen.X, MenuScreen.InstructionScreen.Y + 100);
                e.Graphics.DrawString("A and D:", Form1.fancyFont, new SolidBrush(Color.Black), MenuScreen.InstructionScreen.X, MenuScreen.InstructionScreen.Y + 125);
                e.Graphics.DrawString("      To Rotate", Form1.fancyFont, new SolidBrush(Color.Black), MenuScreen.InstructionScreen.X, MenuScreen.InstructionScreen.Y + 150);
                e.Graphics.DrawString("Escape to Close", Form1.fancyFont, new SolidBrush(Color.Black), MenuScreen.InstructionScreen.X, MenuScreen.InstructionScreen.Y + 175);
            }
        }

        private void ScreenOpener_Tick(object sender, EventArgs e)
        {
            if (isOpening)
            {
                screenFade -= 5;
                if (screenFade < 0)
                {
                    LovecraftOperator.Enabled = true;
                    isOpening = false;
                    ScreenOpener.Enabled = false;
                    return;
                }
                Refresh();
            }
            else
            {
                screenFade += 5;
                if (screenFade > 255)
                {
                    isOpening = true;
                    ScreenOpener.Enabled = false;
                    Form1.isOver = true;
                    Form1.continueSound.Open(new Uri(Application.StartupPath + "\\Resources\\ghost-whispers-6030 (mp3cut.net).wav"));
                    Form1.continueSound.Play();
                    Form1.ScreenChanger(new GameOverScreen(), this);
                    Form1.prot = false;
                    return;
                }
                Refresh();
            }
        }


        private void LovecraftOperator_Tick(object sender, EventArgs e)
        {
            Form1.UserInput();
            Form1.DrawUserInput(Form1.radius, this);

            if (hisTurn)
            {
                turnTracker = 0;
                LovecraftOperator.Enabled = false;
                CompTurn.Enabled = true;
                return;
            }
            else
            {
                correctInc = false;
                correctAlpha = 0;
                Form1.pPattern.Clear();
                LovecraftOperator.Enabled = false;
                YourTurn.Enabled = true;
                return;
            }
        }

        private void CompTurn_Tick(object sender, EventArgs e)
        {
            Form1.UserInput();
            Form1.DrawUserInput(Form1.radius, this);

            eyeTracker = Form1.cthulhuPattern[turnTracker];
            if (inc)
            {
                Form1.eyeColour[eyeTracker].alpha += flashRate;
            }
            else
            {
                Form1.eyeColour[eyeTracker].alpha -= flashRate;
            }

            if (Form1.eyeColour[eyeTracker].alpha > 255 && inc)
            {
                inc = !inc;
                Form1.eyeColour[eyeTracker].alpha = 255;
                playSound(eyeTracker);
            }
            else if (Form1.eyeColour[eyeTracker].alpha < 0 && !inc)
            {
                turnTracker++;
                Form1.eyeColour[eyeTracker].alpha = 0;
                inc = !inc;
            }

            if (turnTracker > Form1.cthulhuPattern.Count - 1)
            {      
                hisTurn = !hisTurn;
                LovecraftOperator.Enabled = true;
                CompTurn.Enabled = false;
                return;
            }

            Refresh();
        }

        public static void playSound(int comp)
        {

            switch (comp)
            {
                case 0:
                    //Red
                    redSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-c_91bpm_C_major.wav"));
                    redSound.Play();

                    break;
                case 1:
                    //Orange
                    orangeSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-d_96bpm_D_major.wav"));
                    orangeSound.Play();

                    break;
                case 2:
                    //Yellow
                    yellowSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-e_66bpm_E_major.wav"));
                    yellowSound.Play();

                    break;
                case 3:
                    //Green
                    greenSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-f_90bpm_F_major.wav"));
                    greenSound.Play();

                    break;
                case 4:
                    //Blue
                    blueSound.Open(new Uri(Application.StartupPath + "Resources\\church-choir-note-g_130bpm_G_major.wav"));
                    blueSound.Play();

                    break;
                case 5:
                    //Pink
                    pinkSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-g-2_69bpm_G#.wav"));
                    pinkSound.Play();

                    break;
                case 6:
                    //Purple
                    purpleSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-a_89bpm_A_major.wav"));
                    purpleSound.Play();

                    break;
                case 7:
                    //Octarine
                    octarineSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-b_112bpm_B_major.wav"));
                    octarineSound.Play();
                    break;
            }
        }

        private void YourTurn_Tick(object sender, EventArgs e)
        {
            Form1.UserInput();
            Form1.DrawUserInput(Form1.radius, this);

            if (correctInc)
            {
                correctAlpha += flashRate;
            }
            else
            {
                correctAlpha -= flashRate;
            }

            if (correctAlpha > 255)
            {
                correctAlpha = 255;
                correctInc = false;
            }
            else if (correctAlpha < 0)
            {
                correctAlpha = 0;
            }

            if (Form1.prot)
            {

            }

            if (Form1.press)
            {
                Form1.DetermineClosest();
                if (!Form1.ComparePattern())
                {
                    YourTurn.Enabled = false;
                    GameOver();
                    return;
                }
            }

            if (Form1.pPattern.Count >= Form1.cthulhuPattern.Count)
            {
                if (Form1.ComparePattern())
                {
                    Form1.cthulhuPattern.Add(Form1.rndGen.Next(0, 8));
                    hisTurn = true;
                    LovecraftOperator.Enabled = true;
                    YourTurn.Enabled = false;
                    return;
                }
                else
                {
                    YourTurn.Enabled = false;
                    GameOver();
                    return;
                }
            }

            Refresh();
        }
    }
}
