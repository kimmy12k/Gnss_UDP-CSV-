using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace UDPMode
{
    public partial class Form1 : XtraForm
    {
        private CancellationTokenSource _hilCts = null;
        private SMBVTCP _tcp;
        private CsvRouteReader _route;

        private string _localIp = "192.168.1.21";
        private string _deviceIp = "";
        private int _scpiPort = 5025;

        private bool IsConnected => _tcp != null && _tcp.IsConnected;

        private static readonly string CFG_PATH =
            System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "config.ini");

        public Form1()
        {
            InitializeComponent();
            _tcp = new SMBVTCP();
            InitCombos();
            LoadIni();
            SetControlState(false);
            ClearStatus();
        }

        private void InitCombos()
        {
            comboTestMode.SelectedIndex = 0;
            comboPosition.SelectedIndex = 0;
            txtLat.Text = "0";
            txtLon.Text = "0";
            txtAlt.Text = "0";
        }

        private void SetControlState(bool connected)
        {
            btnConnect.Enabled = !connected;
            btnDisconnect.Enabled = connected;
            grGnssConfig.Enabled = connected;
            btnInitialize.Enabled = connected;
            btnConfig.Enabled = connected;
            btnGnssOn.Enabled = connected;
            btnGnssOff.Enabled = connected;
            btnRfOn.Enabled = connected;
            btnRfOff.Enabled = connected;
            bool isHil = comboPosition.Text == "HIL";
            btnLoadCsv.Enabled = connected && isHil;
            btnHilStart.Enabled = false;
            btnHilStop.Enabled = false;
        }

        // ════════════════════════════════════════════
        // INI 로드 / 저장
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
        // 로그
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
        // 연결 / 해제
        // ════════════════════════════════════════════
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            btnConnect.Enabled = false;
            btnConnect.Text = "연결 중...";
            DrawStatusDot(Color.FromArgb(230, 81, 0));
            try
            {
                _deviceIp = txtIP.Text.Trim();
                _scpiPort = int.Parse(txtScpiPort.Text.Trim());
                await _tcp.ConnectAsync(_deviceIp, _scpiPort);
                _localIp = "192.168.1.21";
                string idn = await _tcp.GetIdentityAsync();
                string opts = await _tcp.GetOptionsAsync();
                double currentLevel = await _tcp.GetLevelAsync();
                txtLevel.Text = currentLevel.ToString("F1");
                Log($"현재 Level: {currentLevel} dBm");
                DrawStatusDot(Color.FromArgb(46, 125, 50));
                SetControlState(true);
                Log($"Connected: {idn}");
                Log($"Options: {opts}");
                Log($"Local IP: {_localIp}");
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

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            StopHil();
            _tcp.Disconnect();
            DrawStatusDot(Color.FromArgb(198, 40, 40));
            SetControlState(false); btnConnect.Text = "연결";
            ClearStatus(); Log("Disconnected");
        }

        // ════════════════════════════════════════════
        // Position 드롭다운
        // ════════════════════════════════════════════
        private void comboPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isHil = comboPosition.Text == "HIL";
            btnLoadCsv.Enabled = IsConnected && isHil;
            btnHilStart.Enabled = false;
            grHilMonitor.Enabled = isHil;
        }

        // ════════════════════════════════════════════
        // Initialize
        // ════════════════════════════════════════════
        private async void btnInitialize_Click(object sender, EventArgs e)
        {
            if (!IsConnected) return;
            if (!ValidateCoordinates()) return;
            btnInitialize.Enabled = false;
            btnInitialize.Text = "초기화 중...";
            try
            {
                //UI값 → 명령어로 전달
                string mode = comboPosition.Text;
                double lat = double.Parse(txtLat.Text, CultureInfo.InvariantCulture);
                double lon = double.Parse(txtLon.Text, CultureInfo.InvariantCulture);
                double alt = double.Parse(txtAlt.Text, CultureInfo.InvariantCulture);
                int udpPort = int.Parse(txtUdpPort.Text.Trim());

                Log("Initialize 시작...");

                //
                await _tcp.InitGnssAsync(mode, lat, lon, alt, udpPort);
                string level = txtLevel.Text.Trim();
                await _tcp.SendAsync($":SOURce1:BB:GNSS:POWer:REFerence {level}");
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
                btnInitialize.Text = "Initialize";
            }
        }

        // ════════════════════════════════════════════
        // Config
        // ════════════════════════════════════════════
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
                await _tcp.SendAsync(":SOURce1:BB:GNSS:STATe 1");
                Log("GNSS State → ON");
                lblGnssState.Text = "ON";
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

        // ════════════════════════════════════════════
        // RF ON / OFF
        // ════════════════════════════════════════════
        private async void btnRfOn_Click(object sender, EventArgs e)
        {
            if (!IsConnected) return;
            try
            {
                await _tcp.SendAsync(":OUTPut1:STATe 1");
                Log("RF Output → ON");
                lblRfState.Text = "ON";
                lblRfState.ForeColor = Color.FromArgb(46, 125, 50);
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
                lblRfState.Text = "OFF";
                lblRfState.ForeColor = Color.FromArgb(198, 40, 40);
            }
            catch (Exception ex) { Log($"RF OFF 실패: {ex.Message}"); }
        }

        // ════════════════════════════════════════════
        // CSV Load
        // ════════════════════════════════════════════
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
                Log($"CSV 로드 실패: {ex.Message}");
                XtraMessageBox.Show($"CSV 로드 실패\n\n{ex.Message}",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            int intervalMs = 1000;

            btnHilStart.Enabled = false; btnHilStop.Enabled = true;
            btnInitialize.Enabled = false; btnLoadCsv.Enabled = false;
            btnConfig.Enabled = false;
            btnGnssOn.Enabled = false; btnGnssOff.Enabled = false;
            btnRfOn.Enabled = false; btnRfOff.Enabled = false;

            _hilCts = new CancellationTokenSource();

            lblPacketCount.Text = "0"; lblLatency.Text = "-";
            lblUpdateRate.Text = $"{1000 / intervalMs} Hz";
            lblUdpPort.Text = udpPort.ToString();
            lblHilStatus.Text = "PREPARING";
            lblHilStatus.ForeColor = Color.FromArgb(230, 81, 0);

            long packetCount = 0;
            var totalWatch = new Stopwatch();

            try
            {

                //  HWTime 읽기
                double hwTime = await _tcp.GetHwTimeAsync();
                Log($" HWTime = {hwTime:F2}초");
                if (hwTime <= 0) throw new Exception("HWTime = 0. Initialize를 먼저 실행하세요.");

                //  &GTL + TCP 닫기
                var delayWatch = Stopwatch.StartNew();
                await Task.Delay(1000);
                await _tcp.GoToLocalAsync();
                await Task.Delay(1000);
                _tcp.Disconnect();
                Log(" &GTL → TCP 닫기 완료");

                //  오프셋 계산
                double measuredDelay = delayWatch.Elapsed.TotalSeconds;
                double offset = hwTime + measuredDelay + 0.0;   
                Log($" offset = {offset:F2}" + $" (HWTime={hwTime:F2}" +$" + 지연={measuredDelay:F3})");

                //  ECEF 사전 변환
                int n = _route.Count;
                double[] px, py, pz, times;
                ConvertRouteToECEF(_route, out px, out py, out pz, out times);
                Log($" ECEF 사전 변환 완료 ({n}개)");

                //  UDP 소켓 생성
                UdpClient udp;
                try
                {
                    udp = new UdpClient(new IPEndPoint(IPAddress.Parse(_localIp), 0));
                    Log($" UDP 소켓 생성 (바인딩: {_localIp})");
                }
                catch
                {
                    udp = new UdpClient();
                    Log(" UDP 소켓 생성 (자동 라우팅)");
                }
                var endpoint = new IPEndPoint(
                    IPAddress.Parse(_deviceIp), udpPort);

                //  UDP 전송 루프
                totalWatch.Start();
                Log($" UDP HIL 시작: {n}개 / {intervalMs}ms (속도/가속도/저크 포함)");

                BeginInvoke(new Action(() =>
                {
                    lblHilStatus.Text = "RUNNING";
                    lblHilStatus.ForeColor = Color.FromArgb(46, 125, 50);
                    lblRfState.Text = "ON";
                    lblRfState.ForeColor = Color.FromArgb(46, 125, 50);
                    DrawStatusDot(Color.FromArgb(230, 81, 0));
                }));

                packetCount = await Task.Run(() =>
                    UdpHilLoop(px, py, pz, times, n, offset,intervalMs, endpoint, udp, _hilCts.Token));

                udp.Close();

                Log($"경로 재생 완료: 총 {packetCount:N0} 패킷 전송");
                BeginInvoke(new Action(() =>
                {
                    lblHilStatus.Text = "FINISHED";
                    lblHilStatus.ForeColor = Color.FromArgb(21, 101, 192);
                }));
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
                totalWatch.Stop();
                if (lblHilStatus.Text != "FINISHED")
                {
                    lblHilStatus.Text = "STOPPED";
                    lblHilStatus.ForeColor = Color.FromArgb(198, 40, 40);
                }
                Log($"HIL 종료: {packetCount:N0} 패킷" +
                    $" / {totalWatch.Elapsed.TotalSeconds:F1}초");

                Log(" TCP 재연결 중...");
                try
                {
                    await _tcp.ConnectAsync(_deviceIp, _scpiPort);
                    DrawStatusDot(Color.FromArgb(46, 125, 50));
                    Log(" TCP 재연결 완료");
                    string stats = await _tcp.GetHilLatencyStatsAsync(); // 255page Commands 해석
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

        // ════════════════════════════════════════════
        // 속도 계산 — (현재위치 - 이전위치) / dt
        // ════════════════════════════════════════════
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

        // ════════════════════════════════════════════
        // 가속도 계산 — (다음속도 - 현재속도) / dt(시간)
        // ════════════════════════════════════════════
        // i=0 또는 i=n-1: 경계 → 가속도 0
        //
        private double CalcAcceleration(double v, double vNext, double dt)
        {
            return (vNext - v) / dt;
        }

        // ════════════════════════════════════════════
        // 저크 계산 — (다음가속도 - 현재가속도) / dt
        // ════════════════════════════════════════════
        // i < 2 또는 i >= n-2: 경계 → 저크 0
        //
        private double CalcJerk(double a, double aNext, double dt)
        {
            return (aNext - a) / dt;
        }

        // dt 배열 참조용 (UdpHilLoop에서 사용)
        private double[] _times;

        private double GetDt(int i)
        {
            if (i <= 0 || _times == null) return 1.0;
            double dt = _times[i] - _times[i - 1];
            return (dt > 0) ? dt : 1.0;
        }

        // ════════════════════════════════════════════
        // UDP HIL 전송 루프 — 백그라운드 스레드
        // ════════════════════════════════════════════
        //
        // 속도/가속도/저크를 CalcVelocity, CalcAcceleration,
        // CalcJerk 메서드로 캡슐화하여 루프를 깔끔하게 유지
        //
        private long UdpHilLoop(
            double[] px, double[] py, double[] pz,
            double[] times, int n, double offset,
            int intervalMs, IPEndPoint endpoint,
            UdpClient udp, CancellationToken token)
        {
            _times = times; // GetDt에서 사용
            long count = 0;
            var loopWatch = new Stopwatch();

            for (int i = 0; i < n && !token.IsCancellationRequested; i++)
            {
                loopWatch.Restart();
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
                double elapsed = offset + times[i];

                // ── 패킷 빌드 + 전송 ──
                byte[] packet = HilPacket.Build(
                    elapsed,
                    px[i], py[i], pz[i],
                    vx, vy, vz,
                    ax, ay, az,
                    jx, jy, jz);
                udp.Send(packet, packet.Length, endpoint);

                // ── UI 업데이트 ──
                count++;
                int remaining = n - i - 1;
                long currentCount = count;
                long loopMs = loopWatch.ElapsedMilliseconds;

                BeginInvoke(new Action(() =>
                {
                    lblPacketCount.Text = currentCount.ToString("N0");
                    lblLatency.Text = $"{loopMs} ms";
                    lblHilStatus.Text = $"{remaining} left";
                }));

                // ── 주기 대기 ──
                int elapsedMs = (int)loopWatch.ElapsedMilliseconds;
                int delay = intervalMs - elapsedMs;
                if (delay > 0)
                    Thread.Sleep(delay);
            }

            _times = null;
            return count;
        }

        // ════════════════════════════════════════════
        // HIL Stop
        // ════════════════════════════════════════════
        private void btnHilStop_Click(object sender, EventArgs e)
        {
            StopHil();
        }

        private void StopHil()
        {
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
                ? Color.FromArgb(46, 125, 50)
                : Color.FromArgb(198, 40, 40);
            lblRfState.Text = rf;
            lblRfState.ForeColor = rf == "ON"
                ? Color.FromArgb(46, 125, 50)
                : Color.FromArgb(198, 40, 40);
            lblTestMode.Text = mode;
            lblSimInfo.Text = info;
        }

        private void ClearStatus()
        {
            lblGnssState.Text = "-";
            lblGnssState.ForeColor = Color.Gray;
            lblRfState.Text = "-";
            lblRfState.ForeColor = Color.Gray;
            lblTestMode.Text = "-";
            lblSimInfo.Text = "-";
            lblPdop.Text = "-";
            lblPacketCount.Text = "0";
            lblUpdateRate.Text = "-";
            lblLatency.Text = "-";
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopHil();
            _tcp?.Disconnect();
            SaveIni();
            base.OnFormClosing(e);
        }
    }
}
