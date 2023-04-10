using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using UiEMG;
using UiEMG.Response;

namespace Yeol
{
    public partial class MainForm : Form
    {
        private int _xAxisValue;
        /// <summary>
        ///     const: 컴파일 상수(초기화를 1번밖에 못하는 data, 메모리 변경 불가능)
        ///            변수를 상수화하여, 한 번 할당된 상수로 메모리의 모든 비트를 변경X
        ///     장점: const 키워드가 붙은 객체는 외부 변경이 불가능
        ///           class 밖: 전역, namespace 유효 범위의 상수를 정의하는데 사용
        ///           정적/비정적 data 멤버 모두를 상소로 선언 가능
        /// </summary>
        public const int _horizontalScale = 100;
        public int[] _EMG1data = new int[_horizontalScale];
        public int[] _EMG2data = new int[_horizontalScale];
        public static readonly string[] BaudRates =
        {
            "9600", "19200", "38400", "57600", "115200"
        };

        public MainForm()
        {
            InitializeComponent();

            _serialEndPoint = new SerialEndPoint(Program.Name);

            //_serialEndPoint.ProcessValueReceived += SerialEndPoint_ProcessValueReceived; // Recive Data count
            _serialEndPoint.ProcessValueArrayReceived += SerialEndPoint_ProcessValueArrayReceived; // Chart, Label data update
            /// <summary>
            ///     Comport Initialization
            /// </summary>
            cComport.Items.Clear();
            string[] portNames = SerialPort.GetPortNames(); // 포트 검색
            if (portNames.Length <= 0) return;
            cComport.Items.AddRange(portNames.Cast<object>().ToArray()); // PortName[]에  list name 추가
            cBaudRate.Items.AddRange(BaudRates.Cast<object>().ToArray());
            cComport.SelectedIndex = 0; // ComboBox에 index 0 값 보이기
            cBaudRate.SelectedIndex = 0;

            // 버튼 활성화
            bReset.Enabled = false;
            cBaudRate.Enabled = false;
        }

        #region Serial Port
        /// <summary>
        ///     Serial Port ComboBox 선택 이벤트
        /// </summary>
        private void cComport_SelectedIndexChanged(object sender, EventArgs e)
        {
            var serialPortName = cComport.SelectedItem as string;
            bConnect.Enabled = !string.IsNullOrEmpty(serialPortName); // Null or Empty -> btn enable
        }

        private void cBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var serialBuadRate = cBaudRate.SelectedItem as string;
            cBaudRate.Enabled = !string.IsNullOrEmpty(serialBuadRate); // Null or Empty -> btn enable
        }

        /// <summary>
        ///     Serial Port 연결
        /// </summary>
        private void bConnect_Click(object sender, EventArgs e)
        {
            // 포트 연결상태 -> 접속종료
            if (_serialEndPoint.IsOpen)
            {
                Thread.Sleep(200);
                _serialEndPoint.Close();

                bConnect.Text = @"Connect";
                bConnect.BackColor = System.Drawing.Color.Yellow;

                cComport.Enabled = true;
                cBaudRate.Enabled = false;
                bReset.Enabled = true;
            }
            // 포트 미연결상태 -> 접속
            else
            {
                var portName = cComport.SelectedItem as string;
                var baudRate = Convert.ToInt32(cBaudRate.SelectedItem as string);
                _serialEndPoint.Open(portName, baudRate); // _serialPortStream Read + ByteBuffer Write

                /// <summary>  
                ///     1. Receive: ThreadPool.QueueUserWorkItem(Receiver, state);
                ///     2. Receiver(SerialPortBase): ProcessBuffer
                ///     3. ProcessBuffer(SerialEndPortBase):
                ///     4. ProcessFrame(int flag, byte[] payload, int checksum)
                ///     5. ReceiveData(byte[] stream)
                ///     6. ArrayData(IEnumerable<ReceiveData> values)
                /// </summary>
                _serialEndPoint.Receive(SerialFrame.MinSize); // Serial Frame 형태 데이터 받아서 payload 부분 구분

                bConnect.Text = @"DisConnect";
                bConnect.BackColor = System.Drawing.Color.GreenYellow;

                cComport.Enabled = false;
                cBaudRate.Enabled = false;
                bReset.Enabled = false;
            }
        }
        #endregion

        #region Chart 
        private void UpdateLabels(ArrayData processValueArray)
        {
            lEMG1_value.Text = processValueArray.EMG1.Last().ToString();
            lEMG2_value.Text = processValueArray.EMG2.Last().ToString();
        }

        private void UpdateChart(ArrayData processValueArray)
        {
            if (_xAxisValue < _horizontalScale) // 100 보다 작은경우 chat 데이터 추가
            {
                _EMG1data[_xAxisValue] = processValueArray.EMG1[0];
                _EMG2data[_xAxisValue] = processValueArray.EMG2[0];

                uiEMGDataChart.AddChartValue(processValueArray);
            }
            else // 100 보다 클 경우, chat를 왼쪽으로 옮기면서 데이터 추가
            {
                _EMG1data = LeftShift(_EMG1data, 1);
                _EMG1data[_horizontalScale - 1] = processValueArray.EMG1[0];
                _EMG2data = LeftShift(_EMG2data, 1);
                _EMG2data[_horizontalScale - 1] = processValueArray.EMG2[0];
                uiEMGDataChart.MovingChart(_EMG1data, _EMG2data);
            }
            _xAxisValue++;
        }

        public static int[] LeftShift(int[] @this, int shiftIndex)
        { 
            var list = @this.ToList(); // @ ??
            try
            {
                // list.Reverse(start index, reverse count)
                list.Reverse(0, shiftIndex); // 0 1 2 3 4 
                list.Reverse(shiftIndex, list.Count - shiftIndex); // 0 4 3 2 1
                list.Reverse(0, list.Count); // 1 2 3 4 0
            }
            catch
            {
                if (shiftIndex < 0)
                    throw new System.ArgumentException("ShiftIndex is negative.", "shiftIndex");
            }

            return list.ToArray();
        }

        //private void SerialEndPoint_ProcessValueReceived(object sender, EventArgs<ReceiveData> e)
        //{
        //    _dataCount++;
        //    if (_dataCount == int.MaxValue) _dataCount = 0;
        //}

        private void SerialEndPoint_ProcessValueArrayReceived(object sender, EventArgs<ArrayData> e)
        {
            BeginInvoke(new Action(delegate
            {
                UpdateChart(e.Value);
                UpdateLabels(e.Value);
            }));
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            uiEMGDataChart.ClearChartValues();
        }

        #endregion

       
    }
}