using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System.Windows.Media;
using UiEMG.Response;

namespace Ui_EMG.LogWriter
{
    public partial class UiEMGDataChartControl : UiEMGChartControl
    {
        static UiEMGDataChartControl()
        {
            ///<summary>
            ///     1. Foramt
            ///     AxisY의 LabelFormatter에 사용.
            ///     LabelFormatter는 AxisY의 각 라벨의 표시 형식을 지정하는데 사용.
            ///     0 소수점 자리까지 표시.
            /// </summary>
            Format = x => $"{x:0}";
            ///<summary>
            ///     1. Quality
            ///     CearedValues의 품질에 사용. CearedValues: ChartControl에 사용되는 데이터 구조이고,
            ///     많은 양의 데이터를 처리할 때 성능을 최적화하기 위해 사용.
            ///     처리하는 데이터의 품질을 지정하는데 사용.
            /// </summary>
            Quality = Quality.Low; // 처리속도를 높임.
        }
        /// <summary>
        ///     차트의 동작 관련 생성자
        ///     ㄴ 차트의 동작을 비활성화하여, 빠르고 경량화된 차트를 제공하자하는 의도
        /// </summary>
        public UiEMGDataChartControl()
        {
            InitializeComponent();
            // 성능 향상을 위해 차트의 호버와 툴팁 및 확대기능을 비활성화
            CartesianChart.Pan = PanningOptions.None; // 차트의 패닝(이동) 기능을 비활성화
            CartesianChart.Zoom = ZoomingOptions.None; // 차트의 줌 기능을 비활성화
            CartesianChart.DisableAnimations = true; // 차트의 애니메이션 효과를 비활성화
            CartesianChart.Hoverable = false; // 차트에 마우스 호버 이벤트를 비활성화
            CartesianChart.DataTooltip = null; // 차트에 데이터 툴팁을 비활성화
        }
        public void AddChartValue(ArrayData processValueArray) 
        {
            /// <summary>
            ///     Emg1Line, Emg2Line Values: 그래프의 데이터를 설정
            ///     AddRange 매서드를 사용하여 EMG1, EMG2 배열을 IEnumerable 형식으로 변환 후 object 배열로 캐스팅
            ///     ㄴ Emg1Line, Emg2Line 객체에 대한 데이터가 차트에 추가
            /// </summary> 
            Emg1Line.Values.AddRange(processValueArray.EMG1.Cast<object>());
            Emg2Line.Values.AddRange(processValueArray.EMG2.Cast<object>());
        }
        public override void ClearChartValues()
        {
            Emg1Line.Values.Clear();
            Emg2Line.Values.Clear();
            base.ClearChartValues();
        } 
        public void MovingChart(int[] array1, int[] array2)
        {
            Emg1Line.Values.Clear(); // Values 속성에 저장된 데이터를 모두 제거
            Emg2Line.Values.Clear();
            Emg1Line.Values.AddRange(array1.Cast<object>()); // 추가되는 emg 데이터로 chat 업데이트 
            Emg2Line.Values.AddRange(array2.Cast<object>());
        }

        //public void MovingChartTra(int[] array)
        //{
        //    Emg2Line.Values.Clear();
        //    Emg2Line.Values.AddRange(array.Cast<object>());
        //}
    }
}