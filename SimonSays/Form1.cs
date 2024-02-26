using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.IO;
//Grady Rueffer 152446035 Simon Game
//Welcome Mr.T
//The instructions should run on startup
//One small note, press the input key to reverse the game, you'll see this in the name of the game
//Have fun!


namespace SimonSays
{
    public partial class Form1 : Form
    {
        public static Random rndGen = new Random();

        public static List<PointF> circList = new List<PointF>();
        public static Int32 circDrawTime = 0;
        public static int radius = 50;
        public static PointF[] circlePointsBor, circlePointsInsBor, circlePointsIns, circlePointsOuts;

        public static PointF[] widgetRed, widgetOrange, widgetYellow, widgetGreen, widgetBlue, widgetPink, widgetPurple, widgetOctarine;

        public static int octarineRB = 125, octarineG = 125, redR = 125, orangeRG = 125, yellowRG = 125, greenG = 125, blueB = 125, pinkRG = 125, purpleRB = 125, whiteRGB = 125;

        public static int distance = 0;

        public static Boolean moveFor, moveBac, prot, press;

        public static List<Int64> protectionPattern = new List<Int64>();

        public static bool playType = false, scrollType = false;
        public static int scrollSpeed = 5;

        FontFamily fontFamily = new FontFamily("Perpetua");
        public static Font font = new Font(new FontFamily("Perpetua"), 35, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font fancyFont = new Font(new FontFamily("Antiquity Print"), 11, FontStyle.Bold, GraphicsUnit.Pixel);

        public static List<int> cthulhuPattern = new List<int>();
        public static List<int> pPattern = new List<int>();
        public static ARGB[] eyeColour = {new ARGB(), new ARGB(), new ARGB(), new ARGB(), new ARGB(), new ARGB(), new ARGB(), new ARGB()};

        public static bool inGame = false;
        public static bool reverse = false;
        public static bool isOver = false;

        public static System.Windows.Media.MediaPlayer backMusic = new System.Windows.Media.MediaPlayer();

        public static System.Windows.Media.MediaPlayer instructSound = new System.Windows.Media.MediaPlayer();

        public static System.Windows.Media.MediaPlayer continueSound = new System.Windows.Media.MediaPlayer();

        public class ARGB
        {
            public int alpha;
            public int red;
            public int green;
            public int blue;

            public void Setup(int a, int r, int g, int b)
            {
                alpha = a;
                red = r;
                green = g;
                blue = b;
            }
        }

        public Form1()
        {
            InitializeComponent();
            eyeColour[0].Setup(0, 255, 0, 0);
            eyeColour[1].Setup(0, 255, 165, 0);
            eyeColour[2].Setup(0, 255, 255, 0);
            eyeColour[3].Setup(0, 0, 255, 0);
            eyeColour[4].Setup(0, 0, 0, 255);
            eyeColour[5].Setup(0, 255, 192, 203);
            eyeColour[6].Setup(0, 160, 32, 240);
            eyeColour[7].Setup(0, octarineRB, octarineG, octarineRB);
            radius = (this.Width / 2 + 10);
            distance = -radius;
            this.KeyPreview = true;
            backMusic.Open(new Uri(Application.StartupPath + "\\Resources\\spellcraft-142264 (mp3cut.net).wav"));

            backMusic.MediaEnded += new EventHandler(backMedia_MediaEnded);
            backMusic.Play();
        }

        private void backMedia_MediaEnded(object sender, EventArgs e)
        {
            backMusic.Stop();

            backMusic.Play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ScreenChanger(new MenuScreen(), this);
        }

        public static void ScreenChanger(UserControl next, object sender)
        {
            Form f;
            UserControl current = new UserControl();

            if (sender is Form)
            {
                f = (Form)sender;
            }

            else
            {
                current = (UserControl)sender;
                f = current.FindForm();
                f.Controls.Remove(current);
            }

            next.Location = new Point((f.Width - next.Width) / 2, (f.Height - next.Height) / 2);

            f.Controls.Add(next);
        }

        private void OctarineGenerator()
        {
            if (rndGen.Next(0, 2) == 0)
            {
                octarineG += 5;
            }
            else
            {
                octarineG -= 5;
            }

            if (rndGen.Next(0, 2) == 0)
            {
                octarineRB += 5;
            }
            else
            {
                octarineRB -= 5;
            }

            if (octarineG > 255)
            {
                octarineG = 150;
            }
            else if (octarineG < 175)
            {
                octarineG = 0;
            }

            if (octarineRB > 255)
            {
                octarineRB = 255;
            }
            else if (octarineRB < 175)
            {
                octarineRB = 175;
            }
        }

        private void ColourGenerator_Tick(object sender, EventArgs e)
        {
            redR = ColourAdjuster(redR);
            yellowRG = ColourAdjuster(orangeRG);
            orangeRG = ColourAdjuster(orangeRG);
            greenG = ColourAdjuster(orangeRG);
            blueB = ColourAdjuster(orangeRG);
            pinkRG = ColourAdjuster(pinkRG);
            purpleRB = ColourAdjuster(purpleRB);
            whiteRGB = ColourAdjuster(whiteRGB);
            OctarineGenerator();
        }

        int ColourAdjuster(int colour)
        {
            if (rndGen.Next(0, 2) == 0)
            {
                colour += 5;
            }
            else
            {
                colour -= 5;
            }
            if (colour > 255)
            {
                colour = 255;
            }
            else if (colour < 125)
            {
                colour = 125;
            }
            return colour;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isOver)
            {
                isOver = !isOver;
                return;
            }
            switch (e.KeyCode)
            {
                case Keys.A:
                    if (scrollType)
                    {
                        moveBac = true;
                    }
                    else
                    {
                        moveFor = true;
                    }
                    break;
                case Keys.D:
                    if (scrollType)
                    {
                        moveFor = true;
                    }
                    else
                    {
                        moveBac = true;
                    }
                    break;
                case Keys.S:
                    prot = false;
                    break;
                case Keys.W:
                    press = false;
                    break;
                case Keys.Escape:
                    MenuScreen.showInstructions = !MenuScreen.showInstructions;
                    instructSound.Open(new Uri(Application.StartupPath + "\\Resources\\handle-paper-foley-2-172689 (mp3cut.net).wav"));
                    instructSound.Play();
                    break;
                case Keys.Space:
                    press = false;
                    break;

            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {           
            switch (e.KeyCode)
            {
                case Keys.A:
                    if (scrollType)
                    {
                        moveBac = false;
                    }
                    else
                    {
                        moveFor = false;
                    }
                    break;
                case Keys.D:
                    if (scrollType)
                    {
                        moveFor = false;
                    }
                    else
                    {
                        moveBac = false;
                    }
                    break;
                case Keys.S:
                    prot = true;
                    break;
                case Keys.W:
                    press = true;
                    break;
                case Keys.Space:
                    press = true;
                    break;
            }
        }

        public static void UserInput()
        {
            if (!playType)
            {
                if (moveFor && !moveBac)
                {
                    distance -= scrollSpeed;
                }

                else if (moveBac && !moveFor)
                {
                    distance += scrollSpeed;
                }
            }
        }

        static void swap(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        // This function takes last element as pivot,
        // places the pivot element at its correct position
        // in sorted array, and places all smaller to left
        // of pivot and all greater elements to right of pivot
        static int partition(int[] arr, int low, int high)
        {
            // Choosing the pivot
            int pivot = arr[high];

            // Index of smaller element and indicates
            // the right position of pivot found so far
            int i = (low - 1);

            for (int j = low; j <= high - 1; j++)
            {

                // If current element is smaller than the pivot
                if (arr[j] < pivot)
                {

                    // Increment index of smaller element
                    i++;
                    swap(arr, i, j);
                }
            }
            swap(arr, i + 1, high);
            return (i + 1);
        }

        // The main function that implements QuickSort
        // arr[] --> Array to be sorted,
        // low --> Starting index,
        // high --> Ending index
        static public void quickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                // pi is partitioning index, arr[p]
                // is now at right place
                int pi = partition(arr, low, high);

                // Separately sort elements before
                // and after partition index
                quickSort(arr, low, pi - 1);
                quickSort(arr, pi + 1, high);
            }
        }


        public static void DrawUserInput(int radius, Control controller)
        {
            if (distance < -863)
            {
                distance = -radius;
            }
            else if (distance > 543)
            {
                distance = -radius;
            }

            #region draw input borders
            DrawCircle(radius + 25);
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (controller.Height / 2) + 50);
            }
            Form1.circlePointsBor = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circlePointsBor[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
            }

            DrawCircle(radius + 15);
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (controller.Height / 2) + 50);
            }
            Form1.circlePointsOuts = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circlePointsOuts[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
            }

            DrawCircle(radius - 40);
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (controller.Height / 2) + 50);
            }
            Form1.circlePointsInsBor = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circlePointsInsBor[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
            }

            DrawCircle(radius - 50);
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (controller.Height / 2) + 50);
            }
            Form1.circlePointsIns = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circlePointsIns[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
            }
            #endregion

            #region define borders
            double circIn = 2 * (radius - 40) * Math.PI;
            double circOut = 2 * (radius - 40) * Math.PI;
            //double distance = 0  - (radius / 4.25);
            #endregion

            #region red widget
            if (Form1.distance < -radius - 150)
            {
                DrawWidget(Form1.distance + 530 + radius, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetRedTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetRedTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + 530 + radius, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetRedBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetRedBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetRed = new PointF[widgetRedBot.Length + widgetRedTop.Length];
                for (int i = 0; i < widgetRedTop.Length; i++)
                {
                    Form1.widgetRed[i] = new PointF(widgetRedTop[i].X, widgetRedTop[i].Y);
                }
                for (int i = widgetRedTop.Length - 1; i < Form1.widgetRed.Length - 1; i++)
                {
                    Form1.widgetRed[i] = new PointF(widgetRedBot[i + 1 - widgetRedTop.Length].X, widgetRedBot[i + 1 - widgetRedTop.Length].Y);
                }

                if (Form1.widgetRed.Length > 0)
                {
                    Form1.widgetRed[Form1.widgetRed.Length - 1] = new PointF(Form1.widgetRed[Form1.widgetRed.Length - 2].X, Form1.widgetRed[Form1.widgetRed.Length - 2].Y);
                }
            }
            else if (Form1.distance < -radius + 360)
            {
                DrawWidget(Form1.distance - radius / 10, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetRedTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetRedTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance - radius / 10, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetRedBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetRedBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetRed = new PointF[widgetRedBot.Length + widgetRedTop.Length];
                for (int i = 0; i < widgetRedTop.Length; i++)
                {
                    Form1.widgetRed[i] = new PointF(widgetRedTop[i].X, widgetRedTop[i].Y);
                }
                for (int i = widgetRedTop.Length - 1; i < Form1.widgetRed.Length - 1; i++)
                {
                    Form1.widgetRed[i] = new PointF(widgetRedBot[i + 1 - widgetRedTop.Length].X, widgetRedBot[i + 1 - widgetRedTop.Length].Y);
                }

                if (Form1.widgetRed.Length > 0)
                {
                    Form1.widgetRed[Form1.widgetRed.Length - 1] = new PointF(Form1.widgetRed[Form1.widgetRed.Length - 2].X, Form1.widgetRed[Form1.widgetRed.Length - 2].Y);
                }
            }
            else
            {
                DrawWidget(Form1.distance - 562 - radius, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetRedTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetRedTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance - 562 - radius, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetRedBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetRedBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetRed = new PointF[widgetRedBot.Length + widgetRedTop.Length];
                for (int i = 0; i < widgetRedTop.Length; i++)
                {
                    Form1.widgetRed[i] = new PointF(widgetRedTop[i].X, widgetRedTop[i].Y);
                }
                for (int i = widgetRedTop.Length - 1; i < Form1.widgetRed.Length - 1; i++)
                {
                    Form1.widgetRed[i] = new PointF(widgetRedBot[i + 1 - widgetRedTop.Length].X, widgetRedBot[i + 1 - widgetRedTop.Length].Y);
                }

                if (Form1.widgetRed.Length > 0)
                {
                    Form1.widgetRed[Form1.widgetRed.Length - 1] = new PointF(Form1.widgetRed[Form1.widgetRed.Length - 2].X, Form1.widgetRed[Form1.widgetRed.Length - 2].Y);
                }
            }
            #endregion

            #region orange widget
            if (Form1.distance < -radius - 250)
            {
                DrawWidget(Form1.distance + (53 + radius / 10) * 11.25, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetOrangeTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOrangeTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (53 + radius / 10) * 11.25, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetOrangeBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOrangeBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetOrange = new PointF[widgetOrangeBot.Length + widgetOrangeTop.Length];
                for (int i = 0; i < widgetOrangeTop.Length; i++)
                {
                    Form1.widgetOrange[i] = new PointF(widgetOrangeTop[i].X, widgetOrangeTop[i].Y);
                }
                for (int i = widgetOrangeTop.Length - 1; i < Form1.widgetOrange.Length - 1; i++)
                {
                    Form1.widgetOrange[i] = new PointF(widgetOrangeBot[i + 1 - widgetOrangeTop.Length].X, widgetOrangeBot[i + 1 - widgetOrangeTop.Length].Y);
                }

                if (Form1.widgetOrange.Length > 0)
                {
                    Form1.widgetOrange[Form1.widgetOrange.Length - 1] = new PointF(Form1.widgetOrange[Form1.widgetOrange.Length - 2].X, Form1.widgetOrange[Form1.widgetOrange.Length - 2].Y);
                }
            }
            else if (Form1.distance < -radius + 260)
            {
                DrawWidget(Form1.distance + 50 + (radius / 10) * 1.25, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetOrangeTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOrangeTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + 50 + (radius / 10) * 1.25, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetOrangeBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOrangeBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetOrange = new PointF[widgetOrangeBot.Length + widgetOrangeTop.Length];
                for (int i = 0; i < widgetOrangeTop.Length; i++)
                {
                    Form1.widgetOrange[i] = new PointF(widgetOrangeTop[i].X, widgetOrangeTop[i].Y);
                }
                for (int i = widgetOrangeTop.Length - 1; i < Form1.widgetOrange.Length - 1; i++)
                {
                    Form1.widgetOrange[i] = new PointF(widgetOrangeBot[i + 1 - widgetOrangeTop.Length].X, widgetOrangeBot[i + 1 - widgetOrangeTop.Length].Y);
                }

                if (Form1.widgetOrange.Length > 0)
                {
                    Form1.widgetOrange[Form1.widgetOrange.Length - 1] = new PointF(Form1.widgetOrange[Form1.widgetOrange.Length - 2].X, Form1.widgetOrange[Form1.widgetOrange.Length - 2].Y);
                }
            }
            else
            {
                DrawWidget(Form1.distance + (56.25 + radius / 10) * -8.75, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetOrangeTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOrangeTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (56.25 + radius / 10) * -8.75, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetOrangeBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOrangeBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetOrange = new PointF[widgetOrangeBot.Length + widgetOrangeTop.Length];
                for (int i = 0; i < widgetOrangeTop.Length; i++)
                {
                    Form1.widgetOrange[i] = new PointF(widgetOrangeTop[i].X, widgetOrangeTop[i].Y);
                }
                for (int i = widgetOrangeTop.Length - 1; i < Form1.widgetOrange.Length - 1; i++)
                {
                    Form1.widgetOrange[i] = new PointF(widgetOrangeBot[i + 1 - widgetOrangeTop.Length].X, widgetOrangeBot[i + 1 - widgetOrangeTop.Length].Y);
                }

                if (Form1.widgetOrange.Length > 0)
                {
                    Form1.widgetOrange[Form1.widgetOrange.Length - 1] = new PointF(Form1.widgetOrange[Form1.widgetOrange.Length - 2].X, Form1.widgetOrange[Form1.widgetOrange.Length - 2].Y);
                }
            }
            #endregion

            #region yellow widget
            if (Form1.distance < -radius - 350)
            {
                DrawWidget(Form1.distance + (53 + radius / 10) * 12.5, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetYellowTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetYellowTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (53 + radius / 10) * 12.5, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetYellowBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetYellowBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetYellow = new PointF[widgetYellowBot.Length + widgetYellowTop.Length];
                for (int i = 0; i < widgetYellowTop.Length; i++)
                {
                    Form1.widgetYellow[i] = new PointF(widgetYellowTop[i].X, widgetYellowTop[i].Y);
                }
                for (int i = widgetYellowTop.Length - 1; i < Form1.widgetYellow.Length - 1; i++)
                {
                    Form1.widgetYellow[i] = new PointF(widgetYellowBot[i + 1 - widgetYellowTop.Length].X, widgetYellowBot[i + 1 - widgetYellowTop.Length].Y);
                }
                if (Form1.widgetYellow.Length > 0)
                {
                    Form1.widgetYellow[Form1.widgetYellow.Length - 1] = new PointF(Form1.widgetYellow[Form1.widgetYellow.Length - 2].X, Form1.widgetYellow[Form1.widgetYellow.Length - 2].Y);
                }
            }
            else if (Form1.distance < -radius + 160)
            {
                DrawWidget(Form1.distance + (50 + radius / 10) * 2.4, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetYellowTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetYellowTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (50 + radius / 10) * 2.4, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetYellowBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetYellowBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetYellow = new PointF[widgetYellowBot.Length + widgetYellowTop.Length];
                for (int i = 0; i < widgetYellowTop.Length; i++)
                {
                    Form1.widgetYellow[i] = new PointF(widgetYellowTop[i].X, widgetYellowTop[i].Y);
                }
                for (int i = widgetYellowTop.Length - 1; i < Form1.widgetYellow.Length - 1; i++)
                {
                    Form1.widgetYellow[i] = new PointF(widgetYellowBot[i + 1 - widgetYellowTop.Length].X, widgetYellowBot[i + 1 - widgetYellowTop.Length].Y);
                }
                if (Form1.widgetYellow.Length > 0)
                {
                    Form1.widgetYellow[Form1.widgetYellow.Length - 1] = new PointF(Form1.widgetYellow[Form1.widgetYellow.Length - 2].X, Form1.widgetYellow[Form1.widgetYellow.Length - 2].Y);
                }
            }
            else
            {
                DrawWidget(Form1.distance + (56.5 + radius / 10) * -7.5, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetYellowTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetYellowTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (56.5 + radius / 10) * -7.5, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetYellowBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetYellowBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetYellow = new PointF[widgetYellowBot.Length + widgetYellowTop.Length];
                for (int i = 0; i < widgetYellowTop.Length; i++)
                {
                    Form1.widgetYellow[i] = new PointF(widgetYellowTop[i].X, widgetYellowTop[i].Y);
                }
                for (int i = widgetYellowTop.Length - 1; i < Form1.widgetYellow.Length - 1; i++)
                {
                    Form1.widgetYellow[i] = new PointF(widgetYellowBot[i + 1 - widgetYellowTop.Length].X, widgetYellowBot[i + 1 - widgetYellowTop.Length].Y);
                }
                if (Form1.widgetYellow.Length > 0)
                {
                    Form1.widgetYellow[Form1.widgetYellow.Length - 1] = new PointF(Form1.widgetYellow[Form1.widgetYellow.Length - 2].X, Form1.widgetYellow[Form1.widgetYellow.Length - 2].Y);
                }
            }
            #endregion

            #region green widget
            if (Form1.distance < -radius - 350)
            {
                DrawWidget(Form1.distance + (53 + radius / 10) * 13.75, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetGreenTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetGreenTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (53 + radius / 10) * 13.75, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetGreenBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetGreenBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetGreen = new PointF[widgetGreenBot.Length + widgetGreenTop.Length];
                for (int i = 0; i < widgetGreenTop.Length; i++)
                {
                    Form1.widgetGreen[i] = new PointF(widgetGreenTop[i].X, widgetGreenTop[i].Y);
                }
                for (int i = widgetGreenTop.Length - 1; i < Form1.widgetGreen.Length - 1; i++)
                {
                    Form1.widgetGreen[i] = new PointF(widgetGreenBot[i + 1 - widgetGreenTop.Length].X, widgetGreenBot[i + 1 - widgetGreenTop.Length].Y);
                }
                if (Form1.widgetGreen.Length > 0)
                {
                    Form1.widgetGreen[Form1.widgetGreen.Length - 1] = new PointF(Form1.widgetGreen[Form1.widgetGreen.Length - 2].X, Form1.widgetGreen[Form1.widgetGreen.Length - 2].Y);
                }
            }
            else if (Form1.distance < -radius + 60)
            {
                DrawWidget(Form1.distance + (50 + radius / 10) * 3.75, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetGreenTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetGreenTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (50 + radius / 10) * 3.75, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetGreenBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetGreenBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetGreen = new PointF[widgetGreenBot.Length + widgetGreenTop.Length];
                for (int i = 0; i < widgetGreenTop.Length; i++)
                {
                    Form1.widgetGreen[i] = new PointF(widgetGreenTop[i].X, widgetGreenTop[i].Y);
                }
                for (int i = widgetGreenTop.Length - 1; i < Form1.widgetGreen.Length - 1; i++)
                {
                    Form1.widgetGreen[i] = new PointF(widgetGreenBot[i + 1 - widgetGreenTop.Length].X, widgetGreenBot[i + 1 - widgetGreenTop.Length].Y);
                }
                if (Form1.widgetGreen.Length > 0)
                {
                    Form1.widgetGreen[Form1.widgetGreen.Length - 1] = new PointF(Form1.widgetGreen[Form1.widgetGreen.Length - 2].X, Form1.widgetGreen[Form1.widgetGreen.Length - 2].Y);
                }
            }
            else
            {
                DrawWidget(Form1.distance + (56.75 + radius / 10) * -6.25, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetGreenTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetGreenTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (56.75 + radius / 10) * -6.25, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetGreenBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetGreenBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetGreen = new PointF[widgetGreenBot.Length + widgetGreenTop.Length];
                for (int i = 0; i < widgetGreenTop.Length; i++)
                {
                    Form1.widgetGreen[i] = new PointF(widgetGreenTop[i].X, widgetGreenTop[i].Y);
                }
                for (int i = widgetGreenTop.Length - 1; i < Form1.widgetGreen.Length - 1; i++)
                {
                    Form1.widgetGreen[i] = new PointF(widgetGreenBot[i + 1 - widgetGreenTop.Length].X, widgetGreenBot[i + 1 - widgetGreenTop.Length].Y);
                }
                if (Form1.widgetGreen.Length > 0)
                {
                    Form1.widgetGreen[Form1.widgetGreen.Length - 1] = new PointF(Form1.widgetGreen[Form1.widgetGreen.Length - 2].X, Form1.widgetGreen[Form1.widgetGreen.Length - 2].Y);
                }
            }
            #endregion

            #region blue widget
            if (Form1.distance < -radius)
            {
                DrawWidget(Form1.distance + (51 + radius / 10) * 5, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetBlueTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetBlueTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (51 + radius / 10) * 5, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetBlueBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetBlueBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetBlue = new PointF[widgetBlueBot.Length + widgetBlueTop.Length];
                for (int i = 0; i < widgetBlueTop.Length; i++)
                {
                    Form1.widgetBlue[i] = new PointF(widgetBlueTop[i].X, widgetBlueTop[i].Y);
                }
                for (int i = widgetBlueTop.Length - 1; i < Form1.widgetBlue.Length - 1; i++)
                {
                    Form1.widgetBlue[i] = new PointF(widgetBlueBot[i + 1 - widgetBlueTop.Length].X, widgetBlueBot[i + 1 - widgetBlueTop.Length].Y);
                }
                if (Form1.widgetBlue.Length > 0)
                {
                    Form1.widgetBlue[Form1.widgetBlue.Length - 1] = new PointF(Form1.widgetBlue[Form1.widgetBlue.Length - 2].X, Form1.widgetBlue[Form1.widgetBlue.Length - 2].Y);
                }
            }
            else
            {
                DrawWidget(Form1.distance + (57.25 + radius / 10) * -5, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetBlueTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetBlueTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (57.25 + radius / 10) * -5, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetBlueBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetBlueBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetBlue = new PointF[widgetBlueBot.Length + widgetBlueTop.Length];
                for (int i = 0; i < widgetBlueTop.Length; i++)
                {
                    Form1.widgetBlue[i] = new PointF(widgetBlueTop[i].X, widgetBlueTop[i].Y);
                }
                for (int i = widgetBlueTop.Length - 1; i < Form1.widgetBlue.Length - 1; i++)
                {
                    Form1.widgetBlue[i] = new PointF(widgetBlueBot[i + 1 - widgetBlueTop.Length].X, widgetBlueBot[i + 1 - widgetBlueTop.Length].Y);
                }
                if (Form1.widgetBlue.Length > 0)
                {
                    Form1.widgetBlue[Form1.widgetBlue.Length - 1] = new PointF(Form1.widgetBlue[Form1.widgetBlue.Length - 2].X, Form1.widgetBlue[Form1.widgetBlue.Length - 2].Y);
                }
            }
            #endregion

            #region pink widget
            if (Form1.distance < -radius - 150)
            {
                DrawWidget(Form1.distance + (52 + radius / 10) * 6.25, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetPinkTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetPinkTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (52 + radius / 10) * 6.25, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetPinkBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetPinkBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetPink = new PointF[widgetPinkBot.Length + widgetPinkTop.Length];
                for (int i = 0; i < widgetPinkTop.Length; i++)
                {
                    Form1.widgetPink[i] = new PointF(widgetPinkTop[i].X, widgetPinkTop[i].Y);
                }
                for (int i = widgetPinkTop.Length - 1; i < Form1.widgetPink.Length - 1; i++)
                {
                    Form1.widgetPink[i] = new PointF(widgetPinkBot[i + 1 - widgetPinkTop.Length].X, widgetPinkBot[i + 1 - widgetPinkTop.Length].Y);
                }
                if (Form1.widgetPink.Length > 0)
                {
                    Form1.widgetPink[Form1.widgetPink.Length - 1] = new PointF(Form1.widgetPink[Form1.widgetPink.Length - 2].X, Form1.widgetPink[Form1.widgetPink.Length - 2].Y);
                }
            }
            else
            {
                DrawWidget(Form1.distance + (58 + radius / 10) * -3.75, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetPinkTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetPinkTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (58 + radius / 10) * -3.75, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetPinkBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetPinkBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetPink = new PointF[widgetPinkBot.Length + widgetPinkTop.Length];
                for (int i = 0; i < widgetPinkTop.Length; i++)
                {
                    Form1.widgetPink[i] = new PointF(widgetPinkTop[i].X, widgetPinkTop[i].Y);
                }
                for (int i = widgetPinkTop.Length - 1; i < Form1.widgetPink.Length - 1; i++)
                {
                    Form1.widgetPink[i] = new PointF(widgetPinkBot[i + 1 - widgetPinkTop.Length].X, widgetPinkBot[i + 1 - widgetPinkTop.Length].Y);
                }
                if (Form1.widgetPink.Length > 0)
                {
                    Form1.widgetPink[Form1.widgetPink.Length - 1] = new PointF(Form1.widgetPink[Form1.widgetPink.Length - 2].X, Form1.widgetPink[Form1.widgetPink.Length - 2].Y);
                }
            }
            #endregion

            #region purple widget
            if (Form1.distance < -radius)
            {
                DrawWidget(Form1.distance + (52.5 + radius / 10) * 7.5, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetPurpleTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetPurpleTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (52.5 + radius / 10) * 7.5, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetPurpleBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetPurpleBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetPurple = new PointF[widgetPurpleBot.Length + widgetPurpleTop.Length];
                for (int i = 0; i < widgetPurpleTop.Length; i++)
                {
                    Form1.widgetPurple[i] = new PointF(widgetPurpleTop[i].X, widgetPurpleTop[i].Y);
                }
                for (int i = widgetPurpleTop.Length - 1; i < Form1.widgetPurple.Length - 1; i++)
                {
                    Form1.widgetPurple[i] = new PointF(widgetPurpleBot[i + 1 - widgetPurpleTop.Length].X, widgetPurpleBot[i + 1 - widgetPurpleTop.Length].Y);
                }
                if (Form1.widgetPurple.Length > 0)
                {
                    Form1.widgetPurple[Form1.widgetPurple.Length - 1] = new PointF(Form1.widgetPurple[Form1.widgetPurple.Length - 2].X, Form1.widgetPurple[Form1.widgetPurple.Length - 2].Y);
                }
            }
            else
            {
                DrawWidget(Form1.distance + (60 + radius / 10) * -2.5, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetPurpleTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetPurpleTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (60 + radius / 10) * -2.5, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetPurpleBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetPurpleBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetPurple = new PointF[widgetPurpleBot.Length + widgetPurpleTop.Length];
                for (int i = 0; i < widgetPurpleTop.Length; i++)
                {
                    Form1.widgetPurple[i] = new PointF(widgetPurpleTop[i].X, widgetPurpleTop[i].Y);
                }
                for (int i = widgetPurpleTop.Length - 1; i < Form1.widgetPurple.Length - 1; i++)
                {
                    Form1.widgetPurple[i] = new PointF(widgetPurpleBot[i + 1 - widgetPurpleTop.Length].X, widgetPurpleBot[i + 1 - widgetPurpleTop.Length].Y);
                }
                if (Form1.widgetPurple.Length > 0)
                {
                    Form1.widgetPurple[Form1.widgetPurple.Length - 1] = new PointF(Form1.widgetPurple[Form1.widgetPurple.Length - 2].X, Form1.widgetPurple[Form1.widgetPurple.Length - 2].Y);
                }
            }
            #endregion

            #region octarine widget
            if (Form1.distance < -radius)
            {
                DrawWidget(Form1.distance + (52.9 + radius / 10) * 8.75, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetOctarineTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOctarineTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (52.9 + radius / 10) * 8.75, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetOctarineBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOctarineBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetOctarine = new PointF[widgetOctarineBot.Length + widgetOctarineTop.Length];
                for (int i = 0; i < widgetOctarineTop.Length; i++)
                {
                    Form1.widgetOctarine[i] = new PointF(widgetOctarineTop[i].X, widgetOctarineTop[i].Y);
                }
                for (int i = widgetOctarineTop.Length - 1; i < Form1.widgetOctarine.Length - 1; i++)
                {
                    Form1.widgetOctarine[i] = new PointF(widgetOctarineBot[i + 1 - widgetOctarineTop.Length].X, widgetOctarineBot[i + 1 - widgetOctarineTop.Length].Y);
                }
                if (Form1.widgetOctarine.Length > 0)
                {
                    Form1.widgetOctarine[Form1.widgetOctarine.Length - 1] = new PointF(Form1.widgetOctarine[Form1.widgetOctarine.Length - 2].X, Form1.widgetOctarine[Form1.widgetOctarine.Length - 2].Y);
                }
            }
            else
            {
                DrawWidget(Form1.distance + (66 + radius / 10) * -1.25, radius, radius + 15, radius - 40, circIn, circOut, true, controller);
                PointF[] widgetOctarineTop = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOctarineTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                DrawWidget(Form1.distance + (66 + radius / 10) * -1.25, radius, radius + 15, radius - 40, circIn, circOut, false, controller);
                PointF[] widgetOctarineBot = new PointF[Form1.circList.Count];
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    widgetOctarineBot[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
                }

                Form1.widgetOctarine = new PointF[widgetOctarineBot.Length + widgetOctarineTop.Length];
                for (int i = 0; i < widgetOctarineTop.Length; i++)
                {
                    Form1.widgetOctarine[i] = new PointF(widgetOctarineTop[i].X, widgetOctarineTop[i].Y);
                }
                for (int i = widgetOctarineTop.Length - 1; i < Form1.widgetOctarine.Length - 1; i++)
                {
                    Form1.widgetOctarine[i] = new PointF(widgetOctarineBot[i + 1 - widgetOctarineTop.Length].X, widgetOctarineBot[i + 1 - widgetOctarineTop.Length].Y);
                }
                if (Form1.widgetOctarine.Length > 0)
                {
                    Form1.widgetOctarine[Form1.widgetOctarine.Length - 1] = new PointF(Form1.widgetOctarine[Form1.widgetOctarine.Length - 2].X, Form1.widgetOctarine[Form1.widgetOctarine.Length - 2].Y);
                }
            }
            #endregion
        }

        public static void DrawWidget(double distance, int radius, int radiusOut, int radiusIn, double circIn, double circOut, bool top, Control controller)
        {
            Form1.circList.Clear();
            if (top)
            {
                for (double x = distance; x <= (circIn / 10) + distance; x += 0.1)
                {
                    if (x < radiusOut && x > -radiusOut)
                    {
                        Form1.circList.Add(new PointF(Convert.ToSingle(x), -Convert.ToSingle(Math.Sqrt(Math.Pow(radiusOut, 2) - Math.Pow(-x, 2)))));
                    }
                }
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (controller.Height / 2) + 50);
                }
            }
            else
            {
                for (double x = distance + (circIn / 10); x >= distance; x -= 0.1)
                {
                    if (x < radiusIn && x > -radiusIn)
                    {
                        Form1.circList.Add(new PointF(Convert.ToSingle(x), -Convert.ToSingle(Math.Sqrt(Math.Pow(radiusIn, 2) - Math.Pow(x, 2)))));
                    }
                }
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (controller.Height / 2) + 50);
                }
            }
        }

        public static void DrawCircle(int radius)
        {
            Form1.circList.Clear();
            for (double x = -radius; x <= radius; x += 0.1)
            {
                if (x == radius)
                {
                    Form1.circList.Add(new PointF(Convert.ToSingle(x), 0));
                }
                else
                {
                    Form1.circList.Add(new PointF(Convert.ToSingle(x), Convert.ToSingle(Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x, 2)))));
                }
            }
            for (double x = radius; x >= -radius; x -= 0.1)
            {
                if (x == -radius)
                {
                    Form1.circList.Add(new PointF(Convert.ToSingle(x), 0));
                }
                else
                {
                    Form1.circList.Add(new PointF(Convert.ToSingle(x), -1 * Convert.ToSingle(Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x, 2)))));
                }
            }
            Form1.circList.Add(Form1.circList[0]);
        }

        public static void DetermineClosest()
        {
            int[] values = new int[8];

            try
            {
                values[0] = Convert.ToInt16(Form1.widgetRed[Form1.widgetRed.Length / 4].Y);
            }
            catch
            {
                values[0] = 999;
            }

            try
            {
                values[1] = Convert.ToInt16(Form1.widgetOrange[Form1.widgetOrange.Length / 4].Y);
            }
            catch
            {
                values[1] = 999;
            }

            try
            {
                values[2] = Convert.ToInt16(Form1.widgetYellow[Form1.widgetYellow.Length / 4].Y);
            }
            catch
            {
                values[2] = 999;
            }

            try
            {
                values[3] = Convert.ToInt16(Form1.widgetGreen[Form1.widgetGreen.Length / 4].Y);
            }
            catch
            {
                values[3] = 999;
            }

            try
            {
                values[4] = Convert.ToInt16(Form1.widgetBlue[Form1.widgetBlue.Length / 4].Y);
            }
            catch
            {
                values[4] = 999;
            }

            try
            {
                values[5] = Convert.ToInt16(Form1.widgetPink[Form1.widgetPink.Length / 4].Y);
            }
            catch
            {
                values[5] = 999;
            }

            try
            {
                values[6] = Convert.ToInt16(Form1.widgetPurple[Form1.widgetPurple.Length / 4].Y);
            }
            catch
            {
                values[6] = 999;
            }

            try
            {
                values[7] = Convert.ToInt16(Form1.widgetOctarine[Form1.widgetOctarine.Length / 4].Y);
            }
            catch
            {
                values[7] = 999;
            }

            int[] unsort = new int[8];
            for (int i = 0; i < unsort.Length; i++)
            {
                unsort[i] = values[i];
            }

            Form1.quickSort(values, 0, values.Length - 1);

            int comp = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (unsort[i] == values[0])
                {
                    comp = i;
                    i = values.Length;
                }
            }

            if (inGame)
            {
                switch (comp)
                {
                    case 0:
                        //Red
                        pPattern.Add(0);
                        break;
                    case 1:
                        //Orange
                        pPattern.Add(1);
                        break;
                    case 2:
                        //Yellow
                        pPattern.Add(2);
                        break;
                    case 3:
                        //Green
                        pPattern.Add(3);
                        break;
                    case 4:
                        //Blue
                        pPattern.Add(4);
                        break;
                    case 5:
                        //Pink
                        pPattern.Add(5);
                        break;
                    case 6:
                        //Purple
                        pPattern.Add(6);
                        break;
                    case 7:
                        //Octarine
                        pPattern.Add(7);

                        eyeColour[7].red = octarineRB;
                        eyeColour[7].green = octarineG;
                        eyeColour[7].blue = octarineRB;
                        break;
                }
                GameScreen.playSound(comp);
            }
            else
            {
                switch (comp)
                {
                    case 0:
                        //Red
                        break;
                    case 1:
                        //Orange
                        break;
                    case 2:
                        //Yellow
                        break;
                    case 3:
                        //Green
                        break;
                    case 4:
                        //Blue
                        break;
                    case 5:
                        //Pink
                        break;
                    case 6:
                        //Purple
                        break;
                    case 7:
                        //Octarine
                        break;
                }
            }

            Form1.press = false;
        }

        public static bool ComparePattern()
        {
            if (!reverse)
            {
                for (int i = 0; i < pPattern.Count; i++)
                {
                    if (pPattern[i] != cthulhuPattern[i])
                    {
                        return false;
                    }
                }
                GameScreen.correctInc = true;
                return true;
            }
            else
            {
                for (int i = 0; i < pPattern.Count; i++)
                {
                    if (pPattern[i] != cthulhuPattern[cthulhuPattern.Count - 1 - i])
                    {
                        return false;
                    } 
                }
                GameScreen.correctInc = true;
                return true;
            }
        }
    }
}
