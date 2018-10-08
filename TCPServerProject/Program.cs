using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ConversionLibrary;

namespace TCPServerProject
{
    class Program
    {
        public static readonly int Port = 254;
        static void Main(string[] args)
        {
            IPAddress localAddress = IPAddress.Loopback;

           
            TcpListener serverSocket = new TcpListener(localAddress, Port);
            serverSocket.Start();
            Console.WriteLine("Server running on " + localAddress + " port " + Port);
            while (true)
            {
                try
                {
                    TcpClient clientConnection = serverSocket.AcceptTcpClient();
                    Console.WriteLine("Incoming client");
                    Task.Run(() => DoIt(clientConnection));
                }
                catch (SocketException)
                {
                    Console.WriteLine("Socket exception: Will continue working");
                }
            }
        }

        private static void DoIt(TcpClient clientConnection)
        {
            NetworkStream stream = clientConnection.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            while (true)
            {

                string request = reader.ReadLine();
                if (string.IsNullOrEmpty(request)) { break; }
                Console.WriteLine("Request: " + request);
                string response = request;
                var splitRequest = request.Split();
               
                double conversionNumber = double.Parse(splitRequest[1]);
                Console.WriteLine($"Converting {conversionNumber} to ounces:");

                switch (splitRequest[0])
                {
                    case ("toounces"):
                        ConversionMethods toOuncesObject = new ConversionMethods(); 
                        double OunceResult = toOuncesObject.toOunces(conversionNumber);
                        Console.WriteLine($"Result: {OunceResult} oz");
                        break;
                    case ("tograms"):
                        ConversionMethods toGramsObject = new ConversionMethods();
                        double GramResult = toGramsObject.toGrams(conversionNumber);
                        Console.WriteLine($"Result: {GramResult} g");
                        break;
                }

                writer.WriteLine(response);
                writer.Flush();
            }

            clientConnection.Close();
            Console.WriteLine("Socket closed");
        }
    }
}
