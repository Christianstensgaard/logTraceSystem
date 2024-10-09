using System.Net.Sockets;
using MsgC.Interfaces;
using MsgC.Models;

namespace MsgC;
public class Msg_Engine{
  public static Msg_Engine  I {get; private set;} = new();
  Msg_Engine(){
    socket          = new TcpClient();
    newSubscribers  = new List<IMsg>();
    msgsSubs        = new List<IMsg>();
    thread          = new Thread(BackgroundWorker);
    Start();
  }

  public void Start(){
    int maxRetries = 5;
    int delay = 2000;

    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            socket.Connect("message_server", 20200);
            if(socket.Connected)
            {
                thread.Start();
                return; // Exit the method once connected
            }
        }
        catch (System.Exception)
        {
            System.Console.WriteLine($"Connection attempt {i + 1} failed. Retrying...");
            System.Threading.Thread.Sleep(delay); // Wait before retrying
        }
    }
    System.Console.WriteLine("Failed to Start the server after multiple attempts!");
}

  public void SetTraceConnection(string traceSlaveId){
    logTraceInfoconnectionInformation ??= new LogTraceMsgConnectionInformation();
    logTraceInfoconnectionInformation.TraceFunctionName = traceSlaveId;
  }
  public void SetLoggingConnection(string LogSlaveId){
    logTraceInfoconnectionInformation ??= new LogTraceMsgConnectionInformation();
    logTraceInfoconnectionInformation.LoggingFunctionName = LogSlaveId;
  }


  class LogTraceMsgConnectionInformation{
    public string? TraceFunctionName { get; set; } = null;
    public string? LoggingFunctionName { get; set; } = null;
  }
  LogTraceMsgConnectionInformation? logTraceInfoconnectionInformation = null;

  public bool EnableTracing { get; set; } = false;
  public bool EnableLogging { get; set; } = false;

  public void Add(IMsg msg){
    //Subscribing to the Server.
    if(!socket.Connected)
    {
      // - Try to Connect to the master server again.
      Start();

      if(!socket.Connected){
        //- Log this to something local. and push it then connection is open.
      }
    }

    System.Console.WriteLine("Subscribing: " + msg.FunctionName);
    byte[]? stream = null;
    
    if(stream == null)
      return;
    socket.GetStream().Write(stream);
    msgsSubs.Add(msg);
  }
  public void Remove(IMsg msg){
    msgsSubs.Remove(msg);
  }
  internal void Write(IMsg callerInformation){
    System.Console.WriteLine("Writing To MessageServer");

    //- Create a new Trace Log ID
    if(logTraceInformation == null){
      logTraceInformation = new LogTrace();
      logTraceInformation.TraceID = CreateTraceId(callerInformation);
      logTraceInformation.CallStack.AddLast(CreateTraceCallerID(callerInformation));
    } else logTraceInformation.CallStack.AddLast(CreateTraceCallerID(callerInformation));

    socket.GetStream();
    //- Pack the Data into a package. 
  }
  internal void WriteLog(IMsg caller){
    

  }
  void BackgroundWorker(){
    
  }

  byte[] CreateTraceId(IMsg caller){
    //Creating a TraceID
    return new byte[255];
  }

  byte[] CreateTraceCallerID(IMsg caller){
    // creating traceCaller ID.
    return new byte[255];
  }

  byte[] CreateLog(){
    return new byte[255];
  }


  List<IMsg> newSubscribers;
  List<IMsg> msgsSubs;
  readonly TcpClient socket;
  readonly Thread thread;

  LogTrace logTraceInformation {get; set;}

  const int GlobalSize = 55;
}