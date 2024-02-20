using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace SimonSays
{
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
        Rectangle lovecraft = new Rectangle();
        PointF[] povCircle;
        Image[] lovecraftNorm = new Image[5];
        Image[] lovecraftLight = new Image[5];
        Image[] lovecraftDark = new Image[5];

        int screenFade = 0;
        public MenuScreen()
        {
            InitializeComponent();
            lovecraft = new Rectangle(23, 10, 50, 75);

            Form1.UserInput();

            DrawLovecraft();

            Form1.DrawUserInput(Form1.radius, this);

            Refresh();
        }

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
            e.Graphics.FillPolygon(new SolidBrush(Color.LightYellow), povCircle);
            e.Graphics.DrawImage(Properties.Resources.LoveCraftBase__1_, lovecraft);

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

            e.Graphics.DrawString("Lovecraft", Form1.font, new SolidBrush(Color.Wheat), new Point(85, 5));

            if(ScreenFader.Enabled)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(screenFade, 0, 0, 0)), new Rectangle(0, 0, this.Width, this.Height));
            }
        }

        private void MenuOp_Tick(object sender, EventArgs e)
        {
            Form1.UserInput();

            DrawLovecraft();

            Form1.DrawUserInput(Form1.radius, this);

            if (Form1.distance < -775)
            {
                // Form1.distance = -160;
            }

            if (Form1.press)
            {
                Form1.DetermineClosest();
            }

            if (Form1.prot)
            {
                Form1.prot = false;
                ScreenFader.Enabled = true;
                MenuOp.Enabled = false;
                return;
            }

            Refresh();
        }

        private void ScreenFader_Tick(object sender, EventArgs e)
        {
            screenFade += 5;
            if (screenFade > 255)
            {
                Form1.ScreenChanger(new LoadingScreen(), this);
                screenFade = 0;
                ScreenFader.Enabled = false;
                return;
            }
            Refresh();
        }
    }
}
