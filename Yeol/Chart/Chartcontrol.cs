using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System.Windows.Media;
using CartesianChart = LiveCharts.WinForms.CartesianChart;
using Clipboard = System.Windows.Clipboard;

namespace UiEMG
{
    /// <summary>
    ///     Chart Line Check class
    /// </summary>
    public partial class Chartcontrol : UserControl
    {
        public static int QualityLevel = -1;
        private static Quality _quality = Quality.Medium;

        public Chartcontrol()
        {
            InitializeComponent();
        }
        protected static Quality Quality // Chat 그래프의 부드러움 정도
        {
            get
            {
                if (QualityLevel < 0) return _quality;
                switch (QualityLevel)
                {
                    case 0:
                        return Quality.Low;
                    case 1:
                        return Quality.Medium;
                    case 2:
                        return Quality.High;
                    default:
                        return Quality.Highest;
                }
            }
            set => _quality = value;
        }
        protected CartesianChart CartesianChart => cartesianChart;
        public bool VisibleXAxisSection
        {
            set => CartesianChart.AxisX[0].Sections[0].Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }
        public double XAxisSectionWidth
        {
            get => CartesianChart.AxisX[0].Sections[0].SectionWidth;
            set => CartesianChart.AxisX[0].Sections[0].SectionWidth = value;
        }

        public double XAxisSectionValue
        {
            set => CartesianChart.AxisX[0].Sections[0].Value = value;
        }

        public static double StrokeThickness { get; set; } = 0.3;
        public static double StrokeDashArray { get; set; } = 10.0;
        public static double SectionOpacity { get; set; } = 0.05;
        public static double LineSmoothness { get; set; } = 0.0; // 그래프의 부드러운 정도, 0~1 작을수록 부드러워 짐.
        public static double PointGeometrySize { get; set; } = 2.0; // 그래프의 데이터 포인터에 사용할 도형의 크기를 설정
        public static double XAxisChartStep { get; set; } = 10.0;
        protected static Func<double, string> Format { get; set; } = x => $"{x:0}";

        protected void InitializeYAxisSection()
        {
            foreach (var axis in CartesianChart.AxisY)
            {
                if (!(axis.Foreground is SolidColorBrush brush)) continue;

                axis.Sections = new SectionsCollection
                {
                    new AxisSection
                    {
                        Visibility = Visibility.Hidden, Value = 0, SectionWidth = 1,
                        Fill = new SolidColorBrush { Color = brush.Color, Opacity = SectionOpacity }
                    },
                    new AxisSection
                    {
                        Visibility = Visibility.Hidden, Value = 0, SectionWidth = 1,
                        Fill = new SolidColorBrush { Color = brush.Color, Opacity = SectionOpacity }
                    },
                    new AxisSection
                    {
                        Visibility = Visibility.Hidden, Value = 0, SectionWidth = 1,
                        Fill = new SolidColorBrush { Color = brush.Color, Opacity = SectionOpacity }
                    }
                };
            }
        }

        #region Chart Functions

        public virtual void ClearChartValues()
        {
            VisibleXAxisSection = false;
        }

        // Axis Min/Max 차트 값이 표시될 범위, Range(Min/Max) 차트에 값이 있을 때 표시 범위
        public void SetYAxisRange(int axisNumber, double minValue, double maxValue)
        {
            CartesianChart.AxisY[axisNumber].SetRange(minValue, maxValue);
        }

        public void SetYAxisValue(int axisNumber, double minValue, double maxValue)
        {
            CartesianChart.AxisY[axisNumber].MinValue = minValue;
            CartesianChart.AxisY[axisNumber].MaxValue = maxValue;
            // ToDo: Step 조정하기
            //var step = (maxValue - minValue) / 50;
            //CartesianChart.AxisY[axisNumber].Separator.Step = 
        }

        public void SetXAxisRange(double minValue, double maxValue)
        {
            CartesianChart.AxisX[0].SetRange(minValue, maxValue);
        }

        public void SetXAxisValue(double minValue, double maxValue)
        {
            CartesianChart.AxisX[0].MinValue = minValue;
            CartesianChart.AxisX[0].MaxValue = maxValue;
        }

        public void ToClipboard()
        {
            Clipboard.SetText(GetStringFromChart());
        }

        protected virtual string GetStringFromChart()
        {
            return string.Empty;
        }

        #endregion
    }
}