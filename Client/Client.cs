using System;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Server connection
                TcpClient tcpClient = new TcpClient();
                // IP             Port
                tcpClient.Connect("www.microsoft.com", 80);

                NetworkStream netStream = tcpClient.GetStream();

                

                
            }

            catch(SocketException socketException)
            {
                Console.WriteLine($"SocketException {socketException}");
            }

            catch(Exception e)
            {
                Console.WriteLine($"Exception {e}");
            }


        }
    }
}
