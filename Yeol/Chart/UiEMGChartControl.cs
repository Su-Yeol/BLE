using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using UiEMG;
using System.Windows.Media;

namespace Ui_EMG
{
    /// <summary>
    ///     Chart Axis, Line 잡는 Class 
    /// </summary>
    public partial class UiEMGChartControl : Chartcontrol
    {
        public UiEMGChartControl()
        {
            InitializeComponent(); 
            CartesianChart.Series.Add(Emg1Line);
            CartesianChart.Series.Add(Emg2Line);

            // X Axis
            CartesianChart.AxisX.Add(new Axis
            {
                IsEnabled = true, // x축을 사용할지 여부
                IsMerged = false, // 다른 차트와 x축을 합칠지 여부
                MinValue = 0, //double.NaN 축의 최솟값
                MaxValue = 100, //double.NaN 축의 최댓값
                Separator = new Separator // x축의 분리기 설정. 분리기는 x축을 나눌 때 사용되며 Step 값에 따라 x label 값을 표기
                {
                    IsEnabled = true,
                    Step = XAxisChartStep, // X Label 간격
                    StrokeThickness = StrokeThickness,
                    //StrokeDashArray = new DoubleCollection(new[] { StrokeDashArray }), // X축 눈금 점선
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                },
                Sections = new SectionsCollection // 축 상에 특정영역을 강조할때 사용
                {
                    new AxisSection
                    {
                        Visibility = Visibility.Hidden, Value = 0.0, SectionWidth = 1.0, // 섹션 설정
                        Fill = new SolidColorBrush { Color = Colors.Red, Opacity = SectionOpacity } // 섹션의 배경 색상
                    }
                }
            });
            // Y Axis
            CartesianChart.AxisY.Add(new Axis
            {
                IsEnabled = true, // 축의 활성화 여부
                LabelFormatter = Format, // Y Label 값 표시
                IsMerged = false, // 다른 차트와 축이 공유되는 경우, 축을 병합할지 여부 설정
                Foreground = System.Windows.Media.Brushes.Green, // 축 레이블의 색상
                Title = "EMG1", // 축의 제목
                MinValue = 0, //double.NaN 축의 최솟값
                MaxValue = 250, //double.NaN 축의 최댓값
                Position = AxisPosition.LeftBottom, // 축의 위치 설정
                Separator = new Separator // 축의 눈금 설정
                {
                    IsEnabled = true, // 축의 눈금이 표시되지 않음
                    Step = 50, // 눈금의 간격
                    StrokeThickness = StrokeThickness, // Y축 눈금 굵기
                    //StrokeDashArray = new DoubleCollection(new[] { StrokeDashArray }), // Y축 눈금 점선
                    Stroke = System.Windows.Media.Brushes.Black // Y축 눈금 색상
                }/*,
                Sections = new SectionsCollection // 축 상에 특정영역을 강조할때 사용
                {
                    new AxisSection
                    {
                        Visibility = Visibility.Visible, Value = 50.0, SectionWidth = 250.0, // 섹션 설정
                        Fill = new SolidColorBrush { Color = Colors.Red, Opacity = SectionOpacity } // 섹션의 배경 색상
                    }
                }*/
            });
            CartesianChart.AxisY.Add(new Axis
            {
                IsEnabled = true,
                LabelFormatter = Format,
                IsMerged = false,
                Foreground = System.Windows.Media.Brushes.Red,
                Title = "EMG2",
                MinValue = 0, //double.NaN,
                MaxValue = 250, //double.NaN,
                Position = AxisPosition.LeftBottom,
                Separator = new Separator
                {
                    IsEnabled = false,
                    Step = 50, 
                    //StrokeThickness = StrokeThickness,
                    //StrokeDashArray = new DoubleCollection(new[] { StrokeDashArray }),
                    //Stroke = System.Windows.Media.Brushes.Black
                }
            });
        }
        // Chart Graph 설정
        protected readonly GLineSeries Emg1Line = new GLineSeries
        {
            Title = "EMG1", // 그래프의 제목
            Values = new GearedValues<int>().WithQuality(Quality.Low), // 그래프의 데이터를 설정함
            ScalesYAt = 0, // Y축 인덱스를 설정, 여러 개의 Y축이 있는 경우, 해당 그래프가 사용할 Y축의 인덱스를 설정
            Fill = System.Windows.Media.Brushes.Transparent, // 그래프의 채우기 색상을 설정 - 투명(Transparent)
            Stroke = System.Windows.Media.Brushes.LimeGreen, // 그래프의 선 색상
            LineSmoothness = LineSmoothness, // 그래프의 부드러운 정도(0)
            PointGeometrySize = PointGeometrySize, // 그래프의 데이터 포인터에 사용할 도형의 크기를 설정(2)
            PointGeometry = DefaultGeometries.None, // 그래프의 데이터 포인터에 사용할 도형 설정(None)
            StrokeThickness = 3
        };
        public bool VisibleEmg1lLine
        {
            set => Emg1Line.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

        protected readonly GLineSeries Emg2Line = new GLineSeries
        {
            Title = "EMG2",
            Values = new GearedValues<int>().WithQuality(Quality.Low),
            ScalesYAt = 1,
            Fill = System.Windows.Media.Brushes.Transparent,
            Stroke = System.Windows.Media.Brushes.Crimson,
            LineSmoothness = LineSmoothness,
            PointGeometrySize = PointGeometrySize,
            PointGeometry = DefaultGeometries.None,
            StrokeThickness = 3
        };
        public bool VisibleEmg2lLine
        {
            set => Emg2Line.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

    }
}
