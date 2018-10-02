using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SintegrationLib;
using System.Timers;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;

namespace LoR_field_app
{
    class Program
    {
        //判斷form是否重複開啟
        static System.Threading.Mutex _mutex;     
        protected static Sintegration m_Sintegration = new Sintegration();
        private static Client c;
        static bool checkbool;
        static System.Timers.Timer t = new System.Timers.Timer();
        static System.Timers.Timer checkalive = new System.Timers.Timer();
        static void Main(string[] args)
        {
            
            bool createNew;           
            //獲取程式集Guid作為唯一標識
            Attribute guid_attr = Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(GuidAttribute));
            string guid = ((GuidAttribute)guid_attr).Value;
            _mutex = new System.Threading.Mutex(true, guid, out createNew);
            if (false == createNew)
            {
                Environment.Exit(Environment.ExitCode);
            }
            _mutex.ReleaseMutex();

            LoRaCnt();
                  
            m_Sintegration.OnDataReceived += new OnDataReceivedEventHandler(DataReceivedEvent);
            m_Sintegration.OnDisconnect += new OnDisconnectEventHandler(Disconnect);
            checkalive.Interval = 60000;
            checkalive.Elapsed += new ElapsedEventHandler(checkevent);
            checkalive.Start();
            Console.Read();
        }

        private static void checkevent(object sender, ElapsedEventArgs e)
        {
           if(checkbool)
            {
                LoRaCnt();
            }
        }

        private static void Disconnect(object sender, EventArgs e)
        {
            t.Stop();
            checkbool = true;
            Console.WriteLine("LoRa gateway connecting fail");

        }

        private static void DataReceivedEvent(object sender, DataReceiveEventArgs e)
        {

            GTK_UP_LINK LoRaData = e.GTK;
            byte[] buffer = ASCIIEncoding.Default.GetBytes(LoRaData.NODE_MAC+ LoRaData.DATA+DateTime.Now);
            //byte[] buffer = { 45, 24, 12, 12, 74, 41, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };
            c = new Client("125.227.111.240", 503, buffer);
        }
        private static void LoRaCnt()
        {
            if (LoRaconnect() == false)
            {
                Thread.Sleep(10000);
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
                LoRaCnt();
            }
            else
            {
                t.Interval = 3600000;
                t.Elapsed += new ElapsedEventHandler(OnTimeEvent);
                t.Start();
                checkbool = false;
            }
        }
        private static void ClearCurrentConsoleLine()
        {

            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    
        private static void OnTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.Clear();

        }
        public static bool LoRaconnect()
        {
            try
            {

                m_Sintegration.Connect("192.168.0.101", 22);
                Console.WriteLine("LoRa GateWay successful connect");
                return true;
                
                
            }
            catch
            {                
                Console.WriteLine("LoRa GateWay unable to connect(restry 10sec later)");
                return false;
                
            }
        }
    }
}
