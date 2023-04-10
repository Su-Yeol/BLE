
namespace Yeol
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.bConnect = new System.Windows.Forms.Button();
            this.cComport = new System.Windows.Forms.ComboBox();
            this.cBaudRate = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.uiEMGDataChart = new Ui_EMG.LogWriter.UiEMGDataChartControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lEMG1_value = new System.Windows.Forms.Label();
            this.lEMG2_value = new System.Windows.Forms.Label();
            this.lEMG2 = new System.Windows.Forms.Label();
            this.lEMG1 = new System.Windows.Forms.Label();
            this.bReset = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bConnect
            // 
            this.bConnect.Location = new System.Drawing.Point(927, 429);
            this.bConnect.Name = "bConnect";
            this.bConnect.Size = new System.Drawing.Size(82, 35);
            this.bConnect.TabIndex = 1;
            this.bConnect.Text = "Connect";
            this.bConnect.UseVisualStyleBackColor = true;
            this.bConnect.Click += new System.EventHandler(this.bConnect_Click);
            // 
            // cComport
            // 
            this.cComport.FormattingEnabled = true;
            this.cComport.Location = new System.Drawing.Point(834, 303);
            this.cComport.Name = "cComport";
            this.cComport.Size = new System.Drawing.Size(83, 20);
            this.cComport.TabIndex = 2;
            this.cComport.SelectedIndexChanged += new System.EventHandler(this.cComport_SelectedIndexChanged);
            // 
            // cBaudRate
            // 
            this.cBaudRate.FormattingEnabled = true;
            this.cBaudRate.Location = new System.Drawing.Point(998, 303);
            this.cBaudRate.Name = "cBaudRate";
            this.cBaudRate.Size = new System.Drawing.Size(83, 20);
            this.cBaudRate.TabIndex = 3;
            this.cBaudRate.SelectedIndexChanged += new System.EventHandler(this.cBaudRate_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(786, 306);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "COM :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(925, 306);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "BaudRate :";
            // 
            // uiEMGDataChart
            // 
            this.uiEMGDataChart.Location = new System.Drawing.Point(22, 12);
            this.uiEMGDataChart.Name = "uiEMGDataChart";
            this.uiEMGDataChart.Size = new System.Drawing.Size(625, 372);
            this.uiEMGDataChart.TabIndex = 10;
            this.uiEMGDataChart.XAxisSectionWidth = 1D;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.lEMG1_value, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lEMG2_value, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lEMG2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lEMG1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(223, 390);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(278, 32);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // lEMG1_value
            // 
            this.lEMG1_value.AutoSize = true;
            this.lEMG1_value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lEMG1_value.Location = new System.Drawing.Point(73, 1);
            this.lEMG1_value.Name = "lEMG1_value";
            this.lEMG1_value.Size = new System.Drawing.Size(62, 30);
            this.lEMG1_value.TabIndex = 12;
            this.lEMG1_value.Text = "0";
            this.lEMG1_value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lEMG2_value
            // 
            this.lEMG2_value.AutoSize = true;
            this.lEMG2_value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lEMG2_value.Location = new System.Drawing.Point(211, 1);
            this.lEMG2_value.Name = "lEMG2_value";
            this.lEMG2_value.Size = new System.Drawing.Size(63, 30);
            this.lEMG2_value.TabIndex = 13;
            this.lEMG2_value.Text = "0";
            this.lEMG2_value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lEMG2
            // 
            this.lEMG2.AutoSize = true;
            this.lEMG2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lEMG2.Location = new System.Drawing.Point(142, 1);
            this.lEMG2.Name = "lEMG2";
            this.lEMG2.Size = new System.Drawing.Size(62, 30);
            this.lEMG2.TabIndex = 12;
            this.lEMG2.Text = "EMG2";
            this.lEMG2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lEMG1
            // 
            this.lEMG1.AutoSize = true;
            this.lEMG1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lEMG1.Location = new System.Drawing.Point(4, 1);
            this.lEMG1.Name = "lEMG1";
            this.lEMG1.Size = new System.Drawing.Size(62, 30);
            this.lEMG1.TabIndex = 0;
            this.lEMG1.Text = "EMG1";
            this.lEMG1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bReset
            // 
            this.bReset.Location = new System.Drawing.Point(1026, 429);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(73, 35);
            this.bReset.TabIndex = 12;
            this.bReset.Text = "Reset";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 618);
            this.Controls.Add(this.bReset);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.uiEMGDataChart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cBaudRate);
            this.Controls.Add(this.cComport);
            this.Controls.Add(this.bConnect);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bConnect;
        private System.Windows.Forms.ComboBox cComport;
        private System.Windows.Forms.ComboBox cBaudRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private UiEMG.Response.SerialEndPoint _serialEndPoint;
        private Ui_EMG.LogWriter.UiEMGDataChartControl uiEMGDataChart;
        private Yeol.SerialFrame _serialFrame;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lEMG1_value;
        private System.Windows.Forms.Label lEMG2_value;
        private System.Windows.Forms.Label lEMG2;
        private System.Windows.Forms.Label lEMG1;
        private System.Windows.Forms.Button bReset;
    }
}


 