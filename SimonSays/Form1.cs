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
        public static List<PointF> circList = new List<PointF>();
        public static Int32 circDrawTime = 0;
        public static int radius = 50;
        public static PointF[] circlePointsBor;
        public static PointF[] circlePointsInsBor;
        public static PointF[] circlePointsIns;
        public static PointF[] circlePointsOuts;

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
    }
}
