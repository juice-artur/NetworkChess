using System;
using System.Net.Sockets;
using System.Text;

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
                tcpClient.Connect("127.0.0.1", 8888);
                 byte[] data = new byte[256];
                StringBuilder response = new StringBuilder();
                NetworkStream netStream = tcpClient.GetStream();
                int bytes = netStream.Read(data, 0, data.Length);
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));

                Console.WriteLine(response.ToString());

            }

            catch (SocketException socketException)
            {
                Console.WriteLine($"SocketException {socketException}");
            }

            catch (Exception e)
            {
                Console.WriteLine($"Exception {e}");
            }


        }
    }
}
