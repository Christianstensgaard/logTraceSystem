using System.Net;
using System.Net.Sockets;

System.Console.WriteLine("Starting Server");
TcpListener serverSocket = new TcpListener(IPAddress.Parse("127.0.0.1"), 20200);
serverSocket.Start();
System.Console.WriteLine("Server Started");
List<ClientConnection> connections = new List<ClientConnection>();
Thread backgroundWorkerThread = new Thread(BackgroundWorker);

while(true){
  if(serverSocket.Pending())
    connections.Add(new ClientConnection(serverSocket.AcceptTcpClient()));
  else Thread.Sleep(250);
}

void BackgroundWorker(){

  byte[] buffer = new byte[8000];
  ClientConnection? connectionRemove = null;
  while(true){
    if(connections.Count > 0){
      foreach (var connection in connections)
      {
        if(!connection.Socket.Connected){
          connectionRemove = connection;
          continue;
        }

        if(connection.Socket.Available > 0 ){
          System.Console.WriteLine("Server has data pending");
          // Some Data is pending.
        }
      }
    } else Thread.Sleep(200);


    if(connectionRemove != null){
      connections.Remove(connectionRemove);
      connectionRemove = null;
    }
  }
}

class ClientConnection{
  public ClientConnection(TcpClient client){
    Socket = client;
    Stream = Socket.GetStream();
  }
  public NetworkStream Stream { get; set; }
  public TcpClient Socket { get; set; }

}