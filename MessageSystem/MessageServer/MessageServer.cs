using System.Net;
using System.Net.Sockets;
using MsgC.Models;
using MsgC.Tools;

System.Console.WriteLine("Starting Server!");
TcpListener serverSocket = new TcpListener(IPAddress.Any, 20200);
serverSocket.Start();
System.Console.WriteLine("Server Started!");
List<ClientConnection> connections = new List<ClientConnection>();
int simpleIdCounter = 0;
Thread backgroundWorkerThread = new Thread(BackgroundWorker);

while(true){
  if(serverSocket.Pending())
    connections.Add(new ClientConnection(serverSocket.AcceptTcpClient()));
  else Thread.Sleep(250);
}

void BackgroundWorker(){
  byte[] buffer = new byte[8000];
  if(connections.Count > 0)
    foreach (var connection in connections)
    {
      if(!connection.Socket.Connected || connection.Socket.Available <= 0)
        continue;

      //- We have some data in the network stream buffer.
      int size = connection.Stream.Read(buffer);
      StreamPackage streamPackage = StreamConverter.UnPackStream(buffer, size);

      if(streamPackage == null)
        continue;

      switch(streamPackage.RequestType){
        case StreamPackage.NewConnection: //- New Subscribtion
          HandleNewConnection(streamPackage, connection);
        break;

        case StreamPackage.SLAVE:
          HandleSlaveConnection(streamPackage, connection);
        break;

        case StreamPackage.MASTER:
          HandleMasterConneciton(streamPackage, connection);
        break;

        default:
        break;
      }
    }
}

void HandleNewConnection(StreamPackage package, ClientConnection clientInformation){
  //Setup New Subscription for the message server
}

void HandleSlaveConnection(StreamPackage package, ClientConnection clientConnection){
  //- Handle request for a Slave connection.
}

void HandleMasterConneciton(StreamPackage package, ClientConnection clientInformation){
  //- Handle Master request.

  //- Who do we need to call.


  //- Find the client, who has the right. 
}


class ClientConnection{
  public ClientConnection(TcpClient client){
    Socket = client;
    Stream = Socket.GetStream();
  }
  public string FunctionName { get; set; } = string.Empty;
  public int ID { get; set; } = -1;
  public NetworkStream Stream { get; set; }
  public TcpClient Socket { get; set; }
}