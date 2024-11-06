using System.Net.Sockets;
using BitToolbox;
using MsPipeline;
using Terminal;

namespace Service.Core;
public class ServiceEngine{
  public static ServiceEngine I {get; private set;} = new ServiceEngine();


  Func<bool>[] callbacks = new Func<bool>[10];
  ByteBufferController bufferController = new ByteBufferController();
  int position = 0;


  public void AddTask(TaskService taskService){
    //- TODO create this.
  }

  public int AddTask(string taskName, byte[] outputStream, Func<bool> onHandle){
    //- TODO create thiis
    return -1;
  }




  public void Request(byte type, string client, string taskName, byte[] payload){
    socket.GetStream().Write(Package.CreatePackage(type, client, taskName, 0, payload));
    socket.GetStream().Flush();
  }
  public void CallBackRequest(byte type, string client, string taskName, byte[] payload, Func<bool> callback){
    if(position >= 10)
      position = 0;

    int id = position;
    callbacks[position] = callback;

    socket.GetStream().Write(Package.CreatePackage(type, client, taskName, id, payload));
    socket.GetStream().Flush();
  }


  public void Response(string client, string taskName, byte[] payload){
    socket.GetStream().Write(Package.CreatePackage(0x33, client, taskName, 0, payload));
    socket.GetStream().Flush();
  }

  public void ReturnDone(){
    //- Telling the system, all is done.
  }

  public void ReturnError(){
    //- Telling the system, and error ...
  }

  public RequestBuilder CreateRequest => new RequestBuilder(I);

  public class RequestBuilder{
    public RequestBuilder(ServiceEngine engine){
      this.engine = engine;

      clientName = null;
      taskName = "undefined";
    }

    ServiceEngine engine;

    public RequestBuilder Callback(Func<bool> callback){
      this.callback = callback;
      return this;
    }

    public RequestBuilder TaskName(string name){
      this.taskName = name;
      return this;
    }

    public RequestBuilder ClientName(string name){
      this.clientName = name;
      return this;
    }

    public void Send(){
      if(!validate())
        throw new Exception("Could not validate request!");
      if(callback == null)
        engine.Request(0xf2, clientName, taskName, payload?? [0xff]);
      else engine.CallBackRequest(0xf4, clientName, taskName, payload?? [0xff], callback);
    }

    bool validate(){
      if(clientName == null)
        return false;
      return true;
    }



    string? clientName;
    string taskName;
    Func<bool>? callback;
    byte[]? payload;
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

    CreateRequest
      .ClientName("DemoClient")
      .TaskName("DemoTask")
      .Callback(()=>{
        System.Console.WriteLine("Call back function");
        return true;
      }).Send();


    

    while(socket.Connected){

      if(socket.Available <= 0){
        Thread.Sleep(200);
        continue;
      }

      ByteArray[] requests = bufferController.Convert(socket.GetStream());

      foreach (var item in requests)
      {
        byte[] package = item.ToArray();
        switch(item[0]){

          case 0xf1:
            System.Console.WriteLine("Response From Task");
          break;

          case 0xf2:
            System.Console.WriteLine("Request from Source");
          break;

          case 0xf3:
            System.Console.WriteLine("Invoke Task");
          break;

          case 0xf4:
            System.Console.WriteLine("CallBack Function");
          break;

          default:
          break;
        }
      }
    }
  }

  void InvokeTask(){

  }


  TcpClient? socket;
}