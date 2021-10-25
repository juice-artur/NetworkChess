using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ChessRules;

namespace Server
{
    class Server
    {
        static byte[] MessageToByte(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }
        static void SendMessage(NetworkStream player, string message)
        {
            byte[] data = MessageToByte(message);
            player.Write(data, 0, data.Length);
        }

        static void SendMessage(NetworkStream[] players, string message)
        {
            byte[] data = MessageToByte(message);
            foreach(var player in players)
            {
                player.Write(data, 0, data.Length);
            }
        }
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
                NetworkStream[] playersStream = new NetworkStream[2];

                players[0] = server.AcceptTcpClient();
                Console.WriteLine("Player1 connect to server");
                playersStream[0] = players[0].GetStream();

                SendMessage(playersStream[0], "Expect a second player");

                players[1] = server.AcceptTcpClient();
                Console.WriteLine("Player 2 connect to server");
                playersStream[1] = players[1].GetStream();


                SendMessage(playersStream, "Game ready");

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
