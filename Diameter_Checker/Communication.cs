using System;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Windows.Forms;

namespace Diameter_Checker
{
    public class Communication
    {
        public static int test;

        public static int charNumberOfCom_data;

        public static int counter;

        public static int timer;

        public static bool enableReceiveData;

        public static bool enableReadData;

        public static bool AutoReconnect;

        public static bool start;

        public static bool stop;

        public static bool enableConnectToControlBox;

        public static bool refreshDataGridView;

        public static bool enableClearData;

        public static string comPort;

        public static string baudrate;

        public static string serialData;

        public static int totalPASS;

        public static int totalFAIL;

        public static int totalProcessed;

        public static string ID;

        public static string model;

        public static string Date;

        public static string Time;

        public static string Judge;

        public static string A1MeasuredValue;

        public static string A1MaximumValue;

        public static string A1MinimumValue;

        public static string A1Result;

        public static bool A1EnableSave;

        public static string A2MeasuredValue;

        public static string A2MaximumValue;

        public static string A2MinimumValue;

        public static string A2Result;

        public static bool A2EnableSave;

        public static bool A1enableStopTest;

        public static bool A2enableStopTest;

        public static bool A1RecevingData;

        public static bool A2RecevingData;

        public static string A1Average;

        public static double A1SD;

        public static string A1PP;

        public static string A1PPK;

        public static double A1PPU;

        public static double A1PPL;

        public static string A2Average;

        public static double A2SD;

        public static string A2PP;

        public static string A2PPK;

        public static double A2PPU;

        public static double A2PPL;

        public static string loginUser;

        public static string processorIDAdmin;

        public static string processorID;

        public static bool A1Detected;

        public static bool A2Detected;

        public static string A1DetectionLevel;

        public static string A2DetectionLevel;

        public static string A1MaximumOffset;

        public static string A1MinimumOffset;

        public static string A2MaximumOffset;

        public static string A2MinimumOffset;

        public static float detectionOffset;

        public static bool closeComport;

        public static bool subformIsOpen;

        public static SerialPort serialport;

        public static SqlConnection connect;

        public static string con_string;

        static Communication()
        {
            Communication.test = 0;
            Communication.charNumberOfCom_data = 27;
            Communication.counter = 0;
            Communication.timer = 0;
            Communication.enableReceiveData = true;
            Communication.enableReadData = false;
            Communication.AutoReconnect = true;
            Communication.start = false;
            Communication.stop = true;
            Communication.enableConnectToControlBox = false;
            Communication.refreshDataGridView = false;
            Communication.enableClearData = false;
            Communication.A1MeasuredValue = null;
            Communication.A1MaximumValue = null;
            Communication.A1MinimumValue = null;
            Communication.A1EnableSave = false;
            Communication.A2MeasuredValue = null;
            Communication.A2MaximumValue = null;
            Communication.A2MinimumValue = null;
            Communication.A2EnableSave = false;
            Communication.A1enableStopTest = false;
            Communication.A2enableStopTest = false;
            Communication.A1RecevingData = false;
            Communication.A2RecevingData = false;
            Communication.loginUser = "Admin";
            Communication.processorIDAdmin = "BFEBFBFF000306C3";
            Communication.processorID = "BFEBFBFF000906E9";
            Communication.A1Detected = false;
            Communication.A2Detected = false;
            Communication.A1DetectionLevel = "10";
            Communication.A2DetectionLevel = "10";
            Communication.A1MaximumOffset = " ";
            Communication.A1MinimumOffset = " ";
            Communication.A2MaximumOffset = " ";
            Communication.A2MinimumOffset = " ";
            Communication.detectionOffset = 1f;
            Communication.closeComport = false;
            Communication.subformIsOpen = false;
            Communication.serialport = new SerialPort();
            Communication.con_string = "Data Source=.\\SQLEXPRESS;Initial Catalog=DB1;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
        }

        public Communication()
        {
        }

        public void clearReceiveData()
        {
            Communication.A1MeasuredValue = null;
            Communication.A1Result = null;
            Communication.A2MeasuredValue = null;
            Communication.A2Result = null;
        }

        public static bool connectDatabase()
        {
            try
            {
                Communication.connect = new SqlConnection(Communication.con_string);
                Communication.connect.Open();
            }
            catch
            {
                MessageBox.Show("Please check the connection to your Database!");
            }
            return true;
        }

        public static bool ConnectSerial(string comportName_, string baudrate)
        {
            Communication.serialport.BaudRate = Convert.ToInt32(baudrate);
            Communication.serialport.Parity = Parity.None;
            Communication.serialport.StopBits = StopBits.One;
            Communication.serialport.DataBits = 8;
            Communication.serialport.Handshake = Handshake.None;
            Communication.serialport.RtsEnable = true;
            Communication.serialport.PortName = comportName_;
            Communication.serialport.Open();
            return (!Communication.serialport.IsOpen ? false : true);
        }

        public static bool load_ComSetting()
        {
            Communication.connectDatabase();
            SqlDataReader myReader = null;
            myReader = (new SqlCommand("SELECT * FROM ComportSetting", Communication.connect)).ExecuteReader();
            while (myReader.Read())
            {
                Communication.comPort = myReader["comPort"].ToString();
                Communication.baudrate = myReader["baudrate"].ToString();
            }
            Communication.connect.Close();
            return true;
        }
    }
}