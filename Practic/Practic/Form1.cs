using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double tStart=0;
            double tMax = 20;
            double g = 9.8;
            double step = Convert.ToDouble(this.step.Text);
            double r = Convert.ToDouble(this.length.Text);
            double speed = Convert.ToDouble(this.speed.Text);
            double omega = Math.Sqrt(g / r);
            chart2.Series[0].Points.Clear();
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            double wPrev = speed / omega;
            double fPrev = 0;
            for (double t = tStart; t < tMax;t+=step )
            {
                chart1.Series[0].Points.AddXY(t, wPrev - step * Math.Sin(fPrev));
                chart1.Series[1].Points.AddXY(t, fPrev + step * (wPrev - step * Math.Sin(fPrev)));
                chart2.Series[0].Points.AddXY(wPrev - step * Math.Sin(fPrev), fPrev + step * (wPrev - step * Math.Sin(fPrev)));
                wPrev -= step * Math.Sin(fPrev);
                fPrev += step * wPrev;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
