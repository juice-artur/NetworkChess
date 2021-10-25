using System;
using System.Net;
using System.Net.Sockets;
using ChessRules;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // IPAddres and port for incoming connections
                IPAddress localAddr = IPAddress.Parse(args[0]);
                Int32 port = Int32.Parse(args[1]);

                
                server = new TcpListener(localAddr, port);
                server.Start();

                TcpClient[] players = new TcpClient[2];

                players[0] = server.AcceptTcpClient();
                Console.WriteLine("Player1 connect to server");


            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

        }
    }
}
