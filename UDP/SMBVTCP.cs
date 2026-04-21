using DevExpress.DataAccess.Native.Web;
using DevExpress.Utils.Html.Internal;
using DevExpress.XtraEditors;
using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPMode
{
    internal class SMBVTCP
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private const int COMMAND_DELAY_MS = 100;

        public bool IsConnected =>
            _client != null && _client.Connected && _reader != null;

        public async Task ConnectAsync(string ip, int port, int timeoutMs = 3000)
        {
            _client = new TcpClient();
            _client.ReceiveTimeout = timeoutMs;
            _client.SendTimeout = timeoutMs;
            await _client.ConnectAsync(ip, port);
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream, Encoding.ASCII);
            _writer = new StreamWriter(_stream, Encoding.ASCII) { AutoFlush = true };
        }

        public void Disconnect()
        {
            try { _writer?.Close(); _reader?.Close(); _stream?.Close(); _client?.Close(); }
            catch { }
            finally { _writer = null; _reader = null; _stream = null; _client = null; }
        }

        public async Task SendAsync(string command)
        {
            if (!IsConnected)throw new InvalidOperationException("장비가 연결되지 않았습니다.");

            await _writer.WriteLineAsync(command);
            await Task.Delay(COMMAND_DELAY_MS);
        }

        public async Task<string> QueryAsync(string command, int timeoutMs=1000)
        {
            
            await SendAsync(command);
            string response = await _reader.ReadLineAsync();
            return response?.Trim() ?? string.Empty;
        }
 


public async Task CheckErrorAsync()
        {
            string err = await QueryAsync(":SYSTem:ERRor?");
            if (!err.StartsWith("0")) throw new Exception($"장비 에러: {err}");
        }

        public async Task<string> GetErrorAsync()
            => await QueryAsync(":SYSTem:ERRor?");

        public async Task<string> GetIdentityAsync()
            => await QueryAsync("*IDN?");

        public async Task<string> GetOptionsAsync()
            => await QueryAsync("*OPT?");
       

        //----------------------------------------------------------
        public async Task ResetAsync()
        {
            await SendAsync("*RST;");
        }

        public async Task ClearStatusAsync()
            => await SendAsync("*CLS;");


        public async Task SetReferencePowerAsync(string level)
            =>await SendAsync($":SOURce1:BB:GNSS:POWer:REFerence {level}");

        public async Task SetTimeMode(string mode = "UTC")
            => await SendAsync($"BB:GNSS:TIME:STARt:TBASis {mode}");

        public async Task SetDate(string year, string month, string day)
            => await SendAsync($":SOURce1:BB:GNSS:TIME:STARt:DATE {year}, {month}, {day}");

        public async Task SetTime(string hour, string minute, string second)
            => await SendAsync($":SOURce1:BB:GNSS:TIME:STARt:TIME {hour}, {minute}, {second}");

        public async Task SetCurrentTime()
            => await SendAsync(":SOURce1:BB:GNSS:TIME:STARt:SCTime");
        

        public async Task GoToLocalAsync()
            => await SendAsync("&GTL");
        public async Task SendOnGnssAsync()
         => await SendAsync(":SOURce1: BB:GNSS: STATe 1");
        public async Task SendOffGnssAsync()
            => await SendAsync(":SOURce1: BB:GNSS: STATe 0");
        public async Task SendOnRadioFreq()
            => await SendAsync(":OUTPut1:STATe 1");
        public async Task SendOffRadioFreq()
            => await SendAsync(":OUTPut1:STATe 0");


        //27~29pge 장비 실행 절차,246~247page HIL 운영절차
        public async Task InitGnssAsync(
            string mode, double lat, double lon, double alt,
            int udpPort = 7755, double latency = 0.15)
        {
    

            await SendAsync(":SOURce1:BB:GNSS:TMODe NAV");//P.32 -> Switching from one test mode to the other presets all satellites parameters to theirdefault values.
            await SendAsync($":SOURce1:BB:GNSS:RECeiver:V1:POSition {mode}");//

            if (mode == "HIL")// page 251   HIL 진입 조건
            {
                await SendAsync($":SOURce1:BB:GNSS:RECeiver:V1:HIL:SLATency {latency:F3}");
                await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:HIL:ITYPe UDP");
                await SendAsync($":SOURce1:BB:GNSS:RECeiver:V1:HIL:PORT  {udpPort}");

            }
            await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:LOCation:SELect \"User Defined\"");
            await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:LOCation:COORdinates:RFRame WGS84");
            await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:LOCation:COORdinates:FORMat DEC");
            await SendAsync($":SOURce1:BB:GNSS:RECeiver:V1:LOCation:COORdinates:DEC:WGS {lon},{lat},{alt}");

            //await SendAsync(":SOURce1:BB:GNSS:STATe 1");
            //await Task.Delay(3000);
            //await CheckErrorAsync();
            //await SendAsync(":OUTPut1:STATe 1");
        }

        public async Task ResetIni()
        {
            await ResetAsync();
            await ClearStatusAsync();
            await CheckErrorAsync();
            await Task.Delay(2000);
        }

        public async Task ChangePositionAsync(double lat, double lon, double alt)
        {
            await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:LOCation:SELect \"User Defined\"");
            await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:LOCation:COORdinates:RFRame WGS84");
            await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:LOCation:COORdinates:FORMat DEC");
            await SendAsync($":SOURce1:BB:GNSS:RECeiver:V1:LOCation:COORdinates:DEC:WGS {lon},{lat},{alt}");
        }

        public async Task StopGnssAsync()
        {
            await SendAsync(":SOURce1:BB:GNSS:STATe 0");
            await SendAsync(":OUTPut1:STATe 0");
        }

        public async Task<string> GetSimInfoAsync()
            => await QueryAsync(":SOURce1:BB:GNSS:SIMulation:INFO?");

        public async Task<double> GetPdopAsync()
        {
            string response = await QueryAsync(":SOURce1:BB:GNSS:RT:PDOP?");
            return double.TryParse(response, out double pdop) ? pdop : 99.0;
        }

        public async Task<double> GetHwTimeAsync()
        {
            string response = await QueryAsync(":SOURce1:BB:GNSS:RT:HWTime?");
            return double.TryParse(response, NumberStyles.Float, CultureInfo.InvariantCulture, out double t) ? t : 0.0;
        }

        public async Task<string> GetHilLatencyStatsAsync()
            =>await QueryAsync(":SOURce1:BB:GNSS:RT:RECeiver:V1:HILPosition:LATency:STATistics?");


        public async Task<double> GetLevelAsync()
        {
            string response = await QueryAsync(":SOURce1:POWer:LEVel:IMMediate:AMPLitude?");
            return double.TryParse(response, out double level) ? level : -999;
        }

        public async Task<string> GetStartDateAsync()
            => await QueryAsync(":SOURce1:BB:GNSS:TIME:STARt:DATE?");
        public async Task<string> GetStartTimeAsync()
            => await QueryAsync(":SOURce1:BB:GNSS:TIME:STARt:TIME?");
      
        

    }
}
