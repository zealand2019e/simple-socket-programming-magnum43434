using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            Server.Start();
        }

    }

    class Server
    {
        public static void Start()
        {
            TcpListener server = null;
            int clientsConnected = 0;
            try
            {
                Int32 port = 7777;
                IPAddress localAddress = IPAddress.Loopback;

                server = new TcpListener(localAddress, port);

                server.Start();

                while (true)
                {
                    Console.WriteLine("Waiting for a connection");

                    TcpClient socket = server.AcceptTcpClient();
                    clientsConnected++;
                    Console.WriteLine($"Client {clientsConnected} has connected to the server");
                    Task.Run(() => { DoClient(socket ,$"Client {clientsConnected}"); });
                    //socket.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        private static async void DoClient(TcpClient socket, string client)
        {
            Byte[] bytes = new byte[256];
            string data = null;

            NetworkStream ns = socket.GetStream();

            int i;

            try
            {
                while (socket.Connected && (i = await ns.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine($"{client} received: {0}", data);

                    Byte[] msg = Encoding.ASCII.GetBytes(data);

                    await ns.WriteAsync(msg, 0, msg.Length);
                    Console.WriteLine($"{client} sent: {0}", data);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: {0}", e.Message);
                Console.WriteLine($"{client} has disconnected from the server");
                socket.Dispose();
                socket.Close();
            }
        }
    }
}
