using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SintegrationLib;
using System.Net.Sockets;
using System.Net;

namespace LoR_field_app
{
    
    class Client
    {
        //protected static Sintegration c_Sintegration = new Sintegration();
        private Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      public Client(string IP,int Port,byte[] DataForSending)
        {


          
        
       
                try
                {

                ///TCP&&Stream protocal
                //sck.Connect(IPAddress.Parse(IP), Port);
                //sck.Send(DataForSending, 0, DataForSending.Length, 0);
                //sck.Close();
                //sck.Dispose();

                ///UDP&&Dgram protocal
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), Port);
                sck.SendTo(DataForSending, ep);
                sck.Close();

                Console.WriteLine("Successful Sending"+DateTime.Now);
            }
            catch(Exception)
            {
                throw;
                //Console.WriteLine("Fail To Send Data" + DateTime.Now);
            }
            
        }

      //public static bool LoRaconnect()
      //  {
      //      try
      //      {
      //          c_Sintegration.Connect("192.168.0.101", 22);
      //          Console.WriteLine("LoRa GateWay successful connect");
      //          return true;
      //      }
      //      catch
      //      {
      //          Console.WriteLine("LoRa GateWay unable to connect(restry 10sec later)");
      //          return false;
               
      //      }
      //  }
 
}
}
