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
//Oh and one more, the colour octarine is a fictional 8th colour of the rainbow. If you see a weird white or off-purple in cthulhu's eyes, that is octarine (the colour following purple).
//It is easiest to spot as the colour that is not consistent, adding a bit of challenge.
//Have fun!


namespace SimonSays
{
    public partial class Form1 : Form
    {
        //Random Generation for Pattern
        public static Random rndGen = new Random();

        //Store circle points for drawing user control and widgets
        public static List<PointF> circList = new List<PointF>();
        
        //Specify control radius
        public static int radius = 50;
        //Set the user control circles
        public static PointF[] circlePointsBor, circlePointsInsBor, circlePointsIns, circlePointsOuts;

        //Set the control widgets
        public static PointF[] widgetRed, widgetOrange, widgetYellow, widgetGreen, widgetBlue, widgetPink, widgetPurple, widgetOctarine;

        //For the blinking/weirdly-operating colours, record the required rgb values to be altered (See colour operator below)
        public static int octarineRB = 125, octarineG = 125, redR = 125, orangeRG = 125, yellowRG = 125, greenG = 125, blueB = 125, pinkRG = 125, purpleRB = 125, whiteRGB = 125;

        //Store the control rotation for display purposes
        public static int distance = 0;
        
        //store whether to spin forward, back, input, or protect
        //Protect initially, and if there was a bit more time, was meant to be a side event during the game, where you were suppossed to devote your focus to both the game and randomly appearing enemies to protect against, lest you lose
        public static Boolean moveFor, moveBac, prot, press;

        //Playtype was initially going to reverse the way the game was played, but was scrapped when 
        public static bool playType = false, scrollType = false;
        //How fast the control moves
        public static int scrollSpeed = 5;

        //Define print-style fonts
        public static Font font = new Font(new FontFamily("Perpetua"), 35, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font fancyFont = new Font(new FontFamily("Antiquity Print"), 11, FontStyle.Bold, GraphicsUnit.Pixel);

        //Holds player input and computer input lists
        public static List<int> cthulhuPattern = new List<int>();
        public static List<int> pPattern = new List<int>();

        //Store colours for game use in a simple to access array
        public static ARGB[] eyeColour = {new ARGB(), new ARGB(), new ARGB(), new ARGB(), new ARGB(), new ARGB(), new ARGB(), new ARGB()};

        //Simple booleans to track location so that loading screen can be reused
        public static bool inGame = false;
        public static bool isOver = false;

        //Handles the reverse
        public static bool reverse = false;

        //Initialize sound players to overlap
        public static System.Windows.Media.MediaPlayer backMusic = new System.Windows.Media.MediaPlayer();

        public static System.Windows.Media.MediaPlayer instructSound = new System.Windows.Media.MediaPlayer();

        public static System.Windows.Media.MediaPlayer continueSound = new System.Windows.Media.MediaPlayer();

        //This is inefficient now that we learned constructor methods, but this class simply holds a few values for easy reference
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

            //Setup eye colours
            eyeColour[0].Setup(0, 255, 0, 0);
            eyeColour[1].Setup(0, 255, 165, 0);
            eyeColour[2].Setup(0, 255, 255, 0);
            eyeColour[3].Setup(0, 0, 255, 0);
            eyeColour[4].Setup(0, 0, 0, 255);
            eyeColour[5].Setup(0, 255, 192, 203);
            eyeColour[6].Setup(0, 160, 32, 240);
            eyeColour[7].Setup(0, octarineRB, octarineG, octarineRB);

            //Initialize proper radius
            radius = (this.Width / 2 + 10);

            //Place the initial distance
            distance = -radius;

            //Select form1 to collect key presses (this was an original issue that was fixed by placing the reference to keypreview)
            this.KeyPreview = true;

            //Start the background music and get it repeating
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
            //Open the menuscreen
            ScreenChanger(new MenuScreen(), this);
        }

        /// <summary>
        /// This is Identical to the method we created in class for ease of use
        /// </summary>
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

        /// <summary>
        /// This is for the creation of the fictional colour octarine, officially a greenish-purple only seen by wizards and cats
        /// </summary>
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

        /// <summary>
        /// Just meant to alter rgb values
        /// </summary>
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

        /// <summary>
        /// Increment rgb values
        /// </summary>
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
            //Used for "any button" to continue
            if (isOver)
            {
                isOver = !isOver;
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.A:
                    //Checks for the chosen control style, which is always set to false from default
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
                    //Pulls out or hides the instructions and plays a crumple sound
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

        /// <summary>
        /// Just reads the input from the user and rotates the control by changing the distance value
        /// </summary>
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

        //Swaps 2 values, used for quick sort method
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
            //Loops the control if the user should overlap with a previous version of the control
            if (distance < -863)
            {
                distance = -radius;
            }

            else if (distance > 543)
            {
                distance = -radius;
            }

            #region draw input borders
            //Uses the DrawCircle function (see that for more detail) and copies them over to draw the input borders
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
            //Defines widget borders
            double circIn = 2 * (radius - 40) * Math.PI;
            double circOut = 2 * (radius - 40) * Math.PI;
            #endregion

            /* In order to avoid explaining each widget seperately (because there is a lot and they are all very similar)
             * Each Function draws a widget by draw a section of circle on both the top and bottom specified borders
             * It then copies this into an array going from top to bottom, ending by adding its intial value to loop back onto itself and create the filled polygon
             * What seperates them apart is a tiny bit of inconsistency in the spacing (you can see this in the draw widget function calls)
             * Most only have two statements as only two variations of widget exist in the loop, however others appear thrice
             * For specifics, check the draw widget function
             */

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
            //Clear the temporary list
            Form1.circList.Clear();

            //Check for the top or bottom as, in order to properly loop the polygon, the points must follow a distinct shape where the top follows to the right and the bottom follows to the left
            if (top)
            {
                //Take the given x values from the distance and put them through a circle function to solve for the y value
                for (double x = distance; x <= (circIn / 10) + distance; x += 0.1)
                {
                    if (x < radiusOut && x > -radiusOut)
                    {
                        Form1.circList.Add(new PointF(Convert.ToSingle(x), -Convert.ToSingle(Math.Sqrt(Math.Pow(radiusOut, 2) - Math.Pow(-x, 2)))));
                    }
                }

                //Translate the values onto the screen based on the radius, as the circle draws half off screen
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (controller.Height / 2) + 50);
                }
            }

            else
            {
                //Take the given x values from the distance and put them through a circle function to solve for the y value
                for (double x = distance + (circIn / 10); x >= distance; x -= 0.1)
                {
                    if (x < radiusIn && x > -radiusIn)
                    {
                        Form1.circList.Add(new PointF(Convert.ToSingle(x), -Convert.ToSingle(Math.Sqrt(Math.Pow(radiusIn, 2) - Math.Pow(x, 2)))));
                    }
                }

                //Translate the values onto the screen based on the radius, as the circle draws half off screen
                for (int i = 0; i < Form1.circList.Count; i++)
                {
                    Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (controller.Height / 2) + 50);
                }
            }
        }

        public static void DrawCircle(int radius)
        {
            //Clear the temporary list
            Form1.circList.Clear();

            for (double x = -radius; x <= radius; x += 0.1)
            {
                //Works through the x values and adds the positive y values to the list. Adds the values on the radius as they draw oddly otherwise
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
                //Works backwards through the x values and adds the negative y values to the list. Adds the values on the radius as they draw oddly otherwise
                if (x == -radius)
                {
                    Form1.circList.Add(new PointF(Convert.ToSingle(x), 0));
                }

                else
                {
                    Form1.circList.Add(new PointF(Convert.ToSingle(x), -1 * Convert.ToSingle(Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x, 2)))));
                }
            }

            //In order to properly wrap the circle, adds the beginning value as the ending value
            Form1.circList.Add(Form1.circList[0]);
        }

        public static void DetermineClosest()
        {
            int[] values = new int[8];

            //Try to grab the highest value of the widget. Try statements are used as the widgets do not always exist if they would be off-screen
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

            //hold a copy of the unsorted array
            int[] unsort = new int[8];

            for (int i = 0; i < unsort.Length; i++)
            {
                unsort[i] = values[i];
            }

            //Sort the initial array
            quickSort(values, 0, values.Length - 1);

            //hold a comparable value
            int comp = 0;

            //Determine the widget that is closest (the lowest y-value) by comparing the unsorted array that can be read by widget to the sorted array that holds the closest widget
            for (int i = 0; i < values.Length; i++)
            {
                if (unsort[i] == values[0])
                {
                    comp = i;
                    i = values.Length;
                }
            }

            //Pass the selected widget to the player pattern
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

            //This was meant for the menu screen but went unused
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

        /// <summary>
        /// A simple method for comparing both patterns consistently. Returns true if the pattern is correct.
        /// </summary>
        public static bool ComparePattern()
        {
            //Operates the reverse game mide
            if (!reverse)
            {
                //Check the current player pattern
                for (int i = 0; i < pPattern.Count; i++)
                {
                    //Check if one is incorrect and return false immediately if the pattern does not match
                    if (pPattern[i] != cthulhuPattern[i])
                    {
                        return false;
                    }
                }
                
                //Sets the eyes to show correct
                GameScreen.correctInc = true;

                return true;
            }

            else
            {
                //Check the current player pattern
                for (int i = 0; i < pPattern.Count; i++)
                {
                    //Check if one is incorrect and return false immediately if the pattern does not match
                    if (pPattern[i] != cthulhuPattern[cthulhuPattern.Count - 1 - i])
                    {
                        return false;
                    } 
                }

                //Sets the eyes to show correct
                GameScreen.correctInc = true;

                return true;
            }
        }
    }
}
