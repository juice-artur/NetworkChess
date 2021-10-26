using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Collections.Generic;
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
                //StringBuilder response = new StringBuilder();
                NetworkStream netStream = tcpClient.GetStream();
                do
                {
                    //Console.WriteLine(response.ToString());
                    int nuberOfBytes = 2;
                    do
                    {
                        nuberOfBytes = netStream.Read(data, 0, data.Length); 
                        Console.WriteLine( Encoding.UTF8.GetString(data, 0, data.Length)); 
                        Console.WriteLine(nuberOfBytes);
                    }while(nuberOfBytes != 9);

                    Console.WriteLine("Enter move: ");
                    string move = Console.ReadLine();
                    byte[] bytes = Encoding.UTF8.GetBytes(move);
                    netStream.Write(bytes, 0, bytes.Length);
                    StreamWriter writer = new StreamWriter(move);
                } while (true);

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
