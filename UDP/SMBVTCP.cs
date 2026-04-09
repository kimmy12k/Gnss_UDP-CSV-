using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
            if (!IsConnected) throw new InvalidOperationException("장비가 연결되지 않았습니다.");
            await _writer.WriteLineAsync(command);
            await Task.Delay(COMMAND_DELAY_MS);
        }

        public async Task<string> QueryAsync(string command)
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

        public async Task ResetAsync()
        {
            await SendAsync("*RST");
            await Task.Delay(5000);
        }

        public async Task ClearStatusAsync()
            => await SendAsync("*CLS");

        public async Task GoToLocalAsync()
            => await SendAsync("&GTL");
        
        
        //27~29pge 장비 실행 절차,246~247page HIL 운영절차
        public async Task InitGnssAsync(
            string mode, double lat, double lon, double alt,
            int udpPort = 7755, double latency = 0.15)
        {
            await ResetAsync();
            await ClearStatusAsync();
            await CheckErrorAsync();
            await Task.Delay(2000);

            await SendAsync(":SOURce1:BB:GNSS:TMODe NAV");
            await SendAsync($":SOURce1:BB:GNSS:RECeiver:V1:POSition {mode}");

            await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:LOCation:SELect \"User Defined\"");
            await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:LOCation:COORdinates:RFRame WGS84");
            await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:LOCation:COORdinates:FORMat DEC");
            await SendAsync($":SOURce1:BB:GNSS:RECeiver:V1:LOCation:COORdinates:DEC:WGS {lon},{lat},{alt}");

            if (mode == "HIL")
            {
                await SendAsync(":SOURce1:BB:GNSS:RECeiver:V1:HIL:ITYPe UDP");
                await SendAsync($":SOURce1:BB:GNSS:RECeiver:V1:HIL:PORT {udpPort}");
                await SendAsync($":SOURce1:BB:GNSS:RECeiver:V1:HIL:SLATency {latency:F3}");
            }

            await SendAsync(":SOURce1:BB:GNSS:STATe 1");
            await Task.Delay(5000);
            await CheckErrorAsync();
            await SendAsync(":OUTPut1:STATe 1");
        }

        public async Task SendHilPositionAsync(
            double elapsedTime, double ecefX, double ecefY, double ecefZ,
            double velX = 0, double velY = 0, double velZ = 0,
            double accX = 0, double accY = 0, double accZ = 0,
            double yaw = 0, double pitch = 0, double roll = 0)
        {
            string cmd = $":SOURce1:BB:GNSS:RT:RECeiver:V1:HILPosition:MODE:A " +
                string.Format(CultureInfo.InvariantCulture,
                    "{0:F4},{1:F4},{2:F4},{3:F4},{4:F4},{5:F4},{6:F4},{7:F4},{8:F4},{9:F4},{10:F4},{11:F4},{12:F4}",
                    elapsedTime, ecefX, ecefY, ecefZ, velX, velY, velZ, accX, accY, accZ, yaw, pitch, roll);
            await SendAsync(cmd);
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
            => await QueryAsync(":SOURce1:BB:GNSS:RT:RECeiver:V1:HILPosition:LATency:STATistics?");

        public async Task<double> GetLevelAsync()
        {
            string response = await QueryAsync(":SOURce1:POWer:LEVel:IMMediate:AMPLitude?");
            return double.TryParse(response, out double level) ? level : -999;
        }
    }
}
