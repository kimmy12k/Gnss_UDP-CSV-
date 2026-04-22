using DevExpress.CodeParser;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPMode
{
    public partial class Form1 : XtraForm
    {
        private CancellationTokenSource _hilCts = null;
        private SMBVTCP _tcp = null;
        private CsvRouteReader _route = null;

        // dt 배열 참조용 (UdpHilLoop에서 사용)
        private double[] _times;

        private const string StrInitialize = "initialize";
        private const string ModeHIL = "HIL";
        private const string IniFile = "config.ini";
        private const string StatusPreparing = "PREPARING";
        private const string StatusRunning = "RUNNING";
        private const string StatusFinished = "FINISHED";
        private const string StatusStoped = "STOPPED";


        private string _localIp = "";
        private string _deviceIp = "";
        private int _scpiPort = 5025;

        private bool IsConnected => _tcp != null && _tcp.IsConnected;

        private static readonly string CFG_PATH =
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IniFile);

        public Form1()
        {
            InitializeComponent();
        }
        private void Form_Load(object sender, EventArgs e)
        {
            _tcp = new SMBVTCP();
            MakeScrollable();
            InitCombos();
            LoadIni();
            SetControlState(false);
            ClearStatus();
            StyleButtons();
        }

        private void InitCombos()
        {
            comboTestMode.SelectedIndex = 0;
            comboPosition.SelectedIndex = 0;
            txtLat.Text = "0";
            txtLon.Text = "0";
            txtAlt.Text = "0";
        }

        // ════════════════════════════════════════════
        //          Change control states
        // ════════════════════════════════════════════
        private void SetControlState(bool connected)
        {
            btnConnect.Enabled = !connected;
            btnDisconnect.Enabled = connected;

            grSetTime.Enabled = connected;
            grGnssConfig.Enabled = connected;
            grControl.Enabled = connected;
            grStatus.Enabled = connected;
            
            btnInitialize.Enabled = connected;
            btnConfig.Enabled = connected;
            btnGnssOn.Enabled = connected;
            btnGnssOff.Enabled = connected;
            btnRfOn.Enabled = connected;
            btnRfOff.Enabled = connected;

            bool isHil = comboPosition.Text == ModeHIL;
            btnLoadCsv.Enabled = connected && isHil;
            btnHilStart.Enabled = false;
            btnHilStop.Enabled = connected;

        }

        // ════════════════════════════════════════════
        //              Load the INI file 
        // ════════════════════════════════════════════
        private void LoadIni()
        {
            try
            {
                var cfg = IniParser.Load(CFG_PATH);
                txtIP.EditValue = cfg.Get("Network", "IP", "169.254.2.20");
                txtScpiPort.EditValue = cfg.Get("Network", "ScpiPort", "5025");
                txtUdpPort.EditValue = cfg.Get("Network", "UdpPort", "7755");
                string lat = cfg.Get("Location", "Latitude", "");
                string lon = cfg.Get("Location", "Longitude", "");
                string alt = cfg.Get("Location", "Altitude", "");
                if (!string.IsNullOrEmpty(lat)) txtLat.Text = lat;
                if (!string.IsNullOrEmpty(lon)) txtLon.Text = lon;
                if (!string.IsNullOrEmpty(alt)) txtAlt.Text = alt;
            }
            catch { }
        }

        // ════════════════════════════════════════════
        //           Save as INI file
        // ════════════════════════════════════════════
        private void SaveIni()
        {
            try
            {
                var cfg = IniParser.Load(CFG_PATH);
                cfg.Set("Network", "IP", txtIP.Text.Trim());
                cfg.Set("Network", "ScpiPort", txtScpiPort.Text.Trim());
                cfg.Set("Network", "UdpPort", txtUdpPort.Text.Trim());
                cfg.Set("Location", "Latitude", txtLat.Text.Trim());
                cfg.Set("Location", "Longitude", txtLon.Text.Trim());
                cfg.Set("Location", "Altitude", txtAlt.Text.Trim());
                cfg.Save(CFG_PATH);
            }
            catch { }
        }

        // ════════════════════════════════════════════
        //                  Network
        // ════════════════════════════════════════════
        private void Log(string msg)
        {
            if (InvokeRequired) { Invoke(new Action(() => Log(msg))); return; }

            string line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            memoLog.Text += line + Environment.NewLine;
            memoLog.SelectionStart = memoLog.Text.Length;
            memoLog.ScrollToCaret();
        }

        // ════════════════════════════════════════════
        //              TCP/UDP connect
        // ════════════════════════════════════════════
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            btnConnect.Enabled = false;
            btnConnect.Text = "연결 중...";
            DrawStatusDot(Color.FromArgb(230, 81, 0));

            try
            {
                // TcpConnect
                _deviceIp = txtIP.Text.Trim();
                _scpiPort = int.Parse(txtScpiPort.Text.Trim());
                await _tcp.ConnectAsync(_deviceIp, _scpiPort);

                //  Pc Ip
                _localIp = getLocalIp();

                // SMVB identification and optins
                string idn = await _tcp.GetIdentityAsync();
                string opts = await _tcp.GetOptionsAsync();

                // Level
                double currentLevel = await _tcp.GetLevelAsync();
                txtLevel.Text = currentLevel.ToString("F1");
                Log($"현재 Level: {currentLevel} dBm");

                DrawStatusDot(Color.FromArgb(46, 125, 50));

                //enable
                SetControlState(true);

                // Log
                Log($"Connected: {idn}"); Log($"Options: {opts}"); Log($"Local IP: {_localIp}");
            }
            catch (Exception ex)
            {
                _tcp.Disconnect();
                DrawStatusDot(Color.FromArgb(198, 40, 40));
                btnConnect.Enabled = true; btnConnect.Text = "연결";


                Log($"연결 실패: {ex.Message}");
                XtraMessageBox.Show($"연결 실패\n\n{ex.Message}",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  Disconnect
        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            await StopHil();
            _tcp.Disconnect();
            DrawStatusDot(Color.FromArgb(198, 40, 40));
            SetControlState(false); btnConnect.Text = "연결";
            ClearStatus();
            Log("Disconnected");
        }

        // ════════════════════════════════════════════
        //                  Set Time
        // ════════════════════════════════════════════
        private async void btnSetDate_Click(object sender, EventArgs e)
        {
            if (DTOESetTimes.EditValue == null)
            {
                XtraMessageBox.Show("날짜를 먼저 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string year = ((DateTimeOffset)DTOESetTimes.EditValue).Year.ToString();
                string month = ((DateTimeOffset)DTOESetTimes.EditValue).Month.ToString();
                string day = ((DateTimeOffset)DTOESetTimes.EditValue).Day.ToString();
                string hour = ((DateTimeOffset)DTOESetTimes.EditValue).Hour.ToString();
                string minute = ((DateTimeOffset)DTOESetTimes.EditValue).Minute.ToString();
                string second = ((DateTimeOffset)DTOESetTimes.EditValue).Second.ToString();

                await _tcp.SetTimeMode();//SMBV100B uses UTC as the base time reference
                await _tcp.SetDate(year, month, day);
                await _tcp.SetTime(hour, minute, second);
                Log($"Set Date:{year}-{month}-{day}");
                Log($"Set Time:{hour}:{minute}:{second}");
            }
            catch (Exception ex)
            {
                Log($"설정 실패{ex.Message} ");
                XtraMessageBox.Show($"설정 실패\n\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSetCurrentDate_Click(object sender, EventArgs e)
        {
            try
            {
                await _tcp.SetTimeMode();
                await _tcp.SetCurrentTime();
                string Dates = await _tcp.GetStartDateAsync();
                string Times = await _tcp.GetStartTimeAsync();
                DTOESetTimes.DateTimeOffset = SetTimes(Dates, Times);
                string LogDates = (DTOESetTimes.DateTimeOffset.Date.ToString()).Substring(0, 10);
                Log($"Current Date: {LogDates}- {DTOESetTimes.DateTimeOffset.TimeOfDay}");
            }
            catch (Exception ex)
            {
                Log($"설정 실패{ex.Message} ");
                XtraMessageBox.Show(ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DateTimeOffset SetTimes(string Dates, string Times)
        {
            var dateParts = Dates.Split(',');
            var timeParts = Times.Split(',');
            return new DateTimeOffset(
                int.Parse(dateParts[0]),
                int.Parse(dateParts[1]),
                int.Parse(dateParts[2]),
                int.Parse(timeParts[0]),
                int.Parse(timeParts[1]),
                int.Parse(timeParts[2]),
                TimeSpan.Zero
                );
        }

        private async void btnClear_Click(object sender, EventArgs e)
        {
            await _tcp.ResetIni();
            Log("장비 리셋");

        }

        // ════════════════════════════════════════════
        //             Mode and Location
        // ════════════════════════════════════════════
        private void comboPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isHil = comboPosition.Text == ModeHIL;
            btnLoadCsv.Enabled = IsConnected && isHil;
            btnHilStart.Enabled = false;
            grHilMonitor.Enabled = isHil;
        }

        private async void btnInitialize_Click(object sender, EventArgs e)
        {
            if (!IsConnected) return;
            if (!ValidateCoordinates()) return;
            btnInitialize.Enabled = false;
            btnInitialize.Text = "초기화 중...";
            try
            {
                string mode = comboPosition.Text;
                double lat = double.Parse(txtLat.Text, CultureInfo.InvariantCulture);
                double lon = double.Parse(txtLon.Text, CultureInfo.InvariantCulture);
                double alt = double.Parse(txtAlt.Text, CultureInfo.InvariantCulture);
                int udpPort = int.Parse(txtUdpPort.Text.Trim());
                Log("Initialize 시작...");

                // GNSS 시뮬레이션 모드와 장소 설정
                await _tcp.InitGnssAsync(mode, lat, lon, alt, udpPort);

                string level = txtLevel.Text.Trim();
                await _tcp.SetReferencePowerAsync(level);
                Log($"RF Level → {level} dBm");

                string info = await _tcp.GetSimInfoAsync();
                Log($"Initialize 완료: {info}");
                Log(" RF 출력은 HIL Start에서 ON됩니다");

                UpdateStatus("ON", "대기", comboTestMode.Text, info);
            }
            catch (Exception ex)
            {
                Log($"Initialize 실패: {ex.Message}");
                XtraMessageBox.Show($"초기화 실패\n\n{ex.Message}",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnInitialize.Enabled = true;
                btnInitialize.Text = StrInitialize;
            }
        }

        private async void btnConfig_Click(object sender, EventArgs e)
        {
            if (!IsConnected) return;
            if (!ValidateCoordinates()) return;
            btnConfig.Enabled = false;

            try
            {
                double lat = double.Parse(txtLat.Text, CultureInfo.InvariantCulture);
                double lon = double.Parse(txtLon.Text, CultureInfo.InvariantCulture);
                double alt = double.Parse(txtAlt.Text, CultureInfo.InvariantCulture);
                Log("좌표 변경 중...");
                await _tcp.ChangePositionAsync(lat, lon, alt);
                Log($"좌표 변경 완료: {lat}, {lon}, {alt}");
            }
            catch (Exception ex) { Log($"좌표 변경 실패: {ex.Message}"); }
            finally { btnConfig.Enabled = true; }
        }

        // ════════════════════════════════════════════
        // GNSS ON / OFF
        // ════════════════════════════════════════════
        private async void btnGnssOn_Click(object sender, EventArgs e)
        {
            if (!IsConnected) return;
            try
            {
                await _tcp.SendOnGnssAsync();
                Log("GNSS State → ON"); lblGnssState.Text = "ON";
                lblGnssState.ForeColor = Color.FromArgb(46, 125, 50);
            }
            catch (Exception ex) { Log($"GNSS ON 실패: {ex.Message}"); }
        }

        private async void btnGnssOff_Click(object sender, EventArgs e)
        {
            if (!IsConnected) return;
            try
            {
                await _tcp.SendAsync(":SOURce1:BB:GNSS:STATe 0");
                Log("GNSS State → OFF");

                lblGnssState.Text = "OFF";
                lblGnssState.ForeColor = Color.FromArgb(198, 40, 40);
            }
            catch (Exception ex) { Log($"GNSS OFF 실패: {ex.Message}"); }
        }


        private async void btnRfOn_Click(object sender, EventArgs e)
        {
            if (!IsConnected) return;
            try
            {
                await _tcp.SendAsync(":OUTPut1:STATe 1");
                Log("RF Output → ON");
            }
            catch (Exception ex) { Log($"RF ON 실패: {ex.Message}"); }
        }

        private async void btnRfOff_Click(object sender, EventArgs e)
        {
            if (!IsConnected) return;
            try
            {
                await _tcp.SendAsync(":OUTPut1:STATe 0");
                Log("RF Output → OFF");
            }
            catch (Exception ex) { Log($"RF OFF 실패: {ex.Message}"); }
        }


        private void btnLoadCsv_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "경로 CSV 파일 선택",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                _route = new CsvRouteReader();
                _route.Load(dlg.FileName);

                var first = _route.GetAt(0);
                txtLat.Text = first.Latitude.ToString(
                    "F6", CultureInfo.InvariantCulture);

                txtLon.Text = first.Longitude.ToString(
                    "F6", CultureInfo.InvariantCulture);

                txtAlt.Text = first.Altitude.ToString(
                    "F0", CultureInfo.InvariantCulture);

                _route.ResetIndex();
                btnHilStart.Enabled = true;

                Log($"CSV 로드: {System.IO.Path.GetFileName(dlg.FileName)}" +
                    $" ({_route.Count}개 포인트, {_route.TotalDuration:F1}초)");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"CSV 로드 실패\n\n{ex.Message}",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); Log($"CSV 로드 실패: {ex.Message}");
            }
        }

        // ════════════════════════════════════════════
        // HIL Start
        // ════════════════════════════════════════════
        private async void btnHilStart_Click(object sender, EventArgs e)
        {
            if (!IsConnected) return;
            if (_route == null || _route.Count == 0)
            {
                XtraMessageBox.Show("경로 CSV 파일을 먼저 로드하세요.",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int udpPort = int.Parse(txtUdpPort.Text.Trim());

            btnHilStart.Enabled = false; btnHilStop.Enabled = true;
            btnInitialize.Enabled = false; btnLoadCsv.Enabled = false;
            btnConfig.Enabled = false;
            btnGnssOn.Enabled = false; btnGnssOff.Enabled = false;
            btnRfOn.Enabled = false; btnRfOff.Enabled = false;

            _hilCts = new CancellationTokenSource();

            lblPacketCount.Text = "0";
            lblUdpPort.Text = udpPort.ToString();
            lblHilStatus.Text = StatusPreparing;
            lblHilStatus.ForeColor = Color.FromArgb(230, 81, 0);

            long packetCount = 0;
           
            try
            {

                // status  
                lblHilStatus.Text = StatusRunning;
                lblHilStatus.ForeColor = Color.FromArgb(46, 125, 50);
                DrawStatusDot(Color.FromArgb(230, 81, 0));

                // start simulation
                await _tcp.SendAsync(":SOURce1:BB:GNSS:STATe 1");
                await Task.Delay(3000);
                await _tcp.SendAsync(":OUTPut1:STATe 1");


                //  &GTL 
                await _tcp.GoToLocalAsync();// page 245  L.C 5번 Go to Local
                await Task.Delay(1000);
                //_tcp.Disconnect();
                //Log(" &GTL → TCP 닫기 완료");


                //  ECEF 사전 변환
                int n = _route.Count;
                double[] px, py, pz, times;
                ConvertRouteToECEF(_route, out px, out py, out pz, out times);
                Log($" ECEF 사전 변환 완료 ({n}개)");

                //  UDP 소켓 생성
                UdpClient udp;
                udp = new UdpClient();//new IPEndPoint         
                Log($" UDP 소켓 생성 (바인딩: {_localIp})");

                var endpoint = new IPEndPoint(IPAddress.Parse(_deviceIp), udpPort);


                // UDP 전송
                packetCount = await Task.Run(()
                    => UdpHilLoop(px, py, pz, times, n, endpoint, udp, _hilCts.Token));
                Log($" UDP HIL 시작: {n}개 (속도/가속도/저크 포함)");

                // udp close
                udp.Close();
                Log($"경로 재생 완료: 총 {packetCount:N0} 패킷 전송");

                lblHilStatus.Text = StatusFinished;
                lblHilStatus.ForeColor = Color.FromArgb(21, 101, 192);

            }
            catch (OperationCanceledException)
            {
                Log("HIL 사용자 중지");
            }
            catch (Exception ex)
            {
                Log($"HIL 에러: {ex.Message}");
            }
            finally
            {
                if (lblHilStatus.Text != StatusFinished)
                {
                    lblHilStatus.Text = StatusStoped;
                    lblHilStatus.ForeColor = Color.FromArgb(198, 40, 40);
                }
                Log($"HIL 종료: {packetCount:N0} 패킷" + $" / 초");//{totalWatch.Elapsed.TotalSeconds:F1}
                Log(" TCP 재연결 중...");
                try
                {
                    //await _tcp.ConnectAsync(_deviceIp, _scpiPort);
                    //DrawStatusDot(Color.FromArgb(46, 125, 50));
                    //Log(" TCP 재연결 완료");
                    string stats = await _tcp.GetHilLatencyStatsAsync(); // 255page Commands 해석// Latency Calibration
                    Log($" HIL 통계: {stats}");
                    string err = await _tcp.GetErrorAsync();
                    Log($" 장비 에러: {err}");
                    SetControlState(true);
                }
                catch (Exception ex)
                {
                    Log($"TCP 재연결 실패: {ex.Message}");
                    DrawStatusDot(Color.FromArgb(198, 40, 40));
                    SetControlState(false);
                    btnConnect.Enabled = true;
                    btnConnect.Text = "연결";
                }
                await Task.Delay(1000);
                //await StopHil();
                btnHilStart.Enabled = true;
                btnHilStop.Enabled = false;
            }
        }

        // ════════════════════════════════════════════
        // CSV → ECEF 사전 변환
        // ════════════════════════════════════════════
        // CSV 전체를 ECEF 배열로 미리 변환
        // → UdpHilLoop에서 인덱스로 접근 가능
        //
        private void ConvertRouteToECEF(
            CsvRouteReader route,
            out double[] px, out double[] py, out double[] pz,
            out double[] times)
        {
            int n = route.Count;
            px = new double[n];
            py = new double[n];
            pz = new double[n];
            times = new double[n];

            for (int i = 0; i < n; i++)
            {
                var pt = route.GetAt(i);
                CoordConverter.ToECEF(
                    pt.Latitude, pt.Longitude, pt.Altitude,
                    out px[i], out py[i], out pz[i]);
                times[i] = pt.Time;
            }
        }

        // 속도 계산 — (현재위치 - 이전위치) / dt
        // i=0: 이전이 없음 → 속도 0
        //
        private void CalcVelocity(
            double[] p, int i, double dt,
            out double v, out double vNext)
        {
            // 현재 속도
            if (i > 0)
                v = (p[i] - p[i - 1]) / dt;
            else
                v = 0;

            // 다음 속도 (가속도 계산용)
            if (i < p.Length - 1)
            {
                double dtNext = GetDt(i + 1);
                vNext = (p[i + 1] - p[i]) / dtNext;
            }
            else
            {
                vNext = v; // 마지막: 현재 속도 유지
            }
        }


        // 가속도 계산 — (다음속도 - 현재속도) / dt(시간)
        // i=0 또는 i=n-1: 경계 → 가속도 0
        private double CalcAcceleration(double v, double vNext, double dt)
        {
            return (vNext - v) / dt;
        }

        // 저크 계산 — (다음가속도 - 현재가속도) / dt
        // i < 2 또는 i >= n-2: 경계 → 저크 0
        private double CalcJerk(double a, double aNext, double dt)
        {
            return (aNext - a) / dt;
        }

        private double GetDt(int i)
        {
            if (i <= 0 || _times == null) return 1.0;// 왜 1.0이지?

            double dt = _times[i] - _times[i - 1];
            return (dt > 0) ? dt : 1.0;
        }

        // ════════════════════════════════════════════
        // UDP HIL 전송 루프 — 백그라운드 스레드
        // ════════════════════════════════════════════

        private long UdpHilLoop(
            double[] px, double[] py, double[] pz,
            double[] times, int n,
            IPEndPoint endpoint, UdpClient udp, CancellationToken token)
        {
            _times = times; // GetDt에서 사용
            long count = 0;
            double elapsed = 0;
            //var loopWatch = new Stopwatch();
            //loopWatch.Start();

            for (int i = 0; i < n && !token.IsCancellationRequested; i++)
            {
                //loopWatch.Restart();
                double dt = GetDt(i);

                // ── 속도 (X, Y, Z 각각) ──
                CalcVelocity(px, i, dt, out double vx, out double vxNext);
                CalcVelocity(py, i, dt, out double vy, out double vyNext);
                CalcVelocity(pz, i, dt, out double vz, out double vzNext);

                // ── 가속도 ──
                double ax = 0, ay = 0, az = 0;
                double axNext = 0, ayNext = 0, azNext = 0;
                if (i > 0 && i < n - 1)
                {
                    ax = CalcAcceleration(vx, vxNext, dt);
                    ay = CalcAcceleration(vy, vyNext, dt);
                    az = CalcAcceleration(vz, vzNext, dt);

                    // 다음 가속도 (저크 계산용)
                    if (i < n - 2)
                    {
                        double dtNext = GetDt(i + 1);
                        double vx2 = (px[i + 2] - px[i + 1]) / GetDt(i + 2);
                        double vy2 = (py[i + 2] - py[i + 1]) / GetDt(i + 2);
                        double vz2 = (pz[i + 2] - pz[i + 1]) / GetDt(i + 2);
                        axNext = CalcAcceleration(vxNext, vx2, dtNext);
                        ayNext = CalcAcceleration(vyNext, vy2, dtNext);
                        azNext = CalcAcceleration(vzNext, vz2, dtNext);
                    }
                    else
                    {
                        axNext = ax; ayNext = ay; azNext = az;
                    }
                }

                // ── 저크 ──
                double jx = 0, jy = 0, jz = 0;
                if (i > 0 && i < n - 2)
                {
                    jx = CalcJerk(ax, axNext, dt);
                    jy = CalcJerk(ay, ayNext, dt);
                    jz = CalcJerk(az, azNext, dt);
                }
                // ── ElapsedTime ──

                elapsed = times[i] ;//timeOffset

                // ── 패킷 빌드 + 전송 ──
                byte[] packet = HilPacket.Build(
                        elapsed,
                        px[i], py[i], pz[i],
                        vx, vy, vz,
                        ax, ay, az,
                        jx, jy, jz);

                udp.Send(packet, packet.Length, endpoint);

                // ── UI 업데이트 ── // 명려어로 바꾸기
                count++;
                int remaining = n - i - 1;
                long currentCount = count;

                BeginInvoke(new Action(() =>
                {
                    lblPacketCount.Text = currentCount.ToString("N0");
                    lblHilStatus.Text = $"{remaining} left";
                }));

               
                int intervalMs=0;
                if(i<n-1)
                intervalMs = (int)((times[i + 1] - times[i]) * 1000); // 초 → ms 변환!
                
                Thread.Sleep(intervalMs);
            }
                
            _times = null;
            return count;
        }


        private async void btnHilStop_Click(object sender, EventArgs e)
        {
            await StopHil();
        }

        //내 PC의 IPv4 주소 하나를 뽑아서 문자열로 반환
        private string getLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());// 내 pc 이름으로 DNS 조회
            var ip = host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
            return ip?.ToString();
        }

        private async Task StopHil()
        {
            await _tcp.SendOffRadioFreq();
            await Task.Delay(1000);
            await _tcp.SendOffGnssAsync();
            _hilCts?.Cancel();
            _hilCts = null;
        }
        // ════════════════════════════════════════════
        // Log Clear
        // ════════════════════════════════════════════
        private void btnLogClear_Click(object sender, EventArgs e)
        {
            memoLog.Text = string.Empty;
        }

        // ════════════════════════════════════════════
        // Status
        // ════════════════════════════════════════════
        private void UpdateStatus(
            string gnss, string rf, string mode, string info)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                    UpdateStatus(gnss, rf, mode, info)));
                return;
            }
            lblGnssState.Text = gnss;
            lblGnssState.ForeColor = gnss == "ON"
                ? Color.FromArgb(46, 125, 50)// 초록색
                : Color.FromArgb(198, 40, 40);// 빨간색
            lblTestMode.Text = mode;
            lblSimInfo.Text = info;
        }

        private void ClearStatus()
        {
            lblGnssState.Text = "-";
            lblGnssState.ForeColor = Color.Gray;
            lblTestMode.Text = "-";
            lblSimInfo.Text = "-";
            lblPacketCount.Text = "0";
            lblHilStatus.Text = "-";
            lblUdpPort.Text = "-";
        }


        // ════════════════════════════════════════════
        // 검증
        // ════════════════════════════════════════════
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtIP.Text))
            {
                XtraMessageBox.Show("IP를 입력하세요.",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIP.Focus(); return false;
            }
            if (!int.TryParse(txtScpiPort.Text, out int sp) ||
                sp < 1 || sp > 65535)
            {
                XtraMessageBox.Show("SCPI Port: 1~65535",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtScpiPort.Focus(); return false;
            }
            if (!int.TryParse(txtUdpPort.Text, out int up) ||
                up < 1 || up > 65535)
            {
                XtraMessageBox.Show("UDP Port: 1~65535",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUdpPort.Focus(); return false;
            }
            return true;
        }

        private bool ValidateCoordinates()
        {
            if (!double.TryParse(txtLat.Text, NumberStyles.Float,
                CultureInfo.InvariantCulture, out double lat) ||
                lat < -90 || lat > 90)
            {
                XtraMessageBox.Show("위도: -90~90",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLat.Focus(); return false;
            }
            if (!double.TryParse(txtLon.Text, NumberStyles.Float,
                CultureInfo.InvariantCulture, out double lon) ||
                lon < -180 || lon > 180)
            {
                XtraMessageBox.Show("경도: -180~180",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLon.Focus(); return false;
            }
            if (!double.TryParse(txtAlt.Text, NumberStyles.Float,
                CultureInfo.InvariantCulture, out _))
            {
                XtraMessageBox.Show("고도: 숫자 입력",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAlt.Focus(); return false;
            }
            return true;
        }

        private void DrawStatusDot(Color color)
        {
            var old = picStatus.Image;
            var bmp = new Bitmap(16, 16);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            using var brush = new SolidBrush(color);
            g.FillEllipse(brush, 2, 2, 12, 12);
            picStatus.Image = bmp;
            old?.Dispose();
        }

        private void MakeScrollable()
        {
            var scroll = new DevExpress.XtraEditors.XtraScrollableControl();
            scroll.Dock = DockStyle.Fill;
            while (this.Controls.Count > 0)
                scroll.Controls.Add(this.Controls[0]);
            this.Controls.Add(scroll);
        }

        private void StyleButtons()
        {
            var groups = new Control[] { grControl, grNetwork };
            foreach (var group in groups)
            {
                foreach (var btn in grControl.Controls.OfType<SimpleButton>())
                {
                    btn.Appearance.BackColor = Color.FromArgb(60, 60, 60);
                    btn.Appearance.ForeColor = Color.White;
                    btn.Appearance.BorderColor = Color.FromArgb(90, 90, 90);
                    btn.Appearance.Options.UseBackColor = true;
                    btn.Appearance.Options.UseForeColor = true;
                    btn.Appearance.Options.UseBorderColor = true;
                }
            }

            // 강조 버튼 따로
            btnInitialize.Appearance.BackColor = Color.Yellow;
            btnHilStart.Appearance.BackColor = Color.FromArgb(55, 138, 221);
            btnLoadCsv.Appearance.BackColor = Color.FromArgb(29, 158, 117);
            btnHilStop.AppearanceDisabled.ForeColor = Color.White;
            btnHilStop.AppearanceDisabled.Options.UseForeColor = true;
        }
        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            if (IsConnected)
            {
                await StopHil();
                _tcp?.Disconnect();
                SaveIni();
            }
            base.OnFormClosing(e);
        }

        private async void btnCheckpackets_Click(object sender, EventArgs e)
        {
            string stats = await _tcp.GetHilLatencyStatsAsync(); // 255page Commands 해석// Latency Calibration
            Log($" HIL 통계: {stats}");
        }
    }
}
