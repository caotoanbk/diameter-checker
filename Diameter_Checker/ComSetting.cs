using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace Diameter_Checker
{
    public class ComSetting : Form
    {
        private IContainer components = null;

        private GroupBox groupBox1;

        private Button btnDisconnect;

        private Button btnConnect;

        private Label lblConnectStatus;

        private Label lblStatus;

        private Label label13;

        private Label label16;

        private ComboBox cmbBaudrate;

        private ComboBox cmbComPort;

        private Panel panel2;

        private Panel panel1;

        private Timer timer1;

        private Button btnExit;

        private Button btnSave;

        private Timer tmrDisplayData;

        private CheckBox chkAutoReconnect;

        private Timer tmr1Second;

        private Label Timer;

        private TextBox txtTimer;

        private Label Counter;

        private TextBox txtCounter;

        private CheckBox chkDisplayAllData;

        private TextBox txtCommunicationData;

        private Timer tmrDisconnectComPort;

        private TextBox txtTest;

        public ComSetting()
        {
            this.InitializeComponent();
        }

        private void btnDefaut_Click(object sender, EventArgs e)
        {
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Communication.subformIsOpen = false;
            base.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.updateData();
            this.SaveDataToDB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!this.chkAutoReconnect.Checked)
            {
                try
                {
                    Communication.serialport.DtrEnable = false;
                    Communication.serialport.RtsEnable = false;
                    if (Communication.serialport.IsOpen)
                    {
                        Communication.serialport.DiscardInBuffer();
                        Communication.serialport.DiscardOutBuffer();
                        Communication.serialport.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Error! Please Uncheck AutoReconnect.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!this.chkAutoReconnect.Checked)
            {
                Communication.enableReceiveData = false;
                this.tmrDisplayData.Enabled = false;
                this.tmrDisconnectComPort.Enabled = true;
            }
            else
            {
                MessageBox.Show("Error! Please Uncheck AutoReconnect.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!Communication.serialport.IsOpen)
            {
                if (this.cmbBaudrate.Text != "")
                {
                    Communication.baudrate = this.cmbBaudrate.Text;
                }
                if (this.cmbComPort.Text != "")
                {
                    Communication.comPort = this.cmbComPort.Text;
                }
                try
                {
                    Communication.serialport.Close();
                    if (Communication.ConnectSerial(this.cmbComPort.Text, this.cmbBaudrate.Text))
                    {
                        this.lblConnectStatus.Text = "Connected";
                        this.lblConnectStatus.ForeColor = Color.Green;
                        Communication.enableReceiveData = true;
                    }
                }
                catch
                {
                    MessageBox.Show("Failed! Please check your settings and try again!");
                    this.lblConnectStatus.Text = "Not Connected";
                    this.lblConnectStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                MessageBox.Show("The COM Port is already open!");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chkAutoReconnect.Checked)
            {
                Communication.AutoReconnect = false;
            }
            else
            {
                Communication.AutoReconnect = true;
            }
        }

        private void COM_FormClosed(object sender, FormClosedEventArgs e)
        {
            Communication.subformIsOpen = false;
            base.Dispose();
        }

        private void COM_Load(object sender, EventArgs e)
        {
            this.show_data();
            if (!Communication.AutoReconnect)
            {
                this.chkAutoReconnect.Checked = false;
            }
            else
            {
                this.chkAutoReconnect.Checked = true;
            }
            Communication.timer = 0;
            Communication.counter = 0;
            Communication.subformIsOpen = true;
            if (Communication.loginUser != "Developer")
            {
                this.txtCommunicationData.Visible = false;
            }
            else
            {
                this.txtCommunicationData.Visible = true;
            }
            Communication.test = 0;
        }

        private void Counter_Click(object sender, EventArgs e)
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

        private void InitializeComponent()
        {
            this.components = new Container();
            this.groupBox1 = new GroupBox();
            this.txtTest = new TextBox();
            this.chkDisplayAllData = new CheckBox();
            this.chkAutoReconnect = new CheckBox();
            this.panel1 = new Panel();
            this.Timer = new Label();
            this.btnExit = new Button();
            this.Counter = new Label();
            this.txtTimer = new TextBox();
            this.btnSave = new Button();
            this.cmbComPort = new ComboBox();
            this.txtCounter = new TextBox();
            this.btnDisconnect = new Button();
            this.cmbBaudrate = new ComboBox();
            this.btnConnect = new Button();
            this.label16 = new Label();
            this.label13 = new Label();
            this.lblConnectStatus = new Label();
            this.lblStatus = new Label();
            this.panel2 = new Panel();
            this.txtCommunicationData = new TextBox();
            this.timer1 = new Timer(this.components);
            this.tmrDisplayData = new Timer(this.components);
            this.tmr1Second = new Timer(this.components);
            this.tmrDisconnectComPort = new Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.txtTest);
            this.groupBox1.Controls.Add(this.chkDisplayAllData);
            this.groupBox1.Controls.Add(this.chkAutoReconnect);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.lblConnectStatus);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Dock = DockStyle.Bottom;
            this.groupBox1.ForeColor = Color.Black;
            this.groupBox1.Location = new Point(0, 346);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(491, 103);
            this.groupBox1.TabIndex = 89;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settting";
            this.txtTest.Location = new Point(22, 78);
            this.txtTest.Name = "txtTest";
            this.txtTest.Size = new Size(95, 20);
            this.txtTest.TabIndex = 92;
            this.txtTest.TextAlign = HorizontalAlignment.Right;
            this.chkDisplayAllData.AutoSize = true;
            this.chkDisplayAllData.Location = new Point(17, 59);
            this.chkDisplayAllData.Name = "chkDisplayAllData";
            this.chkDisplayAllData.Size = new Size(100, 17);
            this.chkDisplayAllData.TabIndex = 92;
            this.chkDisplayAllData.Text = "Display All Data";
            this.chkDisplayAllData.UseVisualStyleBackColor = true;
            this.chkAutoReconnect.AutoSize = true;
            this.chkAutoReconnect.Checked = true;
            this.chkAutoReconnect.CheckState = CheckState.Checked;
            this.chkAutoReconnect.Location = new Point(17, 40);
            this.chkAutoReconnect.Name = "chkAutoReconnect";
            this.chkAutoReconnect.Size = new Size(104, 17);
            this.chkAutoReconnect.TabIndex = 91;
            this.chkAutoReconnect.Text = "Auto Reconnect";
            this.chkAutoReconnect.UseVisualStyleBackColor = true;
            this.chkAutoReconnect.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.panel1.Controls.Add(this.Timer);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.Counter);
            this.panel1.Controls.Add(this.txtTimer);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.cmbComPort);
            this.panel1.Controls.Add(this.txtCounter);
            this.panel1.Controls.Add(this.btnDisconnect);
            this.panel1.Controls.Add(this.cmbBaudrate);
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Dock = DockStyle.Right;
            this.panel1.Location = new Point(137, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(351, 84);
            this.panel1.TabIndex = 86;
            this.Timer.AutoSize = true;
            this.Timer.Location = new Point(65, 5);
            this.Timer.Name = "Timer";
            this.Timer.Size = new Size(33, 13);
            this.Timer.TabIndex = 94;
            this.Timer.Text = "Timer";
            this.btnExit.ForeColor = Color.Black;
            this.btnExit.Location = new Point(274, 55);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(76, 26);
            this.btnExit.TabIndex = 87;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.Counter.AutoSize = true;
            this.Counter.Location = new Point(208, 5);
            this.Counter.Name = "Counter";
            this.Counter.Size = new Size(44, 13);
            this.Counter.TabIndex = 92;
            this.Counter.Text = "Counter";
            this.Counter.Click += new EventHandler(this.Counter_Click);
            this.txtTimer.Location = new Point(101, 2);
            this.txtTimer.Name = "txtTimer";
            this.txtTimer.Size = new Size(95, 20);
            this.txtTimer.TabIndex = 93;
            this.txtTimer.TextAlign = HorizontalAlignment.Right;
            this.btnSave.ForeColor = Color.Black;
            this.btnSave.Location = new Point(274, 29);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(76, 26);
            this.btnSave.TabIndex = 86;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.cmbComPort.FormattingEnabled = true;
            this.cmbComPort.Items.AddRange(new object[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10", "COM11", "COM12", "COM13", "COM14", "COM15", "COM16", "COM17", "COM18", "COM19", "COM20" });
            this.cmbComPort.Location = new Point(70, 31);
            this.cmbComPort.Name = "cmbComPort";
            this.cmbComPort.Size = new Size(121, 21);
            this.cmbComPort.TabIndex = 79;
            this.txtCounter.Location = new Point(253, 2);
            this.txtCounter.Name = "txtCounter";
            this.txtCounter.Size = new Size(95, 20);
            this.txtCounter.TabIndex = 91;
            this.txtCounter.TextAlign = HorizontalAlignment.Right;
            this.btnDisconnect.ForeColor = Color.Black;
            this.btnDisconnect.Location = new Point(197, 55);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new Size(76, 26);
            this.btnDisconnect.TabIndex = 85;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new EventHandler(this.button3_Click);
            this.cmbBaudrate.FormattingEnabled = true;
            this.cmbBaudrate.Items.AddRange(new object[] { "1200", "2400", "4800", "9600", "19200", "115200" });
            this.cmbBaudrate.Location = new Point(70, 57);
            this.cmbBaudrate.Name = "cmbBaudrate";
            this.cmbBaudrate.Size = new Size(121, 21);
            this.cmbBaudrate.TabIndex = 78;
            this.btnConnect.ForeColor = Color.Black;
            this.btnConnect.Location = new Point(197, 29);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new Size(76, 26);
            this.btnConnect.TabIndex = 84;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new EventHandler(this.button4_Click);
            this.label16.AutoSize = true;
            this.label16.ForeColor = Color.Black;
            this.label16.Location = new Point(11, 36);
            this.label16.Name = "label16";
            this.label16.Size = new Size(56, 13);
            this.label16.TabIndex = 81;
            this.label16.Text = "COM Port:";
            this.label13.AutoSize = true;
            this.label13.ForeColor = Color.Black;
            this.label13.Location = new Point(6, 60);
            this.label13.Name = "label13";
            this.label13.Size = new Size(61, 13);
            this.label13.TabIndex = 80;
            this.label13.Text = "Baud Rate:";
            this.lblConnectStatus.AutoSize = true;
            this.lblConnectStatus.ForeColor = Color.Red;
            this.lblConnectStatus.Location = new Point(60, 21);
            this.lblConnectStatus.Name = "lblConnectStatus";
            this.lblConnectStatus.Size = new Size(79, 13);
            this.lblConnectStatus.TabIndex = 82;
            this.lblConnectStatus.Text = "Not Connected";
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = Color.Black;
            this.lblStatus.Location = new Point(14, 21);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(40, 13);
            this.lblStatus.TabIndex = 83;
            this.lblStatus.Text = "Status:";
            this.panel2.Controls.Add(this.txtCommunicationData);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(491, 449);
            this.panel2.TabIndex = 91;
            this.txtCommunicationData.Dock = DockStyle.Fill;
            this.txtCommunicationData.Location = new Point(0, 0);
            this.txtCommunicationData.Multiline = true;
            this.txtCommunicationData.Name = "txtCommunicationData";
            this.txtCommunicationData.Size = new Size(491, 346);
            this.txtCommunicationData.TabIndex = 90;
            this.timer1.Enabled = true;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.tmrDisplayData.Enabled = true;
            this.tmrDisplayData.Interval = 1;
            this.tmrDisplayData.Tick += new EventHandler(this.timer2_Tick);
            this.tmr1Second.Enabled = true;
            this.tmr1Second.Interval = 1000;
            this.tmr1Second.Tick += new EventHandler(this.tmr1Second_Tick);
            this.tmrDisconnectComPort.Interval = 10;
            this.tmrDisconnectComPort.Tick += new EventHandler(this.tmrDisconnectComPort_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.ControlLightLight;
            base.ClientSize = new Size(491, 449);
            base.Controls.Add(this.panel2);
            base.Name = "ComSetting";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Communication Setting";
            base.FormClosed += new FormClosedEventHandler(this.COM_FormClosed);
            base.Load += new EventHandler(this.COM_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lstCommunicationData_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public void SaveDataToDB()
        {
            SqlConnection con = new SqlConnection(Communication.con_string);
            con.Open();
            SqlCommand sql_cmd = new SqlCommand("UPDATE ComportSetting SET comPort = @comPort, baudrate = @baudrate", con);
            sql_cmd.Parameters.AddWithValue("@comPort", Communication.comPort);
            sql_cmd.Parameters.AddWithValue("@baudrate", Communication.baudrate);
            try
            {
                sql_cmd.ExecuteNonQuery();
            }
            catch
            {
            }
            con.Close();
        }

        public void SetText(string text)
        {
            this.txtCommunicationData.Text = text;
        }

        private void show_data()
        {
            this.cmbComPort.Text = Communication.comPort;
            this.cmbBaudrate.Text = Communication.baudrate;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Communication.serialport.IsOpen)
            {
                this.lblConnectStatus.Text = "Not Connected";
                this.lblConnectStatus.ForeColor = Color.Red;
                this.tmr1Second.Enabled = false;
                this.tmrDisplayData.Enabled = false;
            }
            else
            {
                this.lblConnectStatus.Text = "Connected";
                this.lblConnectStatus.ForeColor = Color.Green;
                this.tmr1Second.Enabled = true;
                this.tmrDisplayData.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Communication.enableReadData)
            {
                Communication.A1MeasuredValue = Communication.serialData.Substring(3, 6);
                Communication.A1Result = Communication.serialData.Substring(11, 2);
                Communication.A2MeasuredValue = Communication.serialData.Substring(17, 6);
                Communication.A2Result = Communication.serialData.Substring(25, 2);
                Communication.enableReadData = false;
                if (this.chkDisplayAllData.Checked)
                {
                    TextBox textBox = this.txtCommunicationData;
                    textBox.Text = string.Concat(textBox.Text, "\n", Communication.serialData, "\r");
                }
                else
                {
                    this.txtCommunicationData.Text = string.Concat(Communication.serialData, "\r");
                }
                Communication.counter++;
                this.txtCounter.Text = Communication.counter.ToString();
            }
            this.txtTest.Text = Communication.test.ToString();
        }

        private void tmr1Second_Tick(object sender, EventArgs e)
        {
            Communication.timer++;
            this.txtTimer.Text = Communication.timer.ToString();
        }

        private void tmrDisconnectComPort_Tick(object sender, EventArgs e)
        {
            this.tmrDisconnectComPort.Enabled = false;
            try
            {
                Communication.serialport.DtrEnable = false;
                Communication.serialport.RtsEnable = false;
                if (Communication.serialport.IsOpen)
                {
                    Communication.serialport.DiscardInBuffer();
                    Communication.serialport.DiscardOutBuffer();
                    Communication.serialport.Dispose();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void updateData()
        {
            Communication.comPort = this.cmbComPort.Text;
            Communication.baudrate = this.cmbBaudrate.Text;
        }
    }
}