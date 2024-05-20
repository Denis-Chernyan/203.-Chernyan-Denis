using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Num2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            double g = 9.8;
            double step = Convert.ToDouble(this.step.Text.Replace('.', ','));
            double startSpeed = Convert.ToDouble(this.startSpeed.Text.Replace('.', ','));
            double startAngle = Convert.ToDouble(this.alpha.Text.Replace('.', ','))*Math.PI/180;
            double lastSpeedX = startSpeed * Math.Cos(startAngle);
            double lastSpeedY = startSpeed * Math.Sin(startAngle);
            double maxY = 0;
            double lastX = 0;
            double lastY = 0;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            for (int i = 30; i < 60; i++)
            {
                chart1.Series[i - 29].Points.Clear();


            }
            while (lastY>=0)
            {
                //Thread.Sleep(1000);
                if(lastY>maxY)maxY = lastY;
                chart1.Series[0].Points.AddXY(lastX, lastY);
                //if (lastY + (-g * step + lastSpeedY) * step < 0) break;
                lastSpeedY = -g * step + lastSpeedY;
                lastX += lastSpeedX * step;
                lastY += lastSpeedY * step;
                
            }
            double l = (startSpeed * startSpeed) / g * Math.Sin(2 * startAngle);
            int bestAngle = 0;
            for (double x = 0; x <=2*l; x+=0.01)
            {
                chart1.Series[1].Points.AddXY(x, x*Math.Tan(startAngle)-(Math.Pow(x,2)*g)/(2*Math.Pow(Math.Cos(startAngle),2)* Math.Pow(startSpeed,2)));
                for (int i = 30; i < 60; i++)
                {
                    chart1.Series[i - 28].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart1.Series[i - 28].IsVisibleInLegend = false;
                    chart1.Series[i-28].Points.AddXY(x, x * Math.Tan(i) - (Math.Pow(x, 2) * g) / (2 * Math.Pow(Math.Cos(i), 2) * Math.Pow(startSpeed, 2)));
                    if(Math.Sin(2 * i) >Math.Sin(2 * bestAngle))
                    {
                        bestAngle = i;
                    }
                }
            }
            textBox1.Text = String.Format("Numerical method:\r\nLength: {0}\r\nHighest point: {1}\r\nAnalitic method:\r\nLength: {2}\r\nBest Angle: {3}",
                lastX, maxY,l,bestAngle);
            /*double d = lastSpeedY * lastSpeedY + 4 * g * lastY;
            double x1 = (-lastSpeedY + d) / (2 * -g);
            double x2 = (-lastSpeedY - d) / (2 * -g);
            step = Math.Max(x1,x2);
            lastSpeedY = -g * step + lastSpeedY;
            lastX += lastSpeedX * step;
            lastY += lastSpeedY * step;
            chart1.Series[0].Points.AddXY(lastX, 0);*/
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = (startSpeed * startSpeed) / g * Math.Sin(2 * bestAngle);
            chart1.ChartAreas[0].AxisY.Minimum = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 30; i < 60; i++)
            {
                chart1.Series[i - 28].Enabled = !chart1.Series[i - 28].Enabled;
            }
        }
    }
}
