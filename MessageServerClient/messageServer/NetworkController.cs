using System.Net;
using System.Net.Sockets;

namespace MessageServer;
public class NetworkController{
  public NetworkController(){
    Service = new ServiceController();

    Socket = new TcpListener(IPAddress.Any, 20200);
  }

  public void Start(){
    Running = true;
    new Thread(NetworkBackgroundRunner).Start();
  }
  public bool Running { get; set; }

  void NetworkBackgroundRunner(){
    Socket.Start();

    System.Console.WriteLine("Message Server Started!");
    while(Running){
      if(Socket.Pending()){
        System.Console.WriteLine("Accepting TcpClient");
        Service.NewConnection(Socket.AcceptTcpClient());
      } else Thread.Sleep(250);
    }
  }
  ServiceController Service {get; set;}
  TcpListener Socket {get; set;}
}