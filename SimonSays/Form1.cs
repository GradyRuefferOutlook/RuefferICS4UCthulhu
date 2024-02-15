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

namespace SimonSays
{
    public partial class Form1 : Form
    {
        public static Random rndGen = new Random();
        public static List<PointF> circList = new List<PointF>();
        public static Int32 circDrawTime = 0;
        public static int radius = 50;
        public static PointF[] circlePointsBor;
        public static PointF[] circlePointsInsBor;
        public static PointF[] circlePointsIns;
        public static PointF[] circlePointsOuts;

        public static PointF[] widgetRed;
        public static PointF[] widgetOrange;
        public static PointF[] widgetYellow;
        public static PointF[] widgetGreen;
        public static PointF[] widgetBlue;
        public static PointF[] widgetPink;
        public static PointF[] widgetPurple;
        public static PointF[] widgetOctarine;

        public static int octarineRB = 125;
        public static int octarineG = 125;
        public static int redR = 125;
        public static int orangeRG = 125;
        public static int yellowRG = 125;
        public static int greenG = 125;
        public static int blueB = 125;
        public static int pinkRG = 125;
        public static int purpleRB = 125;
        public static int whiteRGB = 125;

        //TODO: create a List to store the pattern. Must be accessable on other screens
        public static List<Int64> protectionPattern = new List<Int64>();
        public Form1()
        {
            InitializeComponent();
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

        private void OctarineGenerator_Tick(object sender, EventArgs e)
        {
            if (rndGen.Next(0,2) == 0)
            {
                octarineG += 5;
            } else
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

            if(octarineG > 150)
            {
                octarineG = 150;
            } else if (octarineG < 0)
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
            purpleRB = ColourAdjuster(purpleRB);
            whiteRGB = ColourAdjuster(whiteRGB);
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
    }
}
