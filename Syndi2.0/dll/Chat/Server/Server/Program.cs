using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;

namespace server
{
    class Program
    {
        public static void Main()
        {
            Int32 port = 4323;
            IPAddress ip = IPAddress.Parse("192.168.1.100");

            TcpListener tcpListener = null;
            tcpListener = new TcpListener(ip, port);

            tcpListener.Start();
            Console.WriteLine(tcpListener.LocalEndpoint);
            Console.WriteLine("************This is Server program************");
            Console.WriteLine("Hoe many clients are going to connect to this server?:");
            int numberOfClientsYouNeedToConnect = int.Parse(Console.ReadLine());
            for (int i = 0; i < numberOfClientsYouNeedToConnect; i++)
            {
                //Thread newThread = new Thread(new ThreadStart(Listeners));
                Thread newThread = new Thread(() => Listeners(tcpListener));
                newThread.Start();
            }
        }
        static void Listeners(TcpListener tcpListener)
        {

            Socket socketForClient = tcpListener.AcceptSocket();
            if (socketForClient.Connected)
            {
                Console.WriteLine("Client:" + socketForClient.RemoteEndPoint + " now connected to server.");
                NetworkStream networkStream = new NetworkStream(socketForClient);
                System.IO.StreamWriter streamWriter =
                new System.IO.StreamWriter(networkStream);
                System.IO.StreamReader streamReader =
                new System.IO.StreamReader(networkStream);
                while (true)
                {
                    string theString = streamReader.ReadLine();
                    Console.WriteLine("Message recieved by client:" + theString);
                    if (theString == "exit")
                        break;
                }
                streamReader.Close();
                networkStream.Close();
                streamWriter.Close();
                //}

            }
            socketForClient.Close();
            Console.WriteLine("Press any key to exit from server program");
            Console.ReadKey();
        }
    }
}