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
        private ComboBox cmbComPort2;
        private ComboBox cmbBaudrate2;
        private Label label1;
        private Label label2;
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
            bool flag1 = false, flag2 = false;
            if(Communication.serialport.IsOpen && Communication.serialportA3.IsOpen)
            {
                MessageBox.Show("The COM Ports are already open!");
                return;
            }
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
                    flag1 = Communication.ConnectSerial(this.cmbComPort.Text, this.cmbBaudrate.Text);
                }
                catch
                {
                    MessageBox.Show("Failed! Please check your settings in port 1 and try again!");
                    this.lblConnectStatus.Text = "Not Connected";
                    this.lblConnectStatus.ForeColor = Color.Red;
                }
            }else {
                flag1 = true;
            }
            if (!Communication.serialportA3.IsOpen)
            {
                if (this.cmbBaudrate2.Text != "")
                {
                    Communication.baudrate2 = this.cmbBaudrate2.Text;
                }
                if (this.cmbComPort2.Text != "")
                {
                    Communication.comPort2 = this.cmbComPort2.Text;
                }
                try
                {
                    Communication.serialportA3.Close();
                    flag2 = Communication.ConnectSerialA3(this.cmbComPort2.Text, this.cmbBaudrate2.Text);
                }
                catch
                {
                    MessageBox.Show("Failed! Please check your settings in port 2 and try again!");
                    this.lblConnectStatus.Text = "Not Connected";
                    this.lblConnectStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                flag2 = true;
            }
            if (flag1 && flag2)
            {
                this.lblConnectStatus.Text = "Connected";
                this.lblConnectStatus.ForeColor = Color.Green;
                Communication.enableReceiveData = true;
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTest = new System.Windows.Forms.TextBox();
            this.chkDisplayAllData = new System.Windows.Forms.CheckBox();
            this.chkAutoReconnect = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbComPort2 = new System.Windows.Forms.ComboBox();
            this.cmbBaudrate2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Timer = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.Counter = new System.Windows.Forms.Label();
            this.txtTimer = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbComPort = new System.Windows.Forms.ComboBox();
            this.txtCounter = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.cmbBaudrate = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblConnectStatus = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtCommunicationData = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tmrDisplayData = new System.Windows.Forms.Timer(this.components);
            this.tmr1Second = new System.Windows.Forms.Timer(this.components);
            this.tmrDisconnectComPort = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTest);
            this.groupBox1.Controls.Add(this.chkDisplayAllData);
            this.groupBox1.Controls.Add(this.chkAutoReconnect);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.lblConnectStatus);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(0, 280);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(491, 167);
            this.groupBox1.TabIndex = 89;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settting";
            // 
            // txtTest
            // 
            this.txtTest.Location = new System.Drawing.Point(22, 78);
            this.txtTest.Name = "txtTest";
            this.txtTest.Size = new System.Drawing.Size(95, 20);
            this.txtTest.TabIndex = 92;
            this.txtTest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkDisplayAllData
            // 
            this.chkDisplayAllData.AutoSize = true;
            this.chkDisplayAllData.Location = new System.Drawing.Point(17, 59);
            this.chkDisplayAllData.Name = "chkDisplayAllData";
            this.chkDisplayAllData.Size = new System.Drawing.Size(100, 17);
            this.chkDisplayAllData.TabIndex = 92;
            this.chkDisplayAllData.Text = "Display All Data";
            this.chkDisplayAllData.UseVisualStyleBackColor = true;
            // 
            // chkAutoReconnect
            // 
            this.chkAutoReconnect.AutoSize = true;
            this.chkAutoReconnect.Checked = true;
            this.chkAutoReconnect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoReconnect.Location = new System.Drawing.Point(17, 40);
            this.chkAutoReconnect.Name = "chkAutoReconnect";
            this.chkAutoReconnect.Size = new System.Drawing.Size(104, 17);
            this.chkAutoReconnect.TabIndex = 91;
            this.chkAutoReconnect.Text = "Auto Reconnect";
            this.chkAutoReconnect.UseVisualStyleBackColor = true;
            this.chkAutoReconnect.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbComPort2);
            this.panel1.Controls.Add(this.cmbBaudrate2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(137, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 148);
            this.panel1.TabIndex = 86;
            // 
            // cmbComPort2
            // 
            this.cmbComPort2.FormattingEnabled = true;
            this.cmbComPort2.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15",
            "COM16",
            "COM17",
            "COM18",
            "COM19",
            "COM20"});
            this.cmbComPort2.Location = new System.Drawing.Point(243, 31);
            this.cmbComPort2.Name = "cmbComPort2";
            this.cmbComPort2.Size = new System.Drawing.Size(99, 21);
            this.cmbComPort2.TabIndex = 96;
            // 
            // cmbBaudrate2
            // 
            this.cmbBaudrate2.FormattingEnabled = true;
            this.cmbBaudrate2.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "115200"});
            this.cmbBaudrate2.Location = new System.Drawing.Point(243, 57);
            this.cmbBaudrate2.Name = "cmbBaudrate2";
            this.cmbBaudrate2.Size = new System.Drawing.Size(99, 21);
            this.cmbBaudrate2.TabIndex = 95;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(176, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 98;
            this.label1.Text = "COM Port 2:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(171, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 97;
            this.label2.Text = "Baud Rate 2:";
            // 
            // Timer
            // 
            this.Timer.AutoSize = true;
            this.Timer.Location = new System.Drawing.Point(12, 5);
            this.Timer.Name = "Timer";
            this.Timer.Size = new System.Drawing.Size(33, 13);
            this.Timer.TabIndex = 94;
            this.Timer.Text = "Timer";
            // 
            // btnExit
            // 
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(182, 113);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(76, 26);
            this.btnExit.TabIndex = 87;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Counter
            // 
            this.Counter.AutoSize = true;
            this.Counter.Location = new System.Drawing.Point(155, 5);
            this.Counter.Name = "Counter";
            this.Counter.Size = new System.Drawing.Size(44, 13);
            this.Counter.TabIndex = 92;
            this.Counter.Text = "Counter";
            this.Counter.Click += new System.EventHandler(this.Counter_Click);
            // 
            // txtTimer
            // 
            this.txtTimer.Location = new System.Drawing.Point(48, 2);
            this.txtTimer.Name = "txtTimer";
            this.txtTimer.Size = new System.Drawing.Size(95, 20);
            this.txtTimer.TabIndex = 93;
            this.txtTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnSave
            // 
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(182, 87);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 26);
            this.btnSave.TabIndex = 86;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbComPort
            // 
            this.cmbComPort.FormattingEnabled = true;
            this.cmbComPort.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15",
            "COM16",
            "COM17",
            "COM18",
            "COM19",
            "COM20"});
            this.cmbComPort.Location = new System.Drawing.Point(76, 31);
            this.cmbComPort.Name = "cmbComPort";
            this.cmbComPort.Size = new System.Drawing.Size(91, 21);
            this.cmbComPort.TabIndex = 79;
            // 
            // txtCounter
            // 
            this.txtCounter.Location = new System.Drawing.Point(200, 2);
            this.txtCounter.Name = "txtCounter";
            this.txtCounter.Size = new System.Drawing.Size(95, 20);
            this.txtCounter.TabIndex = 91;
            this.txtCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.ForeColor = System.Drawing.Color.Black;
            this.btnDisconnect.Location = new System.Drawing.Point(105, 113);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(76, 26);
            this.btnDisconnect.TabIndex = 85;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.button3_Click);
            // 
            // cmbBaudrate
            // 
            this.cmbBaudrate.FormattingEnabled = true;
            this.cmbBaudrate.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "115200"});
            this.cmbBaudrate.Location = new System.Drawing.Point(76, 57);
            this.cmbBaudrate.Name = "cmbBaudrate";
            this.cmbBaudrate.Size = new System.Drawing.Size(91, 21);
            this.cmbBaudrate.TabIndex = 78;
            // 
            // btnConnect
            // 
            this.btnConnect.ForeColor = System.Drawing.Color.Black;
            this.btnConnect.Location = new System.Drawing.Point(105, 87);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(76, 26);
            this.btnConnect.TabIndex = 84;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.button4_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(9, 36);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 13);
            this.label16.TabIndex = 81;
            this.label16.Text = "COM Port 1:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(4, 60);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 13);
            this.label13.TabIndex = 80;
            this.label13.Text = "Baud Rate 1:";
            // 
            // lblConnectStatus
            // 
            this.lblConnectStatus.AutoSize = true;
            this.lblConnectStatus.ForeColor = System.Drawing.Color.Red;
            this.lblConnectStatus.Location = new System.Drawing.Point(60, 21);
            this.lblConnectStatus.Name = "lblConnectStatus";
            this.lblConnectStatus.Size = new System.Drawing.Size(79, 13);
            this.lblConnectStatus.TabIndex = 82;
            this.lblConnectStatus.Text = "Not Connected";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(14, 21);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 83;
            this.lblStatus.Text = "Status:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtCommunicationData);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(491, 447);
            this.panel2.TabIndex = 91;
            // 
            // txtCommunicationData
            // 
            this.txtCommunicationData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommunicationData.Location = new System.Drawing.Point(0, 0);
            this.txtCommunicationData.Multiline = true;
            this.txtCommunicationData.Name = "txtCommunicationData";
            this.txtCommunicationData.Size = new System.Drawing.Size(491, 280);
            this.txtCommunicationData.TabIndex = 90;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tmrDisplayData
            // 
            this.tmrDisplayData.Enabled = true;
            this.tmrDisplayData.Interval = 1;
            this.tmrDisplayData.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // tmr1Second
            // 
            this.tmr1Second.Enabled = true;
            this.tmr1Second.Interval = 1000;
            this.tmr1Second.Tick += new System.EventHandler(this.tmr1Second_Tick);
            // 
            // tmrDisconnectComPort
            // 
            this.tmrDisconnectComPort.Interval = 10;
            this.tmrDisconnectComPort.Tick += new System.EventHandler(this.tmrDisconnectComPort_Tick);
            // 
            // ComSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(491, 447);
            this.Controls.Add(this.panel2);
            this.Name = "ComSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Communication Setting";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.COM_FormClosed);
            this.Load += new System.EventHandler(this.COM_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        private void lstCommunicationData_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public void SaveDataToDB()
        {
            SqlConnection con = new SqlConnection(Communication.con_string);
            con.Open();
            SqlCommand sql_cmd = new SqlCommand("UPDATE ComportSetting SET comPort = @comPort, baudrate = @baudrate, comPort2 = @comport2, baudrate2 = @baudrate2", con);
            sql_cmd.Parameters.AddWithValue("@comPort", Communication.comPort);
            sql_cmd.Parameters.AddWithValue("@baudrate", Communication.baudrate);
            sql_cmd.Parameters.AddWithValue("@comPort2", Communication.comPort2);
            sql_cmd.Parameters.AddWithValue("@baudrate2", Communication.baudrate2);
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
            this.cmbComPort2.Text = Communication.comPort2;
            this.cmbBaudrate2.Text = Communication.baudrate2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Communication.serialport.IsOpen || !Communication.serialportA3.IsOpen)
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
                Communication.serialportA3.DtrEnable = false;
                Communication.serialportA3.RtsEnable = false;
                if (Communication.serialportA3.IsOpen)
                {
                    Communication.serialportA3.DiscardInBuffer();
                    Communication.serialportA3.DiscardOutBuffer();
                    Communication.serialportA3.Dispose();
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
            Communication.comPort2 = this.cmbComPort2.Text;
            Communication.baudrate2 = this.cmbBaudrate2.Text;
        }
    }
}