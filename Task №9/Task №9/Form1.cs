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
            dataGridView1.RowCount = 10;
            dataGridView1.ColumnCount = 3;
            dataGridView1[0, 0].Value = "№";
            dataGridView1[1, 0].Value = "X";
            dataGridView1[2, 0].Value = "V";

            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.Series[0].Points.AddY(0.0);
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
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

        double MethodRungeKutta(double xPrevious, double uPrevious, double h)
        {
            double v = uPrevious + h * Function(xPrevious + (h / 2d), uPrevious + (h / 2d) * Function(xPrevious, uPrevious));
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
            double accuracy = Convert.ToDouble(textBox8.Text);
            int numberOfIterations = Convert.ToInt32(textBox9.Text);

            double[] xValue = new double[numberOfIterations + 1];
            double[] uValue = new double[numberOfIterations + 1];

            xValue[0] = x0;
            uValue[0] = u0;
            for (int i = 1; i <= numberOfIterations; i++)
            {
                xValue[i] = xValue[i - 1] + h;
            start:
                uValue[i] = MethodRungeKutta(xValue[i - 1], uValue[i - 1], h);

                double tmpH = h / 2d;
                double tmpUValue_1 = MethodRungeKutta(xValue[i - 1], uValue[i - 1], tmpH);
                double tmpUValue_2 = MethodRungeKutta(xValue[i - 1] + tmpH, tmpUValue_1, tmpH);

                double S = (tmpUValue_2 - uValue[i]) / (Math.Pow(2d, p) - 1);
                uValue[i] = uValue[i] + Math.Pow(2d, p) * S;
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
                dataGridView1[2, i + 1].Value = uValue[i];
            }

            chart1.Series[0].Points.Clear();
            for (int i = 0; i <= numberOfIterations; i++)
            {
                chart1.Series[0].Points.AddXY(xValue[i], uValue[i]);
            }

            Heh.URL = @"hehe.mp4";

        }




    }
}


