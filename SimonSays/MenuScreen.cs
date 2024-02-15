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
    public partial class MenuScreen : UserControl
    {

        public MenuScreen()
        {
            InitializeComponent();
            Refresh();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            //TODO: remove this screen and start the GameScreen
        }


        private void exitButton_Click(object sender, EventArgs e)
        {
            //TODO: end the application
        }

        void DrawUserInput(int radius)
        {
            DrawCircle(radius + 25);
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (this.Height / 2) + 50);
            }
            Form1.circlePointsBor = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circlePointsBor[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
            }

            DrawCircle(radius + 15);
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (this.Height / 2) + 50);
            }
            Form1.circlePointsOuts = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circlePointsOuts[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
            }

            DrawCircle(radius - 40);
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (this.Height / 2) + 50);
            }
            Form1.circlePointsInsBor = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circlePointsInsBor[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
            }

            DrawCircle(radius - 50);
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (this.Height / 2) + 50);
            }
            Form1.circlePointsIns = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
                Form1.circlePointsIns[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
            }

            double circIn = 2 * (radius - 40) * Math.PI;
            double circOut = 2 * (radius - 40) * Math.PI;

            double distance = 0  - (radius / 4.25);

            DrawWidget(distance, radius, radius + 15, radius - 40, circIn, circOut, true);
            PointF[] widgetYellowTop = new PointF[Form1.circList.Count];
            for (int i = 0; i < Form1.circList.Count; i++)
            {
              widgetYellowTop[i] = new PointF(Form1.circList[i].X, Form1.circList[i].Y);
            }

            DrawWidget(distance, radius, radius + 15, radius - 40, circIn, circOut, false);
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
            Form1.widgetYellow[Form1.widgetYellow.Length - 1] = new PointF(Form1.widgetYellow[Form1.widgetYellow.Length -2].X, Form1.widgetYellow[Form1.widgetYellow.Length -2].Y);
        }

        void DrawWidget(double distance, int radius, int radiusOut, int radiusIn, double circIn, double circOut, bool top)
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
                    Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (this.Height / 2) + 50);
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
                    Form1.circList[i] = new PointF(Form1.circList[i].X + radius - 10, Form1.circList[i].Y + radius + (this.Height / 2) + 50);
                }
            }
        }

        void DrawCircle(int radius)
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

        private void MenuScreen_Paint(object sender, PaintEventArgs e)
        {
            DrawUserInput((this.Width / 2) + 10);
            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), Form1.circlePointsBor);
            e.Graphics.DrawPolygon(new Pen(Color.Gray), Form1.circlePointsBor);
            e.Graphics.DrawPolygon(new Pen(Color.Gray), Form1.circlePointsOuts);
            e.Graphics.DrawPolygon(new Pen(Color.Gray), Form1.circlePointsInsBor);
            e.Graphics.DrawPolygon(new Pen(Color.Gray), Form1.circlePointsIns);
            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, Form1.whiteRGB, Form1.whiteRGB, Form1.whiteRGB)), Form1.circlePointsIns);

            //Yellow
            //e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, 125, Form1.octarineG, Form1.octarineRB)), Form1.widgetYellow);

            //Purple
            //e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, Form1.purpleRB / 2, 0, Form1.purpleRB)), Form1.widgetYellow);

            //Octarine
            //e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(255, 125, Form1.octarineG, Form1.octarineRB)), Form1.widgetYellow);
        }

        private void MenuOp_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
