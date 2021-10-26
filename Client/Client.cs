using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPAddress localAddr = IPAddress.Parse(args[0]);
                Int32 port = Int32.Parse(args[1]);
                // Server connection
                TcpClient tcpClient = new TcpClient();
                // IP             Port
                tcpClient.Connect(localAddr, port);
                byte[] data = new byte[256];
                //StringBuilder response = new StringBuilder();
                NetworkStream netStream = tcpClient.GetStream();
                StreamReader reader = new StreamReader(netStream);
                StreamWriter writer = new StreamWriter(netStream);
                string info;
                do
                {
                    //Console.WriteLine(response.ToString());
                    do
                    {
                        info = reader.ReadLine();
                        Console.WriteLine(info);
                    } while (info != "Your move");

                        Console.WriteLine("Enter move: ");
                        string move = Console.ReadLine();
                        writer.WriteLine(move);
                        writer.Flush();
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
