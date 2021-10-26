using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ChessRules;

namespace Server
{
    class Server
    {
        static int NextMoves(int step, Chess chess)
        {
            if (step == 0)
                return 1;
            int count = 0;
            foreach (string moves in chess.YieldValidMoves())
                count += NextMoves(step - 1, chess.Move(moves));
            return count;
        }
        static string GetValidMoves(Chess chess)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string moves in chess.YieldValidMoves())
            {
                sb.AppendLine(moves);
            }
            return sb.ToString();
        }
        static string ChessToAscii(Chess chess)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  +-----------------+");
            for (int y = 7; y >= 0; y--)
            {
                sb.Append(y + 1);
                sb.Append(" | ");
                for (int x = 0; x < 8; x++)
                    sb.Append(chess.GetFigureAt(x, y) + " ");
                sb.AppendLine("| ");
            }
            sb.AppendLine("  +-----------------+");
            sb.AppendLine("    a b c d e f g h  ");
            if (chess.IsCheck)
                sb.AppendLine("IS CHECK");
            if (chess.IsCheckmate)
                sb.AppendLine("IS CHECKMATE");
            if (chess.IsStalemate)
                sb.AppendLine("IS STALEMATE");
            return sb.ToString();
        }
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
            foreach (var player in players)
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
                //IPAddress localAddr = IPAddress.Parse(args[0]);"
                //Int32 port = Int32.Parse(args[1]);
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, 8888);
                server.Start();

                TcpClient[] players = new TcpClient[2];
                NetworkStream[] playersStream = new NetworkStream[2];
                Chess chess = new Chess();



                players[0] = server.AcceptTcpClient();
                Console.WriteLine("Player1 connect to server");
                playersStream[0] = players[0].GetStream();

                SendMessage(playersStream[0], "Expect a second player");

                players[1] = server.AcceptTcpClient();
                Console.WriteLine("Player 2 connect to server");
                playersStream[1] = players[1].GetStream();
                SendMessage(playersStream, "Game ready");

                int i = 0;
                byte[] data = new byte[256];
                StringBuilder response = new StringBuilder();
                while (true)
                {
                    string board = ChessToAscii(chess);
                    string movesToSend = GetValidMoves(chess);

                    SendMessage(playersStream, board);
                    SendMessage(playersStream[i % 2], movesToSend);
                    Thread.Sleep(4000);
                    string move;
                    do
                    {
                        SendMessage(playersStream[i % 2], "Your move");
                        SendMessage(playersStream[i % 2], "");
                        playersStream[i % 2].Read(data, 0, data.Length);
                        move = Encoding.UTF8.GetString(data, 0, data.Length);
                        Console.WriteLine(move);
                    } while (!chess.IsValidMove(move));

                    if (move == "") break;
                    chess = chess.Move(move);

                    i++;

                }


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
