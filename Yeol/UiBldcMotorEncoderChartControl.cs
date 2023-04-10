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
using UiBldcMotor;
using System.Windows.Media;
using UiBldcMotor.Response;

namespace UI_bldc_motor.LogWriter
{
    public partial class UiBldcMotorEncoderChartControl : UiBldcMotorPosChartControl
    {
        static UiBldcMotorEncoderChartControl()
        {
            Format = x => $"{x:0}";
            Quality = Quality.Low;
        }
        public UiBldcMotorEncoderChartControl()
        {
            InitializeComponent();
            // 성능 향상을 위해 차트의 호버와 툴팁 및 확대기능을 비활성화
            CartesianChart.Pan = PanningOptions.None;
            CartesianChart.Zoom = ZoomingOptions.None;
            CartesianChart.DisableAnimations = true;
            CartesianChart.Hoverable = false;
            CartesianChart.DataTooltip = null;
        }
        public void AddChartValue(ArrayData processValueArray)
        {
            EncoderLine.Values.AddRange(processValueArray.Encoder.Cast<object>());
        }
        public void AddChartValueRef(int[] array)
        {
            TrajecLine.Values.AddRange(array.Cast<object>());
        }

        public override void ClearChartValues()
        {
            EncoderLine.Values.Clear();
            TrajecLine.Values.Clear();
            base.ClearChartValues();
        }

        public void MovingChart(int[] array)
        {
            EncoderLine.Values.Clear();
            EncoderLine.Values.AddRange(array.Cast<object>());
        }

        public void MovingChartTra(int[] array)
        {
            TrajecLine.Values.Clear();
            TrajecLine.Values.AddRange(array.Cast<object>());
        }


    }
}
