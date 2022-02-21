using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Resources;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUITRACTOR
{
    public partial class Form1 : Form
    {
        long max = 100, min = -100;
        long max1 = 110, min1 = -110;
        int c;
        Image file;
        Boolean opened = false;
        Bitmap newBitmap;
        int timer = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            this.Location = new Point(0, 0);
            this.Size = new Size(w, h);
            String[] portlist = System.IO.Ports.SerialPort.GetPortNames();
            foreach (String portName in portlist)
                comboBox1.Items.Add(portName);
            comboBox1.Text = comboBox1.Items[comboBox1.Items.Count - 1].ToString();
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("38400");
            comboBox2.Items.Add("57600");
            comboBox2.Items.Add("115200");
            timer1.Stop();
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            timer1.Start();
           
        }
        private void writeCSV(string date,string teks)
        {
            string path = date + ".csv";
            string cureFile = @path;
            if(!File.Exists(cureFile))
            {
                File.Create(cureFile).Close();
            }
            try
            {
                File.AppendAllText(cureFile,teks);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString() + Environment.NewLine + "can't acces" + cureFile + Environment.NewLine + "new file maybe used another application", "write file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            DialogResult save = saveFileDialog1.ShowDialog();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("yakin ingin keluar?", "konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                serialPort1.Close();
                timer1.Stop();
                Application.Exit();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        string RXstring;
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            String receivedMsg = serialPort1.ReadLine();
            Tampilkan(receivedMsg);
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Int32.Parse(comboBox2.Text);
                serialPort1.NewLine = "\r\n";
                serialPort1.Open();
                toolStrip1.Text = serialPort1.PortName + "is connected.";
            }
            catch (Exception ex)
            {
                toolStrip1.Text = "error:" + ex.Message.ToString();
            }
        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            toolStrip1.Text = serialPort1.PortName + "is closed.";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void serialPort1_DataReceived_1(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            String receivedMsg = serialPort1.ReadLine();
            Tampilkan(receivedMsg);
        }
        private delegate void TampilkanDelegate(object item);
        private void Tampilkan(object item)
        {
            if (InvokeRequired)
                listBox1.Invoke(new TampilkanDelegate(Tampilkan), item);
            else
            {
                listBox1.Items.Add(item);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                splitData(item);
            }
        }
        private void splitData(object item)
        {

            try
            {
                String[] data = item.ToString().Split(',');
                textBox1.Text = data[0];
                textBox2.Text = data[1];
                textBox3.Text = data[2];
                textBox4.Text = data[3];
                textBox5.Text = data[4];
                textBox6.Text = data[5];
                label10.Text = data[6];
                chart1.ChartAreas[0].AxisX.Minimum = min;
                chart1.ChartAreas[0].AxisX.Maximum = max;

                chart2.ChartAreas[0].AxisX.Minimum = min1;
                chart2.ChartAreas[0].AxisX.Maximum = max1;

                chart1.ChartAreas[0].AxisY.Minimum = -5000;
                chart1.ChartAreas[0].AxisY.Maximum = 5000;

                chart2.ChartAreas[0].AxisY.Minimum = -5000;
                chart2.ChartAreas[0].AxisY.Maximum = 5000;

                chart1.ChartAreas[0].AxisX.ScaleView.Zoom(min, max);
                serialPort1.Write("1");
                if (data[0] != null)
                {
                    this.chart1.Series[0].Points.AddXY((min + max) / 2, data[0]);
                    max++;
                    min++;
                }
                if (data[1] != null)
                {
                    this.chart2.Series[0].Points.AddXY((min1 + max1) / 2, data[1]);
                    max1++;
                    min1++;
                }
                serialPort1.DiscardInBuffer();
            }
            catch
            {

            }
        }
        int menitlast;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int menitcurrent;
            string headerTeks = "ACC X;ACC Y; ACC Z;GYRO X;GYRO Y;GYRO Z;" + Environment.NewLine;
            string header = label1.Text + ";" + label5.Text + ";" + label6.Text + ";" + label7.Text + ";" + label8.Text + ";" + label9.Text + ";" + Environment.NewLine;
            string logText = textBox1.Text + ";" + textBox2.Text + ";" + textBox3.Text + ";" + textBox4.Text + ";" + textBox5.Text + ";" + textBox6.Text + ";" + Environment.NewLine;
            string namaFile = ENGINE.Text;
            menitcurrent = DateTime.Now.Minute;
            if(menitcurrent!=menitlast)
            {
                writeCSV(namaFile, headerTeks);
            }
            writeCSV(namaFile, logText);
            menitlast = menitcurrent;
        }
    }
}
