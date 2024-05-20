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
            double c = Convert.ToDouble(this.textBox2.Text.Replace('.', ','));
            double m = Convert.ToDouble(this.textBox3.Text.Replace('.', ','));
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
                if(lastY>maxY)maxY = lastY;
                chart1.Series[0].Points.AddXY(lastX, lastY);                
                lastSpeedX = -c/m * step* lastSpeedX + lastSpeedX;
                lastSpeedY = (-g-c/m* lastSpeedY) * step + lastSpeedY;
                lastX += lastSpeedX * step;
                lastY += lastSpeedY * step;
                
            }
            double h = m/c*startSpeed* Math.Sin(startAngle)-m*m*g/c/c*Math.Log(1+startSpeed*c/m/g* Math.Sin(startAngle));
            for (double t = 0; t<=lastX; t+=0.01)
            {
                chart1.Series[1].Points.AddXY(m/c*startSpeed*Math.Cos(startAngle)*(1-Math.Exp(-c/m*t)), m / c *(startSpeed* Math.Sin(startAngle)+m*g/c)*(1- Math.Exp(-c / m * t))-m*g*t/c);
                for (int i = 30; i < 60; i++)
                {
                    chart1.Series[i - 28].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart1.Series[i - 28].IsVisibleInLegend = false;
                    chart1.Series[i-28].Points.AddXY(m / c * startSpeed * Math.Cos(i * Math.PI / 180) * (1 - Math.Exp(-c / m * t)), m / c * (startSpeed * Math.Sin(i * Math.PI / 180) + m * g / c) * (1 - Math.Exp(-c / m * t)) - m * g * t / c);

                }
            }
            textBox1.Text = String.Format("Numerical method:\r\nLength: {0}\r\nHighest point: {1}\r\nAnalitic method:\r\nHighest point: {2}",
                lastX, maxY,h);            
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = lastX;
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
