using ClosedXML.Excel;
using MOIE= Microsoft.Office.Interop.Excel;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Diameter_Checker
{
    public class ExportData : Form
    {
        private static string fromDate;

        private static string toDate;

        private string stringCommand;

        private string cmdSearch;

        public static string strExcelString;

        public static string strdirectory;

        public static string strfileName;

        private IContainer components = null;

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

        private GroupBox groupBox1;

        private GroupBox groupBox2;

        private ComboBox cmbDirectory;

        private Label label6;

        private Button button2;

        private Button btnClearSearch;

        private Button btnSearch;

        private ComboBox cmbModel;

        private Label label1;

        private ComboBox cmbFromYear;

        private Label label3;

        private GroupBox groupBox3;

        private ComboBox cmbFromMonth;

        private ComboBox cmbFromDay;

        private Label label4;

        private GroupBox groupBox4;

        private ComboBox cmbToDay;

        private ComboBox cmbToMonth;

        private Label label5;

        private Label label7;

        private ComboBox cmbToYear;

        private Label label8;

        private Button btnExportToExcel;

        private Button btnDeleteSearch;

        public ExportData()
        {
            this.InitializeComponent();
        }

        private void btnDeleteSearch_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete data?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if ((this.cmbModel.Text == null || this.cmbFromYear.Text == null || this.cmbFromMonth.Text == null || this.cmbFromDay.Text == null || this.cmbToYear.Text == null || this.cmbToMonth.Text == null ? false : this.cmbToDay.Text != null))
                {
                    ExportData.fromDate = string.Concat(new string[] { this.cmbFromYear.Text, "-", this.cmbFromMonth.Text, "-", this.cmbFromDay.Text });
                    ExportData.toDate = string.Concat(new string[] { this.cmbToYear.Text, "-", this.cmbToMonth.Text, "-", this.cmbToDay.Text });
                    try
                    {
                        if ((!(this.cmbModel.Text != "") || !(this.cmbFromYear.Text != "") || !(this.cmbFromMonth.Text != "") || !(this.cmbFromDay.Text != "") || !(this.cmbToYear.Text != "") || !(this.cmbToMonth.Text != "") ? false : this.cmbToDay.Text != ""))
                        {
                            this.stringCommand = string.Concat(new string[] { "DELETE from Data WHERE model='", this.cmbModel.Text, "'and Date >= convert(date,'", ExportData.fromDate, "',23) and Date <= convert(date,'", ExportData.toDate, "',23)" });
                        }
                        if ((!(this.cmbModel.Text == "ALL") || !(this.cmbFromYear.Text != "") || !(this.cmbFromMonth.Text != "") || !(this.cmbFromDay.Text != "") || !(this.cmbToYear.Text != "") || !(this.cmbToMonth.Text != "") ? false : this.cmbToDay.Text != ""))
                        {
                            this.stringCommand = string.Concat(new string[] { "DELETE from Data WHERE Date >= convert(date,'", ExportData.fromDate, "',23) and Date <= convert(date,'", ExportData.toDate, "',23)" });
                        }
                        if ((!(this.cmbModel.Text == "ALL") || !(this.cmbFromYear.Text == "") || !(this.cmbFromMonth.Text == "") || !(this.cmbFromDay.Text == "") || !(this.cmbToYear.Text == "") || !(this.cmbToMonth.Text == "") ? false : this.cmbToDay.Text == ""))
                        {
                            this.stringCommand = "DELETE from Data";
                        }
                        if ((!(this.cmbModel.Text != "ALL") || !(this.cmbFromYear.Text == "") || !(this.cmbFromMonth.Text == "") || !(this.cmbFromDay.Text == "") || !(this.cmbToYear.Text == "") || !(this.cmbToMonth.Text == "") ? false : this.cmbToDay.Text == ""))
                        {
                            this.stringCommand = string.Concat("DELETE from Data WHERE model='", this.cmbModel.Text, "'");
                        }
                        string cmdSearch = this.stringCommand;
                        Communication.connect.Open();
                        SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(cmdSearch, Communication.connect));
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        this.dataGridView1.DataSource = dt;
                    }
                    catch (SystemException systemException)
                    {
                        SystemException ex = systemException;
                        MessageBox.Show(string.Format("An error occurred: {0}", ex.Message));
                    }
                    Communication.connect.Close();
                }
                try
                {
                    string cmdDisplayNothing = "SELECT * from Data WHERE model=' '";
                    Communication.connect.Open();
                    SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(cmdDisplayNothing, Communication.connect));
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    this.dataGridView1.DataSource = dt;
                }
                catch (SystemException systemException1)
                {
                }
                Communication.connect.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if ((this.cmbModel.Text == null || this.cmbFromYear.Text == null || this.cmbFromMonth.Text == null || this.cmbFromDay.Text == null || this.cmbToYear.Text == null || this.cmbToMonth.Text == null ? false : this.cmbToDay.Text != null))
            {
                ExportData.fromDate = string.Concat(new string[] { this.cmbFromYear.Text, "-", this.cmbFromMonth.Text, "-", this.cmbFromDay.Text });
                ExportData.toDate = string.Concat(new string[] { this.cmbToYear.Text, "-", this.cmbToMonth.Text, "-", this.cmbToDay.Text });
                try
                {
                    if ((!(this.cmbModel.Text != "") || !(this.cmbFromYear.Text != "") || !(this.cmbFromMonth.Text != "") || !(this.cmbFromDay.Text != "") || !(this.cmbToYear.Text != "") || !(this.cmbToMonth.Text != "") ? false : this.cmbToDay.Text != ""))
                    {
                        this.stringCommand = string.Concat(new string[] { "SELECT * from Data WHERE model='", this.cmbModel.Text, "'and Date >= convert(date,'", ExportData.fromDate, "',23) and Date <= convert(date,'", ExportData.toDate, "',23) order by CAST(substring(ID,3,10) as int)" });
                    }
                    if ((!(this.cmbModel.Text == "ALL") || !(this.cmbFromYear.Text != "") || !(this.cmbFromMonth.Text != "") || !(this.cmbFromDay.Text != "") || !(this.cmbToYear.Text != "") || !(this.cmbToMonth.Text != "") ? false : this.cmbToDay.Text != ""))
                    {
                        this.stringCommand = string.Concat(new string[] { "SELECT * from Data WHERE Date >= convert(date,'", ExportData.fromDate, "',23) and Date <= convert(date,'", ExportData.toDate, "',23) order by CAST(substring(ID,3,10) as int)" });
                    }
                    if ((!(this.cmbModel.Text == "ALL") || !(this.cmbFromYear.Text == "") || !(this.cmbFromMonth.Text == "") || !(this.cmbFromDay.Text == "") || !(this.cmbToYear.Text == "") || !(this.cmbToMonth.Text == "") ? false : this.cmbToDay.Text == ""))
                    {
                        this.stringCommand = "SELECT * from Data order by CAST(substring(ID,3,10) as int)";
                    }
                    if ((!(this.cmbModel.Text != "ALL") || !(this.cmbFromYear.Text == "") || !(this.cmbFromMonth.Text == "") || !(this.cmbFromDay.Text == "") || !(this.cmbToYear.Text == "") || !(this.cmbToMonth.Text == "") ? false : this.cmbToDay.Text == ""))
                    {
                        this.stringCommand = string.Concat("SELECT * from Data WHERE model='", this.cmbModel.Text, "' order by CAST(substring(ID,3,10) as int)");
                    }
                    string cmdSearch = this.stringCommand;
                    Communication.connect.Open();
                    SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(cmdSearch, Communication.connect));
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    this.dataGridView1.DataSource = dt;
                    Communication.connect.Close();
                    this.RowsColor();
                }
                catch
                {
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ExportToExcelWithFormatting(this.dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Communication.refreshDataGridView = true;
            base.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            try
            {
                string cmdDisplayNothing = "SELECT * from Data WHERE model=' '";
                Communication.connect.Open();
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(cmdDisplayNothing, Communication.connect));
                DataTable dt = new DataTable();
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
            }
            catch (SystemException systemException)
            {
                SystemException ex = systemException;
                MessageBox.Show(string.Format("An error occurred: {0}", ex.Message));
            }
            Communication.connect.Close();
            this.cmbModel.Text = "ALL";
            this.cmbFromYear.Text = null;
            this.cmbFromMonth.Text = null;
            this.cmbFromDay.Text = null;
            this.cmbToYear.Text = null;
            this.cmbToMonth.Text = null;
            this.cmbToDay.Text = null;
        }

        private void cmbDirectory_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void export2Excel(DataGridView g, string directory, string fileName)
        {
            MOIE.Application obj = (MOIE.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
            obj.Application.Workbooks.Add(Type.Missing);
            obj.Columns.ColumnWidth = 15;
            for (int i = 1; i < g.Columns.Count + 1; i++)
            {
                obj.Cells[1, i] = g.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < g.Rows.Count; i++)
            {
                for (int j = 0; j < g.Columns.Count; j++)
                {
                    if (g.Rows[i].Cells[j].Value != null)
                    {
                        obj.Cells[i + 2, j + 1] = g.Rows[i].Cells[j].Value.ToString();
                        ExportData.strExcelString = g.Rows[i].Cells[j].Value.ToString();
                        try
                        {
                        }
                        catch
                        {
                        }
                    }
                }
            }
            obj.ActiveWorkbook.SaveCopyAs(string.Concat(directory, fileName, ".xlsx"));
            obj.ActiveWorkbook.Saved = true;
        }

        private void ExportData_FormClosed(object sender, FormClosedEventArgs e)
        {
            Communication.refreshDataGridView = true;
            base.Dispose();
        }

        private void ExportData_Load(object sender, EventArgs e)
        {
            this.cmbFromYear.Text = null;
            this.cmbFromMonth.Text = null;
            this.cmbFromDay.Text = null;
            this.cmbToYear.Text = null;
            this.cmbToMonth.Text = null;
            this.cmbToDay.Text = null;
            this.loadProductSetting();
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
        }

        public void ExportToExcelWithFormatting(DataGridView dataGridView1)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog()
            {
                Filter = "xls files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "To Excel"
            };
            string text = this.Text;
            DateTime now = DateTime.Now;
            saveFileDialog1.FileName = string.Concat(text, " (", now.ToString("yyyy-MM-dd"), ")");
            saveFileDialog1.InitialDirectory = ExportData.strdirectory;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                XLWorkbook workbook = new XLWorkbook();
                IXLWorksheet worksheet = workbook.Worksheets.Add(this.Text);
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dataGridView1.Columns[i].Name;
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        try
                        {
                            worksheet.Cell(i + 2, j + 1).Value = dataGridView1.Rows[i].Cells[j].Value.ToString().Trim();
                        }
                        catch
                        {
                        }
                        if (worksheet.Cell(i + 2, j + 1).Value.ToString().Length > 0)
                        {
                            worksheet.Cell(i + 2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(i + 2, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(i + 2, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(i + 2, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(i + 2, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i + 2, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(i + 2, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(i + 2, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i + 2, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i + 2, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i + 2, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i + 2, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(i + 2, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(i + 2, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            XLColor xlColor = XLColor.FromColor(Color.Red);
                            worksheet.Cell(i + 2, j + 1).AddConditionalFormat().WhenEquals("NG").Fill.SetBackgroundColor(xlColor);
                            worksheet.Cell(i + 2, j + 1).AddConditionalFormat().WhenEquals("FAIL").Fill.SetBackgroundColor(xlColor);
                            worksheet.Cell(i + 2, j + 1).Style.Font.FontName = dataGridView1.Font.Name;
                            worksheet.Cell(i + 2, j + 1).Style.Font.FontSize = (double)dataGridView1.Font.Size;
                        }
                    }
                }
                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(fileName);
            }
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
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
            this.groupBox1 = new GroupBox();
            this.cmbFromDay = new ComboBox();
            this.cmbFromMonth = new ComboBox();
            this.label4 = new Label();
            this.label1 = new Label();
            this.cmbFromYear = new ComboBox();
            this.label3 = new Label();
            this.groupBox2 = new GroupBox();
            this.cmbDirectory = new ComboBox();
            this.label6 = new Label();
            this.button2 = new Button();
            this.btnClearSearch = new Button();
            this.btnSearch = new Button();
            this.cmbModel = new ComboBox();
            this.groupBox3 = new GroupBox();
            this.groupBox4 = new GroupBox();
            this.cmbToDay = new ComboBox();
            this.cmbToMonth = new ComboBox();
            this.label5 = new Label();
            this.label7 = new Label();
            this.cmbToYear = new ComboBox();
            this.label8 = new Label();
            this.btnExportToExcel = new Button();
            this.btnDeleteSearch = new Button();
            ((ISupportInitialize)this.dataGridView1).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            base.SuspendLayout();
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = AnchorStyles.Right;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = SystemColors.HighlightText;
            this.dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(0, 0, 192);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.GrayText;
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { this.ID, this.model, this.A1MaxValue, this.A1MinValue, this.A1Result, this.A2MaxValue, this.A2MinValue, this.A2Result, this.Date, this.Time, this.Judge, this.TotalProcessed, this.TotalPASS, this.TotalFAIL });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.Red;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.GridColor = Color.White;
            this.dataGridView1.ImeMode = ImeMode.NoControl;
            this.dataGridView1.Location = new Point(0, 2);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.Red;
            dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = Color.White;
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle7.SelectionForeColor = Color.LightGray;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.Size = new Size(1904, 945);
            this.dataGridView1.TabIndex = 3;
            this.ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.ID.DataPropertyName = "ID";
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            this.ID.DefaultCellStyle = dataGridViewCellStyle3;
            this.ID.FillWeight = 120f;
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 100;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.model.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.model.DataPropertyName = "model";
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            this.model.DefaultCellStyle = dataGridViewCellStyle4;
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
            this.A1MinValue.DataPropertyName = "A1MinValue";
            this.A1MinValue.HeaderText = "A1 Min Value";
            this.A1MinValue.MinimumWidth = 130;
            this.A1MinValue.Name = "A1MinValue";
            this.A1MinValue.ReadOnly = true;
            this.A1Result.DataPropertyName = "A1Result";
            this.A1Result.FillWeight = 80f;
            this.A1Result.HeaderText = "A1 Result";
            this.A1Result.MinimumWidth = 80;
            this.A1Result.Name = "A1Result";
            this.A2MaxValue.DataPropertyName = "A2MaxValue";
            this.A2MaxValue.HeaderText = "A2 Max Value";
            this.A2MaxValue.MinimumWidth = 130;
            this.A2MaxValue.Name = "A2MaxValue";
            this.A2MinValue.DataPropertyName = "A2MinValue";
            this.A2MinValue.HeaderText = "A2 Min Value";
            this.A2MinValue.MinimumWidth = 130;
            this.A2MinValue.Name = "A2MinValue";
            this.A2Result.DataPropertyName = "A2Result";
            this.A2Result.FillWeight = 80f;
            this.A2Result.HeaderText = "A2 Result";
            this.A2Result.MinimumWidth = 80;
            this.A2Result.Name = "A2Result";
            this.Date.DataPropertyName = "Date";
            this.Date.FillWeight = 80f;
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 80;
            this.Date.Name = "Date";
            this.Time.DataPropertyName = "Time";
            this.Time.FillWeight = 80f;
            this.Time.HeaderText = "Time";
            this.Time.MinimumWidth = 80;
            this.Time.Name = "Time";
            this.Judge.DataPropertyName = "Judge";
            this.Judge.FillWeight = 80f;
            this.Judge.HeaderText = "Judge";
            this.Judge.MinimumWidth = 80;
            this.Judge.Name = "Judge";
            this.TotalProcessed.DataPropertyName = "TotalProcessed";
            this.TotalProcessed.FillWeight = 120f;
            this.TotalProcessed.HeaderText = "Total Processed";
            this.TotalProcessed.MinimumWidth = 100;
            this.TotalProcessed.Name = "TotalProcessed";
            this.TotalProcessed.Resizable = DataGridViewTriState.False;
            this.TotalPASS.DataPropertyName = "TotalPASS";
            this.TotalPASS.FillWeight = 120f;
            this.TotalPASS.HeaderText = "Total PASS";
            this.TotalPASS.MinimumWidth = 100;
            this.TotalPASS.Name = "TotalPASS";
            this.TotalPASS.Resizable = DataGridViewTriState.False;
            this.TotalFAIL.DataPropertyName = "TotalFAIL";
            this.TotalFAIL.FillWeight = 120f;
            this.TotalFAIL.HeaderText = "Total FAIL";
            this.TotalFAIL.MinimumWidth = 100;
            this.TotalFAIL.Name = "TotalFAIL";
            this.TotalFAIL.Resizable = DataGridViewTriState.False;
            this.groupBox1.Controls.Add(this.cmbFromDay);
            this.groupBox1.Controls.Add(this.cmbFromMonth);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbFromYear);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox1.Location = new Point(142, 948);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(405, 52);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "From";
            this.cmbFromDay.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbFromDay.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbFromDay.FormattingEnabled = true;
            this.cmbFromDay.Items.AddRange(new object[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" });
            this.cmbFromDay.Location = new Point(317, 18);
            this.cmbFromDay.Name = "cmbFromDay";
            this.cmbFromDay.Size = new Size(83, 28);
            this.cmbFromDay.TabIndex = 7;
            this.cmbFromMonth.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbFromMonth.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbFromMonth.FormattingEnabled = true;
            this.cmbFromMonth.Items.AddRange(new object[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" });
            this.cmbFromMonth.Location = new Point(187, 18);
            this.cmbFromMonth.Name = "cmbFromMonth";
            this.cmbFromMonth.Size = new Size(83, 28);
            this.cmbFromMonth.TabIndex = 5;
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label4.Location = new Point(280, 24);
            this.label4.Name = "label4";
            this.label4.Size = new Size(33, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Day";
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label1.Location = new Point(9, 24);
            this.label1.Name = "label1";
            this.label1.Size = new Size(37, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Year";
            this.cmbFromYear.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbFromYear.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbFromYear.FormattingEnabled = true;
            this.cmbFromYear.Items.AddRange(new object[] { "2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029", "2030", "2031", "2032", "2033", "2034", "2035", "2036", "2037", "2038" });
            this.cmbFromYear.Location = new Point(51, 18);
            this.cmbFromYear.Name = "cmbFromYear";
            this.cmbFromYear.Size = new Size(83, 28);
            this.cmbFromYear.TabIndex = 3;
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label3.Location = new Point(141, 24);
            this.label3.Name = "label3";
            this.label3.Size = new Size(44, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Month";
            this.groupBox2.Controls.Add(this.cmbDirectory);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox2.Location = new Point(1360, 948);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(273, 52);
            this.groupBox2.TabIndex = 80;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Directory";
            this.cmbDirectory.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbDirectory.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbDirectory.FormattingEnabled = true;
            this.cmbDirectory.Items.AddRange(new object[] { "D:\\", "E:\\", "F:\\", "D:\\Data\\", "E:\\Data\\", "F:\\Data\\", "D:\\Data\\Backup\\", "E:\\Data\\Backup\\", "F:\\Data\\Backup\\" });
            this.cmbDirectory.Location = new Point(13, 18);
            this.cmbDirectory.Name = "cmbDirectory";
            this.cmbDirectory.Size = new Size(227, 28);
            this.cmbDirectory.TabIndex = 1;
            this.cmbDirectory.Text = "D:\\";
            this.cmbDirectory.Visible = false;
            this.cmbDirectory.SelectedIndexChanged += new EventHandler(this.cmbDirectory_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label6.Location = new Point(18, 22);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0, 16);
            this.label6.TabIndex = 0;
            this.button2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.button2.ForeColor = Color.DarkRed;
            this.button2.Location = new Point(1769, 959);
            this.button2.Name = "button2";
            this.button2.Size = new Size(130, 37);
            this.button2.TabIndex = 82;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.btnClearSearch.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.btnClearSearch.ForeColor = Color.Teal;
            this.btnClearSearch.Location = new Point(1093, 959);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new Size(130, 37);
            this.btnClearSearch.TabIndex = 84;
            this.btnClearSearch.Text = "Clear";
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new EventHandler(this.button3_Click);
            this.btnSearch.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.btnSearch.ForeColor = Color.Teal;
            this.btnSearch.Location = new Point(963, 959);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(130, 37);
            this.btnSearch.TabIndex = 83;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.cmbModel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbModel.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new Point(9, 18);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new Size(120, 28);
            this.cmbModel.TabIndex = 1;
            this.cmbModel.Text = "ALL";
            this.groupBox3.Controls.Add(this.cmbModel);
            this.groupBox3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox3.Location = new Point(1, 948);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(136, 52);
            this.groupBox3.TabIndex = 85;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Model";
            this.groupBox4.BackColor = SystemColors.Control;
            this.groupBox4.Controls.Add(this.cmbToDay);
            this.groupBox4.Controls.Add(this.cmbToMonth);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.cmbToYear);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.groupBox4.Location = new Point(552, 948);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(405, 52);
            this.groupBox4.TabIndex = 86;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "To";
            this.cmbToDay.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbToDay.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbToDay.FormattingEnabled = true;
            this.cmbToDay.Items.AddRange(new object[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" });
            this.cmbToDay.Location = new Point(315, 18);
            this.cmbToDay.Name = "cmbToDay";
            this.cmbToDay.Size = new Size(83, 28);
            this.cmbToDay.TabIndex = 7;
            this.cmbToMonth.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbToMonth.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbToMonth.FormattingEnabled = true;
            this.cmbToMonth.Items.AddRange(new object[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" });
            this.cmbToMonth.Location = new Point(185, 18);
            this.cmbToMonth.Name = "cmbToMonth";
            this.cmbToMonth.Size = new Size(83, 28);
            this.cmbToMonth.TabIndex = 5;
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label5.Location = new Point(278, 24);
            this.label5.Name = "label5";
            this.label5.Size = new Size(33, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Day";
            this.label7.AutoSize = true;
            this.label7.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label7.Location = new Point(7, 24);
            this.label7.Name = "label7";
            this.label7.Size = new Size(37, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "Year";
            this.cmbToYear.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbToYear.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbToYear.FormattingEnabled = true;
            this.cmbToYear.Items.AddRange(new object[] { "2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029", "2030", "2031", "2032", "2033", "2034", "2035", "2036", "2037", "2038" });
            this.cmbToYear.Location = new Point(49, 18);
            this.cmbToYear.Name = "cmbToYear";
            this.cmbToYear.Size = new Size(83, 28);
            this.cmbToYear.TabIndex = 3;
            this.label8.AutoSize = true;
            this.label8.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label8.Location = new Point(139, 24);
            this.label8.Name = "label8";
            this.label8.Size = new Size(44, 16);
            this.label8.TabIndex = 4;
            this.label8.Text = "Month";
            this.btnExportToExcel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.btnExportToExcel.ForeColor = Color.Teal;
            this.btnExportToExcel.Location = new Point(1639, 959);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new Size(130, 37);
            this.btnExportToExcel.TabIndex = 87;
            this.btnExportToExcel.Text = "Export to Excel";
            this.btnExportToExcel.UseVisualStyleBackColor = true;
            this.btnExportToExcel.Click += new EventHandler(this.button1_Click);
            this.btnDeleteSearch.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.btnDeleteSearch.ForeColor = Color.DarkRed;
            this.btnDeleteSearch.Location = new Point(1223, 959);
            this.btnDeleteSearch.Name = "btnDeleteSearch";
            this.btnDeleteSearch.Size = new Size(130, 37);
            this.btnDeleteSearch.TabIndex = 88;
            this.btnDeleteSearch.Text = "Delete Data";
            this.btnDeleteSearch.UseVisualStyleBackColor = true;
            this.btnDeleteSearch.Click += new EventHandler(this.btnDeleteSearch_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(1904, 1002);
            base.Controls.Add(this.btnDeleteSearch);
            base.Controls.Add(this.btnExportToExcel);
            base.Controls.Add(this.groupBox4);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.btnClearSearch);
            base.Controls.Add(this.btnSearch);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.dataGridView1);
            base.Name = "ExportData";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ExportData";
            base.FormClosed += new FormClosedEventHandler(this.ExportData_FormClosed);
            base.Load += new EventHandler(this.ExportData_Load);
            ((ISupportInitialize)this.dataGridView1).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            base.ResumeLayout(false);
        }

        private void loadProductSetting()
        {
            try
            {
                this.cmbModel.Text = null;
                Communication.connect.Open();
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("SELECT model FROM ProductSetting", Communication.connect));
                DataSet dt = new DataSet();
                da.Fill(dt);
                this.cmbModel.DataSource = dt.Tables[0];
                this.cmbModel.ValueMember = "model";
                this.cmbModel.Text = "ALL";
                Communication.connect.Close();
            }
            catch
            {
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
    }
}