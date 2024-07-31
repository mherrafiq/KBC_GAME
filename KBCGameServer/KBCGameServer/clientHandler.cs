using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KBCGameServer
{
    public class clientHandler
    {
        static string[] questions = new string[9] {"What is the capital of Chile?",
                                             "What is the largest country in the world?",
                                             "Who won the FIFA World Cup in 2018?",
                                             "What is the capital of Westeros in Game of Thrones?",
                                             "What is David Bowie’s real name?",
                                             "Vietnamese is an official language in Canada",
                                              "Who painted the Mona Lisa?",
                                             "Which planet is closest to the sun?",
                                             "What is Scooby Doo's full name?"};
        static string[] answers = new string[9] { "Santiago", "Russia", "France", "King's Landing", "David Jones", "False", "Leonardo da Vinci", "Venus", "Scoobert Doo" };
        static Random r = new Random();
        int randNum = r.Next(0, 9);
        public void handle(TcpClient c, int id)
        {
            int c1points = 0, c2points = 0, c3points = 0;
            string c1answer, c2answer, c3answer;
            string c1t, c2t, c3t;
            long c1time=0, c2time=0, c3time=0;
            StreamReader reader = new StreamReader(c.GetStream());
            StreamWriter writer = new StreamWriter(c.GetStream());
            while (true)
            {
                Thread n = new Thread(() => Program.broadcast(questions[randNum], Program.clientCount));
                n.Start();
                if (Program.clientList[0].Connected)
                {
                    c1answer = reader.ReadLine();
                    c1t = reader.ReadLine();
                    c1time = Convert.ToInt64(c1t);
                    if (c1answer == answers[randNum])
                    {
                        writer.WriteLine("Your answer is correct");
                        writer.Flush();
                    }
                    else
                    {
                        writer.WriteLine("Your answer is wrong");
                        writer.Flush();
                    }
                }
                if (Program.clientList.Count > 1)
                {
                    if (Program.clientList[1].Connected)
                    {
                        c2answer = reader.ReadLine();
                        c2t = reader.ReadLine();
                        c2time = Convert.ToInt64(c2t);
                        if (c2answer == answers[randNum])
                        {
                            writer.WriteLine("Your answer is correct");
                            writer.Flush();
                        }
                        else
                        {
                            writer.WriteLine("Your answer is wrong");
                            writer.Flush();
                        }
                    }
                    if (Program.clientList.Count > 2)
                    if (Program.clientList[2].Connected)
                    {
                        c3answer = reader.ReadLine();
                        c3t = reader.ReadLine();
                        c3time = Convert.ToInt64(c3t);
                        if (c3answer == answers[randNum])
                        {
                            writer.WriteLine("Your answer is correct");
                            writer.Flush();
                        }
                        else
                        {
                            writer.WriteLine("Your answer is wrong");
                            writer.Flush();
                        }
                    }
                }
                if (c1time < c2time && c1time < c3time)
                {
                    writer.WriteLine("Client 1 is the fastest to answer!");
                    c1points++;
                }
                else if (c2time < c1time && c2time < c3time)
                {
                    writer.WriteLine("Client 2 is the fastest to answer!");
                    c2points++;
                }
                else if (c3time < c1time && c3time < c2time)
                {
                    writer.WriteLine("Client 3 is the fastest to answer!");
                    c3points++;
                }
                else
                {
                    c1points++;
                    c2points++;
                    c3points++;
                }
                string points = "Client 1:" + c1points.ToString() + "Points \n" + "Client 2:" + c2points.ToString() + "Points \n" + "Client 3:" + c3points.ToString() + "Points \n";
                Program.pointsUpdate(points);
                randNum = r.Next(0, 9);
                if (c1points == 5)
                {
                    writer.WriteLine("Client 1 Won!!!");
                    writer.Flush();
                    break;
                }
                else if (c2points == 5)
                {
                    writer.WriteLine("Client 2 Won!!!");
                    writer.Flush();
                    break;
                }
                else if (c3points == 5)
                {
                    writer.WriteLine("Client 3 Won!!!");
                    writer.Flush();
                    break;
                }
                n.Abort();
            }
            Console.WriteLine("GAME OVER!!!!!");
        }
    }
}
