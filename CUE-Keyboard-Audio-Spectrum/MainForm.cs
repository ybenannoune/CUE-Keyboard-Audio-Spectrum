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

using CUE.NET;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Exceptions;

namespace CUE_Keyboard_Audio_Spectrum
{
    public partial class MainForm : Form
    {
        private BassAnalyzer bassAnalyzer;

        public static int[,] EqualizerKeyMap = new int[5, 12]
        {
                {49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 },
                {37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48 },
                {25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 },
                {13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 },
                {01, 01, 02 ,03, 04, 05, 05, 06, 07, 08, 09, 09 },
        };

        public MainForm()
        {
            InitializeComponent();

            initCUESDK();

            bassAnalyzer = new BassAnalyzer(onAnalysis, 30);
            bassAnalyzer.DeviceList.ForEach((x) => comboBox_deviceList.Items.Add(x));
        }

        private void initCUESDK()
        {
            try
            {
                CueSDK.Initialize();
                Console.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");

                CorsairKeyboard keyboard = CueSDK.KeyboardSDK;
                if (keyboard == null)
                    throw new WrapperException("No keyboard found");
            }
            catch (CUEException ex)
            {
                Console.WriteLine("CUE Exception! ErrorCode: " + Enum.GetName(typeof(CorsairError), ex.Error));
            }
            catch (WrapperException ex)
            {
                Console.WriteLine("Wrapper Exception! Message:" + ex.Message);
            }
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

            // Set everything to Black
            for (int i = 0; i < CueSDK.KeyboardSDK.Leds.Count(); i++)
                CueSDK.KeyboardSDK.Leds.ElementAt(i).Color = Color.FromArgb(255, 0, 0, 0);

            CorsairKeyboard keyboard = CueSDK.KeyboardSDK;
            for (int i = 0; i < EqualizerKeyMap.GetLength(1) ; i++)
            {
                byte val = data[i];
                if (val > 30) //Seuil 
                    CueSDK.KeyboardSDK[(CorsairLedId)EqualizerKeyMap[0, i]].Color = Color.Red;
                if (val > 75)
                    CueSDK.KeyboardSDK[(CorsairLedId)EqualizerKeyMap[1, i]].Color = Color.Orange;
                if (val > 130)
                    CueSDK.KeyboardSDK[(CorsairLedId)EqualizerKeyMap[2, i]].Color = Color.Blue;
                if (val > 170)
                    CueSDK.KeyboardSDK[(CorsairLedId)EqualizerKeyMap[3, i]].Color = Color.BlueViolet;
                if (val > 215)
                    CueSDK.KeyboardSDK[(CorsairLedId)EqualizerKeyMap[4, i]].Color = Color.Violet;
            }

            CueSDK.KeyboardSDK.Update();
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
