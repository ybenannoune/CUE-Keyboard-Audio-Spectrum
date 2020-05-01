using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;
using Un4seen.BassWasapi;

namespace CUE_Keyboard_Audio_Spectrum
{
    class BassAnalyzer
    {
        private List<String> _deviceList;
        private List<byte> _spectrumData;   //spectrum data buffer 
        private int _lines = 16;            // number of spectrum lines
        private float[] _fft;               //buffer for fft data     
        private WASAPIPROC _process;        //callback function to obtain data
        private System.Timers.Timer _t;     //timer that refreshes the analysis
        private bool _initialized;          //initialized flag
        public event EventHandler<List<byte>> ProcessCompleted;


        public List<string> DeviceList { get => _deviceList; }
        public List<byte> getSpectrumData() { return _spectrumData; }
        public bool Enabled { get => _enable; set => _enable = value; }

        private bool _enable = false;               //enabled status

        public BassAnalyzer(int period)
        {
            _fft = new float[1024];
            _t = new System.Timers.Timer(period);

            // Hook up the Elapsed event for the timer. 
            _t.Elapsed += _t_Tick;
            _t.AutoReset = true;
            _t.Enabled = false;

            _process = new WASAPIPROC(Process);
            _spectrumData = new List<byte>();
            _deviceList = new List<string>();

            Init();
        }

        // initialization
        private void Init()
        {
            bool result = false;
            for (int i = 0; i < BassWasapi.BASS_WASAPI_GetDeviceCount(); i++)
            {
                var device = BassWasapi.BASS_WASAPI_GetDeviceInfo(i);
                if (device.IsEnabled && device.IsLoopback)
                {
                    _deviceList.Add(string.Format("{0} - {1}", i, device.name));
                }
            }
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, false);
            result = Bass.BASS_Init(0, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            if (!result)
                throw new Exception("Init Error " + Bass.BASS_ErrorGetCode());
        }

        public void Stop()
        {
            Enabled = false;
            _initialized = false;
            BassWasapi.BASS_WASAPI_Stop(true);
            _t.Enabled = false;
        }
        
        public void ListenOnDeviceId(int deviceId)
        {
            Enabled = true;
            if (!_initialized)
            { 
                bool result = BassWasapi.BASS_WASAPI_Init(deviceId, 0, 0, BASSWASAPIInit.BASS_WASAPI_BUFFER, 1f, 0.05f, _process, IntPtr.Zero);
                if (!result)
                {
                    var error = Bass.BASS_ErrorGetCode();
                }
                else
                {
                    _initialized = true;
                }
            }
            BassWasapi.BASS_WASAPI_Start();
            _t.Enabled = true;
        }

        private void _t_Tick(object sender, EventArgs e)
        {
            int ret = BassWasapi.BASS_WASAPI_GetData(_fft, (int)BASSData.BASS_DATA_FFT2048); //get channel fft data
            if (ret < -1) return;
            int x, y;
            int b0 = 0;

            _spectrumData.Clear();

            //computes the spectrum data, the code is taken from a bass_wasapi sample.
            for (x = 0; x < _lines; x++)
            {
                float peak = 0;
                int b1 = (int)Math.Pow(2, x * 10.0 / (_lines - 1));
                if (b1 > 1023) b1 = 1023;
                if (b1 <= b0) b1 = b0 + 1;
                for (; b0 < b1; b0++)
                {
                    if (peak < _fft[1 + b0]) peak = _fft[1 + b0];
                }
                y = (int)(Math.Sqrt(peak) * 3 * 255 - 4);
                if (y > 255) y = 255;
                if (y < 0) y = 0;
                _spectrumData.Add((byte)y);       
            }


            //int level = BassWasapi.BASS_WASAPI_GetLevel();
            //LPower = Utils.LowWord32(level);
            //RPower = Utils.HighWord32(level);

            ProcessCompleted?.Invoke(this,_spectrumData);
        }

        // WASAPI callback, required for continuous recording
        private int Process(IntPtr buffer, int length, IntPtr user)
        {
            return length;
        }

        //cleanup
        public void Free()
        {
            BassWasapi.BASS_WASAPI_Free();
            Bass.BASS_Free();
        }
    }
}
