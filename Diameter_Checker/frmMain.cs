﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Management;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Diameter_Checker
{
    public class frmMain : Form
    {
        private string InputData = string.Empty;

        public static string strgetProcessorID;

        private string fistSubString;

        private int charNumberOfFirstString;

        private string lastSubString;

        private int charNumberOfLastString;

        private static int i;

        private static int j;

        private static int rowIndex;

        private IContainer components = null;

        private Panel panel3;

        private Panel panel6;

        private MenuStrip menuStrip1;

        private ToolStripMenuItem MenuConfig;

        private ToolStripMenuItem communicatiomToolStripMenuItem;

        private ToolStripMenuItem instructionToolStripMenuItem;

        private Label lblStatus;

        private Panel panel2;

        private Label lblConnectStatus;

        private Panel panel1;

        private Timer tmrConnectionStatus;

        private Panel panelResult;

        private Button btnSelect;

        private Button btnClear;

        private Button btnClearCurrentTest;

        private Button btnDeleteTestData;

        private Button btnEdit;

        private Button btnStart;

        private Panel panel5;

        private Label label2;

        private ComboBox cmbModel;

        private GroupBox groupBox1;

        private Label label4;

        private Label label7;

        private GroupBox groupBox2;

        private Label label10;

        private Label label11;

        private TextBox txtTime;

        private TextBox lblSystemMessage;

        private GroupBox groupBox4;

        private TextBox txtTotalProcessed;

        private Label label16;

        private TextBox txtTotalFAIL;

        private TextBox txtTotalPass;

        private Label label17;

        private Label label18;

        private GroupBox groupBox3;

        private Button btnJudge;

        private TextBox txtA2MinimumValue;

        private TextBox txtA2MaximumValue;

        private Label label13;

        private Panel panel4;

        private Label label1;

        private GroupBox groupBox5;

        private TextBox txtA1PPK;

        private Label label15;

        private TextBox txtA1PP;

        private TextBox txtA1DetectionLevel;

        private TextBox txtA2DetectionLevel;

        private TextBox txtDate;

        private GroupBox groupBox6;

        private TextBox txtA1MinimumValue;

        private TextBox txtA1MaximumValue;

        private TextBox txtA1Result;

        private Label label3;

        private Label label5;

        private TextBox txtA2MinimumOffset;

        private TextBox txtA1MinimumOffset;

        private Label label6;

        private Label label8;

        private TextBox txtA2MaximumOffset;

        private TextBox txtA1MaximumOffset;

        private Label label9;

        private Label label12;

        private TextBox txtA2PPK;

        private Label label14;

        private TextBox txtA2PP;

        private Label label19;

        private TextBox txtA2Result;

        private Timer tmrDisplayData;

        private Button button2;

        private TextBox txtSystemMessage;

        private Timer tmrDateTime;

        private DataGridView dataGridView1;

        private DataGridViewTextBoxColumn ID;

        private DataGridViewTextBoxColumn model;

        private DataGridViewTextBoxColumn A1MaxValue;

        private DataGridViewTextBoxColumn A1MinValue;

        private DataGridViewTextBoxColumn A1Result;

        private DataGridViewTextBoxColumn A2MaxValue;

        private DataGridViewTextBoxColumn A2MinValue;

        private DataGridViewTextBoxColumn A2Result;

        private DataGridViewTextBoxColumn Date;

        private DataGridViewTextBoxColumn Time;

        private DataGridViewTextBoxColumn Judge;

        private DataGridViewTextBoxColumn TotalProcessed;

        private DataGridViewTextBoxColumn TotalPASS;

        private DataGridViewTextBoxColumn TotalFAIL;

        private Chart chartA2;

        private Chart chartA1;

        private Timer tmrRefreshChart;

        private GroupBox groupBox7;

        private CheckBox chkStopScan;

        private Timer tmrA1DetectRemoveObject;

        private Timer tmrA2DetectRemoveObject;

        private Timer tmrEnableReadA1Data;

        private Timer tmrEnableReadA2Data;

        private ComboBox cmbTimeToEnableRead;

        private Label label20;

        private Timer tmrRefreshDataGridView;

        private SerialPort serialPort1;

        private Timer tmrDisplayJudge;

        static frmMain()
        {
            frmMain.i = 0;
            frmMain.j = 0;
        }

        public frmMain()
        {
            this.InitializeComponent();
            Communication.serialport.DataReceived += new SerialDataReceivedEventHandler(this.DataReceive);
        }

        private void AdjustLayout()
        {
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            int widthScreen = workingArea.Width;
            workingArea = Screen.PrimaryScreen.WorkingArea;
            int heightScreen = workingArea.Height;
            base.Location = new Point(widthScreen - base.Width, heightScreen - base.Height);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This action is only accepted with the engineer!", "WARNING!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                string delete = string.Concat(new string[] { "delete from ProductSetting WHERE model='", this.cmbModel.Text, "' and A1DetectionValue='", this.txtA1DetectionLevel.Text, "' and A2DetectionValue='", this.txtA2DetectionLevel.Text, "' and A1MaximumOffset='", this.txtA1MaximumOffset.Text, "' and A1MinimumOffset='", this.txtA1MinimumOffset.Text, "' and A1MaximumOffset='", this.txtA1MaximumOffset.Text, "' and A2MinimumOffset='", this.txtA2MinimumOffset.Text, "'" });
                (new SqlCommand(delete, con)).ExecuteNonQuery();
                MessageBox.Show("Data has been deleted!", "Warning!");
                con.Dispose();
                this.loadProductSetting();
            }
        }

        private void btnClearCurrentData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to clear current Counter data?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Communication.A1EnableSave = false;
                Communication.A2EnableSave = false;
                Communication.A1MaximumValue = null;
                Communication.A1MinimumValue = null;
                Communication.A2MaximumValue = null;
                Communication.A2MinimumValue = null;
                this.controlAlarm_A1ResetAlarm();
                this.controlAlarm_A2ResetAlarm();
                this.txtA1MaximumValue.Text = null;
                this.txtA1MinimumValue.Text = null;
                this.txtA1Result.Text = null;
                this.txtA2MaximumValue.Text = null;
                this.txtA2MinimumValue.Text = null;
                this.txtA2Result.Text = null;
                this.chartA1.Series.Clear();
                this.chartA1Setting();
                this.chartA2.Series.Clear();
                this.chartA2Setting();
            }
        }

        private void btnDeleteTestData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete data?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    SqlConnection con = new SqlConnection(Communication.con_string);
                    con.Open();
                    string delete = string.Concat(new string[] { "delete from Data WHERE ID='", frmMain.deleteData.ID, "' and model='", frmMain.deleteData.model, "'" });
                    (new SqlCommand(delete, con)).ExecuteNonQuery();
                    con.Dispose();
                }
                catch (SystemException systemException)
                {
                    SystemException ex = systemException;
                    MessageBox.Show(string.Format("An error occurred: {0}", ex.Message));
                }
                this.loadData();
                this.btnDeleteTestData.Enabled = false;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.tmrRefreshDataGridView.Enabled = true;
            Communication.connect.Close();
            (new ExportData()).ShowDialog();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This action is only accepted with the engineer!", "WARNING!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if ((this.cmbModel.Text == null || this.txtA1DetectionLevel.Text.Length != 6 || this.txtA2DetectionLevel.Text.Length != 6 || this.txtA1MinimumOffset.Text.Length != 6 || this.txtA1MaximumOffset.Text.Length != 6 || this.txtA2MinimumOffset.Text.Length != 6 ? false : this.txtA2MaximumOffset.Text.Length == 6))
                {
                    SqlConnection con = new SqlConnection(Communication.con_string);
                    con.Open();
                    string add = string.Concat(new string[] { "INSERT INTO ProductSetting (model, A1DetectionValue, A2DetectionValue, A1MaximumOffset, A1MinimumOffset, A2MaximumOffset, A2MinimumOffset) VALUES ('", this.cmbModel.Text, "','", this.txtA1DetectionLevel.Text, "','", this.txtA2DetectionLevel.Text, "','", this.txtA1MaximumOffset.Text, "','", this.txtA1MinimumOffset.Text, "','", this.txtA2MaximumOffset.Text, "','", this.txtA2MinimumOffset.Text, "')" });
                    SqlCommand cmd_add = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = add
                    };
                    cmd_add.ExecuteNonQuery();
                    con.Dispose();
                    cmd_add.Dispose();
                    MessageBox.Show("The new model has been added!");
                }
                else if (MessageBox.Show("Please check the format data.", "WARNING!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                }
                this.loadProductSetting();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.btnStart.Text != "Start")
            {
                this.btnStart.Text = "Start";
                this.btnStart.ForeColor = Color.Teal;
                this.txtSystemMessage.Text = "STOPPED!";
                Communication.start = false;
                Communication.stop = true;
                Communication.enableReceiveData = false;
            }
            else
            {
                this.btnStart.Text = "Stop";
                this.btnStart.ForeColor = Color.DarkRed;
                this.txtSystemMessage.Text = "Working mode";
                Communication.start = true;
                Communication.stop = false;
                Communication.enableReceiveData = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This action is only accepted with the engineer!", "WARNING!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.txtA1DetectionLevel.Text = Communication.A1MeasuredValue;
                this.txtA2DetectionLevel.Text = Communication.A2MeasuredValue;
            }
        }

        private void calculatePPandPPKvalue()
        {
            float single;
            double num;
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlDataAdapter adapterGetAverage = new SqlDataAdapter(new SqlCommand("SELECT AVG(CAST(A1MaxValue as float)), AVG(CAST(A1MinValue as float)), AVG(CAST(A2MaxValue as float)), AVG(CAST(A2MinValue as float))  FROM Data", con));
                DataTable dataTableGetAverage = new DataTable();
                adapterGetAverage.Fill(dataTableGetAverage);
                single = (float.Parse(dataTableGetAverage.Rows[0][0].ToString()) + float.Parse(dataTableGetAverage.Rows[0][1].ToString())) / 2f * 1000f;
                Communication.A1Average = single.ToString();
                SqlCommand cmd_LoadAllValue = new SqlCommand(string.Concat("SELECT A1MaxValue, A1MinValue, A2MaxValue, A2MinValue FROM Data Where model='", this.cmbModel.Text, "'"), con);
                SqlDataAdapter adapterLoadAllValue = new SqlDataAdapter(cmd_LoadAllValue);
                DataTable dataTableLoadAllValue = new DataTable();
                adapterLoadAllValue.Fill(dataTableLoadAllValue);
                Communication.A1SD = 0;
                double b = 0;
                frmMain.rowIndex = 0;
                while (frmMain.rowIndex <= dataTableLoadAllValue.Rows.Count - 1)
                {
                    double a = (double.Parse(dataTableLoadAllValue.Rows[frmMain.rowIndex][0].ToString()) + double.Parse(dataTableLoadAllValue.Rows[frmMain.rowIndex][1].ToString())) / 2;
                    b = double.Parse(Communication.A1Average.ToString());
                    Communication.A1SD += Math.Pow(a - b, 2);
                    frmMain.rowIndex++;
                }
                Communication.A1SD = Math.Sqrt(Communication.A1SD / (double)(frmMain.rowIndex - 1));
                Communication.A1SD /= 1000;
                num = (double.Parse(Communication.A1MaximumOffset) - double.Parse(Communication.A1MinimumOffset)) / (6 * Communication.A1SD);
                Communication.A1PP = num.ToString();
                num = double.Parse(Communication.A1PP) / 1000;
                Communication.A1PP = num.ToString();
                this.txtA1PP.Text = Communication.A1PP.Substring(0, 10);
                Communication.A1PPU = (double.Parse(Communication.A1MaximumOffset) - double.Parse(Communication.A1Average)) / (3 * Communication.A1SD);
                Communication.A1PPU /= 1000;
                Communication.A1PPL = (double.Parse(Communication.A1Average) - double.Parse(Communication.A1MinimumOffset)) / (3 * Communication.A1SD);
                Communication.A1PPL /= 1000;
                if (Communication.A1PPU >= Communication.A1PPL)
                {
                    this.txtA1PPK.Text = Communication.A1PPL.ToString().Substring(0, 10);
                }
                else
                {
                    this.txtA1PPK.Text = Communication.A1PPU.ToString().Substring(0, 10);
                }
                con.Close();
            }
            catch
            {
            }
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlDataAdapter adapterGetAverage = new SqlDataAdapter(new SqlCommand("SELECT AVG(CAST(A1MaxValue as float)), AVG(CAST(A1MinValue as float)), AVG(CAST(A2MaxValue as float)), AVG(CAST(A2MinValue as float))  FROM Data", con));
                DataTable dataTableGetAverage = new DataTable();
                adapterGetAverage.Fill(dataTableGetAverage);
                single = (float.Parse(dataTableGetAverage.Rows[0][2].ToString()) + float.Parse(dataTableGetAverage.Rows[0][3].ToString())) / 2f * 1000f;
                Communication.A2Average = single.ToString();
                SqlCommand cmd_LoadAllValue = new SqlCommand(string.Concat("SELECT A1MaxValue, A1MinValue, A2MaxValue, A2MinValue FROM Data Where model='", this.cmbModel.Text, "'"), con);
                SqlDataAdapter adapterLoadAllValue = new SqlDataAdapter(cmd_LoadAllValue);
                DataTable dataTableLoadAllValue = new DataTable();
                adapterLoadAllValue.Fill(dataTableLoadAllValue);
                Communication.A2SD = 0;
                double b = 0;
                frmMain.rowIndex = 0;
                while (frmMain.rowIndex <= dataTableLoadAllValue.Rows.Count - 1)
                {
                    double a = (double.Parse(dataTableLoadAllValue.Rows[frmMain.rowIndex][2].ToString()) + double.Parse(dataTableLoadAllValue.Rows[frmMain.rowIndex][3].ToString())) / 2;
                    b = double.Parse(Communication.A2Average.ToString());
                    Communication.A2SD += Math.Pow(a - b, 2);
                    frmMain.rowIndex++;
                }
                Communication.A2SD = Math.Sqrt(Communication.A2SD / (double)(frmMain.rowIndex - 1));
                Communication.A2SD /= 1000;
                num = (double.Parse(Communication.A2MaximumOffset) - double.Parse(Communication.A2MinimumOffset)) / (6 * Communication.A2SD);
                Communication.A2PP = num.ToString();
                num = double.Parse(Communication.A2PP) / 1000;
                Communication.A2PP = num.ToString();
                this.txtA2PP.Text = Communication.A2PP.Substring(0, 10);
                Communication.A2PPU = (double.Parse(Communication.A2MaximumOffset) - double.Parse(Communication.A2Average)) / (3 * Communication.A2SD);
                Communication.A2PPU /= 1000;
                Communication.A2PPL = (double.Parse(Communication.A2Average) - double.Parse(Communication.A2MinimumOffset)) / (3 * Communication.A2SD);
                Communication.A2PPL /= 1000;
                if (Communication.A2PPU >= Communication.A2PPL)
                {
                    this.txtA2PPK.Text = Communication.A2PPL.ToString().Substring(0, 10);
                }
                else
                {
                    this.txtA2PPK.Text = Communication.A2PPU.ToString().Substring(0, 10);
                }
                con.Close();
            }
            catch
            {
            }
        }

        private void chartA1Display()
        {
            DataPointCollection points = this.chartA1.Series["A1 Max Offset"].Points;
            int num = frmMain.i;
            frmMain.i = num + 1;
            points.AddXY((double)num, (double)(float.Parse(Communication.A1MaximumOffset.Replace(".", "")) / 1000f));
            DataPointCollection dataPointCollection = this.chartA1.Series["A1 Measuring"].Points;
            int num1 = frmMain.i;
            frmMain.i = num1 + 1;
            dataPointCollection.AddXY((double)num1, (double)(float.Parse(Communication.A1MeasuredValue.Replace(".", "")) / 1000f));
            DataPointCollection points1 = this.chartA1.Series["A1 Min Offset"].Points;
            int num2 = frmMain.i;
            frmMain.i = num2 + 1;
            points1.AddXY((double)num2, (double)(float.Parse(Communication.A1MinimumOffset.Replace(".", "")) / 1000f));
        }

        private void chartA1Setting()
        {
            frmMain.i = 0;
            ChartArea chart1 = this.chartA1.ChartAreas[0];
            this.chartA1.Series.Clear();
            chart1.AxisX.Minimum = 0;
            chart1.AxisY.Maximum = (double)(float.Parse(Communication.A1MaximumOffset.Replace(".", "")) / 1000f) + 0.01;
            chart1.AxisY.Minimum = (double)(float.Parse(Communication.A1MinimumOffset.Replace(".", "")) / 1000f) - 0.01;
            chart1.AxisY.IntervalType = DateTimeIntervalType.Number;
            this.chartA1.Series.Add("A1 Max Offset");
            this.chartA1.Series["A1 Max Offset"].ChartType = SeriesChartType.Line;
            this.chartA1.Series["A1 Max Offset"].Color = Color.Red;
            this.chartA1.Series["A1 Max Offset"].BorderWidth = 3;
            this.chartA1.Series.Add("A1 Measuring");
            this.chartA1.Series["A1 Measuring"].ChartType = SeriesChartType.Line;
            this.chartA1.Series["A1 Measuring"].Color = Color.Blue;
            this.chartA1.Series["A1 Measuring"].BorderWidth = 3;
            this.chartA1.Series.Add("A1 Min Offset");
            this.chartA1.Series["A1 Min Offset"].ChartType = SeriesChartType.Line;
            this.chartA1.Series["A1 Min Offset"].Color = Color.Red;
            this.chartA1.Series["A1 Min Offset"].BorderWidth = 3;
            DataPointCollection points = this.chartA1.Series["A1 Max Offset"].Points;
            int num = frmMain.i;
            frmMain.i = num + 1;
            points.AddXY((double)num, (double)(float.Parse(Communication.A1MaximumOffset.Replace(".", "")) / 1000f));
            DataPointCollection dataPointCollection = this.chartA1.Series["A1 Measuring"].Points;
            int num1 = frmMain.i;
            frmMain.i = num1 + 1;
            dataPointCollection.AddXY((double)num1, (double)(float.Parse(Communication.A1MinimumOffset.Replace(".", "")) / 1000f));
            DataPointCollection points1 = this.chartA1.Series["A1 Min Offset"].Points;
            int num2 = frmMain.i;
            frmMain.i = num2 + 1;
            points1.AddXY((double)num2, (double)(float.Parse(Communication.A1MinimumOffset.Replace(".", "")) / 1000f));
        }

        private void chartA2Display()
        {
            DataPointCollection points = this.chartA2.Series["A2 Max Offset"].Points;
            int num = frmMain.j;
            frmMain.j = num + 1;
            points.AddXY((double)num, (double)(float.Parse(Communication.A2MaximumOffset.Replace(".", "")) / 1000f));
            DataPointCollection dataPointCollection = this.chartA2.Series["A2 Measuring"].Points;
            int num1 = frmMain.j;
            frmMain.j = num1 + 1;
            dataPointCollection.AddXY((double)num1, (double)(float.Parse(Communication.A2MeasuredValue.Replace(".", "")) / 1000f));
            DataPointCollection points1 = this.chartA2.Series["A2 Min Offset"].Points;
            int num2 = frmMain.j;
            frmMain.j = num2 + 1;
            points1.AddXY((double)num2, (double)(float.Parse(Communication.A2MinimumOffset.Replace(".", "")) / 1000f));
        }

        private void chartA2Setting()
        {
            frmMain.j = 0;
            ChartArea chart2 = this.chartA2.ChartAreas[0];
            this.chartA2.Series.Clear();
            chart2.AxisX.Minimum = 0;
            chart2.AxisY.Maximum = (double)(float.Parse(Communication.A2MaximumOffset.Replace(".", "")) / 1000f) + 0.01;
            chart2.AxisY.Minimum = (double)(float.Parse(Communication.A2MinimumOffset.Replace(".", "")) / 1000f) - 0.01;
            chart2.AxisY.IntervalType = DateTimeIntervalType.Number;
            this.chartA2.Series.Add("A2 Max Offset");
            this.chartA2.Series["A2 Max Offset"].ChartType = SeriesChartType.Line;
            this.chartA2.Series["A2 Max Offset"].Color = Color.Red;
            this.chartA2.Series["A2 Max Offset"].BorderWidth = 3;
            this.chartA2.Series.Add("A2 Measuring");
            this.chartA2.Series["A2 Measuring"].ChartType = SeriesChartType.Line;
            this.chartA2.Series["A2 Measuring"].Color = Color.Blue;
            this.chartA2.Series["A2 Measuring"].BorderWidth = 3;
            this.chartA2.Series.Add("A2 Min Offset");
            this.chartA2.Series["A2 Min Offset"].ChartType = SeriesChartType.Line;
            this.chartA2.Series["A2 Min Offset"].Color = Color.Red;
            this.chartA2.Series["A2 Min Offset"].BorderWidth = 3;
            DataPointCollection points = this.chartA2.Series["A2 Max Offset"].Points;
            int num = frmMain.j;
            frmMain.j = num + 1;
            points.AddXY((double)num, (double)(float.Parse(Communication.A2MaximumOffset.Replace(".", "")) / 1000f));
            DataPointCollection dataPointCollection = this.chartA2.Series["A2 Measuring"].Points;
            int num1 = frmMain.j;
            frmMain.j = num1 + 1;
            dataPointCollection.AddXY((double)num1, (double)(float.Parse(Communication.A2MinimumOffset.Replace(".", "")) / 1000f));
            DataPointCollection points1 = this.chartA2.Series["A2 Min Offset"].Points;
            int num2 = frmMain.j;
            frmMain.j = num2 + 1;
            points1.AddXY((double)num2, (double)(float.Parse(Communication.A2MinimumOffset.Replace(".", "")) / 1000f));
        }

        private void chkStopScan_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void cmbModel_TextChanged(object sender, EventArgs e)
        {
        }

        private void cmbTimeToEnableRead_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void COM_Connect()
        {
            try
            {
                Communication.serialport.Close();
                if (Communication.ConnectSerial(Communication.comPort, Communication.baudrate))
                {
                    this.lblConnectStatus.Text = "Connected";
                    this.lblConnectStatus.ForeColor = Color.Green;
                }
            }
            catch
            {
                MessageBox.Show("Failed! Please check your settings and try again!");
                this.lblConnectStatus.Text = "Not Connected";
                this.lblConnectStatus.ForeColor = Color.Red;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectProductSetting();
        }

        private void communicatiomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new SettingLogin()).ShowDialog();
        }

        private void controlAlarm_A1ResetAlarm()
        {
            Communication.enableConnectToControlBox = true;
            this.serialPort1.Write("2");
            Communication.enableConnectToControlBox = false;
        }

        private void controlAlarm_A1SetAlarm()
        {
            Communication.enableConnectToControlBox = true;
            this.serialPort1.Write("1");
            Communication.enableConnectToControlBox = false;
        }

        private void controlAlarm_A2ResetAlarm()
        {
            Communication.enableConnectToControlBox = true;
            this.serialPort1.Write("4");
            Communication.enableConnectToControlBox = false;
        }

        private void controlAlarm_A2SetAlarm()
        {
            Communication.enableConnectToControlBox = true;
            this.serialPort1.Write("3");
            Communication.enableConnectToControlBox = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                this.btnDeleteTestData.Enabled = true;
                foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
                {
                    frmMain.deleteData.ID = row.Cells[0].Value.ToString();
                    frmMain.deleteData.model = row.Cells[1].Value.ToString();
                    frmMain.deleteData.A1MaximumValue = row.Cells[2].Value.ToString();
                    frmMain.deleteData.A1MinimumValue = row.Cells[3].Value.ToString();
                    frmMain.deleteData.A1Result = row.Cells[4].Value.ToString();
                    frmMain.deleteData.A2MaximumValue = row.Cells[5].Value.ToString();
                    frmMain.deleteData.A2MinimumValue = row.Cells[6].Value.ToString();
                    frmMain.deleteData.A2Result = row.Cells[7].Value.ToString();
                    frmMain.deleteData.Date = row.Cells[8].Value.ToString();
                    frmMain.deleteData.Time = row.Cells[9].Value.ToString();
                    frmMain.deleteData.Judge = row.Cells[10].Value.ToString();
                    frmMain.deleteData.totalProcessed = Convert.ToInt32(row.Cells[11].Value.ToString());
                    frmMain.deleteData.totalPASS = Convert.ToInt32(row.Cells[12].Value.ToString());
                    frmMain.deleteData.totalFAIL = Convert.ToInt32(row.Cells[13].Value.ToString());
                }
            }
        }

        public void DataReceive(object obj, SerialDataReceivedEventArgs e)
        {
            if ((Communication.closeComport || !Communication.enableReceiveData ? false : !Communication.enableConnectToControlBox))
            {
                this.InputData = string.Concat(this.InputData, Communication.serialport.ReadExisting());
                this.InputData = this.InputData.Replace("\r", string.Empty);
                this.InputData = this.InputData.Replace("\n", string.Empty);
                Communication.test++;
                if (this.InputData.Length > Communication.charNumberOfCom_data * 5)
                {
                    this.InputData = Communication.serialport.ReadExisting();
                }
                if ((!Communication.start ? false : this.InputData.Length >= Communication.charNumberOfCom_data) && this.InputData != string.Empty)
                {
                    this.SetText(this.InputData);
                }
            }
        }

        private void deleteA1BufferData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                (new SqlCommand("delete from A1BufferData", con)).ExecuteNonQuery();
                con.Close();
            }
            catch
            {
            }
        }

        private void deleteA2BufferData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                (new SqlCommand("delete from A2BufferData", con)).ExecuteNonQuery();
                con.Close();
            }
            catch
            {
            }
        }

        private void displayJudge_Tick(object sender, EventArgs e)
        {
            this.tmrDisplayJudge.Enabled = false;
            this.btnJudge.Text = Communication.Judge;
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            base.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //if (DateTime.Today.Year.ToString() != "2021")
            //{
            //    MessageBox.Show("System Error!");
            //    base.Close();
            //}
            this.serialPort1.Open();
            this.controlAlarm_A1ResetAlarm();
            this.controlAlarm_A2ResetAlarm();
            this.AdjustLayout();
            this.RefreshMainForm();
            Communication.load_ComSetting();
            this.loadProductSetting();
            this.loadData();
            this.COM_Connect();
            this.txtSystemMessage.Text = "Please press the 'Start' button to begin!";
            this.dataGridView1.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dataGridView1.Columns["model"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dataGridView1.Columns["A1MaxValue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["A1MinValue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["A1Result"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["A2MaxValue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["A2MinValue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["A2Result"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["Judge"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["TotalProcessed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["TotalPASS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["TotalFAIL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.chartA1Setting();
            this.chartA2Setting();
            //using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = (new ManagementClass("win32_processor")).GetInstances().GetEnumerator())
            //{
            //    if (enumerator.MoveNext())
            //    {
            //        ManagementObject managObj = (ManagementObject)enumerator.Current;
            //        frmMain.strgetProcessorID = managObj.Properties["processorID"].Value.ToString();
            //    }
            //}
            //if ((frmMain.strgetProcessorID.Trim() == Communication.processorID.Trim() ? false : frmMain.strgetProcessorID.Trim() != Communication.processorIDAdmin.Trim()))
            //{
            //    MessageBox.Show("System Error!", "WARNING!");
            //    base.Dispose();
            //}
            this.calculatePPandPPKvalue();
        }

        private void getA1BufferData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("select A1MaxValue, A1MinValue, A1Result from A1BufferData", con));
                DataTable dt = new DataTable();
                da.Fill(dt);
                Communication.A1MaximumValue = Convert.ToString(dt.Rows[dt.Rows.Count - 5][0]).Trim();
                Communication.A1MinimumValue = Convert.ToString(dt.Rows[dt.Rows.Count - 5][1]).Trim();
                Communication.A1Result = Convert.ToString(dt.Rows[dt.Rows.Count - 5][2]).Trim();
                this.txtA1MaximumValue.Text = Communication.A1MaximumValue;
                this.txtA1MinimumValue.Text = Communication.A1MinimumValue;
                if ((float.Parse(Communication.A1MaximumValue) >= float.Parse(Communication.A1MaximumOffset) ? false : float.Parse(Communication.A1MinimumValue) > float.Parse(Communication.A1MinimumOffset)))
                {
                    Communication.A1Result = "OK";
                }
                else
                {
                    Communication.A1Result = "NG";
                    this.controlAlarm_A1SetAlarm();
                }
                this.txtA1Result.Text = Communication.A1Result;
                if (this.txtA1Result.Text != "NG")
                {
                    this.txtA1Result.ForeColor = Color.ForestGreen;
                }
                else
                {
                    this.txtA1Result.ForeColor = Color.Red;
                    if (this.chkStopScan.Checked)
                    {
                        Communication.A1enableStopTest = true;
                    }
                    this.controlAlarm_A1SetAlarm();
                }
                con.Close();
            }
            catch
            {
            }
        }

        private void getA2BufferData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("select A2MaxValue, A2MinValue, A2Result from A2BufferData", con));
                DataTable dt = new DataTable();
                da.Fill(dt);
                Communication.A2MaximumValue = Convert.ToString(dt.Rows[dt.Rows.Count - 5][0]).Trim();
                Communication.A2MinimumValue = Convert.ToString(dt.Rows[dt.Rows.Count - 5][1]).Trim();
                Communication.A2Result = Convert.ToString(dt.Rows[dt.Rows.Count - 5][2]).Trim();
                this.txtA2MaximumValue.Text = Communication.A2MaximumValue;
                this.txtA2MinimumValue.Text = Communication.A2MinimumValue;
                if ((float.Parse(Communication.A2MaximumValue) >= float.Parse(Communication.A2MaximumOffset) ? false : float.Parse(Communication.A2MinimumValue) > float.Parse(Communication.A2MinimumOffset)))
                {
                    Communication.A2Result = "OK";
                }
                else
                {
                    Communication.A2Result = "NG";
                    this.controlAlarm_A2SetAlarm();
                }
                this.txtA2Result.Text = Communication.A2Result;
                if (this.txtA2Result.Text != "NG")
                {
                    this.txtA2Result.ForeColor = Color.ForestGreen;
                }
                else
                {
                    this.txtA2Result.ForeColor = Color.Red;
                    if (this.chkStopScan.Checked)
                    {
                        Communication.A2enableStopTest = true;
                    }
                    this.controlAlarm_A2SetAlarm();
                }
                con.Close();
            }
            catch
            {
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ChartArea chartArea5 = new ChartArea();
            Legend legend5 = new Legend();
            Series series5 = new Series();
            ChartArea chartArea6 = new ChartArea();
            Legend legend6 = new Legend();
            Series series6 = new Series();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle21 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmMain));
            this.panel3 = new Panel();
            this.panel6 = new Panel();
            this.groupBox7 = new GroupBox();
            this.txtA2PPK = new TextBox();
            this.label14 = new Label();
            this.txtA2PP = new TextBox();
            this.label19 = new Label();
            this.groupBox5 = new GroupBox();
            this.txtA1PPK = new TextBox();
            this.label15 = new Label();
            this.txtA1PP = new TextBox();
            this.label13 = new Label();
            this.groupBox3 = new GroupBox();
            this.chartA1 = new Chart();
            this.chartA2 = new Chart();
            this.groupBox4 = new GroupBox();
            this.txtTotalProcessed = new TextBox();
            this.label16 = new Label();
            this.txtTotalFAIL = new TextBox();
            this.txtTotalPass = new TextBox();
            this.label17 = new Label();
            this.label18 = new Label();
            this.btnJudge = new Button();
            this.dataGridView1 = new DataGridView();
            this.ID = new DataGridViewTextBoxColumn();
            this.model = new DataGridViewTextBoxColumn();
            this.A1MaxValue = new DataGridViewTextBoxColumn();
            this.A1MinValue = new DataGridViewTextBoxColumn();
            this.A1Result = new DataGridViewTextBoxColumn();
            this.A2MaxValue = new DataGridViewTextBoxColumn();
            this.A2MinValue = new DataGridViewTextBoxColumn();
            this.A2Result = new DataGridViewTextBoxColumn();
            this.Date = new DataGridViewTextBoxColumn();
            this.Time = new DataGridViewTextBoxColumn();
            this.Judge = new DataGridViewTextBoxColumn();
            this.TotalProcessed = new DataGridViewTextBoxColumn();
            this.TotalPASS = new DataGridViewTextBoxColumn();
            this.TotalFAIL = new DataGridViewTextBoxColumn();
            this.panel5 = new Panel();
            this.groupBox6 = new GroupBox();
            this.txtDate = new TextBox();
            this.txtTime = new TextBox();
            this.groupBox2 = new GroupBox();
            this.txtA2Result = new TextBox();
            this.txtA1MinimumValue = new TextBox();
            this.txtA1MaximumValue = new TextBox();
            this.txtA1Result = new TextBox();
            this.label3 = new Label();
            this.label5 = new Label();
            this.txtA2MinimumValue = new TextBox();
            this.txtA2MaximumValue = new TextBox();
            this.label10 = new Label();
            this.label11 = new Label();
            this.groupBox1 = new GroupBox();
            this.cmbTimeToEnableRead = new ComboBox();
            this.label20 = new Label();
            this.txtA2MaximumOffset = new TextBox();
            this.txtA1MaximumOffset = new TextBox();
            this.label9 = new Label();
            this.label12 = new Label();
            this.txtA2MinimumOffset = new TextBox();
            this.txtA1MinimumOffset = new TextBox();
            this.label6 = new Label();
            this.label8 = new Label();
            this.txtA2DetectionLevel = new TextBox();
            this.txtA1DetectionLevel = new TextBox();
            this.label7 = new Label();
            this.label4 = new Label();
            this.cmbModel = new ComboBox();
            this.label2 = new Label();
            this.menuStrip1 = new MenuStrip();
            this.MenuConfig = new ToolStripMenuItem();
            this.communicatiomToolStripMenuItem = new ToolStripMenuItem();
            this.instructionToolStripMenuItem = new ToolStripMenuItem();
            this.lblStatus = new Label();
            this.panel2 = new Panel();
            this.lblConnectStatus = new Label();
            this.panel1 = new Panel();
            this.panel4 = new Panel();
            this.label1 = new Label();
            this.tmrConnectionStatus = new Timer(this.components);
            this.panelResult = new Panel();
            this.chkStopScan = new CheckBox();
            this.txtSystemMessage = new TextBox();
            this.button2 = new Button();
            this.btnSelect = new Button();
            this.btnClear = new Button();
            this.btnClearCurrentTest = new Button();
            this.btnDeleteTestData = new Button();
            this.btnEdit = new Button();
            this.lblSystemMessage = new TextBox();
            this.btnStart = new Button();
            this.tmrDisplayData = new Timer(this.components);
            this.tmrDateTime = new Timer(this.components);
            this.tmrRefreshChart = new Timer(this.components);
            this.tmrA1DetectRemoveObject = new Timer(this.components);
            this.tmrA2DetectRemoveObject = new Timer(this.components);
            this.tmrEnableReadA1Data = new Timer(this.components);
            this.tmrEnableReadA2Data = new Timer(this.components);
            this.tmrRefreshDataGridView = new Timer(this.components);
            this.serialPort1 = new SerialPort(this.components);
            this.tmrDisplayJudge = new Timer(this.components);
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((ISupportInitialize)this.chartA1).BeginInit();
            ((ISupportInitialize)this.chartA2).BeginInit();
            this.groupBox4.SuspendLayout();
            ((ISupportInitialize)this.dataGridView1).BeginInit();
            this.panel5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelResult.SuspendLayout();
            base.SuspendLayout();
            this.panel3.BackColor = SystemColors.ActiveCaption;
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Dock = DockStyle.Fill;
            this.panel3.Location = new Point(0, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(1904, 928);
            this.panel3.TabIndex = 71;
            this.panel6.BackColor = SystemColors.HighlightText;
            this.panel6.Controls.Add(this.groupBox7);
            this.panel6.Controls.Add(this.groupBox5);
            this.panel6.Controls.Add(this.groupBox3);
            this.panel6.Controls.Add(this.groupBox4);
            this.panel6.Controls.Add(this.btnJudge);
            this.panel6.Controls.Add(this.dataGridView1);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Dock = DockStyle.Fill;
            this.panel6.Location = new Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(1904, 928);
            this.panel6.TabIndex = 2;
            this.groupBox7.Controls.Add(this.txtA2PPK);
            this.groupBox7.Controls.Add(this.label14);
            this.groupBox7.Controls.Add(this.txtA2PP);
            this.groupBox7.Controls.Add(this.label19);
            this.groupBox7.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox7.Location = new Point(1720, 335);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new Size(178, 108);
            this.groupBox7.TabIndex = 27;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "A2 Index";
            this.txtA2PPK.BackColor = SystemColors.MenuText;
            this.txtA2PPK.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA2PPK.ForeColor = Color.Yellow;
            this.txtA2PPK.Location = new Point(42, 62);
            this.txtA2PPK.Name = "txtA2PPK";
            this.txtA2PPK.Size = new Size(130, 35);
            this.txtA2PPK.TabIndex = 25;
            this.txtA2PPK.Text = "0";
            this.txtA2PPK.TextAlign = HorizontalAlignment.Right;
            this.label14.AutoSize = true;
            this.label14.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label14.Location = new Point(4, 72);
            this.label14.Name = "label14";
            this.label14.Size = new Size(39, 20);
            this.label14.TabIndex = 26;
            this.label14.Text = "PPK";
            this.txtA2PP.BackColor = SystemColors.MenuText;
            this.txtA2PP.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA2PP.ForeColor = Color.Yellow;
            this.txtA2PP.Location = new Point(42, 21);
            this.txtA2PP.Name = "txtA2PP";
            this.txtA2PP.Size = new Size(130, 35);
            this.txtA2PP.TabIndex = 20;
            this.txtA2PP.Text = "0";
            this.txtA2PP.TextAlign = HorizontalAlignment.Right;
            this.label19.AutoSize = true;
            this.label19.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label19.Location = new Point(14, 31);
            this.label19.Name = "label19";
            this.label19.Size = new Size(29, 20);
            this.label19.TabIndex = 24;
            this.label19.Text = "PP";
            this.groupBox5.Controls.Add(this.txtA1PPK);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.txtA1PP);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox5.Location = new Point(1535, 335);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(178, 108);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "A1 Index";
            this.txtA1PPK.BackColor = SystemColors.MenuText;
            this.txtA1PPK.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA1PPK.ForeColor = Color.Yellow;
            this.txtA1PPK.Location = new Point(40, 62);
            this.txtA1PPK.Name = "txtA1PPK";
            this.txtA1PPK.Size = new Size(132, 35);
            this.txtA1PPK.TabIndex = 25;
            this.txtA1PPK.Text = "0";
            this.txtA1PPK.TextAlign = HorizontalAlignment.Right;
            this.label15.AutoSize = true;
            this.label15.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label15.Location = new Point(2, 72);
            this.label15.Name = "label15";
            this.label15.Size = new Size(39, 20);
            this.label15.TabIndex = 26;
            this.label15.Text = "PPK";
            this.txtA1PP.BackColor = SystemColors.MenuText;
            this.txtA1PP.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA1PP.ForeColor = Color.Yellow;
            this.txtA1PP.Location = new Point(40, 21);
            this.txtA1PP.Name = "txtA1PP";
            this.txtA1PP.Size = new Size(132, 35);
            this.txtA1PP.TabIndex = 20;
            this.txtA1PP.Text = "0";
            this.txtA1PP.TextAlign = HorizontalAlignment.Right;
            this.label13.AutoSize = true;
            this.label13.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label13.Location = new Point(12, 31);
            this.label13.Name = "label13";
            this.label13.Size = new Size(29, 20);
            this.label13.TabIndex = 24;
            this.label13.Text = "PP";
            this.label13.Click += new EventHandler(this.label13_Click);
            this.groupBox3.Controls.Add(this.chartA1);
            this.groupBox3.Controls.Add(this.chartA2);
            this.groupBox3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox3.ForeColor = Color.Teal;
            this.groupBox3.Location = new Point(6, 102);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(1522, 341);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time Chart";
            chartArea5.AxisY.Title = "A1 Air Pressure";
            chartArea5.AxisY.TitleForeColor = SystemColors.MenuHighlight;
            chartArea5.Name = "ChartArea1";
            chartArea5.ShadowColor = Color.Gray;
            this.chartA1.ChartAreas.Add(chartArea5);
            legend5.DockedToChartArea = "ChartArea1";
            legend5.Enabled = false;
            legend5.HeaderSeparatorColor = Color.DarkGray;
            legend5.IsTextAutoFit = false;
            legend5.ItemColumnSeparatorColor = Color.LightGray;
            legend5.Name = "Legend1";
            this.chartA1.Legends.Add(legend5);
            this.chartA1.Location = new Point(2, 16);
            this.chartA1.Name = "chartA1";
            this.chartA1.Palette = ChartColorPalette.None;
            series5.BorderWidth = 2;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = SeriesChartType.Spline;
            series5.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            series5.Legend = "Legend1";
            series5.Name = "A2";
            this.chartA1.Series.Add(series5);
            this.chartA1.Size = new Size(753, 314);
            this.chartA1.TabIndex = 2;
            this.chartA1.Text = "Chart A1";
            chartArea6.AxisY.Title = "A2 Air Pressure";
            chartArea6.AxisY.TitleForeColor = SystemColors.MenuHighlight;
            chartArea6.Name = "ChartArea1";
            chartArea6.ShadowColor = Color.Gray;
            this.chartA2.ChartAreas.Add(chartArea6);
            legend6.DockedToChartArea = "ChartArea1";
            legend6.Enabled = false;
            legend6.HeaderSeparatorColor = Color.DarkGray;
            legend6.ItemColumnSeparatorColor = Color.LightGray;
            legend6.Name = "Legend1";
            this.chartA2.Legends.Add(legend6);
            this.chartA2.Location = new Point(742, 16);
            this.chartA2.Name = "chartA2";
            this.chartA2.Palette = ChartColorPalette.None;
            series6.BorderWidth = 2;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = SeriesChartType.Spline;
            series6.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            series6.IsXValueIndexed = true;
            series6.Legend = "Legend1";
            series6.Name = "A2";
            this.chartA2.Series.Add(series6);
            this.chartA2.Size = new Size(774, 314);
            this.chartA2.TabIndex = 1;
            this.chartA2.Text = "Chart A2";
            this.groupBox4.Controls.Add(this.txtTotalProcessed);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.txtTotalFAIL);
            this.groupBox4.Controls.Add(this.txtTotalPass);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox4.Location = new Point(1535, 187);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(363, 145);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Counter";
            this.txtTotalProcessed.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtTotalProcessed.ForeColor = Color.Teal;
            this.txtTotalProcessed.Location = new Point(154, 20);
            this.txtTotalProcessed.Name = "txtTotalProcessed";
            this.txtTotalProcessed.Size = new Size(190, 35);
            this.txtTotalProcessed.TabIndex = 21;
            this.txtTotalProcessed.Text = "0";
            this.txtTotalProcessed.TextAlign = HorizontalAlignment.Right;
            this.label16.AutoSize = true;
            this.label16.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label16.Location = new Point(25, 29);
            this.label16.Name = "label16";
            this.label16.Size = new Size(123, 20);
            this.label16.TabIndex = 20;
            this.label16.Text = "Total Processed";
            this.txtTotalFAIL.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtTotalFAIL.ForeColor = Color.FromArgb(192, 0, 0);
            this.txtTotalFAIL.Location = new Point(154, 97);
            this.txtTotalFAIL.Name = "txtTotalFAIL";
            this.txtTotalFAIL.Size = new Size(190, 35);
            this.txtTotalFAIL.TabIndex = 19;
            this.txtTotalFAIL.Text = "0";
            this.txtTotalFAIL.TextAlign = HorizontalAlignment.Right;
            this.txtTotalPass.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtTotalPass.ForeColor = Color.Teal;
            this.txtTotalPass.Location = new Point(154, 58);
            this.txtTotalPass.Name = "txtTotalPass";
            this.txtTotalPass.Size = new Size(190, 35);
            this.txtTotalPass.TabIndex = 18;
            this.txtTotalPass.Text = "0";
            this.txtTotalPass.TextAlign = HorizontalAlignment.Right;
            this.label17.AutoSize = true;
            this.label17.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label17.Location = new Point(25, 103);
            this.label17.Name = "label17";
            this.label17.Size = new Size(83, 20);
            this.label17.TabIndex = 10;
            this.label17.Text = "Total FAIL";
            this.label18.AutoSize = true;
            this.label18.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label18.Location = new Point(25, 66);
            this.label18.Name = "label18";
            this.label18.Size = new Size(91, 20);
            this.label18.TabIndex = 8;
            this.label18.Text = "Total PASS";
            this.btnJudge.BackColor = Color.White;
            this.btnJudge.Font = new Font("Microsoft Sans Serif", 72f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.btnJudge.ForeColor = Color.FromArgb(0, 192, 0);
            this.btnJudge.Location = new Point(1534, 11);
            this.btnJudge.Name = "btnJudge";
            this.btnJudge.Size = new Size(365, 175);
            this.btnJudge.TabIndex = 5;
            this.btnJudge.UseVisualStyleBackColor = false;
            dataGridViewCellStyle15.BackColor = SystemColors.Control;
            dataGridViewCellStyle15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle15.ForeColor = SystemColors.ActiveCaptionText;
            dataGridViewCellStyle15.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle15.SelectionForeColor = Color.Black;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridView1.Anchor = AnchorStyles.Right;
            this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dataGridView1.BackgroundColor = SystemColors.HighlightText;
            this.dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.BackColor = Color.White;
            dataGridViewCellStyle16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle16.ForeColor = Color.FromArgb(0, 0, 192);
            dataGridViewCellStyle16.SelectionBackColor = SystemColors.GrayText;
            dataGridViewCellStyle16.SelectionForeColor = Color.White;
            dataGridViewCellStyle16.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { this.ID, this.model, this.A1MaxValue, this.A1MinValue, this.A1Result, this.A2MaxValue, this.A2MinValue, this.A2Result, this.Date, this.Time, this.Judge, this.TotalProcessed, this.TotalPASS, this.TotalFAIL });
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = Color.Red;
            dataGridViewCellStyle19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle19.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle19.SelectionBackColor = Color.White;
            dataGridViewCellStyle19.SelectionForeColor = Color.White;
            dataGridViewCellStyle19.WrapMode = DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle19;
            this.dataGridView1.GridColor = Color.White;
            this.dataGridView1.ImeMode = ImeMode.NoControl;
            this.dataGridView1.Location = new Point(0, 449);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = Color.Red;
            dataGridViewCellStyle20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle20.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle20.SelectionBackColor = Color.White;
            dataGridViewCellStyle20.SelectionForeColor = Color.White;
            dataGridViewCellStyle20.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle20;
            dataGridViewCellStyle21.BackColor = SystemColors.Control;
            dataGridViewCellStyle21.ForeColor = Color.Black;
            dataGridViewCellStyle21.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle21.SelectionForeColor = Color.LightGray;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle21;
            this.dataGridView1.Size = new Size(1904, 480);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.ID.DataPropertyName = "ID";
            dataGridViewCellStyle17.BackColor = Color.White;
            dataGridViewCellStyle17.ForeColor = Color.White;
            dataGridViewCellStyle17.SelectionBackColor = Color.White;
            dataGridViewCellStyle17.SelectionForeColor = Color.White;
            this.ID.DefaultCellStyle = dataGridViewCellStyle17;
            this.ID.FillWeight = 120f;
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 100;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.model.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.model.DataPropertyName = "model";
            dataGridViewCellStyle18.BackColor = Color.White;
            dataGridViewCellStyle18.ForeColor = Color.White;
            dataGridViewCellStyle18.SelectionBackColor = Color.White;
            dataGridViewCellStyle18.SelectionForeColor = Color.White;
            this.model.DefaultCellStyle = dataGridViewCellStyle18;
            this.model.FillWeight = 120f;
            this.model.HeaderText = "Model";
            this.model.MinimumWidth = 100;
            this.model.Name = "model";
            this.model.ReadOnly = true;
            this.A1MaxValue.DataPropertyName = "A1MaxValue";
            this.A1MaxValue.HeaderText = "A1 Max Value";
            this.A1MaxValue.MinimumWidth = 130;
            this.A1MaxValue.Name = "A1MaxValue";
            this.A1MaxValue.ReadOnly = true;
            this.A1MaxValue.Width = 133;
            this.A1MinValue.DataPropertyName = "A1MinValue";
            this.A1MinValue.HeaderText = "A1 Min Value";
            this.A1MinValue.MinimumWidth = 130;
            this.A1MinValue.Name = "A1MinValue";
            this.A1MinValue.ReadOnly = true;
            this.A1MinValue.Width = 133;
            this.A1Result.DataPropertyName = "A1Result";
            this.A1Result.FillWeight = 80f;
            this.A1Result.HeaderText = "A1 Result";
            this.A1Result.MinimumWidth = 80;
            this.A1Result.Name = "A1Result";
            this.A1Result.Width = 106;
            this.A2MaxValue.DataPropertyName = "A2MaxValue";
            this.A2MaxValue.HeaderText = "A2 Max Value";
            this.A2MaxValue.MinimumWidth = 130;
            this.A2MaxValue.Name = "A2MaxValue";
            this.A2MaxValue.Width = 133;
            this.A2MinValue.DataPropertyName = "A2MinValue";
            this.A2MinValue.HeaderText = "A2 Min Value";
            this.A2MinValue.MinimumWidth = 130;
            this.A2MinValue.Name = "A2MinValue";
            this.A2MinValue.Width = 133;
            this.A2Result.DataPropertyName = "A2Result";
            this.A2Result.FillWeight = 80f;
            this.A2Result.HeaderText = "A2 Result";
            this.A2Result.MinimumWidth = 80;
            this.A2Result.Name = "A2Result";
            this.A2Result.Width = 106;
            this.Date.DataPropertyName = "Date";
            this.Date.FillWeight = 80f;
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 80;
            this.Date.Name = "Date";
            this.Date.Width = 107;
            this.Time.DataPropertyName = "Time";
            this.Time.FillWeight = 80f;
            this.Time.HeaderText = "Time";
            this.Time.MinimumWidth = 80;
            this.Time.Name = "Time";
            this.Time.Width = 106;
            this.Judge.DataPropertyName = "Judge";
            this.Judge.FillWeight = 80f;
            this.Judge.HeaderText = "Judge";
            this.Judge.MinimumWidth = 80;
            this.Judge.Name = "Judge";
            this.Judge.Width = 106;
            this.TotalProcessed.DataPropertyName = "TotalProcessed";
            this.TotalProcessed.FillWeight = 120f;
            this.TotalProcessed.HeaderText = "Total Processed";
            this.TotalProcessed.MinimumWidth = 100;
            this.TotalProcessed.Name = "TotalProcessed";
            this.TotalProcessed.Resizable = DataGridViewTriState.False;
            this.TotalProcessed.Width = 160;
            this.TotalPASS.DataPropertyName = "TotalPASS";
            this.TotalPASS.FillWeight = 120f;
            this.TotalPASS.HeaderText = "Total PASS";
            this.TotalPASS.MinimumWidth = 100;
            this.TotalPASS.Name = "TotalPASS";
            this.TotalPASS.Resizable = DataGridViewTriState.False;
            this.TotalPASS.Width = 159;
            this.TotalFAIL.DataPropertyName = "TotalFAIL";
            this.TotalFAIL.FillWeight = 120f;
            this.TotalFAIL.HeaderText = "Total FAIL";
            this.TotalFAIL.MinimumWidth = 100;
            this.TotalFAIL.Name = "TotalFAIL";
            this.TotalFAIL.Resizable = DataGridViewTriState.False;
            this.TotalFAIL.Width = 160;
            this.panel5.BackColor = SystemColors.HighlightText;
            this.panel5.Controls.Add(this.groupBox6);
            this.panel5.Controls.Add(this.groupBox2);
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Location = new Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(1530, 102);
            this.panel5.TabIndex = 0;
            this.groupBox6.Controls.Add(this.txtDate);
            this.groupBox6.Controls.Add(this.txtTime);
            this.groupBox6.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox6.Location = new Point(766, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new Size(124, 90);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "System Time";
            this.txtDate.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtDate.ForeColor = SystemColors.WindowFrame;
            this.txtDate.Location = new Point(7, 22);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new Size(110, 26);
            this.txtDate.TabIndex = 23;
            this.txtDate.TextAlign = HorizontalAlignment.Right;
            this.txtTime.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtTime.ForeColor = SystemColors.WindowFrame;
            this.txtTime.Location = new Point(7, 52);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new Size(110, 26);
            this.txtTime.TabIndex = 22;
            this.txtTime.TextAlign = HorizontalAlignment.Right;
            this.groupBox2.Controls.Add(this.txtA2Result);
            this.groupBox2.Controls.Add(this.txtA1MinimumValue);
            this.groupBox2.Controls.Add(this.txtA1MaximumValue);
            this.groupBox2.Controls.Add(this.txtA1Result);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtA2MinimumValue);
            this.groupBox2.Controls.Add(this.txtA2MaximumValue);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox2.ForeColor = Color.Black;
            this.groupBox2.Location = new Point(895, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(633, 90);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Measuring Data";
            this.txtA2Result.Font = new Font("Microsoft Sans Serif", 32.25f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA2Result.ForeColor = Color.FromArgb(0, 192, 0);
            this.txtA2Result.Location = new Point(544, 22);
            this.txtA2Result.Name = "txtA2Result";
            this.txtA2Result.Size = new Size(82, 56);
            this.txtA2Result.TabIndex = 31;
            this.txtA2Result.TextAlign = HorizontalAlignment.Center;
            this.txtA1MinimumValue.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA1MinimumValue.Location = new Point(132, 52);
            this.txtA1MinimumValue.Name = "txtA1MinimumValue";
            this.txtA1MinimumValue.Size = new Size(95, 26);
            this.txtA1MinimumValue.TabIndex = 30;
            this.txtA1MinimumValue.TextAlign = HorizontalAlignment.Right;
            this.txtA1MaximumValue.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA1MaximumValue.Location = new Point(132, 22);
            this.txtA1MaximumValue.Name = "txtA1MaximumValue";
            this.txtA1MaximumValue.Size = new Size(95, 26);
            this.txtA1MaximumValue.TabIndex = 29;
            this.txtA1MaximumValue.TextAlign = HorizontalAlignment.Right;
            this.txtA1Result.Font = new Font("Microsoft Sans Serif", 32.25f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA1Result.ForeColor = Color.FromArgb(0, 192, 0);
            this.txtA1Result.Location = new Point(232, 22);
            this.txtA1Result.Name = "txtA1Result";
            this.txtA1Result.Size = new Size(82, 56);
            this.txtA1Result.TabIndex = 28;
            this.txtA1Result.TextAlign = HorizontalAlignment.Center;
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label3.Location = new Point(11, 56);
            this.label3.Name = "label3";
            this.label3.Size = new Size(118, 16);
            this.label3.TabIndex = 27;
            this.label3.Text = "A1 Minimum Value";
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label5.Location = new Point(11, 26);
            this.label5.Name = "label5";
            this.label5.Size = new Size(122, 16);
            this.label5.TabIndex = 26;
            this.label5.Text = "A1 Maximum Value";
            this.txtA2MinimumValue.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA2MinimumValue.Location = new Point(444, 52);
            this.txtA2MinimumValue.Name = "txtA2MinimumValue";
            this.txtA2MinimumValue.Size = new Size(95, 26);
            this.txtA2MinimumValue.TabIndex = 25;
            this.txtA2MinimumValue.TextAlign = HorizontalAlignment.Right;
            this.txtA2MaximumValue.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA2MaximumValue.Location = new Point(444, 22);
            this.txtA2MaximumValue.Name = "txtA2MaximumValue";
            this.txtA2MaximumValue.Size = new Size(95, 26);
            this.txtA2MaximumValue.TabIndex = 24;
            this.txtA2MaximumValue.TextAlign = HorizontalAlignment.Right;
            this.label10.AutoSize = true;
            this.label10.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label10.Location = new Point(323, 56);
            this.label10.Name = "label10";
            this.label10.Size = new Size(118, 16);
            this.label10.TabIndex = 15;
            this.label10.Text = "A2 Minimum Value";
            this.label11.AutoSize = true;
            this.label11.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label11.Location = new Point(323, 26);
            this.label11.Name = "label11";
            this.label11.Size = new Size(122, 16);
            this.label11.TabIndex = 13;
            this.label11.Text = "A2 Maximum Value";
            this.groupBox1.Controls.Add(this.cmbTimeToEnableRead);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtA2MaximumOffset);
            this.groupBox1.Controls.Add(this.txtA1MaximumOffset);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtA2MinimumOffset);
            this.groupBox1.Controls.Add(this.txtA1MinimumOffset);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtA2DetectionLevel);
            this.groupBox1.Controls.Add(this.txtA1DetectionLevel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbModel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox1.Location = new Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(755, 90);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Setting";
            this.cmbTimeToEnableRead.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbTimeToEnableRead.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbTimeToEnableRead.FormattingEnabled = true;
            this.cmbTimeToEnableRead.Items.AddRange(new object[] { "0.5", "1", "1.5", "2", "2.5", "3" });
            this.cmbTimeToEnableRead.Location = new Point(115, 52);
            this.cmbTimeToEnableRead.Name = "cmbTimeToEnableRead";
            this.cmbTimeToEnableRead.Size = new Size(52, 28);
            this.cmbTimeToEnableRead.TabIndex = 84;
            this.cmbTimeToEnableRead.Text = "1";
            this.cmbTimeToEnableRead.SelectedIndexChanged += new EventHandler(this.cmbTimeToEnableRead_SelectedIndexChanged);
            this.label20.AutoSize = true;
            this.label20.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label20.Location = new Point(10, 57);
            this.label20.Name = "label20";
            this.label20.Size = new Size(99, 16);
            this.label20.TabIndex = 83;
            this.label20.Text = "Detect Time (s)";
            this.txtA2MaximumOffset.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA2MaximumOffset.ForeColor = SystemColors.InactiveCaptionText;
            this.txtA2MaximumOffset.Location = new Point(652, 52);
            this.txtA2MaximumOffset.Name = "txtA2MaximumOffset";
            this.txtA2MaximumOffset.Size = new Size(95, 26);
            this.txtA2MaximumOffset.TabIndex = 82;
            this.txtA2MaximumOffset.TextAlign = HorizontalAlignment.Right;
            this.txtA1MaximumOffset.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA1MaximumOffset.ForeColor = SystemColors.InactiveCaptionText;
            this.txtA1MaximumOffset.Location = new Point(652, 22);
            this.txtA1MaximumOffset.Name = "txtA1MaximumOffset";
            this.txtA1MaximumOffset.Size = new Size(95, 26);
            this.txtA1MaximumOffset.TabIndex = 81;
            this.txtA1MaximumOffset.TextAlign = HorizontalAlignment.Right;
            this.label9.AutoSize = true;
            this.label9.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label9.Location = new Point(562, 56);
            this.label9.Name = "label9";
            this.label9.Size = new Size(89, 16);
            this.label9.TabIndex = 80;
            this.label9.Text = "A2 Max Offset";
            this.label12.AutoSize = true;
            this.label12.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label12.Location = new Point(562, 26);
            this.label12.Name = "label12";
            this.label12.Size = new Size(89, 16);
            this.label12.TabIndex = 79;
            this.label12.Text = "A1 Max Offset";
            this.txtA2MinimumOffset.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA2MinimumOffset.ForeColor = SystemColors.InactiveCaptionText;
            this.txtA2MinimumOffset.Location = new Point(462, 52);
            this.txtA2MinimumOffset.Name = "txtA2MinimumOffset";
            this.txtA2MinimumOffset.Size = new Size(95, 26);
            this.txtA2MinimumOffset.TabIndex = 78;
            this.txtA2MinimumOffset.TextAlign = HorizontalAlignment.Right;
            this.txtA1MinimumOffset.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA1MinimumOffset.ForeColor = SystemColors.InactiveCaptionText;
            this.txtA1MinimumOffset.Location = new Point(462, 22);
            this.txtA1MinimumOffset.Name = "txtA1MinimumOffset";
            this.txtA1MinimumOffset.Size = new Size(95, 26);
            this.txtA1MinimumOffset.TabIndex = 77;
            this.txtA1MinimumOffset.TextAlign = HorizontalAlignment.Right;
            this.label6.AutoSize = true;
            this.label6.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label6.Location = new Point(375, 56);
            this.label6.Name = "label6";
            this.label6.Size = new Size(85, 16);
            this.label6.TabIndex = 76;
            this.label6.Text = "A2 Min Offset";
            this.label8.AutoSize = true;
            this.label8.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label8.Location = new Point(375, 26);
            this.label8.Name = "label8";
            this.label8.Size = new Size(85, 16);
            this.label8.TabIndex = 75;
            this.label8.Text = "A1 Min Offset";
            this.txtA2DetectionLevel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA2DetectionLevel.ForeColor = SystemColors.InactiveCaptionText;
            this.txtA2DetectionLevel.Location = new Point(276, 52);
            this.txtA2DetectionLevel.Name = "txtA2DetectionLevel";
            this.txtA2DetectionLevel.Size = new Size(95, 26);
            this.txtA2DetectionLevel.TabIndex = 74;
            this.txtA2DetectionLevel.TextAlign = HorizontalAlignment.Right;
            this.txtA1DetectionLevel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtA1DetectionLevel.ForeColor = SystemColors.InactiveCaptionText;
            this.txtA1DetectionLevel.Location = new Point(276, 22);
            this.txtA1DetectionLevel.Name = "txtA1DetectionLevel";
            this.txtA1DetectionLevel.Size = new Size(95, 26);
            this.txtA1DetectionLevel.TabIndex = 73;
            this.txtA1DetectionLevel.TextAlign = HorizontalAlignment.Right;
            this.label7.AutoSize = true;
            this.label7.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label7.Location = new Point(173, 57);
            this.label7.Name = "label7";
            this.label7.Size = new Size(102, 16);
            this.label7.TabIndex = 8;
            this.label7.Text = "A2 Detect Level";
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label4.Location = new Point(173, 26);
            this.label4.Name = "label4";
            this.label4.Size = new Size(102, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "A1 Detect Level";
            this.cmbModel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbModel.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new Point(61, 22);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new Size(106, 28);
            this.cmbModel.TabIndex = 1;
            this.cmbModel.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.cmbModel.TextChanged += new EventHandler(this.cmbModel_TextChanged);
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label2.Location = new Point(10, 27);
            this.label2.Name = "label2";
            this.label2.Size = new Size(46, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Model";
            this.label2.Click += new EventHandler(this.label2_Click);
            this.menuStrip1.Dock = DockStyle.None;
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.MenuConfig });
            this.menuStrip1.Location = new Point(245, 6);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(89, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            this.MenuConfig.DropDownItems.AddRange(new ToolStripItem[] { this.communicatiomToolStripMenuItem, this.instructionToolStripMenuItem });
            this.MenuConfig.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.MenuConfig.Name = "MenuConfig";
            this.MenuConfig.Size = new Size(81, 20);
            this.MenuConfig.Text = "&Configuration";
            this.communicatiomToolStripMenuItem.Name = "communicatiomToolStripMenuItem";
            this.communicatiomToolStripMenuItem.Size = new Size(146, 22);
            this.communicatiomToolStripMenuItem.Text = "&Communication";
            this.communicatiomToolStripMenuItem.Click += new EventHandler(this.communicatiomToolStripMenuItem_Click);
            this.instructionToolStripMenuItem.Name = "instructionToolStripMenuItem";
            this.instructionToolStripMenuItem.Size = new Size(146, 22);
            this.instructionToolStripMenuItem.Text = "&Instruction";
            this.instructionToolStripMenuItem.Click += new EventHandler(this.instructionToolStripMenuItem_Click);
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblStatus.ForeColor = Color.LightGoldenrodYellow;
            this.lblStatus.Location = new Point(34, 11);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(103, 16);
            this.lblStatus.TabIndex = 54;
            this.lblStatus.Text = "Connect. Status:";
            this.panel2.BackColor = SystemColors.Desktop;
            this.panel2.Controls.Add(this.lblConnectStatus);
            this.panel2.Controls.Add(this.lblStatus);
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(1564, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(340, 37);
            this.panel2.TabIndex = 56;
            this.lblConnectStatus.AutoSize = true;
            this.lblConnectStatus.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblConnectStatus.ForeColor = Color.Lime;
            this.lblConnectStatus.Location = new Point(141, 11);
            this.lblConnectStatus.Name = "lblConnectStatus";
            this.lblConnectStatus.Size = new Size(73, 16);
            this.lblConnectStatus.TabIndex = 55;
            this.lblConnectStatus.Text = "Connected";
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.ForeColor = Color.FromArgb(192, 192, 0);
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(1904, 37);
            this.panel1.TabIndex = 70;
            this.panel4.BackColor = SystemColors.Desktop;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = DockStyle.Fill;
            this.panel4.Location = new Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(1564, 37);
            this.panel4.TabIndex = 59;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label1.Location = new Point(20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(222, 18);
            this.label1.TabIndex = 58;
            this.label1.Text = "HALLA VINA - Diameter Checker";
            this.tmrConnectionStatus.Enabled = true;
            this.tmrConnectionStatus.Tick += new EventHandler(this.tmrConnectionStatus_Tick);
            this.panelResult.BackColor = SystemColors.Control;
            this.panelResult.BorderStyle = BorderStyle.Fixed3D;
            this.panelResult.Controls.Add(this.chkStopScan);
            this.panelResult.Controls.Add(this.txtSystemMessage);
            this.panelResult.Controls.Add(this.button2);
            this.panelResult.Controls.Add(this.btnSelect);
            this.panelResult.Controls.Add(this.btnClear);
            this.panelResult.Controls.Add(this.btnClearCurrentTest);
            this.panelResult.Controls.Add(this.btnDeleteTestData);
            this.panelResult.Controls.Add(this.btnEdit);
            this.panelResult.Controls.Add(this.lblSystemMessage);
            this.panelResult.Controls.Add(this.btnStart);
            this.panelResult.Dock = DockStyle.Bottom;
            this.panelResult.ForeColor = SystemColors.WindowFrame;
            this.panelResult.Location = new Point(0, 965);
            this.panelResult.Name = "panelResult";
            this.panelResult.Size = new Size(1904, 37);
            this.panelResult.TabIndex = 68;
            this.chkStopScan.AutoSize = true;
            this.chkStopScan.ForeColor = SystemColors.InactiveCaptionText;
            this.chkStopScan.Location = new Point(806, 9);
            this.chkStopScan.Name = "chkStopScan";
            this.chkStopScan.Size = new Size(169, 17);
            this.chkStopScan.TabIndex = 92;
            this.chkStopScan.Text = "Stop scan when NG Detected";
            this.chkStopScan.UseVisualStyleBackColor = true;
            this.chkStopScan.CheckedChanged += new EventHandler(this.chkStopScan_CheckedChanged);
            this.txtSystemMessage.BackColor = SystemColors.Control;
            this.txtSystemMessage.BorderStyle = BorderStyle.None;
            this.txtSystemMessage.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtSystemMessage.ForeColor = Color.DarkRed;
            this.txtSystemMessage.Location = new Point(119, 10);
            this.txtSystemMessage.Multiline = true;
            this.txtSystemMessage.Name = "txtSystemMessage";
            this.txtSystemMessage.Size = new Size(606, 20);
            this.txtSystemMessage.TabIndex = 79;
            this.button2.Dock = DockStyle.Right;
            this.button2.ForeColor = Color.Teal;
            this.button2.Location = new Point(990, 0);
            this.button2.Name = "button2";
            this.button2.Size = new Size(130, 33);
            this.button2.TabIndex = 78;
            this.button2.Text = "Auto Detect Object";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.btnSelect.Dock = DockStyle.Right;
            this.btnSelect.ForeColor = Color.Teal;
            this.btnSelect.Location = new Point(1120, 0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new Size(130, 33);
            this.btnSelect.TabIndex = 76;
            this.btnSelect.Text = "&Add New Model";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
            this.btnClear.Dock = DockStyle.Right;
            this.btnClear.ForeColor = Color.DarkRed;
            this.btnClear.Location = new Point(1250, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(130, 33);
            this.btnClear.TabIndex = 77;
            this.btnClear.Text = "&Delete Current Model";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.btnClearCurrentTest.Dock = DockStyle.Right;
            this.btnClearCurrentTest.ForeColor = Color.Teal;
            this.btnClearCurrentTest.Location = new Point(1380, 0);
            this.btnClearCurrentTest.Name = "btnClearCurrentTest";
            this.btnClearCurrentTest.Size = new Size(130, 33);
            this.btnClearCurrentTest.TabIndex = 74;
            this.btnClearCurrentTest.Text = "&Clear Current Test";
            this.btnClearCurrentTest.UseVisualStyleBackColor = true;
            this.btnClearCurrentTest.Click += new EventHandler(this.btnClearCurrentData_Click);
            this.btnDeleteTestData.Dock = DockStyle.Right;
            this.btnDeleteTestData.Enabled = false;
            this.btnDeleteTestData.ForeColor = Color.DarkRed;
            this.btnDeleteTestData.Location = new Point(1510, 0);
            this.btnDeleteTestData.Name = "btnDeleteTestData";
            this.btnDeleteTestData.Size = new Size(130, 33);
            this.btnDeleteTestData.TabIndex = 73;
            this.btnDeleteTestData.Text = "Delete Test Data";
            this.btnDeleteTestData.UseVisualStyleBackColor = true;
            this.btnDeleteTestData.Click += new EventHandler(this.btnDeleteTestData_Click);
            this.btnEdit.Dock = DockStyle.Right;
            this.btnEdit.ForeColor = Color.Teal;
            this.btnEdit.Image = (Image)resources.GetObject("btnEdit.Image");
            this.btnEdit.ImageAlign = ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new Point(1640, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(130, 33);
            this.btnEdit.TabIndex = 72;
            this.btnEdit.Text = "       &Report Data";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.lblSystemMessage.BackColor = SystemColors.Control;
            this.lblSystemMessage.BorderStyle = BorderStyle.None;
            this.lblSystemMessage.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.lblSystemMessage.ForeColor = Color.DarkRed;
            this.lblSystemMessage.Location = new Point(16, 10);
            this.lblSystemMessage.Multiline = true;
            this.lblSystemMessage.Name = "lblSystemMessage";
            this.lblSystemMessage.Size = new Size(102, 20);
            this.lblSystemMessage.TabIndex = 71;
            this.lblSystemMessage.Text = "System Message:";
            this.btnStart.Dock = DockStyle.Right;
            this.btnStart.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnStart.ForeColor = Color.Teal;
            this.btnStart.Location = new Point(1770, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new Size(130, 33);
            this.btnStart.TabIndex = 70;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new EventHandler(this.btnStart_Click);
            this.tmrDisplayData.Interval = 50;
            this.tmrDisplayData.Tick += new EventHandler(this.tmrDisplayData_Tick);
            this.tmrDateTime.Enabled = true;
            this.tmrDateTime.Interval = 1000;
            this.tmrDateTime.Tick += new EventHandler(this.tmrDateTime_Tick);
            this.tmrRefreshChart.Interval = 10;
            this.tmrRefreshChart.Tick += new EventHandler(this.tmrRefreshChart_Tick);
            this.tmrA1DetectRemoveObject.Interval = 1000;
            this.tmrA1DetectRemoveObject.Tick += new EventHandler(this.tmrA1DetectRemoveObject_Tick);
            this.tmrA2DetectRemoveObject.Interval = 1000;
            this.tmrA2DetectRemoveObject.Tick += new EventHandler(this.tmrA2DetectRemoveObject_Tick);
            this.tmrEnableReadA1Data.Interval = 1000;
            this.tmrEnableReadA1Data.Tick += new EventHandler(this.tmrEnableReadA1Data_Tick);
            this.tmrEnableReadA2Data.Interval = 1000;
            this.tmrEnableReadA2Data.Tick += new EventHandler(this.tmrEnableReadA2Data_Tick);
            this.tmrRefreshDataGridView.Interval = 10;
            this.tmrRefreshDataGridView.Tick += new EventHandler(this.tmrRefreshDataGridView_Tick);
            this.serialPort1.BaudRate = 19200;
            this.serialPort1.PortName = "COM3";
            this.tmrDisplayJudge.Interval = 500;
            this.tmrDisplayJudge.Tick += new EventHandler(this.displayJudge_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(1904, 1002);
            base.Controls.Add(this.panel3);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.panelResult);
            base.Name = "frmMain";
            base.StartPosition = FormStartPosition.CenterScreen;
            base.FormClosed += new FormClosedEventHandler(this.Form1_FormClosed);
            base.Load += new EventHandler(this.Form1_Load);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((ISupportInitialize)this.chartA1).EndInit();
            ((ISupportInitialize)this.chartA2).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((ISupportInitialize)this.dataGridView1).EndInit();
            this.panel5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panelResult.ResumeLayout(false);
            this.panelResult.PerformLayout();
            base.ResumeLayout(false);
        }

        private void instructionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact to us via Phone No. (+84) 913.183.822 or Liemdtvt@gmail.com");
        }

        private void label13_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void loadData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("select ID, model, A1MaxValue, A1MinValue, A1Result, A2MaxValue, A2MinValue, A2Result, Date, Time, Judge, TotalProcessed, TotalPASS, TotalFAIL from (select top 21 * from (select CAST(substring(ID,3,10) as int) as NEWID, * from Data)A1 order by NEWID DESC)B1 order by NEWID", con));
                DataTable dt = new DataTable();
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
                Communication.totalProcessed = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1][11]);
                Communication.totalPASS = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1][12]);
                Communication.totalFAIL = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1][13]);
                this.txtTotalProcessed.Text = Communication.totalProcessed.ToString();
                this.txtTotalPass.Text = Communication.totalPASS.ToString();
                this.txtTotalFAIL.Text = Communication.totalFAIL.ToString();
                this.RowsColor();
                con.Close();
            }
            catch
            {
            }
        }

        private void loadProductSetting()
        {
            try
            {
                this.cmbModel.Text = null;
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("SELECT model FROM ProductSetting", con));
                DataSet dt = new DataSet();
                da.Fill(dt);
                this.cmbModel.DataSource = dt.Tables[0];
                this.cmbModel.ValueMember = "model";
                con.Close();
                this.SelectProductSetting();
            }
            catch
            {
            }
        }

        private void processReceivedData()
        {
            if ((Communication.serialData.Length != Communication.charNumberOfCom_data || !(Communication.serialData.Substring(0, 2) == "A1") ? false : Communication.serialData.Substring(14, 2) == "A2"))
            {
                Communication.A1MeasuredValue = Communication.serialData.Substring(3, 6);
                Communication.A1Result = Communication.serialData.Substring(11, 2);
                Communication.A2MeasuredValue = Communication.serialData.Substring(17, 6);
                Communication.A2Result = Communication.serialData.Substring(25, 2);
            }
        }

        private void RefreshMainForm()
        {
            try
            {
                if (!Communication.serialport.IsOpen)
                {
                    Communication.ConnectSerial(Communication.comPort, Communication.baudrate);
                }
            }
            catch
            {
                base.Show();
            }
            if (!Communication.serialport.IsOpen)
            {
                this.lblConnectStatus.Text = "Not Connected";
                this.lblConnectStatus.ForeColor = Color.Red;
            }
            else
            {
                this.lblConnectStatus.Text = "Connected";
                this.lblConnectStatus.ForeColor = Color.GreenYellow;
            }
        }

        public void RowsColor()
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Cells[4].Value.ToString().Trim() == "NG")
                {
                    this.dataGridView1.Rows[i].Cells[4].Style.ForeColor = Color.Red;
                }
                if (this.dataGridView1.Rows[i].Cells[7].Value.ToString().Trim() == "NG")
                {
                    this.dataGridView1.Rows[i].Cells[7].Style.ForeColor = Color.Red;
                }
                if (this.dataGridView1.Rows[i].Cells[10].Value.ToString().Trim() != "FAIL")
                {
                    this.dataGridView1.Rows[i].Cells[10].Style.ForeColor = Color.Green;
                }
                else
                {
                    this.dataGridView1.Rows[i].Cells[10].Style.ForeColor = Color.Red;
                }
            }
        }

        private void saveA1BufferData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                string add = string.Concat(new string[] { "INSERT INTO A1BufferData (A1MaxValue, A1MinValue, A1Result) VALUES ('", Communication.A1MaximumValue, "','", Communication.A1MinimumValue, "','", Communication.A1Result, "')" });
                SqlCommand cmd_saveData = new SqlCommand()
                {
                    Connection = con,
                    CommandText = add
                };
                cmd_saveData.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
            }
        }

        private void saveA2BufferData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                string add = string.Concat(new string[] { "INSERT INTO A2BufferData (A2MaxValue, A2MinValue, A2Result) VALUES ('", Communication.A2MaximumValue, "','", Communication.A2MinimumValue, "','", Communication.A2Result, "')" });
                SqlCommand cmd_saveData = new SqlCommand()
                {
                    Connection = con,
                    CommandText = add
                };
                cmd_saveData.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
            }
        }

        private void saveData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                string add = string.Concat(new object[] { "INSERT INTO Data (ID, model, A1MaxValue, A1MinValue, A1Result, A2MaxValue, A2MinValue, A2Result, Date, Time, Judge, TotalProcessed, TotalPASS, TotalFAIL) VALUES ('", Communication.ID, "','", Communication.model, "','", Communication.A1MaximumValue, "','", Communication.A1MinimumValue, "','", Communication.A1Result, "','", Communication.A2MaximumValue, "','", Communication.A2MinimumValue, "','", Communication.A2Result, "','", Communication.Date, "','", Communication.Time, "','", Communication.Judge, "','", Communication.totalProcessed, "','", Communication.totalPASS, "','", Communication.totalFAIL, "')" });
                SqlCommand cmd_saveData = new SqlCommand()
                {
                    Connection = con,
                    CommandText = add
                };
                cmd_saveData.ExecuteNonQuery();
                con.Close();
                con.Dispose();
            }
            catch
            {
            }
        }

        private void SelectProductSetting()
        {
            this.txtA1DetectionLevel.Text = null;
            this.txtA2DetectionLevel.Text = null;
            this.txtA1MinimumOffset.Text = null;
            this.txtA1MaximumOffset.Text = null;
            this.txtA2MinimumOffset.Text = null;
            this.txtA2MaximumOffset.Text = null;
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlCommand cmdProductSelect = new SqlCommand(string.Concat("SELECT * FROM ProductSetting WHERE model ='", this.cmbModel.Text, "'"), con);
                SqlDataReader dt = cmdProductSelect.ExecuteReader();
                while (dt.Read())
                {
                    Communication.model = dt["model"].ToString().Trim();
                    Communication.A1DetectionLevel = dt["A1DetectionValue"].ToString().Trim();
                    Communication.A2DetectionLevel = dt["A2DetectionValue"].ToString().Trim();
                    Communication.A1MinimumOffset = dt["A1MinimumOffset"].ToString().Trim();
                    Communication.A1MaximumOffset = dt["A1MaximumOffset"].ToString().Trim();
                    Communication.A2MinimumOffset = dt["A2MinimumOffset"].ToString().Trim();
                    Communication.A2MaximumOffset = dt["A2MaximumOffset"].ToString().Trim();
                    this.txtA1DetectionLevel.Text = dt["A1DetectionValue"].ToString().Trim();
                    this.txtA2DetectionLevel.Text = dt["A2DetectionValue"].ToString().Trim();
                    this.txtA1MinimumOffset.Text = dt["A1MinimumOffset"].ToString().Trim();
                    this.txtA1MaximumOffset.Text = dt["A1MaximumOffset"].ToString().Trim();
                    this.txtA2MinimumOffset.Text = dt["A2MinimumOffset"].ToString().Trim();
                    this.txtA2MaximumOffset.Text = dt["A2MaximumOffset"].ToString().Trim();
                }
                con.Close();
            }
            catch
            {
            }
        }

        public void SetText(string text)
        {
            bool flag;
            if (base.InvokeRequired)
            {
                try
                {
                    frmMain.SetTextCallback d = new frmMain.SetTextCallback(this.SetText);
                    base.Invoke(d, new object[] { text });
                }
                catch (InvalidOperationException invalidOperationException)
                {
                }
            }
            else if (Communication.start)
            {
                if (this.InputData.Length >= Communication.charNumberOfCom_data)
                {
                    this.charNumberOfFirstString = this.InputData.IndexOf("A1");
                    if (this.charNumberOfFirstString <= 0)
                    {
                        this.charNumberOfFirstString = 0;
                    }
                    else
                    {
                        this.fistSubString = this.InputData.Substring(0, this.charNumberOfFirstString);
                    }
                    if (this.InputData.Length >= this.charNumberOfFirstString + Communication.charNumberOfCom_data)
                    {
                        Communication.serialData = this.InputData.Substring(this.charNumberOfFirstString, Communication.charNumberOfCom_data);
                        if (Communication.serialData.Length != Communication.charNumberOfCom_data || !(Communication.serialData.Substring(0, 2) == "A1") || !(Communication.serialData.Substring(14, 2) == "A2") || !(Communication.serialData.Substring(11, 2) == "OK") && !(Communication.serialData.Substring(11, 2) == "NG"))
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = (Communication.serialData.Substring(25, 2) == "OK" ? true : Communication.serialData.Substring(25, 2) == "NG");
                        }
                        if (flag)
                        {
                            Communication.enableReadData = true;
                            this.tmrDisplayData.Enabled = true;
                        }
                        this.charNumberOfLastString = this.InputData.Length - this.charNumberOfFirstString - Communication.charNumberOfCom_data;
                        this.lastSubString = this.InputData.Substring(this.InputData.Length - this.charNumberOfLastString, this.charNumberOfLastString);
                        this.InputData = string.Concat(this.fistSubString, this.lastSubString);
                    }
                }
            }
        }

        private void testCycleFinish()
        {
            if ((!Communication.A1EnableSave ? false : Communication.A2EnableSave))
            {
                if ((this.txtA1Result.Text != "OK" ? true : this.txtA2Result.Text != "OK"))
                {
                    Communication.Judge = "FAIL";
                    this.btnJudge.ForeColor = Color.Red;
                    Communication.totalFAIL++;
                    this.txtTotalFAIL.Text = Communication.totalFAIL.ToString();
                }
                else
                {
                    Communication.Judge = "PASS";
                    this.btnJudge.ForeColor = Color.ForestGreen;
                    Communication.totalPASS++;
                    this.txtTotalPass.Text = Communication.totalPASS.ToString();
                }
                Communication.A1Result = this.txtA1Result.Text;
                Communication.A2Result = this.txtA2Result.Text;
                this.btnJudge.Text = Communication.Judge;
                Communication.totalProcessed++;
                this.txtTotalProcessed.Text = Communication.totalProcessed.ToString();
                Communication.ID = string.Concat("HL", Communication.totalProcessed);
                this.saveData();
                Communication.A1EnableSave = false;
                Communication.A2EnableSave = false;
                Communication.A1MaximumValue = null;
                Communication.A1MinimumValue = null;
                Communication.A2MaximumValue = null;
                Communication.A2MinimumValue = null;
                this.txtA1MaximumValue.Text = null;
                this.txtA1MinimumValue.Text = null;
                this.txtA1Result.Text = null;
                this.txtA2MaximumValue.Text = null;
                this.txtA2MinimumValue.Text = null;
                this.txtA2Result.Text = null;
                this.chartA1.Series.Clear();
                this.chartA1Setting();
                this.chartA2.Series.Clear();
                this.chartA2Setting();
                this.loadData();
            }
            if ((Communication.A1Detected ? false : !Communication.A2Detected))
            {
                this.txtSystemMessage.Text = "None Object Detected!";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void tmrA1DetectRemoveObject_Tick(object sender, EventArgs e)
        {
        }

        private void tmrA2DetectRemoveObject_Tick(object sender, EventArgs e)
        {
        }

        private void tmrConnectionStatus_Tick(object sender, EventArgs e)
        {
            if (!Communication.serialport.IsOpen)
            {
                this.lblConnectStatus.Text = "Not Connected";
                this.lblConnectStatus.ForeColor = Color.Red;
            }
            else
            {
                this.lblConnectStatus.Text = "Connected";
                this.lblConnectStatus.ForeColor = Color.GreenYellow;
            }
            if ((Communication.serialport.IsOpen ? false : Communication.AutoReconnect))
            {
                try
                {
                    if (Communication.ConnectSerial(Communication.comPort, Communication.baudrate))
                    {
                        this.lblConnectStatus.Text = "Connected";
                        this.lblConnectStatus.ForeColor = Color.GreenYellow;
                    }
                }
                catch
                {
                }
            }
            if (this.cmbTimeToEnableRead.Text == "0.5")
            {
                this.tmrEnableReadA1Data.Interval = 500;
                this.tmrEnableReadA2Data.Interval = 500;
            }
            if (this.cmbTimeToEnableRead.Text == "1")
            {
                this.tmrEnableReadA1Data.Interval = 1000;
                this.tmrEnableReadA2Data.Interval = 1000;
            }
            if (this.cmbTimeToEnableRead.Text == "1.5")
            {
                this.tmrEnableReadA1Data.Interval = 1500;
                this.tmrEnableReadA2Data.Interval = 1500;
            }
            if (this.cmbTimeToEnableRead.Text == "2")
            {
                this.tmrEnableReadA1Data.Interval = 2000;
                this.tmrEnableReadA2Data.Interval = 2000;
            }
            if (this.cmbTimeToEnableRead.Text == "2.5")
            {
                this.tmrEnableReadA1Data.Interval = 2500;
                this.tmrEnableReadA2Data.Interval = 2500;
            }
            if (this.cmbTimeToEnableRead.Text == "3")
            {
                this.tmrEnableReadA1Data.Interval = 3000;
                this.tmrEnableReadA2Data.Interval = 3000;
            }
        }

        private void tmrDateTime_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            Communication.Date = now.ToString("yyyy-MM-dd");
            now = DateTime.Now;
            Communication.Time = now.ToString().Substring(10, 9).Trim();
            this.txtDate.Text = Communication.Date;
            this.txtTime.Text = Communication.Time;
        }

        private void tmrDisplayData_Tick(object sender, EventArgs e)
        {
            bool flag;
            bool flag1;
            bool flag2;
            this.tmrDisplayData.Enabled = false;
            if (Communication.subformIsOpen)
            {
                this.txtA1MaximumValue.Text = null;
                this.txtA1MinimumValue.Text = null;
                this.txtA1Result.Text = null;
                this.txtA2MaximumValue.Text = null;
                this.txtA2MinimumValue.Text = null;
                this.txtA2Result.Text = null;
            }
            else
            {
                if (Communication.serialData.Length != Communication.charNumberOfCom_data || !(Communication.serialData.Substring(0, 2) == "A1") || !(Communication.serialData.Substring(14, 2) == "A2") || !(Communication.serialData.Substring(11, 2) == "OK") && !(Communication.serialData.Substring(11, 2) == "NG"))
                {
                    flag = false;
                }
                else
                {
                    flag = (Communication.serialData.Substring(25, 2) == "OK" ? true : Communication.serialData.Substring(25, 2) == "NG");
                }
                if (flag)
                {
                    Communication.A1MeasuredValue = Communication.serialData.Substring(3, 6);
                    Communication.A1Result = Communication.serialData.Substring(11, 2);
                    if (float.Parse(Communication.A1MeasuredValue) < float.Parse(Communication.A1DetectionLevel) - Communication.detectionOffset)
                    {
                        this.tmrEnableReadA1Data.Enabled = true;
                        if ((Communication.A1enableStopTest ? false : Communication.A1RecevingData))
                        {
                            if (!this.tmrA1DetectRemoveObject.Enabled)
                            {
                                this.tmrA1DetectRemoveObject.Enabled = true;
                            }
                            Communication.A1Detected = true;
                            if ((!Communication.A1Detected ? true : !Communication.A2Detected))
                            {
                                this.txtSystemMessage.Text = "A1 Detected!";
                            }
                            else
                            {
                                this.txtSystemMessage.Text = "A1 + A2 Detected!";
                            }
                            if (Communication.A1MaximumValue == null)
                            {
                                Communication.A1MaximumValue = Communication.A1MeasuredValue;
                                this.txtA1MaximumValue.Text = Communication.A1MaximumValue;
                            }
                            if (Communication.A1MinimumValue == null)
                            {
                                Communication.A1MinimumValue = Communication.A1MeasuredValue;
                                this.txtA1MinimumValue.Text = Communication.A1MinimumValue;
                            }
                            if (float.Parse(Communication.A1MaximumValue) <= float.Parse(Communication.A1MeasuredValue))
                            {
                                Communication.A1MaximumValue = Communication.A1MeasuredValue;
                                this.txtA1MaximumValue.Text = Communication.A1MaximumValue;
                            }
                            if (float.Parse(Communication.A1MinimumValue) >= float.Parse(Communication.A1MeasuredValue))
                            {
                                Communication.A1MinimumValue = Communication.A1MeasuredValue;
                                this.txtA1MinimumValue.Text = Communication.A1MinimumValue;
                            }
                            this.saveA1BufferData();
                            this.chartA1Display();
                            if (Communication.enableClearData)
                            {
                                Communication.enableClearData = false;
                                this.chartA1.Series.Clear();
                                this.chartA1Setting();
                                this.chartA2.Series.Clear();
                                this.chartA2Setting();
                                this.txtA1Result.Text = "";
                                this.txtA2Result.Text = "";
                                this.btnJudge.Text = "";
                                this.controlAlarm_A1ResetAlarm();
                                this.controlAlarm_A2ResetAlarm();
                            }
                        }
                    }
                    else if ((!Communication.A1Detected ? true : !Communication.A1RecevingData))
                    {
                        Communication.A1Detected = false;
                    }
                    else
                    {
                        this.getA1BufferData();
                        this.deleteA1BufferData();
                        Communication.A1Detected = false;
                        Communication.A1EnableSave = true;
                        Communication.A1enableStopTest = false;
                        this.tmrEnableReadA1Data.Enabled = false;
                        Communication.A1RecevingData = false;
                    }
                }
                if (Communication.serialData.Length != Communication.charNumberOfCom_data || !(Communication.serialData.Substring(0, 2) == "A1") || !(Communication.serialData.Substring(14, 2) == "A2") || !(Communication.serialData.Substring(11, 2) == "OK") && !(Communication.serialData.Substring(11, 2) == "NG"))
                {
                    flag1 = false;
                }
                else
                {
                    flag1 = (Communication.serialData.Substring(25, 2) == "OK" ? true : Communication.serialData.Substring(25, 2) == "NG");
                }
                if (flag1)
                {
                    Communication.A2MeasuredValue = Communication.serialData.Substring(17, 6);
                    Communication.A2Result = Communication.serialData.Substring(25, 2);
                    if (float.Parse(Communication.A2MeasuredValue) < float.Parse(Communication.A2DetectionLevel) - Communication.detectionOffset)
                    {
                        this.tmrEnableReadA2Data.Enabled = true;
                        if ((Communication.A2enableStopTest ? false : Communication.A2RecevingData))
                        {
                            if (!this.tmrA2DetectRemoveObject.Enabled)
                            {
                                this.tmrA2DetectRemoveObject.Enabled = true;
                            }
                            Communication.A2Detected = true;
                            if ((!Communication.A1Detected ? true : !Communication.A2Detected))
                            {
                                this.txtSystemMessage.Text = "A2 Detected!";
                            }
                            else
                            {
                                this.txtSystemMessage.Text = "A1 + A2 Detected!";
                            }
                            if (Communication.A2MaximumValue == null)
                            {
                                Communication.A2MaximumValue = Communication.A2MeasuredValue;
                                this.txtA2MaximumValue.Text = Communication.A2MaximumValue;
                            }
                            if (Communication.A2MinimumValue == null)
                            {
                                Communication.A2MinimumValue = Communication.A2MeasuredValue;
                                this.txtA2MinimumValue.Text = Communication.A2MinimumValue;
                            }
                            if (float.Parse(Communication.A2MaximumValue) <= float.Parse(Communication.A2MeasuredValue))
                            {
                                Communication.A2MaximumValue = Communication.A2MeasuredValue;
                                this.txtA2MaximumValue.Text = Communication.A2MaximumValue;
                            }
                            if (float.Parse(Communication.A2MinimumValue) >= float.Parse(Communication.A2MeasuredValue))
                            {
                                Communication.A2MinimumValue = Communication.A2MeasuredValue;
                                this.txtA2MinimumValue.Text = Communication.A2MinimumValue;
                            }
                            this.saveA2BufferData();
                            this.chartA2Display();
                            if (Communication.enableClearData)
                            {
                                Communication.enableClearData = false;
                                this.chartA1.Series.Clear();
                                this.chartA1Setting();
                                this.chartA2.Series.Clear();
                                this.chartA2Setting();
                                this.txtA1Result.Text = "";
                                this.txtA2Result.Text = "";
                                this.btnJudge.Text = "";
                                this.controlAlarm_A1ResetAlarm();
                                this.controlAlarm_A2ResetAlarm();
                            }
                        }
                    }
                    else if ((!Communication.A2Detected ? true : !Communication.A2RecevingData))
                    {
                        Communication.A2Detected = false;
                    }
                    else
                    {
                        this.getA2BufferData();
                        this.deleteA2BufferData();
                        Communication.A2Detected = false;
                        Communication.A2EnableSave = true;
                        Communication.A2enableStopTest = false;
                        this.tmrEnableReadA2Data.Enabled = false;
                        Communication.A2RecevingData = false;
                    }
                }
                if (!Communication.A1EnableSave || !Communication.A2EnableSave || Communication.A1Detected || Communication.A2Detected || !(this.txtA1Result.Text == "OK") && !(this.txtA1Result.Text == "NG"))
                {
                    flag2 = false;
                }
                else
                {
                    flag2 = (this.txtA2Result.Text == "OK" ? true : this.txtA2Result.Text == "NG");
                }
                if (flag2)
                {
                    if ((this.txtA1Result.Text != "OK" ? true : this.txtA2Result.Text != "OK"))
                    {
                        Communication.Judge = "FAIL";
                        this.btnJudge.ForeColor = Color.Red;
                        Communication.totalFAIL++;
                        this.txtTotalFAIL.Text = Communication.totalFAIL.ToString();
                    }
                    else
                    {
                        Communication.Judge = "PASS";
                        this.btnJudge.ForeColor = Color.ForestGreen;
                        Communication.totalPASS++;
                        this.txtTotalPass.Text = Communication.totalPASS.ToString();
                    }
                    Communication.A1Result = this.txtA1Result.Text;
                    Communication.A2Result = this.txtA2Result.Text;
                    this.tmrDisplayJudge.Enabled = true;
                    Communication.totalProcessed++;
                    this.txtTotalProcessed.Text = Communication.totalProcessed.ToString();
                    Communication.ID = string.Concat("HL", Communication.totalProcessed);
                    this.saveData();
                    Communication.A1EnableSave = false;
                    Communication.A2EnableSave = false;
                    Communication.A1MaximumValue = null;
                    Communication.A1MinimumValue = null;
                    Communication.A2MaximumValue = null;
                    Communication.A2MinimumValue = null;
                    Communication.enableClearData = true;
                    this.loadData();
                    this.tmrEnableReadA1Data.Enabled = false;
                    this.tmrEnableReadA2Data.Enabled = false;
                    this.calculatePPandPPKvalue();
                }
                if ((Communication.A1Detected ? false : !Communication.A2Detected))
                {
                    this.txtSystemMessage.Text = "None Object Detected!";
                }
            }
        }

        private void tmrEnableReadA1Data_Tick(object sender, EventArgs e)
        {
            this.tmrEnableReadA1Data.Enabled = false;
            Communication.A1RecevingData = true;
        }

        private void tmrEnableReadA2Data_Tick(object sender, EventArgs e)
        {
            this.tmrEnableReadA2Data.Enabled = false;
            Communication.A2RecevingData = true;
        }

        private void tmrRefreshChart_Tick(object sender, EventArgs e)
        {
        }

        private void tmrRefreshDataGridView_Tick(object sender, EventArgs e)
        {
            if (Communication.refreshDataGridView)
            {
                Communication.refreshDataGridView = false;
                this.tmrRefreshDataGridView.Enabled = false;
                this.loadData();
            }
        }

        public class deleteData
        {
            public static string ID;

            public static string model;

            public static string A1MaximumValue;

            public static string A1MinimumValue;

            public static string A1Result;

            public static string A2MaximumValue;

            public static string A2MinimumValue;

            public static string A2Result;

            public static string Date;

            public static string Time;

            public static string Judge;

            public static int totalProcessed;

            public static int totalPASS;

            public static int totalFAIL;

            public deleteData()
            {
            }
        }

        private delegate void SetTextCallback(string text);
    }
}