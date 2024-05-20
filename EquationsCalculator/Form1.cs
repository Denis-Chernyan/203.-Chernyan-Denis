using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;



namespace EquationsCalculator
{
    public partial class Form1 : Form
    {
        private bool _equationSet = false;
        private int roundCoeff = 10;
        private double[] _coeffs;
        private List<double> _solutions;
        private List<Complex> _complexSolutions;
        public Form1()
        {
            InitializeComponent();
        }

        private void sizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupType1.Visible = sizeComboBox.SelectedIndex == 0;
            groupType2.Visible = sizeComboBox.SelectedIndex == 1;
            groupType3.Visible = sizeComboBox.SelectedIndex == 2;
            groupType4.Visible = sizeComboBox.SelectedIndex == 3;
            groupType5.Visible = sizeComboBox.SelectedIndex == 4;
            CLearAllCoeffs();
            evaluateButton.Visible = true;
        }
        private void evaluateButton_Click(object sender, EventArgs e)
        {
            if (!_equationSet)
            {
                try
                {
                    _coeffs = new double[6];
                    switch (sizeComboBox.SelectedIndex)
                    {
                        case 0:
                            _coeffs[0] = Convert.ToDouble(bType1TextBox.Text.Replace('.', ','));
                            _coeffs[1] = Convert.ToDouble(aType1TextBox.Text.Replace('.', ','));
                            break;
                        case 1:
                            _coeffs[0] = Convert.ToDouble(cType2TextBox.Text.Replace('.', ','));
                            _coeffs[1] = Convert.ToDouble(bType2TextBox.Text.Replace('.', ','));
                            _coeffs[2] = Convert.ToDouble(aType2TextBox.Text.Replace('.', ','));
                            if (_coeffs[2] == 0)
                            {
                                throw new Exception("Квадратное уравнение должно иметь ненулевой коэффициент при x^2");
                            }
                            break;
                        case 2:
                            _coeffs[0] = Convert.ToDouble(dType3TextBox.Text.Replace('.', ','));
                            _coeffs[1] = Convert.ToDouble(cType3TextBox.Text.Replace('.', ','));
                            _coeffs[2] = Convert.ToDouble(bType3TextBox.Text.Replace('.', ','));
                            _coeffs[3] = Convert.ToDouble(aType3TextBox.Text.Replace('.', ','));
                            if (_coeffs[3] == 0)
                            {
                                throw new Exception("Кубическое уравнение должно иметь ненулевой коэффициент при x^3");
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                _solutions = new List<double>();
                _complexSolutions = new List<Complex>();
                findSolutionsButton.Visible = sizeComboBox.SelectedIndex < 3;
                solutionGroupbox.Visible = true;
                chart1.Enabled = true;
                solutionGroupbox.Enabled = true;
                equationsStartGroupbox.Enabled = false;
                evaluateButton.Text = "Ввести новое уравнение";
                _equationSet = true;
            }
            else
            {
                checkSolutionsTextbox.Text = "";
                solutionsTextBox.Text = "";
                chart1.Series[0].Points.Clear();
                chart1.Enabled = false;
                solutionGroupbox.Enabled = false;
                equationsStartGroupbox.Enabled = true;
                evaluateButton.Text = "Вычислить";
                _equationSet = false;
            }


        }
        private void CLearAllCoeffs()
        {
            aType1TextBox.Text = "";
            bType1TextBox.Text = "";

            aType2TextBox.Text = "";
            bType2TextBox.Text = "";
            cType2TextBox.Text = "";

            aType3TextBox.Text = "";
            bType3TextBox.Text = "";
            cType3TextBox.Text = "";
            dType3TextBox.Text = "";

            aType4TextBox.Text = "";
            bType4TextBox.Text = "";
            cType4TextBox.Text = "";
            dType4TextBox.Text = "";
            eType4TextBox.Text = "";

            aType5TextBox.Text = "";
            bType5TextBox.Text = "";
            cType5TextBox.Text = "";
            dType5TextBox.Text = "";
            eType5TextBox.Text = "";
            fType5TextBox.Text = "";
        }
        private void buildChartButton_Click(object sender, EventArgs e)
        {
            double xMin = -10;
            double xMax = 10;
            double yMin = -10;
            double yMax = 10;
            chart1.Series[0].Points.Clear();
            if(chartXmin.Text!=""&& chartXmax.Text!=""&& chartYmin.Text != "" && chartYmax.Text != "")
            {
                xMin = Convert.ToDouble(chartXmin.Text.Replace('.', ',')) - 1;
                xMax = Convert.ToDouble(chartXmax.Text.Replace('.', ',')) + 1;
                yMin = Convert.ToDouble(chartYmin.Text.Replace('.', ','));
                yMax = Convert.ToDouble(chartYmax.Text.Replace('.', ','));
            }
            
            for (double i = xMin; i <= xMax; i+=0.01)
            {
                chart1.Series[0].Points.AddXY(i,SolutionInPoint(i));
            }
            chart1.ChartAreas[0].AxisX.Maximum = xMax;
            chart1.ChartAreas[0].AxisX.Minimum = xMin;
            chart1.ChartAreas[0].AxisY.Maximum = yMax;
            chart1.ChartAreas[0].AxisY.Minimum = yMin;
            numberEvaluationButton.Visible = sizeComboBox.SelectedIndex > 1;
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != '.' && number != 8 && number != ',' && number != '-')
            {
                e.Handled = true;
            }
        }
        private void findSolutionsButton_Click(object sender, EventArgs e)
        {
            solutionsTextBox.Visible = true;
            checkSolutionsButton.Visible = true;
            _solutions = new List<double>();
            _complexSolutions = new List<Complex>();
            switch (sizeComboBox.SelectedIndex)
            {
                case 0:
                    AddSolution(-_coeffs[0] / _coeffs[1]);
                    solutionsTextBox.Text = "x1 = " + _solutions[0];
                    break;
                case 1:
                    double d = _coeffs[1] * _coeffs[1] - 4 * _coeffs[2] * _coeffs[0];
                    if (d < 0)
                    {
                        _complexSolutions.Add(new Complex(Math.Round(-_coeffs[1] / (2 * _coeffs[2]), roundCoeff), Math.Round(Math.Sqrt(-d) / (2 * _coeffs[2]), roundCoeff)));
                        _complexSolutions.Add(new Complex(Math.Round(-_coeffs[1] / (2 * _coeffs[2]), roundCoeff), Math.Round(-Math.Sqrt(-d) / (2 * _coeffs[2]), roundCoeff)));
                        solutionsTextBox.Text = "x1 = " + _complexSolutions[0];
                        solutionsTextBox.Text += "\r\nx2 = " + _complexSolutions[1];
                    }
                    else
                    {
                        AddSolution(Math.Round((-_coeffs[1] + Math.Sqrt(d)) / (2 * _coeffs[2]), roundCoeff));
                        AddSolution(Math.Round((-_coeffs[1] - Math.Sqrt(d)) / (2 * _coeffs[2]), roundCoeff));
                        
                    }
                    break;
                case 2:
                    double p = (3 * _coeffs[3] * _coeffs[1] - Math.Pow(_coeffs[2], 2)) / (3 * Math.Pow(_coeffs[3], 2));
                    double q = (2 * Math.Pow(_coeffs[2], 3) - 9 * _coeffs[3] * _coeffs[2] * _coeffs[1] + 27 * Math.Pow(_coeffs[3], 2) * _coeffs[0])
                        / (27 * Math.Pow(_coeffs[3], 3));
                    double delta = Math.Pow(q / 2.0, 2) + Math.Pow(p / 3.0, 3);
                    if (delta > 0)
                    {
                        double a = Math.Sign(-q / 2.0 + Math.Sqrt(delta)) * Math.Pow(Math.Abs(-q / 2.0 + Math.Sqrt(delta)), 1 / 3.0);
                        double b = Math.Sign(-q / 2.0 - Math.Sqrt(delta)) * Math.Pow(Math.Abs(-q / 2.0 - Math.Sqrt(delta)), 1d / 3);
                        AddSolution(Math.Round(a + b - _coeffs[2] / (3 * _coeffs[3]), roundCoeff));
                        _complexSolutions.Add(new Complex(Math.Round(-1 / 2.0 * (a + b) - _coeffs[2] / (3 * _coeffs[3]), roundCoeff), Math.Round(Math.Sqrt(3) / 2.0 * (a - b), roundCoeff)));
                        _complexSolutions.Add(new Complex(Math.Round(-1 / 2.0 * (a + b) - _coeffs[2] / (3 * _coeffs[3]), roundCoeff), Math.Round(-Math.Sqrt(3) / 2.0 * (a - b), roundCoeff)));
                        
                        solutionsTextBox.Text += "\r\nx2 = " + _complexSolutions[0];
                        solutionsTextBox.Text += "\r\nx3 = " + _complexSolutions[1];
                    }
                    else if (delta < 0)
                    {
                        double fi = q == 0 ? Math.PI / 2 : (q > 0 ? Math.Atan(Math.Sqrt(-delta) / (-q / 2.0)) + Math.PI : Math.Atan(Math.Sqrt(-delta) / (-q / 2.0)));
                        AddSolution(Math.Round(2 * Math.Sqrt(-p / 3.0) * Math.Cos(fi / 3.0) - _coeffs[2] / (3 * _coeffs[3]), roundCoeff));
                        AddSolution(Math.Round(2 * Math.Sqrt(-p / 3.0) * Math.Cos(fi / 3.0 + 2 * Math.PI / 3) - _coeffs[2] / (3 * _coeffs[3]), roundCoeff));
                        AddSolution(Math.Round(2 * Math.Sqrt(-p / 3.0) * Math.Cos(fi / 3.0 + 4 * Math.PI / 3) - _coeffs[2] / (3 * _coeffs[3]), roundCoeff));
                        
                    }
                    else if (delta == 0 && p != 0 && q != 0)
                    {
                        AddSolution(Math.Round(2 * Math.Sign(-q / 2) * Math.Pow(Math.Abs(-q / 2), 1 / 3.0), roundCoeff));
                        AddSolution(Math.Round(Math.Sign(-q / 2) * -Math.Pow(Math.Abs(-q / 2), 1 / 3.0), roundCoeff));
                        
                    }
                    else if (delta == 0)
                    {
                        AddSolution(Math.Round(-_coeffs[2] / (3 * _coeffs[3]), roundCoeff));
                        
                    }
                    break;
            }


        }
        public void AddSolution(double s)
        {
            _solutions.Add(s);
            solutionsTextBox.Visible = true;
            checkSolutionsButton.Visible = true;
            solutionsTextBox.Text += String.Format("\r\nx{0} = {1}", _solutions.Count, s);
        }
        public void ClearSolutionTextbox()
        {
            solutionsTextBox.Text = "";
        }
        public double SolutionInPoint(double point)
        {
            double result = _coeffs[0];
            for (int i = 1; i < _coeffs.Length; i++)
            {
                result += Math.Pow(point, i) * _coeffs[i];
            }
            return result;
        }
        private void checkSolutionsButton_Click(object sender, EventArgs e)
        {
            checkSolutionsTextbox.Visible = true;
            checkSolutionsTextbox.Text = "";
            int counter = 1;
            foreach (var solution in _solutions)
            {
                double result = _coeffs[0];
                for (int i = 1; i < _coeffs.Length; i++)
                {
                    result += Math.Pow(solution, i) * _coeffs[i];
                }
                checkSolutionsTextbox.Text += String.Format("\r\nf(x{0}) = {1}", counter, Math.Round(result, 10));
                counter++;
            }
            foreach (var solution in _complexSolutions)
            {
                Complex result = _coeffs[0];
                for (int i = 1; i < _coeffs.Length; i++)
                {
                    result += Complex.Pow(solution, i) * _coeffs[i];
                }
                checkSolutionsTextbox.Text += String.Format("\r\nf(x{0}) = {1}", counter, new Complex(Math.Round(result.Real, 10), Math.Round(result.Imaginary, 10)));
                counter++;
            }
        }

        private void numberEvaluationButton_Click(object sender, EventArgs e)
        {
            NumberEvaluation form = new NumberEvaluation(this);
            form.ShowDialog();

        }
    }
}
