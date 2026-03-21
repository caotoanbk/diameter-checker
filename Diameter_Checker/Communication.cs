using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;


namespace Diameter_Checker
{
    /// <summary>
    /// Holds the result of parsing a single weight frame from the scale's serial output.
    /// When changing the scale machine, update ParseWeightFrame() and these constants only.
    /// </summary>
    public struct WeightReading
    {
        /// <summary>Parsed weight value in the same unit as the min/max settings (e.g. grams).</summary>
        public float Value;
        /// <summary>Raw weight string stored to the database.</summary>
        public string RawText;
        /// <summary>True when the frame was valid and Value was successfully parsed.</summary>
        public bool IsValid;
    }

    public class Communication
    {
        // ── Weight-scale serial protocol constants ────────────────────────────
        // Change these when swapping the scale machine:
        //   FrameHeader       : leading marker that identifies a stable-reading frame
        //   FrameLength       : total expected character count of one frame
        //   ValueOffset       : byte index where the numeric value starts inside the frame
        //   ValueLength       : number of characters that make up the numeric value
        //   UnitSuffix        : unit string appended by the scale (stripped before parsing)
        //   ValueScaleDivisor : divisor applied after stripping decimals (e.g. "0123.4g" -> 1234 / 10)
        private const string WeightFrameHeader    = " ST,GS,+";
        private const int    WeightFrameLength    = 19;   // == charNumberOfCom_data2
        private const int    WeightValueOffset    = 8;
        private const int    WeightValueLength    = 11;
        private const string WeightUnitSuffix     = "g";
        private const float  WeightValueScaleDivisor = 10f;

        /// <summary>
        /// Parses a raw serial frame from the weight scale.
        /// All machine-specific logic is isolated here — change only this method
        /// (and the constants above) when swapping the scale hardware.
        /// </summary>
        /// <param name="raw">One complete frame as received from the serial port.</param>
        /// <returns>A <see cref="WeightReading"/> with IsValid=false on any parse failure.</returns>
        public static WeightReading ParseWeightFrame(string raw)
        {
            if (raw == null || raw.Length < WeightFrameLength ||
                !raw.StartsWith(WeightFrameHeader))
                return new WeightReading { IsValid = false };

            string valueStr = raw.Substring(WeightValueOffset, WeightValueLength).Trim();
            valueStr = valueStr.Replace(WeightUnitSuffix, "").Trim();

            float numericValue;
            if (!float.TryParse(valueStr.Replace(".", ""), out numericValue))
                return new WeightReading { IsValid = false };

            return new WeightReading
            {
                Value   = numericValue / WeightValueScaleDivisor,
                RawText = valueStr,
                IsValid = true
            };
        }
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
        public static double D1Value=0, D2Value=0;
        public static string loginUser;
        public static List<string> listProcessorId;
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
        public static double MIN_PPK = 1;
        public static double MAX_PPK = 1.67;
        public static double MAX_D1 = 0.02;
        public static double MAX_D2 = 0.02;

        public static bool isStartPassword = false;
        public static bool isPreviousNG = false;

        static Communication()
        {
            Weight = "0";
            test = 0;
            test2 = 0;
            charNumberOfCom_data  = 27;
            charNumberOfCom_data2 = WeightFrameLength;
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
            //con_string = "Data Source=.;Initial Catalog=DB1;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";//--DEBUG
            con_string = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;
            serialport2 = new SerialPort();
            cntProductInSet = 0;
            receivedWeightFlg = false;
            listProcessorId = new List<string> { "BFEBFBFF000306C3", "BFEBFBFF000906E9", "BFEBFBFF000A0653" };

            string configPath = @"config.ini";
            if (File.Exists(configPath))
            {
                var lines = File.ReadAllLines(configPath);
                for (var i = 0; i < lines.Length; i += 1)
                {
                    listProcessorId.Add(Base64Decode(lines[i]));
                }
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
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

        /// <summary>Creates and opens a new SqlConnection. Caller is responsible for disposing it.</summary>
        public static SqlConnection CreateConnection()
        {
            var conn = new SqlConnection(con_string);
            conn.Open();
            return conn;
        }

        public static bool connectDatabase()
        {
            try
            {
                if (connect != null && connect.State == System.Data.ConnectionState.Open)
                    return true;

                connect = new SqlConnection(con_string);
                connect.Open();
                return true;
            }
            catch
            {
                MessageBox.Show("Please check the connection to your Database!");
                return false;
            }
        }

        private static bool ConnectSerialPort(SerialPort port, string comportName, string baudrate)
        {
            port.BaudRate = int.Parse(baudrate);
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.DataBits = 8;
            port.Handshake = Handshake.None;
            port.RtsEnable = true;
            port.PortName = comportName;
            port.Open();
            return port.IsOpen;
        }

        public static bool ConnectSerial(string comportName_, string baudrate)
        {
            return ConnectSerialPort(serialport, comportName_, baudrate);
        }

        public static bool ConnectSerial2(string comportName_, string baudrate)
        {
            return ConnectSerialPort(serialport2, comportName_, baudrate);
        }

        public static bool load_ComSetting()
        {
            try
            {
                using (var con = CreateConnection())
                using (var cmd = new SqlCommand("SELECT comPort, baudrate, comPort2, baudrate2 FROM ComportSetting", con))
                using (SqlDataReader myReader = cmd.ExecuteReader())
                {
                    while (myReader.Read())
                    {
                        comPort   = myReader["comPort"].ToString();
                        baudrate  = myReader["baudrate"].ToString();
                        comPort2  = myReader["comPort2"].ToString();
                        baudrate2 = myReader["baudrate2"].ToString();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}