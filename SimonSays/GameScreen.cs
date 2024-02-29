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
        //set the fade in variable
        int screenFade = 255;

        //Define cthulhu
        Rectangle CthulhuHead;
        PointF[] cthulhuLeftEye;
        PointF[] cthulhuRightEye;

        //Operate the turns
        bool hisTurn = true;
        int eyeTracker = 0;

        //Operate user turn
        int turnTracker = 0;
        bool inc = true;

        //Operates eye flashing on correct input
        public static bool correctInc = false;
        int correctAlpha = 0;

        //operates screen fade
        bool isOpening = true;

        //Operates how fast the eyes flash
        public static int flashRate = 15;

        //Initialize local sounds
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
            
            //Define cthulhu size
            CthulhuHead = new Rectangle(50, -10, this.Width - 100, this.Height - 100);
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            ScreenOpener.Enabled = true;
            
            //Clear the pattern and generate the first pattern
            Form1.cthulhuPattern.Clear();
            Form1.cthulhuPattern.Add(Form1.rndGen.Next(0, 8));
        }

        public void GameOver()
        {
            //Fade the screen out
            ScreenOpener.Enabled = true;
            return;
        }

        void DrawEyes()
        {
            //Draw a circle and fill in an array with the circle points to substitute the eye
            //To give the effect of the eyes actually glowing, the circles are placed behind the head
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
            //Draw eyes and fill in black to hide the background from shining through
            DrawEyes();
            e.Graphics.FillPolygon(new SolidBrush(Color.Black), cthulhuLeftEye);
            e.Graphics.FillPolygon(new SolidBrush(Color.Black), cthulhuRightEye);

            //Colour the eyes appropriately based on eye flashing and turn order
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

            //Draw the cthulhu head
            e.Graphics.DrawImage(Properties.Resources.CthulhuHead, CthulhuHead);

            //Draw the user control
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

            //Fade the screen if neccessary
            if (ScreenOpener.Enabled)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(screenFade, 0, 0, 0)), new Rectangle(0, 0, this.Width, this.Height));
            }

            //Show instructions if toggled
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
            //Same as other screens, increments or lowers an integer to fade in or out 
            if (isOpening)
            {
                screenFade -= 5;
                if (screenFade < 0)
                {
                    //Starts the game
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
                    //Changes to the game over screen
                    isOpening = true;
                    ScreenOpener.Enabled = false;
                    Form1.isOver = true;
                    Form1.ScreenChanger(new GameOverScreen(), this);
                    Form1.prot = false;
                    return;
                }
                Refresh();
            }
        }


        private void LovecraftOperator_Tick(object sender, EventArgs e)
        {
            //Check and draw the input
            Form1.UserInput();
            Form1.DrawUserInput(Form1.radius, this);

            //Determines the turn and starts the turn
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
            //Check and draw the input
            Form1.UserInput();
            Form1.DrawUserInput(Form1.radius, this);

            //Operates the flashing of the eyes to show the pattern
            eyeTracker = Form1.cthulhuPattern[turnTracker];
            if (inc)
            {
                Form1.eyeColour[eyeTracker].alpha += flashRate;
            }
            else
            {
                Form1.eyeColour[eyeTracker].alpha -= flashRate;
            }

            //Play the note when the colour is at its peak
            if (Form1.eyeColour[eyeTracker].alpha > 255 && inc)
            {
                inc = !inc;
                Form1.eyeColour[eyeTracker].alpha = 255;
                playSound(eyeTracker);
            }
            //Works through the pattern if the blinking is done
            else if (Form1.eyeColour[eyeTracker].alpha < 0 && !inc)
            {
                turnTracker++;
                Form1.eyeColour[eyeTracker].alpha = 0;
                inc = !inc;
            }

            //When the pattern is finished, passes the turn
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
            //Simply for playing local sounds based on input
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
            //Check and draw the input
            Form1.UserInput();
            Form1.DrawUserInput(Form1.radius, this);

            //Operates the eyes on correct input
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

            //No current use in-game
            if (Form1.prot)
            {

            }

            //Input the pattern
            if (Form1.press)
            {
                //Determine the selected widget
                Form1.DetermineClosest();

                //Runs comparison to end the game early if you get it incorrect
                if (!Form1.ComparePattern())
                {
                    //Ends the turn and passes to game over
                    YourTurn.Enabled = false;
                    GameOver();
                    return;
                }
            }

            //Determines if the pattern has been met and passes the turn when true
            if (Form1.pPattern.Count >= Form1.cthulhuPattern.Count)
            {
                if (Form1.ComparePattern())
                {
                    //Adds a new piece to the pattern and passes the turn
                    Form1.cthulhuPattern.Add(Form1.rndGen.Next(0, 8));
                    hisTurn = true;
                    LovecraftOperator.Enabled = true;
                    YourTurn.Enabled = false;
                    return;
                }
                else
                {
                    //passes to game over
                    YourTurn.Enabled = false;
                    GameOver();
                    return;
                }
            }

            Refresh();
        }
    }
}
