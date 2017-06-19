using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphics_introduction
{
    public partial class Form1 : Form
    {

        System.Drawing.Graphics graphicsObj;
        Pen myPen = new Pen(System.Drawing.Color.Black, 3);
        Bitmap bmp = new Bitmap(759, 348);


        public Form1()
        {
            InitializeComponent();
        }


        int line(int x, int y, int shift)
        {
            graphicsObj = Graphics.FromImage(bmp);
            graphicsObj.DrawLine(myPen, x, y, x + shift, y);
            return x + shift;
        }


        void vertical_line(int x, DashStyle Dash, int high1, int high2)
        {

            graphicsObj = Graphics.FromImage(bmp);
            Pen myPen = new Pen(Color.Black,1.5F);
            myPen.DashStyle = Dash;
            graphicsObj.DrawLine(myPen, x, high1, x, high2);
           
          
        }


        string nrzl(string signal)
        {
            string nrzl_encoding = "";
            for (int i = 0; i < signal.Length; i++)
            {
                nrzl_encoding += (((int)signal[i] + 1) % 2).ToString();

            }

            return nrzl_encoding;
        }

        string nrzi(string signal)
        {
            string nrzi_encoding = "";
            int level = 0;

            for (int i = 0; i < signal.Length; i++)
            {
                if (signal[i] == '1')
                {
                    level = (level + 1) % 2;
                }

                nrzi_encoding += level;
            }

            return nrzi_encoding;

        }

        string manchester(string signal)
        {
            string manchester_encoding = "";
            for (int i = 0; i < signal.Length; i++)
            {
                if (signal[i] == '1') { manchester_encoding += "01"; }
                else { manchester_encoding += "10"; }
            }

            return manchester_encoding;
        }

        string Bipolar_AMI(string signal)
        {
            string ami_encoding = "";
            int sign = 1;
            for (int i = 0; i < signal.Length; i++)
            {
                if (signal[i] == '1')
                {
                    ami_encoding += sign == 1 ? '1' : '-';
                    sign = -sign;
                }
                else {
                    ami_encoding += "0";
                }
            }

            return ami_encoding;

        }

        string Pseudoternary(string signal)
        {

            string Pseudoternary_encoding = "";
            int sign = 1;
            for (int i = 0; i < signal.Length; i++)
            {
                if (signal[i] == '0')
                {
                    Pseudoternary_encoding += sign == 1 ? '1' : '-';
                    sign = -sign;
                }
                else
                {
                    Pseudoternary_encoding += "0";
                }
            }

            return Pseudoternary_encoding;
        }

        void draw_number(string signal)
        {
            int x = 5;
            Pen myPen = new Pen(Color.Black, 1.5F);
            myPen.DashStyle = DashStyle.DashDot;
            for (int i = 0; i < signal.Length; i++)
            {
                graphicsObj = Graphics.FromImage(bmp);
                graphicsObj.DrawString(signal[i].ToString(), new Font(FontFamily.GenericSansSerif,15,FontStyle.Regular), Brushes.Black, x, 5);
                graphicsObj.DrawLine(myPen, x, 5, x, 348);
                x = x + 50;
            }
            graphicsObj.DrawLine(myPen, x, 5, x, 348);
        }

        string effect_noise(string signal)
        {
            Random rnd = new Random();
            int bit_location=rnd.Next(signal.Length-1);
            signal = signal.Remove(bit_location, 1).Insert(bit_location, (((int)signal[bit_location] + 1) % 2).ToString());
            signal = signal.Remove(bit_location+1, 1).Insert(bit_location+1, (((int)signal[bit_location + 1] + 1) % 2).ToString());

            return signal;
        }

        void draw_normal(string signal, int shift, int x)
        {
            string set_bit = signal;
            int low_y = 45;
            int high_y = 20;
            int below_low_y = 70;

            int st_x = 5;


            int now = 0;
            for (int i = 0; i < set_bit.Length; i++)
            {
              
                if (set_bit[i] == '1')
                {
                    st_x = line(st_x, x + high_y, shift);//st_x + 50
                    now = x + high_y;

                }
                else if (set_bit[i] == '0')
                {
                    st_x = line(st_x, x + low_y, shift);//st_x + 50
                    now = x + low_y;

                }
                else if (set_bit[i] == '-') {

                    st_x = line(st_x, x + below_low_y, shift);
                    now = x + below_low_y;

                }


                if (i < set_bit.Length - 1)
                {
                    if (set_bit[i] != set_bit[i + 1])
                    {
                        int y = 0;
                        if (set_bit[i + 1] == '1')
                        {
                            y = x + high_y;
                        }
                        else if (set_bit[i + 1] == '0')
                        {
                            y = x + low_y;
                        }
                        else {
                            y = x + below_low_y;
                        }
                        vertical_line(st_x, DashStyle.Solid, y, now);

                    }
                }

                
            }
       
        }
                   

        private void button1_Click(object sender, EventArgs e)
        {
            string sgn = txtSignal.Text;
            string modified_sign = effect_noise(sgn);
            lblnoise.Text = modified_sign;

            pictureBox1.Image = null;
            bmp = new Bitmap(759, 348);
            draw_number(sgn);


            if (comboBox1.Text == "NRZ_L")
            {

                draw_normal(nrzl(sgn), 50, 100);
                draw_normal(nrzl(modified_sign), 50, 250);

            }
            else if (comboBox1.Text == "NRZ_I")
            {
                draw_normal(nrzi(sgn), 50, 100);
                draw_normal(nrzi(modified_sign), 50, 250);

            }
            else if (comboBox1.Text == "Manchester")
            {
                draw_normal(manchester(sgn), 25, 100);
                draw_normal(manchester(modified_sign), 25, 250);


            }
            else if (comboBox1.Text == "Bipolar-AMI")
            {
                draw_normal(Bipolar_AMI(sgn), 50, 100);
                draw_normal(Bipolar_AMI(modified_sign), 50, 250);

            }
            else if (comboBox1.Text == "Pseudoternary") {
                draw_normal(Pseudoternary(sgn), 50, 100);
                draw_normal(Pseudoternary(modified_sign), 50, 250);

            }

            pictureBox1.Image = bmp;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedText = comboBox1.Items[0].ToString();
            lblEncoding.Text = comboBox1.Text;
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblEncoding.Text = comboBox1.Text;
        }
    }
}
