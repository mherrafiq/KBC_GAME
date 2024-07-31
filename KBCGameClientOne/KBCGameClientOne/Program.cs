using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KBCGameClientOne
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient c = new TcpClient("192.168.0.103", 12000);
            handler h = new handler();
            Thread n = new Thread(() => h.read(c));
            n.Start();
            Thread n2 = new Thread(() => h.write(c));
            n2.Start();

        }
    }
    public class handler
    {
        public void read(TcpClient client)
        {
            while (true)
            {
                StreamReader reader = new StreamReader(client.GetStream());
                string n = reader.ReadLine();
                Console.WriteLine(n);
            }
        }
        public void write(TcpClient client)
        {
            while (true)
            {
                StreamWriter w = new StreamWriter(client.GetStream());
                string n = Console.ReadLine();
                DateTime dt = DateTime.Now;
                string time = dt.Ticks.ToString();
                w.WriteLine(n);
                w.Flush();
                w.WriteLine(time);
                w.Flush();
            }
        }
    }
}
