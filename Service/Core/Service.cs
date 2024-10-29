using System.Net.Sockets;
using BitToolbox;

namespace Service.Core;
public class ServiceController{
  static readonly object _lock = new();
  static object _lock_service = new();

  public static bool lockArray = false;
  public static ServiceController Runtime {get; private set;} = new ServiceController();

  public string ServiceClientName { get; set; } = "Undefined";

  public void AddRequest(RequestModel model){
    lock(_lock){
      while(lockArray)
        Thread.Sleep(2);
      if(requestPosition >= 68)
      return;
      Requests[requestPosition++] = model;
    }
  }

  public void AddService(ServiceFunction serviceFunction)
  {
    lock(_lock_service){
      Subscribers.Add(serviceFunction);
    }
  }


  public void Stop(){
    socket.Close();
  }


  public bool Start(string address, int port){
    if(!openConnection(address, port)){
      System.Console.WriteLine("Unable to Connect to Message Server!");
      return false;
    }

    if(!SubscribeAll()){
      System.Console.WriteLine("Error under subscribing all functions to server");
      return false;
    }

    MasterHeader = HeaderManager.CreateHeader(ServiceClientName, "MASTER");
    ToolBox.RunTime.WriteAndFlush(PackageManager.Pack(HeaderManager.CreateHeader(0xee, ServiceClientName, "MASTER"), new byte[2]));

    System.Console.WriteLine("Connected to BusServer");
    running = true;
    tService.Start();
    return true;
  }

  //------------------------------------------------------------------
  //- PRIVATE

  ServiceController(){
    socket = new TcpClient();
    tService = new Thread(BackgroundRunner);
    Requests = new RequestModel[70];
    Subscribers = new List<ServiceFunction>();
    ByteBuffer = new ByteBufferController();
    requestPosition = 0;
  }

  TcpClient socket;
  Thread tService;
  bool running = false;
  byte[] MasterHeader;
  ByteBufferController ByteBuffer {get; set;}

  RequestModel[] Requests {get; set;}
  List<ServiceFunction> Subscribers {get; set;}
  int requestPosition;


  bool WriteAndFlush(byte[] stream){
    if(!socket.Connected)
      return false;
    socket.GetStream().Write(stream);
    socket.GetStream().Flush();
    return true;
  }

  void BackgroundRunner(){
    while(running){
      try
      {
        lock(_lock){
          lockArray = true;
          for (int i = 0; i < requestPosition; i++)
          {
            if(Requests[i].Header[0] != 0xee && HeaderManager.EqualServiceName(Requests[i].Header, MasterHeader)){
              InvokeSubscribers(Requests[i].Header, Requests[i].Payload);
              continue;
            }
            socket.GetStream().Write(PackageManager.Pack(Requests[i].Header, Requests[i].Payload));
            System.Console.WriteLine("Request Sendt to the Server");
          }

          if(requestPosition != 0){
            socket.GetStream().Flush();
            requestPosition = 0;
          }
          lockArray = false;
        }


        if(socket.Available > 0){
          ByteArray[]? requests = ByteBuffer.Convert(socket.GetStream());

          for (int i = 0; i < requests.Length; i++)
          {
            byte[] aa = requests[i].ToArray();
            string[] strings = HeaderManager.ConverToString(aa);
            System.Console.WriteLine(strings[0]+" : "+strings[1]);
            Thread.Sleep(25);
          }
        }
      }

      //- Handle Error as needed.
      catch (System.Exception)
      {

      }
    }
  }
  bool openConnection(string address, int port){
    for (int i = 0; i < 5; i++)
    {
      try
      {
        socket.Connect(address, port);
        return true;
      }
      catch (System.Exception){
        Thread.Sleep(300);
      }
    }
    return false;
  }
  bool SubscribeAll(){
    lock(_lock)
    foreach (var item in Subscribers)
    {
      item.OnInit(item.Settings);
      item.FunctionHeader = HeaderManager.CreateHeader(item.ServiceClientName, item.SerivceFunctionName);
    }
    return true;
  }

  void InvokeSubscribers(byte[] header, byte[] payload){
    foreach (var item in Subscribers)
    {
      if(HeaderManager.EqualFunctionName(header, item.FunctionHeader)){
        item.OnRequest();
        break;
      }
    }
  }
  void InvokeSubscribers(ByteArray array){
    foreach (var item in Subscribers)
    {
      if(HeaderManager.EqualFunctionName(array.ToArray(), item.FunctionHeader)){
        item.OnRequest();
      }
    }
  }
}