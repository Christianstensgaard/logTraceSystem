using System.Net.Sockets;
using MsPipeline;
using Terminal;

namespace Service.Core;
public class ServiceEngine{
  public static ServiceEngine I {get; private set;} = new ServiceEngine();

  Func<bool>[] callbacks = new Func<bool>[10];
  int position = 0;

  public void InvokeTask(string client, string taskName, byte[] payload){
    socket.GetStream().Write(Package.CreatePackage(0xf3, client, taskName, 0, payload));
    socket.GetStream().Flush();
  }
  public void InvokeTask(string client, string taskName, byte[] payload, Func<bool> callback){
    if(position >= 10)
      position = 0;

    int id = position;
    callbacks[position] = callback;

    socket.GetStream().Write(Package.CreatePackage(0xf4, client, taskName, id, payload));
    socket.GetStream().Flush();
  }

  public void Start(string address, int port){
    IO.Print("Service Engine Started");
    socket = new TcpClient(address, port);
    
    ConnectionHandler();


    IO.Print("Service Engine Stopped");
  }


  void ConnectionHandler(){
    if(socket == null)
      return;
    byte[] buffer = new byte[1024];


    while(socket.Connected){

      //- Async Call back Task Call. 
      InvokeTask("DemoClient", "DemoTask", [0xff,0xff], ()=>{
        System.Console.WriteLine("Hello from Callback 1");
        return true;
      });


      if(socket.Available <= 0){
        Thread.Sleep(200);
        continue;
      }

      int size = socket.GetStream().Read(buffer);

      switch(buffer[0]){

        case 0xf1:
          System.Console.WriteLine("Response From Task");
        break;

        case 0xf2:
          System.Console.WriteLine("Request from Source");
        break;

        case 0xf3:
          System.Console.WriteLine("Invoke Task");
          System.Console.WriteLine(Package.GetId(buffer));
          callbacks[Package.GetId(buffer)]();
        break;

        case 0xf4:
          System.Console.WriteLine("CallBack Function");
          System.Console.WriteLine(Package.GetId(buffer));
          callbacks[Package.GetId(buffer)]();
        break;

        default:
        break;
      }
    }
  }


  TcpClient? socket;
}