using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task__9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.RowCount = 15;
            dataGridView1.ColumnCount = 5;
            dataGridView1[0, 0].Value = "i";
            dataGridView1[1, 0].Value = "X";
            dataGridView1[2, 0].Value = "Vi";
            dataGridView1[3, 0].Value = "Ui";
            dataGridView1[4, 0].Value = "|Ui-Vi|";

            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

        }

        double m;
        double a1;
        double a2;


        double Function(double x, double u)
        {
            double value;
            double R;
            R = -1d * (a1 * u + a2 * u * u);
            value = R / m;
            return value;
        }

        double FunctionOfExactSolution(double x, double m, double a1, double a2, double x0, double u0)
        {
            double c = Math.Log(u0 * Math.Exp(a1 * x0 / m) / (a1 + a2 * u0)) / a1;
            double value = -1d * ((a1 * Math.Pow(Math.E, a1 * c)) / (a2 * Math.Pow(Math.E, a1 * c) - Math.Pow(Math.E, a1 * x / m)));
            return value;
        }

        double MethodRungeKutta(double xPrevious, double vPrevious, double h)
        {
            double v = vPrevious + h * Function(xPrevious + (h / 2d), vPrevious + (h / 2d) * Function(xPrevious, vPrevious));
            return v;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int p = 2;
            m = Convert.ToDouble(textBox1.Text);
            a1 = Convert.ToDouble(textBox2.Text);
            a2 = Convert.ToDouble(textBox3.Text);
            double x0 = Convert.ToDouble(textBox4.Text);
            double u0 = Convert.ToDouble(textBox5.Text);

            double h = Convert.ToDouble(textBox6.Text);
            double eps = Convert.ToDouble(textBox7.Text);
            int numberOfIterations = Convert.ToInt32(textBox9.Text);

            double[] xValue = new double[numberOfIterations + 1];
            double[] vValue = new double[numberOfIterations + 1];
            double[] uValue = new double[numberOfIterations + 1];

            xValue[0] = x0;
            vValue[0] = u0;
            uValue[0] = u0;
            for (int i = 1; i <= numberOfIterations; i++)
            {
                xValue[i] = xValue[i - 1] + h;
                uValue[i] = FunctionOfExactSolution(xValue[i], m, a1, a2, x0, u0);
            start:
                vValue[i] = MethodRungeKutta(xValue[i - 1], vValue[i - 1], h);

                double tmpH = h / 2d;
                double tmpvValue_1 = MethodRungeKutta(xValue[i - 1], vValue[i - 1], tmpH);
                double tmpvValue_2 = MethodRungeKutta(xValue[i - 1] + tmpH, tmpvValue_1, tmpH);

                double S = (tmpvValue_2 - vValue[i]) / (Math.Pow(2d, p) - 1);
                vValue[i] = vValue[i] + Math.Pow(2d, p) * S;
                if (((eps / Math.Pow(2d, p + 1)) <= Math.Abs(S)) && (Math.Abs(S) <= eps))
                {
                    h = h;
                }
                else if (Math.Abs(S) <= (eps / Math.Pow(2d, p + 1)))
                {
                    h = 2 * h;
                }
                else if (Math.Abs(S) >= eps)
                {
                    h = h / 2;
                    goto start;
                }

            }

            dataGridView1.RowCount = numberOfIterations + 2;
            for (int i = 0; i <= numberOfIterations; i++)
            {
                dataGridView1[0, i + 1].Value = i;
                dataGridView1[1, i + 1].Value = xValue[i];
                dataGridView1[2, i + 1].Value = vValue[i];
                dataGridView1[3, i + 1].Value = uValue[i];
                dataGridView1[4, i + 1].Value = Math.Abs(uValue[i]-vValue[i]);
            
            
            }

            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            for (int i = 0; i <= numberOfIterations; i++)
            {
                chart1.Series[0].Points.AddXY(xValue[i], vValue[i]);
                chart1.Series[1].Points.AddXY(xValue[i], uValue[i]);
            }

            // Heh.URL = @"hehe.mp4";

        }




    }
}


