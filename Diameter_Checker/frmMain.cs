using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Diameter_Checker
{
    public class frmMain : Form
    {
        private string InputData = string.Empty;
        private string InputData2 = string.Empty;
        public static string strgetProcessorID;
        private string fistSubString;
        private string fistSubString2;
        private int charNumberOfFirstString;
        private int charNumberOfFirstString2;
        private string lastSubString;
        private string lastSubString2;
        private int charNumberOfLastString;
        private int charNumberOfLastString2;
        private static int i;
        private static int j;
        private static int k;
        private static int l;
        private static int m;
        private static int rowIndex;
        private IContainer components = null;
        private Panel panel3;
        private Panel panel6;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem MenuConfig;
        private ToolStripMenuItem communicatiomToolStripMenuItem;
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
        private Panel panel4;
        private Label label1;
        private TextBox txtA1DetectionLevel;
        private TextBox txtA2DetectionLevel;
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
        private TextBox txtA2PP;
        private TextBox txtA2Result;
        private Timer tmrDisplayData;
        private Button button2;
        private TextBox txtSystemMessage;
        private Timer tmrDateTime;
        private DataGridView dataGridView1;
        private Chart chartA2;
        private Chart chartA1;
        private Timer tmrRefreshChart;
        private CheckBox chkStopScan;
        private Timer tmrA1DetectRemoveObject;
        private Timer tmrA2DetectRemoveObject;
        private Timer tmrEnableReadA1Data;
        private Timer tmrEnableReadA2Data;
        private ComboBox cmbTimeToEnableRead;
        private Label label20;
        private Timer tmrRefreshDataGridView;
        private SerialPort serialPort2;
        private GroupBox groupBox8;
        private Label label22;
        private Button btnCntProduct;
        private NumericUpDown numProductInSet;
        private TextBox txtWeight;
        private Label label23;
        private Label label24;
        private Label lblConnectStatus2;
        private Label lblStatus2;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn model;
        private DataGridViewTextBoxColumn Weight;
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
        private Label label25;
        private Label label26;
        private Label lblDateTime;
        private TextBox txtWeightResult;
        private GroupBox groupBox6;
        private TextBox txtWeightMin;
        private TextBox txtWeightMax;
        private Chart chartWeight;
        private DateTimePicker dateTimeFilter;
        private TextBox txtA1PPK;
        private Label label27;
        private TextBox txtA1PP;
        private Label label28;
        private Label label14;
        private TextBox txtWeightPPK;
        private TextBox txtWeightPP;
        private Label label13;
        private Label label29;
        private CheckBox checkBoxFilterDate;
        private Chart chart2;
        private Chart chart1;
        private Timer tmrDisplayJudge;

        static frmMain()
        {
            frmMain.i = 0;
            frmMain.j = 0;
            frmMain.k = 0;
            frmMain.l = 0; 
            frmMain.m = 0;

        }

        public frmMain()
        {
            this.InitializeComponent();
            Communication.serialport.DataReceived += new SerialDataReceivedEventHandler(this.DataReceive);
            Communication.serialport2.DataReceived += new SerialDataReceivedEventHandler(this.DataReceive2);
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
                Communication.Weight = "0";
                this.controlAlarm_A1ResetAlarm();
                this.controlAlarm_A2ResetAlarm();
                this.txtA1MaximumValue.Text = null;
                this.txtA1MinimumValue.Text = null;
                this.txtA1Result.Text = null;
                this.txtA2MaximumValue.Text = null;
                this.txtA2MinimumValue.Text = null;
                this.txtA2Result.Text = null;
                this.txtWeight.Text = null;
                this.txtWeightResult.Text = null;
                this.chart1.Series.Clear();
                this.chart1Setting();
                this.chart2.Series.Clear();
                this.chart2Setting();
                //this.chartWeight.Series.Clear();
                //this.chartWeightSetting();
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
            ExportData child = new ExportData();
            child.ShowDialog();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This action is only accepted with the engineer!", "WARNING!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if ((this.cmbModel.Text == null || this.txtA1DetectionLevel.Text.Length != 6 || this.txtA2DetectionLevel.Text.Length != 6 || this.txtA1MinimumOffset.Text.Length != 6 || this.txtA1MaximumOffset.Text.Length != 6 || this.txtA2MinimumOffset.Text.Length != 6 || string.IsNullOrEmpty(txtWeightMax.Text) || string.IsNullOrEmpty(txtWeightMin.Text) ? false : this.txtA2MaximumOffset.Text.Length == 6))
                {
                    SqlConnection con = new SqlConnection(Communication.con_string);
                    con.Open();
                    string add = string.Concat(new string[] { "INSERT INTO ProductSetting (model, A1DetectionValue, A2DetectionValue, A1MaximumOffset, A1MinimumOffset, A2MaximumOffset, A2MinimumOffset, maxWeight, minWeight) VALUES ('", this.cmbModel.Text, "','", this.txtA1DetectionLevel.Text, "','", this.txtA2DetectionLevel.Text, "','", this.txtA1MaximumOffset.Text, "','", this.txtA1MinimumOffset.Text, "','", this.txtA2MaximumOffset.Text, "','", this.txtA2MinimumOffset.Text, "','", this.txtWeightMax.Text, "','", this.txtWeightMin.Text, "')" });
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
                this.numProductInSet.Enabled = true;
                Communication.start = false;
                Communication.stop = true;
                Communication.enableReceiveData = false;
            }
            else
            {
                this.btnStart.Text = "Stop";
                this.numProductInSet.Enabled = false;
                this.btnStart.ForeColor = Color.DarkRed;
                this.txtSystemMessage.Text = "Working mode";
                Communication.start = true;
                Communication.stop = false;
                Communication.enableReceiveData = true;
                Communication.cntProductInSet = 0;
                this.btnCntProduct.Text = "0";
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
            Random rnd = new Random(); // for testing

            //A1
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlDataAdapter adapterGetAverage = new SqlDataAdapter(new SqlCommand("SELECT AVG(CAST(A1MaxValue as float)), AVG(CAST(A1MinValue as float)), AVG(CAST(A2MaxValue as float)), AVG(CAST(A2MinValue as float))  FROM Data where Display = 1", con));
                DataTable dataTableGetAverage = new DataTable();
                adapterGetAverage.Fill(dataTableGetAverage);
                single = (float.Parse(dataTableGetAverage.Rows[0][0].ToString()) + float.Parse(dataTableGetAverage.Rows[0][1].ToString())) / 2f * 1000f;
                Communication.A1Average = single.ToString();
                SqlCommand cmd_LoadAllValue = new SqlCommand(string.Concat("SELECT A1MaxValue, A1MinValue, A2MaxValue, A2MinValue FROM Data Where model='", this.cmbModel.Text, "' and Display = 1"), con);
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
                    Communication.A1PPK = Communication.A1PPL;
                }
                else
                {
                    this.txtA1PPK.Text = Communication.A1PPU.ToString().Substring(0, 10);
                    Communication.A1PPK = Communication.A1PPU;
                }


                con.Close();
            }
            catch
            {
                Communication.A1PPK = 0;
                this.txtA1PPK.Text = "0";
            }

            //A2
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlDataAdapter adapterGetAverage = new SqlDataAdapter(new SqlCommand("SELECT AVG(CAST(A1MaxValue as float)), AVG(CAST(A1MinValue as float)), AVG(CAST(A2MaxValue as float)), AVG(CAST(A2MinValue as float))  FROM Data Where Display = 1", con));
                DataTable dataTableGetAverage = new DataTable();
                adapterGetAverage.Fill(dataTableGetAverage);
                single = (float.Parse(dataTableGetAverage.Rows[0][2].ToString()) + float.Parse(dataTableGetAverage.Rows[0][3].ToString())) / 2f * 1000f;
                Communication.A2Average = single.ToString();
                SqlCommand cmd_LoadAllValue = new SqlCommand(string.Concat("SELECT A1MaxValue, A1MinValue, A2MaxValue, A2MinValue FROM Data Where model='", this.cmbModel.Text, "' and Display = 1"), con);
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
                    Communication.A2PPK = Communication.A2PPL;
                }
                else
                {
                    this.txtA2PPK.Text = Communication.A2PPU.ToString().Substring(0, 10);
                    Communication.A2PPK = Communication.A2PPU;
                }
                con.Close();
            }
            catch
            {
                Communication.A2PPK = 0;
                this.txtA2PPK.Text = "0";
            }

            //Weight
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                SqlDataAdapter adapterGetAverage = new SqlDataAdapter(new SqlCommand("SELECT AVG(CAST(weight as float))  FROM Data where Display = 1", con));
                DataTable dataTableGetAverage = new DataTable();
                adapterGetAverage.Fill(dataTableGetAverage);
                int decimalIndex = dataTableGetAverage.Rows[0][0].ToString().IndexOf('.');
                single = 0;
                if(dataTableGetAverage.Rows[0][0] != null)
                {
                    single = float.Parse(dataTableGetAverage.Rows[0][0].ToString());
                }
                Communication.WeightAverage = single.ToString();

                SqlCommand cmd_LoadAllValue = new SqlCommand(string.Concat("SELECT weight FROM Data Where model='", this.cmbModel.Text, "' and Display = 1"), con);
                SqlDataAdapter adapterLoadAllValue = new SqlDataAdapter(cmd_LoadAllValue);
                DataTable dataTableLoadAllValue = new DataTable();
                adapterLoadAllValue.Fill(dataTableLoadAllValue);
                Communication.WeightSD = 0;
                double b = 0;
                frmMain.rowIndex = 0;
                while (frmMain.rowIndex <= dataTableLoadAllValue.Rows.Count - 1)
                {
                    float a = float.Parse(dataTableLoadAllValue.Rows[frmMain.rowIndex][0].ToString().Replace(".", ","));
                    b = single;
                    if(a-b != 0)
                    {

                    Communication.WeightSD += Math.Pow(a-b, 2);
                    }
                    frmMain.rowIndex++;
                }
                if(Communication.WeightSD > 0)
                {
                    Communication.WeightSD = Math.Sqrt(Communication.WeightSD / (double)(frmMain.rowIndex - 1));
                }
                num = (float.Parse(Communication.maxWeight) - float.Parse(Communication.minWeight)) / (6 * Communication.WeightSD);
                Communication.WeightPP = num.ToString();
                Communication.WeightPP = num.ToString();
                if(Communication.WeightPP.Length > 10)
                {
                    this.txtWeightPP.Text = Communication.WeightPP.Substring(0, 10);
                }
                Communication.WeightPPU = (float.Parse(Communication.maxWeight) - single) / (3 * Communication.WeightSD);
                Communication.WeightPPL = (single - double.Parse(Communication.minWeight)) / (3 * Communication.WeightSD);
                if (Communication.WeightPPU >= Communication.WeightPPL)
                {
                    //int len = Communication.WeightPPL.ToString().Length > 10 ? 10 : Communication.WeightPPL.ToString().Length;
                    //this.txtWeightPPK.Text = Communication.WeightPPL.ToString().Substring(0, len);
                    Communication.WeightPPK = Communication.WeightPPL;
                }
                else
                {
                    //int len = (Communication.WeightPPU.ToString().Length > 10) ? 10 : Communication.WeightPPU.ToString().Length;
                    //this.txtWeightPPK.Text = Communication.WeightPPU.ToString().Substring(0, len);
                    Communication.WeightPPK = Communication.WeightPPU;
                }
                if (Double.IsInfinity(Communication.WeightPPK))
                {
                    Communication.WeightPPK = 0;
                }
                this.txtWeightPPK.Text = Communication.WeightPPK.ToString();
                con.Close();
            }
            catch(Exception ex)
            {
                Communication.WeightPPK = 0;
                this.txtWeightPPK.Text = "0";
            }

            chartA1Display();
            chartA2Display();
            chartWDisplay();
        }

        private void chartA1Display()
        {
            int num = frmMain.i;
            //DataPointCollection points = this.chartA1.Series["A1 Max PPK"].Points;
            ////frmMain.i = num + 1;
            //points.AddXY((double)num, Communication.MAX_PPK);

            DataPointCollection dataPointCollection = this.chartA1.Series["A1 Actual PPK"].Points;
            int num1 = frmMain.i;
            //frmMain.i = num1 + 1;
            dataPointCollection.AddXY((double)num1, Communication.A1PPK);

            DataPointCollection points1 = this.chartA1.Series["A1 Min PPK"].Points;
            int num2 = frmMain.i;
            points1.AddXY((double)num2, Communication.MIN_PPK);
            frmMain.i = num2 + 1;
        }

        private void chartA1Setting()
        {
            frmMain.i = 0;
            ChartArea chart1PPK = this.chartA1.ChartAreas[0];
            this.chartA1.Series.Clear();
            chart1PPK.AxisX.Minimum = 0;
            //chart1.AxisY.Maximum = Math.Floor((Communication.MAX_PPK+0.1f)*100)/100;
            chart1PPK.AxisY.Minimum = Math.Floor((Communication.MIN_PPK-0.1f)*100)/100;
            chart1PPK.AxisY.IntervalType = DateTimeIntervalType.Number;
            //this.chartA1.Series.Add("A1 Max PPK");
            //this.chartA1.Series["A1 Max PPK"].ChartType = SeriesChartType.Line;
            //this.chartA1.Series["A1 Max PPK"].Color = Color.Red;
            //this.chartA1.Series["A1 Max PPK"].BorderWidth = 3;

            this.chartA1.Series.Add("A1 Actual PPK");
            this.chartA1.Series["A1 Actual PPK"].ChartType = SeriesChartType.Line;
            this.chartA1.Series["A1 Actual PPK"].Color = Color.Blue;
            this.chartA1.Series["A1 Actual PPK"].BorderWidth = 3;

            this.chartA1.Series.Add("A1 Min PPK");
            this.chartA1.Series["A1 Min PPK"].ChartType = SeriesChartType.Line;
            this.chartA1.Series["A1 Min PPK"].Color = Color.Red;
            this.chartA1.Series["A1 Min PPK"].BorderWidth = 3;

            //DataPointCollection points = this.chartA1.Series["A1 Max PPK"].Points;
            //int num = frmMain.i;
            ////frmMain.i = num + 1;
            //points.AddXY((double)num, Communication.MAX_PPK);

            //DataPointCollection dataPointCollection = this.chartA1.Series["A1 Actual PPK"].Points;
            //int num1 = frmMain.i;
            ////frmMain.i = num1 + 1;
            //dataPointCollection.AddXY((double)num1, Communication.A1PPK);

            //DataPointCollection points1 = this.chartA1.Series["A1 Min PPK"].Points;
            //int num2 = frmMain.i;
            //points1.AddXY((double)num2, Communication.MIN_PPK);

            //frmMain.i = num2 + 1;
        }

        private void chartA2Display()
        {
            int num = frmMain.j;
            //DataPointCollection points = this.chartA2.Series["A2 Max PPK"].Points;
            ////frmMain.i = num + 1;
            //points.AddXY((double)num, Communication.MAX_PPK);

            DataPointCollection dataPointCollection = this.chartA2.Series["A2 Actual PPK"].Points;
            int num1 = frmMain.j;
            //frmMain.i = num1 + 1;
            dataPointCollection.AddXY((double)num1, Communication.A2PPK);

            DataPointCollection points1 = this.chartA2.Series["A2 Min PPK"].Points;
            int num2 = frmMain.j;
            points1.AddXY((double)num2, Communication.MIN_PPK);
            frmMain.j = num2 + 1;
        }

        private void chartA2Setting()
        {
            frmMain.j = 0;
            ChartArea chart2PPK = this.chartA2.ChartAreas[0];
            this.chartA2.Series.Clear();
            chart2PPK.AxisX.Minimum = 0;
            //chart2.AxisY.Maximum = Math.Floor((Communication.MAX_PPK + 0.1f) * 100) / 100;
            chart2PPK.AxisY.Minimum = Math.Floor((Communication.MIN_PPK-0.1f)*100)/100;
            chart2PPK.AxisY.IntervalType = DateTimeIntervalType.Number;
            //this.chartA2.Series.Add("A2 Max PPK");
            //this.chartA2.Series["A2 Max PPK"].ChartType = SeriesChartType.Line;
            //this.chartA2.Series["A2 Max PPK"].Color = Color.Red;
            //this.chartA2.Series["A2 Max PPK"].BorderWidth = 3;

            this.chartA2.Series.Add("A2 Actual PPK");
            this.chartA2.Series["A2 Actual PPK"].ChartType = SeriesChartType.Line;
            this.chartA2.Series["A2 Actual PPK"].Color = Color.Blue;
            this.chartA2.Series["A2 Actual PPK"].BorderWidth = 3;

            this.chartA2.Series.Add("A2 Min PPK");
            this.chartA2.Series["A2 Min PPK"].ChartType = SeriesChartType.Line;
            this.chartA2.Series["A2 Min PPK"].Color = Color.Red;
            this.chartA2.Series["A2 Min PPK"].BorderWidth = 3;
        }
        private void chartWDisplay()
        {
            int num = frmMain.k;
            //DataPointCollection points = this.chartWeight.Series["Weight Max PPK"].Points;
            //MessageBox.Show(String.Concat("k: ", frmMain.k));
            //points.AddXY((double)num, Communication.MAX_PPK);

            DataPointCollection dataPointCollection = this.chartWeight.Series["Weight Actual PPK"].Points;
            dataPointCollection.AddXY((double)num, Communication.WeightPPK);

            DataPointCollection points1 = this.chartWeight.Series["Weight Min PPK"].Points;
            points1.AddXY((double)num, Communication.MIN_PPK);
            frmMain.k++;

        }
        private void chartWeightSetting()
        {
            frmMain.k = 0;
            ChartArea chart3 = this.chartWeight.ChartAreas[0];
            this.chartWeight.Series.Clear();
            chart3.AxisX.Minimum = 0;
            //chart3.AxisY.Maximum = Math.Floor((Communication.MAX_PPK + 0.1f) * 100) / 100;
            chart3.AxisY.Minimum = Math.Floor((Communication.MIN_PPK-0.1f)*100)/100;
            chart3.AxisY.IntervalType = DateTimeIntervalType.Number;
            //this.chartWeight.Series.Add("Weight Max PPK");
            //this.chartWeight.Series["Weight Max PPK"].ChartType = SeriesChartType.Line;
            //this.chartWeight.Series["Weight Max PPK"].Color = Color.Red;
            //this.chartWeight.Series["Weight Max PPK"].BorderWidth = 3;

            this.chartWeight.Series.Add("Weight Actual PPK");
            this.chartWeight.Series["Weight Actual PPK"].ChartType = SeriesChartType.Line;
            this.chartWeight.Series["Weight Actual PPK"].Color = Color.Blue;
            this.chartWeight.Series["Weight Actual PPK"].BorderWidth = 3;

            this.chartWeight.Series.Add("Weight Min PPK");
            this.chartWeight.Series["Weight Min PPK"].ChartType = SeriesChartType.Line;
            this.chartWeight.Series["Weight Min PPK"].Color = Color.Red;
            this.chartWeight.Series["Weight Min PPK"].BorderWidth = 3;
        }

        private void chart1Display()
        {
            DataPointCollection points = this.chart1.Series["A1 Max Offset"].Points;
            int num = frmMain.l;
            frmMain.l = num + 1;
            points.AddXY((double)num, (double)(float.Parse(Communication.A1MaximumOffset.Replace(".", "")) / 1000f));
            DataPointCollection dataPointCollection = this.chart1.Series["A1 Measuring"].Points;
            int num1 = frmMain.l;
            frmMain.l = num1 + 1;
            dataPointCollection.AddXY((double)num1, (double)(float.Parse(Communication.A1MeasuredValue.Replace(".", "")) / 1000f));
            DataPointCollection points1 = this.chart1.Series["A1 Min Offset"].Points;
            int num2 = frmMain.l;
            frmMain.l = num2 + 1;
            points1.AddXY((double)num2, (double)(float.Parse(Communication.A1MinimumOffset.Replace(".", "")) / 1000f));
        }

        private void chart1Setting()
        {
            frmMain.l = 0;
            ChartArea chart1 = this.chart1.ChartAreas[0];
            this.chart1.Series.Clear();
            chart1.AxisX.Minimum = 0;
            chart1.AxisY.Maximum = (double)(float.Parse(Communication.A1MaximumOffset.Replace(".", "")) / 1000f) + 0.01;
            chart1.AxisY.Minimum = (double)(float.Parse(Communication.A1MinimumOffset.Replace(".", "")) / 1000f) - 0.01;
            chart1.AxisY.IntervalType = DateTimeIntervalType.Number;
            this.chart1.Series.Add("A1 Max Offset");
            this.chart1.Series["A1 Max Offset"].ChartType = SeriesChartType.Line;
            this.chart1.Series["A1 Max Offset"].Color = Color.Red;
            this.chart1.Series["A1 Max Offset"].BorderWidth = 3;
            this.chart1.Series.Add("A1 Measuring");
            this.chart1.Series["A1 Measuring"].ChartType = SeriesChartType.Line;
            this.chart1.Series["A1 Measuring"].Color = Color.Blue;
            this.chart1.Series["A1 Measuring"].BorderWidth = 3;
            this.chart1.Series.Add("A1 Min Offset");
            this.chart1.Series["A1 Min Offset"].ChartType = SeriesChartType.Line;
            this.chart1.Series["A1 Min Offset"].Color = Color.Red;
            this.chart1.Series["A1 Min Offset"].BorderWidth = 3;
            DataPointCollection points = this.chart1.Series["A1 Max Offset"].Points;
            int num = frmMain.l;
            frmMain.l = num + 1;
            points.AddXY((double)num, (double)(float.Parse(Communication.A1MaximumOffset.Replace(".", "")) / 1000f));
            DataPointCollection dataPointCollection = this.chart1.Series["A1 Measuring"].Points;
            int num1 = frmMain.l;
            frmMain.l = num1 + 1;
            dataPointCollection.AddXY((double)num1, (double)(float.Parse(Communication.A1MinimumOffset.Replace(".", "")) / 1000f));
            DataPointCollection points1 = this.chart1.Series["A1 Min Offset"].Points;
            int num2 = frmMain.l;
            frmMain.l = num2 + 1;
            points1.AddXY((double)num2, (double)(float.Parse(Communication.A1MinimumOffset.Replace(".", "")) / 1000f));
        }

        private void chart2Display()
        {
            DataPointCollection points = this.chart2.Series["A2 Max Offset"].Points;
            int num = frmMain.m;
            frmMain.m = num + 1;
            points.AddXY((double)num, (double)(float.Parse(Communication.A2MaximumOffset.Replace(".", "")) / 1000f));
            DataPointCollection dataPointCollection = this.chart2.Series["A2 Measuring"].Points;
            int num1 = frmMain.m;
            frmMain.m = num1 + 1;

            dataPointCollection.AddXY((double)num1, (double)(float.Parse(Communication.A2MeasuredValue.Replace(".", "")) / 1000f));
            DataPointCollection points1 = this.chart2.Series["A2 Min Offset"].Points;
            int num2 = frmMain.m;
            frmMain.m = num2 + 1;
            points1.AddXY((double)num2, (double)(float.Parse(Communication.A2MinimumOffset.Replace(".", "")) / 1000f));
        }

        private void chart2Setting()
        {
            frmMain.m = 0;
            ChartArea chart2 = this.chart2.ChartAreas[0];
            this.chart2.Series.Clear();
            chart2.AxisX.Minimum = 0;
            chart2.AxisY.Maximum = (double)(float.Parse(Communication.A2MaximumOffset.Replace(".", "")) / 1000f) + 0.01;
            chart2.AxisY.Minimum = (double)(float.Parse(Communication.A2MinimumOffset.Replace(".", "")) / 1000f) - 0.01;
            chart2.AxisY.IntervalType = DateTimeIntervalType.Number;
            this.chart2.Series.Add("A2 Max Offset");
            this.chart2.Series["A2 Max Offset"].ChartType = SeriesChartType.Line;
            this.chart2.Series["A2 Max Offset"].Color = Color.Red;
            this.chart2.Series["A2 Max Offset"].BorderWidth = 3;
            this.chart2.Series.Add("A2 Measuring");
            this.chart2.Series["A2 Measuring"].ChartType = SeriesChartType.Line;
            this.chart2.Series["A2 Measuring"].Color = Color.Blue;
            this.chart2.Series["A2 Measuring"].BorderWidth = 3;
            this.chart2.Series.Add("A2 Min Offset");
            this.chart2.Series["A2 Min Offset"].ChartType = SeriesChartType.Line;
            this.chart2.Series["A2 Min Offset"].Color = Color.Red;
            this.chart2.Series["A2 Min Offset"].BorderWidth = 3;
            DataPointCollection points = this.chart2.Series["A2 Max Offset"].Points;
            int num = frmMain.m;
            frmMain.m = num + 1;
            points.AddXY((double)num, (double)(float.Parse(Communication.A2MaximumOffset.Replace(".", "")) / 1000f));
            DataPointCollection dataPointCollection = this.chart2.Series["A2 Measuring"].Points;
            int num1 = frmMain.m;
            frmMain.m = num1 + 1;
            dataPointCollection.AddXY((double)num1, (double)(float.Parse(Communication.A2MinimumOffset.Replace(".", "")) / 1000f));
            DataPointCollection points1 = this.chart2.Series["A2 Min Offset"].Points;
            int num2 = frmMain.m;
            frmMain.m = num2 + 1;
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

        private void COM_Connect2()
        {
            try
            {
                Communication.serialport2.Close();
                if (Communication.ConnectSerial2(Communication.comPort2, Communication.baudrate2))
                {
                    this.lblConnectStatus2.Text = "Connected";
                    this.lblConnectStatus2.ForeColor = Color.Green;
                }
            }
            catch
            {
                MessageBox.Show("Failed! Please check your settings and try again!");
                this.lblConnectStatus2.Text = "Not Connected";
                this.lblConnectStatus2.ForeColor = Color.Red;
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
            //this.serialPort1.Write("2");s
            Communication.enableConnectToControlBox = false;
        }

        private void controlAlarm_A1SetAlarm()
        {
            Communication.enableConnectToControlBox = true;
            //this.serialPort1.Write("1");
            Communication.enableConnectToControlBox = false;
        }

        private void controlAlarm_A2ResetAlarm()
        {
            Communication.enableConnectToControlBox = true;
            //this.serialPort1.Write("4");
            Communication.enableConnectToControlBox = false;
        }

        private void controlAlarm_A2SetAlarm()
        {
            Communication.enableConnectToControlBox = true;
            //this.serialPort1.Write("3");
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

        public void DataReceive2(object obj, SerialDataReceivedEventArgs e)
        {
            if (Communication.enableReceiveData)
            {
                this.InputData2 = string.Concat(this.InputData2, Communication.serialport2.ReadExisting());
                this.InputData2 = this.InputData2.Replace("\r", string.Empty);
                this.InputData2 = this.InputData2.Replace("\n", string.Empty);
                Communication.test2++;
                //if (this.InputData2.Length >= Communication.charNumberOfCom_data2 )
                //{
                //    this.InputData2 = Communication.serialport2.ReadExisting();
                //}
                if (this.InputData2.Length >= Communication.charNumberOfCom_data2)
                {
                    this.SetText2(this.InputData2);
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
            this.btnCntProduct.Text = Communication.cntProductInSet.ToString();
            if (this.numProductInSet.Value == Communication.cntProductInSet)
            {
                MessageBox.Show("Đã đủ số lượng!");
            }
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
            //if (Convert.ToInt32(DateTime.Today.Year.ToString()) > 2023)
            //{
            //    MessageBox.Show("System Error!");
            //    base.Close();
            //}
            //this.serialPort1.Open();
            this.controlAlarm_A1ResetAlarm();
            this.controlAlarm_A2ResetAlarm();
            this.AdjustLayout();
            this.RefreshMainForm();
            Communication.load_ComSetting();
            this.loadProductSetting();
            this.loadData();
            this.COM_Connect();
            this.COM_Connect2();
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
            chartWeightSetting();
            chart1Setting();
            chart2Setting();
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = (new ManagementClass("win32_processor")).GetInstances().GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    ManagementObject managObj = (ManagementObject)enumerator.Current;
                    strgetProcessorID = managObj.Properties["processorID"].Value.ToString().Trim();
                }
            }
            if (strgetProcessorID != Communication.processorID1.Trim() && strgetProcessorID != Communication.processorID2.Trim() && strgetProcessorID != Communication.processorID3.Trim())
            {
                MessageBox.Show("System Error!", "WARNING!");
                base.Dispose();
            }
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.txtWeightPPK = new System.Windows.Forms.TextBox();
            this.txtWeightPP = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.txtA2PPK = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txtA1PPK = new System.Windows.Forms.TextBox();
            this.txtA2PP = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.txtWeightResult = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtA1PP = new System.Windows.Forms.TextBox();
            this.btnCntProduct = new System.Windows.Forms.Button();
            this.numProductInSet = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chartWeight = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartA1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartA2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtTotalProcessed = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtTotalFAIL = new System.Windows.Forms.TextBox();
            this.txtTotalPass = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btnJudge = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.A1MaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.A1MinValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.A1Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.A2MaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.A2MinValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.A2Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Judge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalProcessed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPASS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalFAIL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtA2Result = new System.Windows.Forms.TextBox();
            this.txtA1MinimumValue = new System.Windows.Forms.TextBox();
            this.txtA1MaximumValue = new System.Windows.Forms.TextBox();
            this.txtA1Result = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtA2MinimumValue = new System.Windows.Forms.TextBox();
            this.txtA2MaximumValue = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtWeightMin = new System.Windows.Forms.TextBox();
            this.txtWeightMax = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.cmbTimeToEnableRead = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtA2MaximumOffset = new System.Windows.Forms.TextBox();
            this.txtA1MaximumOffset = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtA2MinimumOffset = new System.Windows.Forms.TextBox();
            this.txtA1MinimumOffset = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtA2DetectionLevel = new System.Windows.Forms.TextBox();
            this.txtA1DetectionLevel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimeFilter = new System.Windows.Forms.DateTimePicker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.communicatiomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblConnectStatus2 = new System.Windows.Forms.Label();
            this.lblStatus2 = new System.Windows.Forms.Label();
            this.lblConnectStatus = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.checkBoxFilterDate = new System.Windows.Forms.CheckBox();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tmrConnectionStatus = new System.Windows.Forms.Timer(this.components);
            this.panelResult = new System.Windows.Forms.Panel();
            this.chkStopScan = new System.Windows.Forms.CheckBox();
            this.txtSystemMessage = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClearCurrentTest = new System.Windows.Forms.Button();
            this.btnDeleteTestData = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lblSystemMessage = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.tmrDisplayData = new System.Windows.Forms.Timer(this.components);
            this.tmrDateTime = new System.Windows.Forms.Timer(this.components);
            this.tmrRefreshChart = new System.Windows.Forms.Timer(this.components);
            this.tmrA1DetectRemoveObject = new System.Windows.Forms.Timer(this.components);
            this.tmrA2DetectRemoveObject = new System.Windows.Forms.Timer(this.components);
            this.tmrEnableReadA1Data = new System.Windows.Forms.Timer(this.components);
            this.tmrEnableReadA2Data = new System.Windows.Forms.Timer(this.components);
            this.tmrRefreshDataGridView = new System.Windows.Forms.Timer(this.components);
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.tmrDisplayJudge = new System.Windows.Forms.Timer(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProductInSet)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartA1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartA2)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1908, 771);
            this.panel3.TabIndex = 71;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel6.Controls.Add(this.label14);
            this.panel6.Controls.Add(this.txtWeightPPK);
            this.panel6.Controls.Add(this.txtWeightPP);
            this.panel6.Controls.Add(this.label13);
            this.panel6.Controls.Add(this.label29);
            this.panel6.Controls.Add(this.txtA2PPK);
            this.panel6.Controls.Add(this.label27);
            this.panel6.Controls.Add(this.txtA1PPK);
            this.panel6.Controls.Add(this.txtA2PP);
            this.panel6.Controls.Add(this.label28);
            this.panel6.Controls.Add(this.label24);
            this.panel6.Controls.Add(this.groupBox8);
            this.panel6.Controls.Add(this.txtA1PP);
            this.panel6.Controls.Add(this.btnCntProduct);
            this.panel6.Controls.Add(this.numProductInSet);
            this.panel6.Controls.Add(this.groupBox3);
            this.panel6.Controls.Add(this.label22);
            this.panel6.Controls.Add(this.groupBox4);
            this.panel6.Controls.Add(this.btnJudge);
            this.panel6.Controls.Add(this.dataGridView1);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1908, 771);
            this.panel6.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(1537, 456);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 16);
            this.label14.TabIndex = 33;
            this.label14.Text = "Weight";
            // 
            // txtWeightPPK
            // 
            this.txtWeightPPK.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtWeightPPK.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtWeightPPK.ForeColor = System.Drawing.Color.Yellow;
            this.txtWeightPPK.Location = new System.Drawing.Point(1764, 456);
            this.txtWeightPPK.Name = "txtWeightPPK";
            this.txtWeightPPK.Size = new System.Drawing.Size(132, 35);
            this.txtWeightPPK.TabIndex = 32;
            this.txtWeightPPK.Text = "0";
            this.txtWeightPPK.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtWeightPP
            // 
            this.txtWeightPP.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtWeightPP.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtWeightPP.ForeColor = System.Drawing.Color.Yellow;
            this.txtWeightPP.Location = new System.Drawing.Point(1610, 456);
            this.txtWeightPP.Name = "txtWeightPP";
            this.txtWeightPP.Size = new System.Drawing.Size(132, 35);
            this.txtWeightPP.TabIndex = 31;
            this.txtWeightPP.Text = "0";
            this.txtWeightPP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(1537, 410);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 16);
            this.label13.TabIndex = 30;
            this.label13.Text = "A2 Index";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(1537, 364);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(67, 16);
            this.label29.TabIndex = 29;
            this.label29.Text = "A1 Index";
            // 
            // txtA2PPK
            // 
            this.txtA2PPK.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtA2PPK.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA2PPK.ForeColor = System.Drawing.Color.Yellow;
            this.txtA2PPK.Location = new System.Drawing.Point(1764, 410);
            this.txtA2PPK.Name = "txtA2PPK";
            this.txtA2PPK.Size = new System.Drawing.Size(132, 35);
            this.txtA2PPK.TabIndex = 25;
            this.txtA2PPK.Text = "0";
            this.txtA2PPK.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label27.Location = new System.Drawing.Point(1760, 341);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(39, 20);
            this.label27.TabIndex = 26;
            this.label27.Text = "PPK";
            // 
            // txtA1PPK
            // 
            this.txtA1PPK.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtA1PPK.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA1PPK.ForeColor = System.Drawing.Color.Yellow;
            this.txtA1PPK.Location = new System.Drawing.Point(1764, 364);
            this.txtA1PPK.Name = "txtA1PPK";
            this.txtA1PPK.Size = new System.Drawing.Size(132, 35);
            this.txtA1PPK.TabIndex = 25;
            this.txtA1PPK.Text = "0";
            this.txtA1PPK.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA2PP
            // 
            this.txtA2PP.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtA2PP.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA2PP.ForeColor = System.Drawing.Color.Yellow;
            this.txtA2PP.Location = new System.Drawing.Point(1610, 410);
            this.txtA2PP.Name = "txtA2PP";
            this.txtA2PP.Size = new System.Drawing.Size(132, 35);
            this.txtA2PP.TabIndex = 20;
            this.txtA2PP.Text = "0";
            this.txtA2PP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label28.Location = new System.Drawing.Point(1606, 341);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(29, 20);
            this.label28.TabIndex = 24;
            this.label28.Text = "PP";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label24.Location = new System.Drawing.Point(1543, 153);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(53, 16);
            this.label24.TabIndex = 8;
            this.label24.Text = "Current:";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.txtWeightResult);
            this.groupBox8.Controls.Add(this.groupBox6);
            this.groupBox8.Controls.Add(this.txtWeight);
            this.groupBox8.Controls.Add(this.label23);
            this.groupBox8.Location = new System.Drawing.Point(1535, 6);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(366, 90);
            this.groupBox8.TabIndex = 28;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Weight";
            // 
            // txtWeightResult
            // 
            this.txtWeightResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtWeightResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.txtWeightResult.Location = new System.Drawing.Point(204, 22);
            this.txtWeightResult.Name = "txtWeightResult";
            this.txtWeightResult.Size = new System.Drawing.Size(82, 56);
            this.txtWeightResult.TabIndex = 32;
            this.txtWeightResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(7, 95);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 100);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "groupBox6";
            // 
            // txtWeight
            // 
            this.txtWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtWeight.Location = new System.Drawing.Point(84, 22);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(84, 26);
            this.txtWeight.TabIndex = 7;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label23.Location = new System.Drawing.Point(13, 26);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(53, 16);
            this.label23.TabIndex = 6;
            this.label23.Text = "Weight:";
            // 
            // txtA1PP
            // 
            this.txtA1PP.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtA1PP.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA1PP.ForeColor = System.Drawing.Color.Yellow;
            this.txtA1PP.Location = new System.Drawing.Point(1610, 364);
            this.txtA1PP.Name = "txtA1PP";
            this.txtA1PP.Size = new System.Drawing.Size(132, 35);
            this.txtA1PP.TabIndex = 20;
            this.txtA1PP.Text = "0";
            this.txtA1PP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCntProduct
            // 
            this.btnCntProduct.BackColor = System.Drawing.Color.White;
            this.btnCntProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCntProduct.ForeColor = System.Drawing.Color.DodgerBlue;
            this.btnCntProduct.Location = new System.Drawing.Point(1619, 138);
            this.btnCntProduct.Name = "btnCntProduct";
            this.btnCntProduct.Size = new System.Drawing.Size(105, 41);
            this.btnCntProduct.TabIndex = 4;
            this.btnCntProduct.Text = "0";
            this.btnCntProduct.UseCompatibleTextRendering = true;
            this.btnCntProduct.UseVisualStyleBackColor = false;
            // 
            // numProductInSet
            // 
            this.numProductInSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numProductInSet.Location = new System.Drawing.Point(1620, 106);
            this.numProductInSet.Name = "numProductInSet";
            this.numProductInSet.Size = new System.Drawing.Size(102, 26);
            this.numProductInSet.TabIndex = 5;
            this.numProductInSet.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.chart2);
            this.groupBox3.Controls.Add(this.chart1);
            this.groupBox3.Controls.Add(this.chartWeight);
            this.groupBox3.Controls.Add(this.chartA1);
            this.groupBox3.Controls.Add(this.chartA2);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox3.ForeColor = System.Drawing.Color.Teal;
            this.groupBox3.Location = new System.Drawing.Point(6, 102);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1522, 514);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time Chart";
            // 
            // chartWeight
            // 
            chartArea3.AxisX.Title = "Weight PPK";
            chartArea3.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea3.AxisX.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea3.AxisY.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea3.Name = "ChartArea1";
            chartArea3.ShadowColor = System.Drawing.Color.Gray;
            this.chartWeight.ChartAreas.Add(chartArea3);
            legend3.DockedToChartArea = "ChartArea1";
            legend3.Enabled = false;
            legend3.HeaderSeparatorColor = System.Drawing.Color.DarkGray;
            legend3.IsTextAutoFit = false;
            legend3.ItemColumnSeparatorColor = System.Drawing.Color.LightGray;
            legend3.Name = "Legend1";
            this.chartWeight.Legends.Add(legend3);
            this.chartWeight.Location = new System.Drawing.Point(1024, 291);
            this.chartWeight.Name = "chartWeight";
            this.chartWeight.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series3.BorderWidth = 2;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            series3.Legend = "Legend1";
            series3.Name = "Weight";
            this.chartWeight.Series.Add(series3);
            this.chartWeight.Size = new System.Drawing.Size(486, 205);
            this.chartWeight.TabIndex = 3;
            this.chartWeight.Text = "Chart Weight";
            // 
            // chartA1
            // 
            chartArea4.AxisX.Title = "A1 PPK";
            chartArea4.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea4.AxisX.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea4.AxisY.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea4.Name = "ChartArea1";
            chartArea4.ShadowColor = System.Drawing.Color.Gray;
            this.chartA1.ChartAreas.Add(chartArea4);
            legend4.DockedToChartArea = "ChartArea1";
            legend4.Enabled = false;
            legend4.HeaderSeparatorColor = System.Drawing.Color.DarkGray;
            legend4.IsTextAutoFit = false;
            legend4.ItemColumnSeparatorColor = System.Drawing.Color.LightGray;
            legend4.Name = "Legend1";
            this.chartA1.Legends.Add(legend4);
            this.chartA1.Location = new System.Drawing.Point(24, 291);
            this.chartA1.Name = "chartA1";
            this.chartA1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series4.BorderWidth = 2;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            series4.Legend = "Legend1";
            series4.Name = "A1";
            this.chartA1.Series.Add(series4);
            this.chartA1.Size = new System.Drawing.Size(486, 205);
            this.chartA1.TabIndex = 2;
            this.chartA1.Text = "Chart A1";
            // 
            // chartA2
            // 
            chartArea5.AxisX.Title = "A2 PPK";
            chartArea5.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea5.AxisX.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea5.AxisY.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea5.Name = "ChartArea1";
            chartArea5.ShadowColor = System.Drawing.Color.Gray;
            this.chartA2.ChartAreas.Add(chartArea5);
            legend5.DockedToChartArea = "ChartArea1";
            legend5.Enabled = false;
            legend5.HeaderSeparatorColor = System.Drawing.Color.DarkGray;
            legend5.ItemColumnSeparatorColor = System.Drawing.Color.LightGray;
            legend5.Name = "Legend1";
            this.chartA2.Legends.Add(legend5);
            this.chartA2.Location = new System.Drawing.Point(524, 291);
            this.chartA2.Name = "chartA2";
            this.chartA2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series5.BorderWidth = 2;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            series5.IsXValueIndexed = true;
            series5.Legend = "Legend1";
            series5.Name = "A2";
            this.chartA2.Series.Add(series5);
            this.chartA2.Size = new System.Drawing.Size(486, 205);
            this.chartA2.TabIndex = 1;
            this.chartA2.Text = "Chart A2";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label22.Location = new System.Drawing.Point(1540, 111);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(74, 16);
            this.label22.TabIndex = 2;
            this.label22.Text = "Số SP /set:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtTotalProcessed);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.txtTotalFAIL);
            this.groupBox4.Controls.Add(this.txtTotalPass);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox4.Location = new System.Drawing.Point(1535, 187);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(363, 145);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Counter";
            // 
            // txtTotalProcessed
            // 
            this.txtTotalProcessed.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTotalProcessed.ForeColor = System.Drawing.Color.Teal;
            this.txtTotalProcessed.Location = new System.Drawing.Point(154, 20);
            this.txtTotalProcessed.Name = "txtTotalProcessed";
            this.txtTotalProcessed.Size = new System.Drawing.Size(190, 35);
            this.txtTotalProcessed.TabIndex = 21;
            this.txtTotalProcessed.Text = "0";
            this.txtTotalProcessed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label16.Location = new System.Drawing.Point(25, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(123, 20);
            this.label16.TabIndex = 20;
            this.label16.Text = "Total Processed";
            // 
            // txtTotalFAIL
            // 
            this.txtTotalFAIL.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTotalFAIL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTotalFAIL.Location = new System.Drawing.Point(154, 97);
            this.txtTotalFAIL.Name = "txtTotalFAIL";
            this.txtTotalFAIL.Size = new System.Drawing.Size(190, 35);
            this.txtTotalFAIL.TabIndex = 19;
            this.txtTotalFAIL.Text = "0";
            this.txtTotalFAIL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotalPass
            // 
            this.txtTotalPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTotalPass.ForeColor = System.Drawing.Color.Teal;
            this.txtTotalPass.Location = new System.Drawing.Point(154, 58);
            this.txtTotalPass.Name = "txtTotalPass";
            this.txtTotalPass.Size = new System.Drawing.Size(190, 35);
            this.txtTotalPass.TabIndex = 18;
            this.txtTotalPass.Text = "0";
            this.txtTotalPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label17.Location = new System.Drawing.Point(25, 103);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 20);
            this.label17.TabIndex = 10;
            this.label17.Text = "Total FAIL";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label18.Location = new System.Drawing.Point(25, 66);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(91, 20);
            this.label18.TabIndex = 8;
            this.label18.Text = "Total PASS";
            // 
            // btnJudge
            // 
            this.btnJudge.BackColor = System.Drawing.Color.White;
            this.btnJudge.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnJudge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnJudge.Location = new System.Drawing.Point(1738, 102);
            this.btnJudge.Name = "btnJudge";
            this.btnJudge.Size = new System.Drawing.Size(161, 84);
            this.btnJudge.TabIndex = 5;
            this.btnJudge.UseCompatibleTextRendering = true;
            this.btnJudge.UseVisualStyleBackColor = false;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GrayText;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.model,
            this.Weight,
            this.A1MaxValue,
            this.A1MinValue,
            this.A1Result,
            this.A2MaxValue,
            this.A2MinValue,
            this.A2Result,
            this.Date,
            this.Time,
            this.Judge,
            this.TotalProcessed,
            this.TotalPASS,
            this.TotalFAIL});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dataGridView1.Location = new System.Drawing.Point(4, 622);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.LightGray;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.Size = new System.Drawing.Size(1899, 420);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ID.DataPropertyName = "ID";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.ID.DefaultCellStyle = dataGridViewCellStyle3;
            this.ID.FillWeight = 120F;
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 100;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // model
            // 
            this.model.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.model.DataPropertyName = "model";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.model.DefaultCellStyle = dataGridViewCellStyle4;
            this.model.FillWeight = 120F;
            this.model.HeaderText = "Model";
            this.model.MinimumWidth = 100;
            this.model.Name = "model";
            this.model.ReadOnly = true;
            // 
            // Weight
            // 
            this.Weight.DataPropertyName = "weight";
            this.Weight.HeaderText = "Weight";
            this.Weight.Name = "Weight";
            // 
            // A1MaxValue
            // 
            this.A1MaxValue.DataPropertyName = "A1MaxValue";
            this.A1MaxValue.HeaderText = "A1 Max Value";
            this.A1MaxValue.MinimumWidth = 130;
            this.A1MaxValue.Name = "A1MaxValue";
            this.A1MaxValue.ReadOnly = true;
            // 
            // A1MinValue
            // 
            this.A1MinValue.DataPropertyName = "A1MinValue";
            this.A1MinValue.HeaderText = "A1 Min Value";
            this.A1MinValue.MinimumWidth = 130;
            this.A1MinValue.Name = "A1MinValue";
            this.A1MinValue.ReadOnly = true;
            // 
            // A1Result
            // 
            this.A1Result.DataPropertyName = "A1Result";
            this.A1Result.FillWeight = 80F;
            this.A1Result.HeaderText = "A1 Result";
            this.A1Result.MinimumWidth = 80;
            this.A1Result.Name = "A1Result";
            // 
            // A2MaxValue
            // 
            this.A2MaxValue.DataPropertyName = "A2MaxValue";
            this.A2MaxValue.HeaderText = "A2 Max Value";
            this.A2MaxValue.MinimumWidth = 130;
            this.A2MaxValue.Name = "A2MaxValue";
            // 
            // A2MinValue
            // 
            this.A2MinValue.DataPropertyName = "A2MinValue";
            this.A2MinValue.HeaderText = "A2 Min Value";
            this.A2MinValue.MinimumWidth = 130;
            this.A2MinValue.Name = "A2MinValue";
            // 
            // A2Result
            // 
            this.A2Result.DataPropertyName = "A2Result";
            this.A2Result.FillWeight = 80F;
            this.A2Result.HeaderText = "A2 Result";
            this.A2Result.MinimumWidth = 80;
            this.A2Result.Name = "A2Result";
            // 
            // Date
            // 
            this.Date.DataPropertyName = "Date";
            this.Date.FillWeight = 80F;
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 80;
            this.Date.Name = "Date";
            // 
            // Time
            // 
            this.Time.DataPropertyName = "Time";
            this.Time.FillWeight = 80F;
            this.Time.HeaderText = "Time";
            this.Time.MinimumWidth = 80;
            this.Time.Name = "Time";
            // 
            // Judge
            // 
            this.Judge.DataPropertyName = "Judge";
            this.Judge.FillWeight = 80F;
            this.Judge.HeaderText = "Judge";
            this.Judge.MinimumWidth = 80;
            this.Judge.Name = "Judge";
            // 
            // TotalProcessed
            // 
            this.TotalProcessed.DataPropertyName = "TotalProcessed";
            this.TotalProcessed.FillWeight = 120F;
            this.TotalProcessed.HeaderText = "Total Processed";
            this.TotalProcessed.MinimumWidth = 100;
            this.TotalProcessed.Name = "TotalProcessed";
            this.TotalProcessed.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // TotalPASS
            // 
            this.TotalPASS.DataPropertyName = "TotalPASS";
            this.TotalPASS.FillWeight = 120F;
            this.TotalPASS.HeaderText = "Total PASS";
            this.TotalPASS.MinimumWidth = 100;
            this.TotalPASS.Name = "TotalPASS";
            this.TotalPASS.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // TotalFAIL
            // 
            this.TotalFAIL.DataPropertyName = "TotalFAIL";
            this.TotalFAIL.FillWeight = 120F;
            this.TotalFAIL.HeaderText = "Total FAIL";
            this.TotalFAIL.MinimumWidth = 100;
            this.TotalFAIL.Name = "TotalFAIL";
            this.TotalFAIL.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel5.Controls.Add(this.groupBox2);
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1533, 102);
            this.panel5.TabIndex = 0;
            // 
            // groupBox2
            // 
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
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(937, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(591, 90);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Measuring Data";
            // 
            // txtA2Result
            // 
            this.txtA2Result.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA2Result.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.txtA2Result.Location = new System.Drawing.Point(474, 22);
            this.txtA2Result.Name = "txtA2Result";
            this.txtA2Result.Size = new System.Drawing.Size(82, 56);
            this.txtA2Result.TabIndex = 31;
            this.txtA2Result.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtA1MinimumValue
            // 
            this.txtA1MinimumValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA1MinimumValue.Location = new System.Drawing.Point(107, 52);
            this.txtA1MinimumValue.Name = "txtA1MinimumValue";
            this.txtA1MinimumValue.Size = new System.Drawing.Size(80, 26);
            this.txtA1MinimumValue.TabIndex = 30;
            this.txtA1MinimumValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA1MaximumValue
            // 
            this.txtA1MaximumValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA1MaximumValue.Location = new System.Drawing.Point(107, 22);
            this.txtA1MaximumValue.Name = "txtA1MaximumValue";
            this.txtA1MaximumValue.Size = new System.Drawing.Size(80, 26);
            this.txtA1MaximumValue.TabIndex = 29;
            this.txtA1MaximumValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA1Result
            // 
            this.txtA1Result.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA1Result.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.txtA1Result.Location = new System.Drawing.Point(193, 23);
            this.txtA1Result.Name = "txtA1Result";
            this.txtA1Result.Size = new System.Drawing.Size(82, 56);
            this.txtA1Result.TabIndex = 28;
            this.txtA1Result.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(11, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 16);
            this.label3.TabIndex = 27;
            this.label3.Text = "A1 Min Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.Location = new System.Drawing.Point(11, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 16);
            this.label5.TabIndex = 26;
            this.label5.Text = "A1 Max Value";
            // 
            // txtA2MinimumValue
            // 
            this.txtA2MinimumValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA2MinimumValue.Location = new System.Drawing.Point(384, 52);
            this.txtA2MinimumValue.Name = "txtA2MinimumValue";
            this.txtA2MinimumValue.Size = new System.Drawing.Size(84, 26);
            this.txtA2MinimumValue.TabIndex = 25;
            this.txtA2MinimumValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA2MaximumValue
            // 
            this.txtA2MaximumValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA2MaximumValue.Location = new System.Drawing.Point(384, 22);
            this.txtA2MaximumValue.Name = "txtA2MaximumValue";
            this.txtA2MaximumValue.Size = new System.Drawing.Size(84, 26);
            this.txtA2MaximumValue.TabIndex = 24;
            this.txtA2MaximumValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label10.Location = new System.Drawing.Point(286, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 16);
            this.label10.TabIndex = 15;
            this.label10.Text = "A2 Min Value";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label11.Location = new System.Drawing.Point(286, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 16);
            this.label11.TabIndex = 13;
            this.label11.Text = "A2 Max Value";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtWeightMin);
            this.groupBox1.Controls.Add(this.txtWeightMax);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.label26);
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
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(925, 90);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Setting";
            // 
            // txtWeightMin
            // 
            this.txtWeightMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtWeightMin.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtWeightMin.Location = new System.Drawing.Point(816, 52);
            this.txtWeightMin.Name = "txtWeightMin";
            this.txtWeightMin.Size = new System.Drawing.Size(80, 26);
            this.txtWeightMin.TabIndex = 88;
            this.txtWeightMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtWeightMax
            // 
            this.txtWeightMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtWeightMax.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtWeightMax.Location = new System.Drawing.Point(816, 22);
            this.txtWeightMax.Name = "txtWeightMax";
            this.txtWeightMax.Size = new System.Drawing.Size(80, 26);
            this.txtWeightMax.TabIndex = 87;
            this.txtWeightMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label25.Location = new System.Drawing.Point(716, 57);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(90, 16);
            this.label25.TabIndex = 86;
            this.label25.Text = "Weight Min(g)";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label26.Location = new System.Drawing.Point(716, 27);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(94, 16);
            this.label26.TabIndex = 85;
            this.label26.Text = "Weight Max(g)";
            // 
            // cmbTimeToEnableRead
            // 
            this.cmbTimeToEnableRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cmbTimeToEnableRead.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.cmbTimeToEnableRead.FormattingEnabled = true;
            this.cmbTimeToEnableRead.Items.AddRange(new object[] {
            "0.5",
            "1",
            "1.5",
            "2",
            "2.5",
            "3"});
            this.cmbTimeToEnableRead.Location = new System.Drawing.Point(115, 52);
            this.cmbTimeToEnableRead.Name = "cmbTimeToEnableRead";
            this.cmbTimeToEnableRead.Size = new System.Drawing.Size(52, 28);
            this.cmbTimeToEnableRead.TabIndex = 84;
            this.cmbTimeToEnableRead.Text = "1";
            this.cmbTimeToEnableRead.SelectedIndexChanged += new System.EventHandler(this.cmbTimeToEnableRead_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label20.Location = new System.Drawing.Point(10, 57);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(99, 16);
            this.label20.TabIndex = 83;
            this.label20.Text = "Detect Time (s)";
            // 
            // txtA2MaximumOffset
            // 
            this.txtA2MaximumOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA2MaximumOffset.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtA2MaximumOffset.Location = new System.Drawing.Point(627, 52);
            this.txtA2MaximumOffset.Name = "txtA2MaximumOffset";
            this.txtA2MaximumOffset.Size = new System.Drawing.Size(80, 26);
            this.txtA2MaximumOffset.TabIndex = 82;
            this.txtA2MaximumOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA1MaximumOffset
            // 
            this.txtA1MaximumOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA1MaximumOffset.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtA1MaximumOffset.Location = new System.Drawing.Point(627, 22);
            this.txtA1MaximumOffset.Name = "txtA1MaximumOffset";
            this.txtA1MaximumOffset.Size = new System.Drawing.Size(80, 26);
            this.txtA1MaximumOffset.TabIndex = 81;
            this.txtA1MaximumOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label9.Location = new System.Drawing.Point(537, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 16);
            this.label9.TabIndex = 80;
            this.label9.Text = "A2 Max Offset";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label12.Location = new System.Drawing.Point(537, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 16);
            this.label12.TabIndex = 79;
            this.label12.Text = "A1 Max Offset";
            // 
            // txtA2MinimumOffset
            // 
            this.txtA2MinimumOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA2MinimumOffset.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtA2MinimumOffset.Location = new System.Drawing.Point(450, 52);
            this.txtA2MinimumOffset.Name = "txtA2MinimumOffset";
            this.txtA2MinimumOffset.Size = new System.Drawing.Size(80, 26);
            this.txtA2MinimumOffset.TabIndex = 78;
            this.txtA2MinimumOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA1MinimumOffset
            // 
            this.txtA1MinimumOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA1MinimumOffset.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtA1MinimumOffset.Location = new System.Drawing.Point(450, 22);
            this.txtA1MinimumOffset.Name = "txtA1MinimumOffset";
            this.txtA1MinimumOffset.Size = new System.Drawing.Size(80, 26);
            this.txtA1MinimumOffset.TabIndex = 77;
            this.txtA1MinimumOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label6.Location = new System.Drawing.Point(363, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 16);
            this.label6.TabIndex = 76;
            this.label6.Text = "A2 Min Offset";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label8.Location = new System.Drawing.Point(363, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 16);
            this.label8.TabIndex = 75;
            this.label8.Text = "A1 Min Offset";
            // 
            // txtA2DetectionLevel
            // 
            this.txtA2DetectionLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA2DetectionLevel.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtA2DetectionLevel.Location = new System.Drawing.Point(276, 52);
            this.txtA2DetectionLevel.Name = "txtA2DetectionLevel";
            this.txtA2DetectionLevel.Size = new System.Drawing.Size(80, 26);
            this.txtA2DetectionLevel.TabIndex = 74;
            this.txtA2DetectionLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtA1DetectionLevel
            // 
            this.txtA1DetectionLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtA1DetectionLevel.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtA1DetectionLevel.Location = new System.Drawing.Point(276, 22);
            this.txtA1DetectionLevel.Name = "txtA1DetectionLevel";
            this.txtA1DetectionLevel.Size = new System.Drawing.Size(80, 26);
            this.txtA1DetectionLevel.TabIndex = 73;
            this.txtA1DetectionLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.Location = new System.Drawing.Point(173, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 16);
            this.label7.TabIndex = 8;
            this.label7.Text = "A2 Detect Level";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.Location = new System.Drawing.Point(173, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "A1 Detect Level";
            // 
            // cmbModel
            // 
            this.cmbModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cmbModel.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new System.Drawing.Point(61, 22);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(106, 28);
            this.cmbModel.TabIndex = 1;
            this.cmbModel.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.cmbModel.TextChanged += new System.EventHandler(this.cmbModel_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(10, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Model";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // dateTimeFilter
            // 
            this.dateTimeFilter.Enabled = false;
            this.dateTimeFilter.Location = new System.Drawing.Point(996, 7);
            this.dateTimeFilter.Name = "dateTimeFilter";
            this.dateTimeFilter.Size = new System.Drawing.Size(207, 20);
            this.dateTimeFilter.TabIndex = 28;
            this.dateTimeFilter.ValueChanged += new System.EventHandler(this.dateTimeFilter_ValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuConfig});
            this.menuStrip1.Location = new System.Drawing.Point(595, 6);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(89, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuConfig
            // 
            this.MenuConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.communicatiomToolStripMenuItem});
            this.MenuConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuConfig.Name = "MenuConfig";
            this.MenuConfig.Size = new System.Drawing.Size(81, 20);
            this.MenuConfig.Text = "&Configuration";
            // 
            // communicatiomToolStripMenuItem
            // 
            this.communicatiomToolStripMenuItem.Name = "communicatiomToolStripMenuItem";
            this.communicatiomToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.communicatiomToolStripMenuItem.Text = "&Communication";
            this.communicatiomToolStripMenuItem.Click += new System.EventHandler(this.communicatiomToolStripMenuItem_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
            this.lblStatus.Location = new System.Drawing.Point(64, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(122, 16);
            this.lblStatus.TabIndex = 54;
            this.lblStatus.Text = "Serialport 1(A1,A2):";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Desktop;
            this.panel2.Controls.Add(this.lblConnectStatus2);
            this.panel2.Controls.Add(this.lblStatus2);
            this.panel2.Controls.Add(this.lblConnectStatus);
            this.panel2.Controls.Add(this.lblStatus);
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1216, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(692, 37);
            this.panel2.TabIndex = 56;
            // 
            // lblConnectStatus2
            // 
            this.lblConnectStatus2.AutoSize = true;
            this.lblConnectStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectStatus2.ForeColor = System.Drawing.Color.Lime;
            this.lblConnectStatus2.Location = new System.Drawing.Point(460, 10);
            this.lblConnectStatus2.Name = "lblConnectStatus2";
            this.lblConnectStatus2.Size = new System.Drawing.Size(73, 16);
            this.lblConnectStatus2.TabIndex = 57;
            this.lblConnectStatus2.Text = "Connected";
            // 
            // lblStatus2
            // 
            this.lblStatus2.AutoSize = true;
            this.lblStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus2.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
            this.lblStatus2.Location = new System.Drawing.Point(326, 10);
            this.lblStatus2.Name = "lblStatus2";
            this.lblStatus2.Size = new System.Drawing.Size(129, 16);
            this.lblStatus2.TabIndex = 56;
            this.lblStatus2.Text = "Serialport 2(Weight):";
            // 
            // lblConnectStatus
            // 
            this.lblConnectStatus.AutoSize = true;
            this.lblConnectStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectStatus.ForeColor = System.Drawing.Color.Lime;
            this.lblConnectStatus.Location = new System.Drawing.Point(192, 10);
            this.lblConnectStatus.Name = "lblConnectStatus";
            this.lblConnectStatus.Size = new System.Drawing.Size(73, 16);
            this.lblConnectStatus.TabIndex = 55;
            this.lblConnectStatus.Text = "Connected";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1908, 37);
            this.panel1.TabIndex = 70;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Desktop;
            this.panel4.Controls.Add(this.checkBoxFilterDate);
            this.panel4.Controls.Add(this.dateTimeFilter);
            this.panel4.Controls.Add(this.lblDateTime);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1216, 37);
            this.panel4.TabIndex = 59;
            // 
            // checkBoxFilterDate
            // 
            this.checkBoxFilterDate.AutoSize = true;
            this.checkBoxFilterDate.Location = new System.Drawing.Point(910, 8);
            this.checkBoxFilterDate.Name = "checkBoxFilterDate";
            this.checkBoxFilterDate.Size = new System.Drawing.Size(72, 17);
            this.checkBoxFilterDate.TabIndex = 60;
            this.checkBoxFilterDate.Text = "Filter date";
            this.checkBoxFilterDate.UseVisualStyleBackColor = true;
            this.checkBoxFilterDate.CheckedChanged += new System.EventHandler(this.checkBoxFilterDate_CheckedChanged);
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.ForeColor = System.Drawing.Color.GhostWhite;
            this.lblDateTime.Location = new System.Drawing.Point(282, 13);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(41, 13);
            this.lblDateTime.TabIndex = 59;
            this.lblDateTime.Text = "label27";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 18);
            this.label1.TabIndex = 58;
            this.label1.Text = "HALLA VINA - Diameter Checker";
            // 
            // tmrConnectionStatus
            // 
            this.tmrConnectionStatus.Enabled = true;
            this.tmrConnectionStatus.Tick += new System.EventHandler(this.tmrConnectionStatus_Tick);
            // 
            // panelResult
            // 
            this.panelResult.BackColor = System.Drawing.SystemColors.Control;
            this.panelResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
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
            this.panelResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelResult.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.panelResult.Location = new System.Drawing.Point(0, 808);
            this.panelResult.Name = "panelResult";
            this.panelResult.Size = new System.Drawing.Size(1908, 37);
            this.panelResult.TabIndex = 68;
            // 
            // chkStopScan
            // 
            this.chkStopScan.AutoSize = true;
            this.chkStopScan.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.chkStopScan.Location = new System.Drawing.Point(806, 9);
            this.chkStopScan.Name = "chkStopScan";
            this.chkStopScan.Size = new System.Drawing.Size(169, 17);
            this.chkStopScan.TabIndex = 92;
            this.chkStopScan.Text = "Stop scan when NG Detected";
            this.chkStopScan.UseVisualStyleBackColor = true;
            this.chkStopScan.CheckedChanged += new System.EventHandler(this.chkStopScan_CheckedChanged);
            // 
            // txtSystemMessage
            // 
            this.txtSystemMessage.BackColor = System.Drawing.SystemColors.Control;
            this.txtSystemMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSystemMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtSystemMessage.ForeColor = System.Drawing.Color.DarkRed;
            this.txtSystemMessage.Location = new System.Drawing.Point(119, 10);
            this.txtSystemMessage.Multiline = true;
            this.txtSystemMessage.Name = "txtSystemMessage";
            this.txtSystemMessage.Size = new System.Drawing.Size(606, 20);
            this.txtSystemMessage.TabIndex = 79;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.ForeColor = System.Drawing.Color.Teal;
            this.button2.Location = new System.Drawing.Point(994, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 33);
            this.button2.TabIndex = 78;
            this.button2.Text = "Auto Detect Object";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSelect.ForeColor = System.Drawing.Color.Teal;
            this.btnSelect.Location = new System.Drawing.Point(1124, 0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(130, 33);
            this.btnSelect.TabIndex = 76;
            this.btnSelect.Text = "&Add New Model";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.ForeColor = System.Drawing.Color.DarkRed;
            this.btnClear.Location = new System.Drawing.Point(1254, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(130, 33);
            this.btnClear.TabIndex = 77;
            this.btnClear.Text = "&Delete Current Model";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClearCurrentTest
            // 
            this.btnClearCurrentTest.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClearCurrentTest.ForeColor = System.Drawing.Color.Teal;
            this.btnClearCurrentTest.Location = new System.Drawing.Point(1384, 0);
            this.btnClearCurrentTest.Name = "btnClearCurrentTest";
            this.btnClearCurrentTest.Size = new System.Drawing.Size(130, 33);
            this.btnClearCurrentTest.TabIndex = 74;
            this.btnClearCurrentTest.Text = "&Clear Current Test";
            this.btnClearCurrentTest.UseVisualStyleBackColor = true;
            this.btnClearCurrentTest.Click += new System.EventHandler(this.btnClearCurrentData_Click);
            // 
            // btnDeleteTestData
            // 
            this.btnDeleteTestData.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeleteTestData.Enabled = false;
            this.btnDeleteTestData.ForeColor = System.Drawing.Color.DarkRed;
            this.btnDeleteTestData.Location = new System.Drawing.Point(1514, 0);
            this.btnDeleteTestData.Name = "btnDeleteTestData";
            this.btnDeleteTestData.Size = new System.Drawing.Size(130, 33);
            this.btnDeleteTestData.TabIndex = 73;
            this.btnDeleteTestData.Text = "Delete Test Data";
            this.btnDeleteTestData.UseVisualStyleBackColor = true;
            this.btnDeleteTestData.Click += new System.EventHandler(this.btnDeleteTestData_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEdit.ForeColor = System.Drawing.Color.Teal;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(1644, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(130, 33);
            this.btnEdit.TabIndex = 72;
            this.btnEdit.Text = "       &Report Data";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // lblSystemMessage
            // 
            this.lblSystemMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblSystemMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblSystemMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblSystemMessage.ForeColor = System.Drawing.Color.DarkRed;
            this.lblSystemMessage.Location = new System.Drawing.Point(16, 10);
            this.lblSystemMessage.Multiline = true;
            this.lblSystemMessage.Name = "lblSystemMessage";
            this.lblSystemMessage.Size = new System.Drawing.Size(102, 20);
            this.lblSystemMessage.TabIndex = 71;
            this.lblSystemMessage.Text = "System Message:";
            // 
            // btnStart
            // 
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.Teal;
            this.btnStart.Location = new System.Drawing.Point(1774, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(130, 33);
            this.btnStart.TabIndex = 70;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tmrDisplayData
            // 
            this.tmrDisplayData.Interval = 50;
            this.tmrDisplayData.Tick += new System.EventHandler(this.tmrDisplayData_Tick);
            // 
            // tmrDateTime
            // 
            this.tmrDateTime.Enabled = true;
            this.tmrDateTime.Interval = 1000;
            this.tmrDateTime.Tick += new System.EventHandler(this.tmrDateTime_Tick);
            // 
            // tmrRefreshChart
            // 
            this.tmrRefreshChart.Interval = 10;
            this.tmrRefreshChart.Tick += new System.EventHandler(this.tmrRefreshChart_Tick);
            // 
            // tmrA1DetectRemoveObject
            // 
            this.tmrA1DetectRemoveObject.Interval = 1000;
            this.tmrA1DetectRemoveObject.Tick += new System.EventHandler(this.tmrA1DetectRemoveObject_Tick);
            // 
            // tmrA2DetectRemoveObject
            // 
            this.tmrA2DetectRemoveObject.Interval = 1000;
            this.tmrA2DetectRemoveObject.Tick += new System.EventHandler(this.tmrA2DetectRemoveObject_Tick);
            // 
            // tmrEnableReadA1Data
            // 
            this.tmrEnableReadA1Data.Interval = 1000;
            this.tmrEnableReadA1Data.Tick += new System.EventHandler(this.tmrEnableReadA1Data_Tick);
            // 
            // tmrEnableReadA2Data
            // 
            this.tmrEnableReadA2Data.Interval = 1000;
            this.tmrEnableReadA2Data.Tick += new System.EventHandler(this.tmrEnableReadA2Data_Tick);
            // 
            // tmrRefreshDataGridView
            // 
            this.tmrRefreshDataGridView.Interval = 10;
            this.tmrRefreshDataGridView.Tick += new System.EventHandler(this.tmrRefreshDataGridView_Tick);
            // 
            // serialPort2
            // 
            this.serialPort2.BaudRate = 19200;
            this.serialPort2.PortName = "COM3";
            // 
            // tmrDisplayJudge
            // 
            this.tmrDisplayJudge.Interval = 500;
            this.tmrDisplayJudge.Tick += new System.EventHandler(this.displayJudge_Tick);
            // 
            // chart1
            // 
            chartArea2.AxisX.Title = "A1 Air Pressure";
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea2.AxisX.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea2.AxisX2.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea2.AxisY.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea2.Name = "ChartArea1";
            chartArea2.ShadowColor = System.Drawing.Color.Gray;
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.DockedToChartArea = "ChartArea1";
            legend2.Enabled = false;
            legend2.HeaderSeparatorColor = System.Drawing.Color.DarkGray;
            legend2.IsTextAutoFit = false;
            legend2.ItemColumnSeparatorColor = System.Drawing.Color.LightGray;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(7, 25);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            series2.Legend = "Legend1";
            series2.Name = "A1 Pressue";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(746, 234);
            this.chart1.TabIndex = 4;
            this.chart1.Text = "Chart A1 Air Pressure";
            // 
            // chart2
            // 
            chartArea1.AxisX.Title = "A2 Air Pressure";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea1.AxisY.TitleForeColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea1.Name = "ChartArea1";
            chartArea1.ShadowColor = System.Drawing.Color.Gray;
            this.chart2.ChartAreas.Add(chartArea1);
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Enabled = false;
            legend1.HeaderSeparatorColor = System.Drawing.Color.DarkGray;
            legend1.ItemColumnSeparatorColor = System.Drawing.Color.LightGray;
            legend1.Name = "Legend1";
            this.chart2.Legends.Add(legend1);
            this.chart2.Location = new System.Drawing.Point(767, 25);
            this.chart2.Name = "chart2";
            this.chart2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.Name = "A2Pressure";
            this.chart2.Series.Add(series1);
            this.chart2.Size = new System.Drawing.Size(746, 234);
            this.chart2.TabIndex = 5;
            this.chart2.Text = "Chart A2 Air Pressure";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1908, 845);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelResult);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProductInSet)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartA1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartA2)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel5.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);

        }

        private void label13_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void loadData(string d="")
        {
            try
            {
                SqlConnection con = new SqlConnection(Communication.con_string);
                con.Open();
                string whereClause = " where Display = 1";
                if(!string.IsNullOrEmpty(d))
                {
                    whereClause = " where Date = '" + d + "'";
                }
                
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("select ID, model, weight, A1MaxValue, A1MinValue, A1Result, A2MaxValue, A2MinValue, A2Result, Date, Time, Judge, TotalProcessed, TotalPASS, TotalFAIL from (select top 21 * from (select CAST(substring(ID,3,10) as int) as NEWID, * from Data"+whereClause+")A1 order by NEWID DESC)B1 order by NEWID", con));

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
            for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
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
                string add = string.Concat(new object[] { "INSERT INTO Data (ID, model, weight, A1MaxValue, A1MinValue, A1Result, A2MaxValue, A2MinValue, A2Result, Date, Time, Judge, TotalProcessed, TotalPASS, TotalFAIL) VALUES ('", Communication.ID, "','", Communication.model, "','", Communication.Weight.Trim(), "','", Communication.A1MaximumValue, "','", Communication.A1MinimumValue, "','", Communication.A1Result, "','", Communication.A2MaximumValue, "','", Communication.A2MinimumValue, "','", Communication.A2Result, "','", Communication.Date, "','", Communication.Time, "','", Communication.Judge, "','", Communication.totalProcessed, "','", Communication.totalPASS, "','", Communication.totalFAIL, "')" });
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
            this.txtWeightMax.Text = null;
            this.txtWeightMin.Text = null;

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
                    Communication.maxWeight = dt["maxWeight"].ToString().Trim();
                    Communication.minWeight = dt["minWeight"].ToString().Trim();
                    this.txtA1DetectionLevel.Text = dt["A1DetectionValue"].ToString().Trim();
                    this.txtA2DetectionLevel.Text = dt["A2DetectionValue"].ToString().Trim();
                    this.txtA1MinimumOffset.Text = dt["A1MinimumOffset"].ToString().Trim();
                    this.txtA1MaximumOffset.Text = dt["A1MaximumOffset"].ToString().Trim();
                    this.txtA2MinimumOffset.Text = dt["A2MinimumOffset"].ToString().Trim();
                    this.txtA2MaximumOffset.Text = dt["A2MaximumOffset"].ToString().Trim();
                    this.txtWeightMax.Text = dt["maxWeight"].ToString().Trim();
                    this.txtWeightMin.Text = dt["minWeight"].ToString().Trim();
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
                if (InputData.Length >= Communication.charNumberOfCom_data)
                {
                    charNumberOfFirstString = InputData.IndexOf("A1");
                    if (charNumberOfFirstString <= 0)
                    {
                        charNumberOfFirstString = 0;
                    }
                    else
                    {
                        fistSubString = InputData.Substring(0, charNumberOfFirstString);
                    }
                    if (InputData.Length >= charNumberOfFirstString + Communication.charNumberOfCom_data)
                    {
                        Communication.serialData = InputData.Substring(charNumberOfFirstString, Communication.charNumberOfCom_data);
                        if (Communication.serialData.Length != Communication.charNumberOfCom_data ||
                            !(Communication.serialData.Substring(0, 2) == "A1") ||
                            !(Communication.serialData.Substring(14, 2) == "A2") ||
                            !(Communication.serialData.Substring(11, 2) == "OK") &&
                            !(Communication.serialData.Substring(11, 2) == "NG"))
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = Communication.serialData.Substring(25, 2) == "OK" || Communication.serialData.Substring(25, 2) == "NG";
                        }
                        if (flag)
                        {
                            Communication.enableReadData = true;
                            tmrDisplayData.Enabled = true;
                        }
                        charNumberOfLastString = InputData.Length - charNumberOfFirstString - Communication.charNumberOfCom_data;
                        lastSubString = InputData.Substring(InputData.Length - charNumberOfLastString, charNumberOfLastString);
                        InputData = string.Concat(fistSubString, lastSubString);
                    }
                }
            }
        }

        public void SetText2(string text)
        {
            string weightResult;
            float weight, minWeight, maxWeight;
            if (base.InvokeRequired)
            {
                try
                {
                    frmMain.SetTextCallback d = new frmMain.SetTextCallback(this.SetText2);
                    base.Invoke(d, new object[] { text });
                }
                catch (InvalidOperationException e)
                {
                }
            }
            else
            {
                if (this.InputData2.Length >= Communication.charNumberOfCom_data2)
                {
                    this.charNumberOfFirstString2 = this.InputData2.IndexOf(" ST,GS,+");
                    if (this.charNumberOfFirstString2 <= 0)
                    {
                        this.charNumberOfFirstString2 = 0;
                    }
                    else
                    {
                        this.fistSubString2 = this.InputData2.Substring(0, this.charNumberOfFirstString2);
                    }
                    if (this.InputData2.Length >= this.charNumberOfFirstString2 + Communication.charNumberOfCom_data2)
                    {
                        Communication.serialData2 = this.InputData2.Substring(this.charNumberOfFirstString2, Communication.charNumberOfCom_data2);
                        if (Communication.serialData2.Length == Communication.charNumberOfCom_data2 && Communication.serialData2.Substring(0, 8) == " ST,GS,+")
                        {
                            if (!string.IsNullOrEmpty(btnJudge.Text))
                            {
                                ClearAllData();
                            }
                            weightResult = this.txtWeight.Text = Communication.serialData2.Substring(8, 11).Trim();
                            Communication.receivedWeightFlg = true;
                            Communication.Weight = weightResult = weightResult.Replace("g", "").Trim();
                            try
                            {
                                weight = float.Parse(weightResult.Replace(".", ""))/10f;
                               
                                minWeight = float.Parse(txtWeightMin.Text);
                                maxWeight = float.Parse(txtWeightMax.Text);
                                
                                if (weight < minWeight || weight > maxWeight)
                                {
                                    this.txtWeightResult.ForeColor = Color.Red;
                                    this.txtWeightResult.Text = "NG";
                                }
                                else
                                {
                                    this.txtWeightResult.ForeColor = Color.ForestGreen;
                                    this.txtWeightResult.Text = "OK";
                                }
                            }
                            catch (Exception e)
                            {

                                MessageBox.Show(e.Message);
                            }
                            //string format = string.Format("weight = {0}, min weight = {1}, max weight = {2}", weight.ToString(), minWeight.ToString(), maxWeight.ToString());
                            //MessageBox.Show(format);
                            //CheckAndMakeDecision();
                            //ClearAllData();
                        }
                        this.charNumberOfLastString2 = this.InputData2.Length - this.charNumberOfFirstString2 - Communication.charNumberOfCom_data2;
                        this.lastSubString2 = this.InputData2.Substring(this.InputData2.Length - this.charNumberOfLastString2, this.charNumberOfLastString2);
                        this.InputData2 = string.Concat(this.fistSubString2, this.lastSubString2);
                    }
                }
            }
        }

        private void testCycleFinish()
        {
            if ((!Communication.A1EnableSave ? false : Communication.A2EnableSave))
            {
                if ((txtA1Result.Text != "OK" ? true : this.txtA2Result.Text != "OK"))
                {
                    Communication.Judge = "FAIL";
                    this.btnJudge.ForeColor = Color.Red;
                    Communication.totalFAIL++;
                    this.txtTotalFAIL.Text = Communication.totalFAIL.ToString();
                }
                else
                {
                    Communication.Judge = "PASS";
                    btnJudge.ForeColor = Color.ForestGreen;
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
                this.chart1.Series.Clear();
                this.chart1Setting();
                this.chart2.Series.Clear();
                this.chart2Setting();
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
                lblConnectStatus.Text = "Not Connected";
                lblConnectStatus.ForeColor = Color.Red;
            }
            else
            {
                lblConnectStatus.Text = "Connected";
                lblConnectStatus.ForeColor = Color.GreenYellow;
            }
            if (!Communication.serialport.IsOpen && Communication.AutoReconnect)
            {
                try
                {
                    if (Communication.ConnectSerial(Communication.comPort, Communication.baudrate))
                    {
                        lblConnectStatus.Text = "Connected";
                        lblConnectStatus.ForeColor = Color.GreenYellow;
                    }
                }
                catch
                {
                }
            }
            if (!Communication.serialport2.IsOpen)
            {
                lblConnectStatus2.Text = "Not Connected";
                lblConnectStatus2.ForeColor = Color.Red;
            }
            else
            {
                lblConnectStatus2.Text = "Connected";
                lblConnectStatus2.ForeColor = Color.GreenYellow;
            }
            if (!Communication.serialport2.IsOpen && Communication.AutoReconnect2)
            {
                try
                {
                    if (Communication.ConnectSerial2(Communication.comPort2, Communication.baudrate2))
                    {
                        lblConnectStatus2.Text = "Connected";
                        lblConnectStatus2.ForeColor = Color.GreenYellow;
                    }
                }
                catch
                {
                }
            }
            if (cmbTimeToEnableRead.Text == "0.5")
            {
                tmrEnableReadA1Data.Interval = 500;
                tmrEnableReadA2Data.Interval = 500;
            }
            if (cmbTimeToEnableRead.Text == "1")
            {
                tmrEnableReadA1Data.Interval = 1000;
                tmrEnableReadA2Data.Interval = 1000;
            }
            if (cmbTimeToEnableRead.Text == "1.5")
            {
                tmrEnableReadA1Data.Interval = 1500;
                tmrEnableReadA2Data.Interval = 1500;
            }
            if (cmbTimeToEnableRead.Text == "2")
            {
                tmrEnableReadA1Data.Interval = 2000;
                tmrEnableReadA2Data.Interval = 2000;
            }
            if (cmbTimeToEnableRead.Text == "2.5")
            {
                tmrEnableReadA1Data.Interval = 2500;
                tmrEnableReadA2Data.Interval = 2500;
            }
            if (cmbTimeToEnableRead.Text == "3")
            {
                tmrEnableReadA1Data.Interval = 3000;
                tmrEnableReadA2Data.Interval = 3000;
            }
        }

        private void tmrDateTime_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            Communication.Date = now.ToString("yyyy-MM-dd");
            now = DateTime.Now;
            Communication.Time = now.ToString("H:mm");
            this.lblDateTime.Text = Communication.Date + " " + Communication.Time;
        }

        private void tmrDisplayData_Tick(object sender, EventArgs e)
        {
            bool flag;
            bool flag1;

            tmrDisplayData.Enabled = false;
            if (Communication.subformIsOpen)
            {
                txtA1MaximumValue.Text = null;
                txtA1MinimumValue.Text = null;
                txtA1Result.Text = null;
                txtA2MaximumValue.Text = null;
                txtA2MinimumValue.Text = null;
                txtA2Result.Text = null;
                txtWeightResult.Text = null;
            }
            else
            {
                if (Communication.serialData.Length != Communication.charNumberOfCom_data ||
                    !(Communication.serialData.Substring(0, 2) == "A1") ||
                    !(Communication.serialData.Substring(14, 2) == "A2") ||
                    !(Communication.serialData.Substring(11, 2) == "OK") &&
                    !(Communication.serialData.Substring(11, 2) == "NG"))
                {
                    flag = false;
                }
                else
                {
                    flag = Communication.serialData.Substring(25, 2) == "OK" || Communication.serialData.Substring(25, 2) == "NG";
                }
                if (flag)
                {
                    Communication.A1MeasuredValue = Communication.serialData.Substring(3, 6);
                    Communication.A1Result = Communication.serialData.Substring(11, 2);
                    if (float.Parse(Communication.A1MeasuredValue) < float.Parse(Communication.A1DetectionLevel) - Communication.detectionOffset)
                    {
                        tmrEnableReadA1Data.Enabled = true;
                        if (!Communication.A1enableStopTest && Communication.A1RecevingData)
                        {
                            if (!tmrA1DetectRemoveObject.Enabled)
                            {
                                tmrA1DetectRemoveObject.Enabled = true;
                            }
                            Communication.A1Detected = true;
                            if (!Communication.A2Detected)
                            {
                                txtSystemMessage.Text = "A1 Detected!";
                            }
                            else
                            {
                                txtSystemMessage.Text = "A1 + A2 Detected!";
                            }
                            if (Communication.A1MaximumValue == null)
                            {
                                Communication.A1MaximumValue = Communication.A1MeasuredValue;
                                txtA1MaximumValue.Text = Communication.A1MaximumValue;
                            }
                            if (Communication.A1MinimumValue == null)
                            {
                                Communication.A1MinimumValue = Communication.A1MeasuredValue;
                                txtA1MinimumValue.Text = Communication.A1MinimumValue;
                            }
                            if (float.Parse(Communication.A1MaximumValue) <= float.Parse(Communication.A1MeasuredValue))
                            {
                                Communication.A1MaximumValue = Communication.A1MeasuredValue;
                                txtA1MaximumValue.Text = Communication.A1MaximumValue;
                            }
                            if (float.Parse(Communication.A1MinimumValue) >= float.Parse(Communication.A1MeasuredValue))
                            {
                                Communication.A1MinimumValue = Communication.A1MeasuredValue;
                                txtA1MinimumValue.Text = Communication.A1MinimumValue;
                            }
                            saveA1BufferData();
                            chart1Display();
                            ClearAllData();
                        }
                    }
                    else if (!Communication.A1Detected || !Communication.A1RecevingData)
                    {
                        Communication.A1Detected = false;
                    }
                    else
                    {
                        getA1BufferData();
                        deleteA1BufferData();
                        Communication.A1Detected = false;
                        Communication.A1EnableSave = true;
                        Communication.A1enableStopTest = false;
                        tmrEnableReadA1Data.Enabled = false;
                        Communication.A1RecevingData = false;
                    }
                }
                if (Communication.serialData.Length != Communication.charNumberOfCom_data ||
                    !(Communication.serialData.Substring(0, 2) == "A1") ||
                    !(Communication.serialData.Substring(14, 2) == "A2") ||
                    !(Communication.serialData.Substring(11, 2) == "OK") &&
                    !(Communication.serialData.Substring(11, 2) == "NG"))
                {
                    flag1 = false;
                }
                else
                {
                    flag1 = Communication.serialData.Substring(25, 2) == "OK" || Communication.serialData.Substring(25, 2) == "NG";
                }
                if (flag1)
                {
                    Communication.A2MeasuredValue = Communication.serialData.Substring(17, 6);
                    Communication.A2Result = Communication.serialData.Substring(25, 2);
                    if (float.Parse(Communication.A2MeasuredValue) < float.Parse(Communication.A2DetectionLevel) - Communication.detectionOffset)
                    {
                        tmrEnableReadA2Data.Enabled = true;
                        if (!Communication.A2enableStopTest && Communication.A2RecevingData)
                        {
                            if (!tmrA2DetectRemoveObject.Enabled)
                            {
                                tmrA2DetectRemoveObject.Enabled = true;
                            }
                            Communication.A2Detected = true;
                            if (!Communication.A1Detected)
                            {
                                txtSystemMessage.Text = "A2 Detected!";
                            }
                            else
                            {
                                txtSystemMessage.Text = "A1 + A2 Detected!";
                            }
                            if (Communication.A2MaximumValue == null)
                            {
                                Communication.A2MaximumValue = Communication.A2MeasuredValue;
                                txtA2MaximumValue.Text = Communication.A2MaximumValue;
                            }
                            if (Communication.A2MinimumValue == null)
                            {
                                Communication.A2MinimumValue = Communication.A2MeasuredValue;
                                txtA2MinimumValue.Text = Communication.A2MinimumValue;
                            }
                            if (float.Parse(Communication.A2MaximumValue) <= float.Parse(Communication.A2MeasuredValue))
                            {
                                Communication.A2MaximumValue = Communication.A2MeasuredValue;
                                txtA2MaximumValue.Text = Communication.A2MaximumValue;
                            }
                            if (float.Parse(Communication.A2MinimumValue) >= float.Parse(Communication.A2MeasuredValue))
                            {
                                Communication.A2MinimumValue = Communication.A2MeasuredValue;
                                txtA2MinimumValue.Text = Communication.A2MinimumValue;
                            }
                            saveA2BufferData();
                            chart2Display();
                            ClearAllData();
                        }
                    }
                    else if (!Communication.A2Detected || !Communication.A2RecevingData)
                    {
                        Communication.A2Detected = false;
                    }
                    else
                    {
                        getA2BufferData();
                        deleteA2BufferData();
                        Communication.A2Detected = false;
                        Communication.A2EnableSave = true;
                        Communication.A2enableStopTest = false;
                        tmrEnableReadA2Data.Enabled = false;
                        Communication.A2RecevingData = false;
                    }
                }
                if (
                    Communication.receivedWeightFlg &&
                    Communication.A1EnableSave &&
                    Communication.A2EnableSave)
                {
                    CheckAndMakeDecision();
                }

                if (!Communication.A1Detected && !Communication.A2Detected)
                {
                    txtSystemMessage.Text = "None Object Detected!";
                }
            }
        }

        private void ClearAllData()
        {
            if (Communication.enableClearData)
            {
                Communication.enableClearData = false;
                chart1.Series.Clear();
                chart1Setting();
                chart2.Series.Clear();
                chart2Setting();
                //chartWeight.Series.Clear();
                //chartWeightSetting();
                txtA1Result.Text = "";
                txtA2Result.Text = "";
                //txtWeight.Text = "";
                txtWeightResult.Text = "";
                btnJudge.Text = "";
                controlAlarm_A1ResetAlarm();
                controlAlarm_A2ResetAlarm();
                if (!Communication.receivedWeightFlg) txtWeight.Text = "";
            }
        }

        private void CheckAndMakeDecision()
        {
            bool flag2;
            //Communication.Weight = this.txtWeight.Text.Trim();
            if (
                (!(this.txtWeightResult.Text == "OK") && !(this.txtWeightResult.Text == "NG")) ||
                !Communication.A1EnableSave || !Communication.A2EnableSave ||
                Communication.A1Detected || Communication.A2Detected ||
                !(txtA1Result.Text == "OK") && !(txtA1Result.Text == "NG"))
            {
                flag2 = false;
            }
            else
            {
                flag2 = txtA2Result.Text == "OK" || txtA2Result.Text == "NG";
            }
            if (flag2)
            {
                if (txtA1Result.Text != "OK" || txtA2Result.Text != "OK" || txtWeightResult.Text != "OK")
                {
                    Communication.Judge = "FAIL";
                    btnJudge.ForeColor = Color.Red;
                    Communication.totalFAIL++;
                    txtTotalFAIL.Text = Communication.totalFAIL.ToString();
                    if (chkStopScan.Checked)
                    {
                        stopWorking();
                    }
                }
                else
                {
                    Communication.Judge = "PASS";
                    btnJudge.ForeColor = Color.ForestGreen;
                    Communication.totalPASS++;
                    txtTotalPass.Text = Communication.totalPASS.ToString();
                    Communication.cntProductInSet++;
                    if (Communication.cntProductInSet >= this.numProductInSet.Value)
                    {
                        stopWorking();
                    }
                }
                Communication.A1Result = this.txtA1Result.Text;
                Communication.A2Result = this.txtA2Result.Text;
                tmrDisplayJudge.Enabled = true;
                Communication.totalProcessed++;
                txtTotalProcessed.Text = Communication.totalProcessed.ToString();
                Communication.ID = string.Concat("HL", Communication.totalProcessed);
                saveData();
                Communication.A1EnableSave = false;
                Communication.A2EnableSave = false;
                Communication.A1MaximumValue = null;
                Communication.A1MinimumValue = null;
                Communication.A2MaximumValue = null;
                Communication.A2MinimumValue = null;
                Communication.enableClearData = true;
                Communication.receivedWeightFlg = false;
                loadData();
                tmrEnableReadA1Data.Enabled = false;
                tmrEnableReadA2Data.Enabled = false;
                calculatePPandPPKvalue();
            }

        }

        private void stopWorking()
        {
            btnStart.Text = "Start";
            btnStart.ForeColor = Color.Teal;
            txtSystemMessage.Text = "STOPPED!";
            numProductInSet.Enabled = true;
            Communication.start = false;
            Communication.stop = true;
            Communication.enableReceiveData = false;
        }
        private void tmrEnableReadA1Data_Tick(object sender, EventArgs e)
        {
            tmrEnableReadA1Data.Enabled = false;
            Communication.A1RecevingData = true;
        }

        private void tmrEnableReadA2Data_Tick(object sender, EventArgs e)
        {
            tmrEnableReadA2Data.Enabled = false;
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
                tmrRefreshDataGridView.Enabled = false;
                loadData();
                chartA1Setting();
                chartA2Setting();
                chartWeightSetting();
                calculatePPandPPKvalue();
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

        private void dateTimeFilter_ValueChanged(object sender, EventArgs e)
        {
            string theDate = dateTimeFilter.Value.ToString("yyyy-MM-dd");
            this.loadData(theDate);
        }

        private void checkBoxFilterDate_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = checkBoxFilterDate.Checked;
            if (chk)
            {
                dateTimeFilter.Enabled = true;
                string theDate = dateTimeFilter.Value.ToString("yyyy-MM-dd");
                this.loadData(theDate);
            }
            else
            {
                dateTimeFilter.Enabled = false;
                this.loadData();
            }
        }
    }
}