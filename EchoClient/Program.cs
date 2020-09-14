using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EchoClient
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello world!");

            Client.Start("localhost");
        }
    }

    class Client
    {
        public static void Start(string server)
        {
            try
            {
                Int32 port = 7777;
                TcpClient socket = new TcpClient(server, port);
                NetworkStream ns = null;
                string message = "";

                while (message != "exit")
                {
                    Console.Clear();
                    Console.WriteLine("Write dumbass:");

                    message = Console.ReadLine();

                    Byte[] data = Encoding.ASCII.GetBytes(message);

                    ns = socket.GetStream();

                    ns.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", message);

                    data = new Byte[256];

                    String responseData = String.Empty;

                    Int32 bytes = ns.Read(data, 0, data.Length);
                    responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", responseData);
                }
                ns.Close();
                socket.Close();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}
