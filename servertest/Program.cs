using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace server
{
    class Server
    {
        static void Main()
        {
            TcpListener server = null;
            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                int port = 12345;

                server = new TcpListener(ipAddress, port);

                server.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine($"Установлено соединение с клиентом: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");


                NetworkStream stream = client.GetStream();

                try
                {
                    using (FileStream file = File.Create("info.txt"))
                    {
                        while (!file.Equals("aaaa"))
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead;

                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                file.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                Console.WriteLine("Передача данных завершена. Соединение закрыто.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
