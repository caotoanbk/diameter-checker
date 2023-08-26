using System;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Windows.Forms;

namespace Diameter_Checker
{
    public class Communication
    {
        public static int test;
        public static int test2;
        public static int charNumberOfCom_data;
        public static int charNumberOfCom_data2;
        public static int counter;
        public static int timer;
        public static int timer2;
        public static bool enableReceiveData;
        public static bool enableReadData;
        public static bool AutoReconnect;
        public static bool AutoReconnect2;
        public static bool start;
        public static bool stop;
        public static bool enableConnectToControlBox;
        public static bool refreshDataGridView;
        public static bool enableClearData;
        public static string comPort;
        public static string baudrate;
        public static string comPort2;
        public static string baudrate2;
        public static string serialData;
        public static string serialData2;
        public static int totalPASS;
        public static int cntProductInSet;
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
        public static string Weight;
        public static string A1PP;
        public static double A1PPK;
        public static double A1PPU;
        public static double A1PPL;
        public static string A2Average;
        public static double A2SD;
        public static string A2PP;
        public static double A2PPK;
        public static double A2PPU;
        public static double A2PPL;
        public static string WeightAverage;
        public static double WeightSD;
        public static string WeightPP;
        public static double WeightPPK;
        public static double WeightPPU;
        public static double WeightPPL;
        public static string loginUser;
        public static string processorID1;
        public static string processorID2;
        public static string processorID3;
        public static bool A1Detected;
        public static bool A2Detected;
        public static string A1DetectionLevel;
        public static string A2DetectionLevel;
        public static string A1MaximumOffset;
        public static string A1MinimumOffset;
        public static string A2MaximumOffset;
        public static string A2MinimumOffset;
        public static string maxWeight;
        public static string minWeight;
        public static float detectionOffset;
        public static bool closeComport;
        public static bool subformIsOpen;
        public static SerialPort serialport;
        public static SerialPort serialport2;
        public static SqlConnection connect;
        public static string con_string;
        public static bool receivedWeightFlg;
        public static double MIN_PPK = 1.33;
        public static double MAX_PPK = 1.67;

        static Communication()
        {
            test = 0;
            test2 = 0;
            charNumberOfCom_data = 27;
            charNumberOfCom_data2 = 19;
            counter = 0;
            timer = 0;
            timer2 = 0;
            enableReceiveData = true;
            enableReadData = false;
            AutoReconnect = true;
            AutoReconnect2 = true;
            start = false;
            stop = true;
            enableConnectToControlBox = false;
            refreshDataGridView = false;
            enableClearData = false;
            A1MeasuredValue = null;
            A1MaximumValue = null;
            A1MinimumValue = null;
            A1EnableSave = false;
            A2MeasuredValue = null;
            A2MaximumValue = null;
            A2MinimumValue = null;
            A2EnableSave = false;
            A1enableStopTest = false;
            A2enableStopTest = false;
            A1RecevingData = false;
            A2RecevingData = false;
            loginUser = "Admin";
            processorID1 = "BFEBFBFF000306C3";
            processorID2 = "BFEBFBFF000906E9";
            processorID3 = "BFEBFBFF000A0653";
            A1Detected = false;
            A2Detected = false;
            A1DetectionLevel = "10";
            A2DetectionLevel = "10";
            A1MaximumOffset = " ";
            A1MinimumOffset = " ";
            A2MaximumOffset = " ";
            A2MinimumOffset = " ";
            maxWeight = " ";
            minWeight = " ";
            detectionOffset = 1f;
            closeComport = false;
            subformIsOpen = false;
            serialport = new SerialPort();
            con_string = "Data Source=.\\SQLEXPRESS;Initial Catalog=DB1;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            serialport2 = new SerialPort();
            cntProductInSet = 0;
            receivedWeightFlg = false;
        }

        public Communication()
        {
        }

        public void clearReceiveData()
        {
            A1MeasuredValue = null;
            A1Result = null;
            A2MeasuredValue = null;
            A2Result = null;
        }

        public static bool connectDatabase()
        {
            try
            {
                connect = new SqlConnection(con_string);
                connect.Open();
            }
            catch
            {
                MessageBox.Show("Please check the connection to your Database!");
            }
            return true;
        }

        public static bool ConnectSerial(string comportName_, string baudrate)
        {
            serialport.BaudRate = Convert.ToInt32(baudrate);
            serialport.Parity = Parity.None;
            serialport.StopBits = StopBits.One;
            serialport.DataBits = 8;
            serialport.Handshake = Handshake.None;
            serialport.RtsEnable = true;
            serialport.PortName = comportName_;
            serialport.Open();
            return (serialport.IsOpen);
        }

        public static bool ConnectSerial2(string comportName_, string baudrate)
        {
            serialport2.BaudRate = Convert.ToInt32(baudrate);
            serialport2.Parity = Parity.None;
            serialport2.StopBits = StopBits.One;
            serialport2.DataBits = 8;
            serialport2.Handshake = Handshake.None;
            serialport2.RtsEnable = true;
            serialport2.PortName = comportName_;
            serialport2.Open();
            return (serialport2.IsOpen);
        }

        public static bool load_ComSetting()
        {
            connectDatabase();
            SqlDataReader myReader = new SqlCommand("SELECT * FROM ComportSetting", connect).ExecuteReader();
            while (myReader.Read())
            {
                comPort = myReader["comPort"].ToString();
                baudrate = myReader["baudrate"].ToString();
                comPort2 = myReader["comPort2"].ToString();
                baudrate2 = myReader["baudrate2"].ToString();
            }
            connect.Close();
            return true;
        }
    }
}