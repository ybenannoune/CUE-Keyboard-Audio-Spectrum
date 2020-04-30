using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CUE_Keyboard_Audio_Spectrum
{
    public partial class MainForm : Form
    {
        private BassAnalyzer bassAnalyzer;


        public MainForm()
        {
            InitializeComponent();

            bassAnalyzer = new BassAnalyzer(onAnalysis, 30);
            bassAnalyzer.DeviceList.ForEach((x) => comboBox_deviceList.Items.Add(x));
        }

        private void onAnalysis()
        {
            List<byte> data = bassAnalyzer.getSpectrumData();

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate () {
                    chart_Spectrum.Series.Clear();
                    Series series = this.chart_Spectrum.Series.Add("Spectrum");                
                    series.ChartType = SeriesChartType.Column;
                    foreach (byte b in data)
                    {
                        series.Points.Add(b);
                    }
                });
            }
         
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox_deviceList.Text))
            {
                MessageBox.Show("Please select a device");
            }
            else
            {
                var id = Convert.ToInt32((comboBox_deviceList.SelectedItem as string).Split(' ')[0]);           
                bassAnalyzer.ListenOnDeviceId(id);
                comboBox_deviceList.Enabled = false;
                ((Button)sender).Enabled = false;
            }      
        }
    }
}
