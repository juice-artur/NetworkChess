using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        static void SendMessage(Player[] players, string message)
        {
            foreach (var p in players)
            {
                p.SendMessage(message);

            }
        }
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse(args[0]);
                Int32 port = Int32.Parse(args[1]);

                server = new TcpListener(localAddr, port);
                server.Start();
                Console.WriteLine("Server started");
                Chess chess = new Chess();
                Player[] players = new Player[2];


                players[0] = new Player(server.AcceptTcpClient());
                players[0].SendMessage("Wait second player");
                Console.WriteLine("Player1 connect to server");

                players[1] = new Player(server.AcceptTcpClient());
                Console.WriteLine("Player 2 connect to server");
                SendMessage(players, "Game ready");
                int i = 0;
                while (!(chess.IsCheckmate && chess.IsStalemate))
                {
                    string board = ChessToAscii(chess);
                    string movesToSend = GetValidMoves(chess);

                    SendMessage(players, board);
                    players[i].SendMessage(movesToSend);
                    string move;
                    do
                    {
                        players[i].SendMessage("Your move");
                        move = players[i].GetMessage();
                        Console.WriteLine(move);

                    } while (!chess.IsValidMove(move));

                    chess = chess.Move(move);
                    i = i == 0 ? 1 : 0;

                }

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

        }
    }
}
