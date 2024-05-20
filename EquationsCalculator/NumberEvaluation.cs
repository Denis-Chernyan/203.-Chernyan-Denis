using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EquationsCalculator
{
    public partial class NumberEvaluation : Form
    {
        public Form1 form;
        public double xMax;
        public double xMin;
        public int precision;
        public NumberEvaluation(Form1 form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                xMax = Convert.ToDouble(textBox1.Text.Replace('.', ','));
                xMin = Convert.ToDouble(textBox2.Text.Replace('.', ','));
                precision = Convert.ToInt32(textBox3.Text);
                if (xMax <= xMin)
                {
                    throw new Exception("Максимальный х должен быть строго больше минимального");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            form.ClearSolutionTextbox();
            FindNumericalSolutions(xMax, xMin, checkBox1.Checked, 1/Math.Pow(10,precision));
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != '.' && number != 8 && number != ',' && number != '-')
            {
                e.Handled = true;
            }
        }
        private void FindNumericalSolutions(double max, double min,bool multipleSolutions,double precision)
        { 
            int counter = 0;
            double middle;
            for (double step = min; step < max; step += 0.001)
            {
                if (form.SolutionInPoint(min) * form.SolutionInPoint(step) < 0)
                {
                    while (form.SolutionInPoint(min) < precision && form.SolutionInPoint(step) > precision)
                    {
                        middle = (step + min) / 2;
                        if (form.SolutionInPoint(min) * form.SolutionInPoint(middle) < 0)
                        {
                            step = middle;
                        }
                        else
                        {
                            min = middle;
                        }
                    }
                    form.AddSolution(Math.Round(step,10));
                    counter++;
                    min = step;
                    if (!multipleSolutions) break;
                }
            }
            if (counter == 0)
            {
                MessageBox.Show("Корней не найдено");
            }
            else
            {
                Close();
            }
        }
        
    } 
}
