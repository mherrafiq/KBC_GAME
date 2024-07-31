using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace KBCGameServer
{
    class Program
    {
        public static List<TcpClient> clientList = new List<TcpClient>();
        public static int clientCount = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("KBC Game Server");
            clientHandler handle = new clientHandler();
            TcpListener listen = null;
            try
            {
                listen = new TcpListener(IPAddress.Parse("192.168.0.103"),12000);
                listen.Start();
                while (clientList.Count < 3)
                {
                    TcpClient client = listen.AcceptTcpClient();
                    clientCount++;
                    clientList.Add(client);

                    Thread n2 = new Thread(() => handle.handle(client, clientCount));
                    n2.Start();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public static void broadcast(string ques, int id)
        {
            foreach (TcpClient item in clientList)
            {
                StreamWriter writer = new StreamWriter(item.GetStream());
                writer.WriteLine("Question: "+ques);
                writer.Flush();
            }           
        }
        public static void pointsUpdate(string points)
        {
            foreach (TcpClient item in clientList)
            {
                StreamWriter writer = new StreamWriter(item.GetStream());
                writer.WriteLine("First to 5 points will win!");
                writer.Flush();
                writer.WriteLine(points);
                writer.Flush();
            }
        }
    }
}
