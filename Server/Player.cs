using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Server
{
    class Player
    {
        NetworkStream playersStream;
        StreamWriter playersWriter;
        StreamReader playersReader;

        public Player(TcpClient player)
        {
            playersStream = player.GetStream();
            playersWriter = new StreamWriter(playersStream);
            playersReader = new StreamReader(playersStream);
        }
        public void SendMessage(string message)
        {
            playersWriter.WriteLine(message);
            playersWriter.Flush();
        }

        public string GetMessage()
        {
            return  playersReader.ReadLine();
        }

        public void Close()
        {
            playersStream.Close();
        }

    }
}