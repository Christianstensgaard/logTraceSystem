using System.Net.Sockets;
using System.Text;

namespace MsgC;
public class Msg_Engine{
  public static Msg_Engine  I {get; private set;} = new();
  Msg_Engine(){
    socket          = new TcpClient();
    newSubscribers  = new List<IMsg>();
    msgsSubs        = new List<IMsg>();
    thread          = new Thread(BackgroundWorker);
  }

  public void Start(){
    try
    {
      socket.Connect("127.0.0.1", 20200);

      if(socket.Connected)
        thread.Start();
    }
    catch (System.Exception)
    {
      System.Console.WriteLine("Failed to Start the server!");
      return;
    }
  }
  public void Add(IMsg msg){
    if(!socket.Connected)
      return;

    ushort type = msg.Type;
    ushort functionID = 0;
    string functionName = msg.FunctionName;

    byte[] initBuffer = new byte[1 + functionName.Length];
    initBuffer[0] = (byte) msg.Type;
    Array.Copy(Encoding.ASCII.GetBytes(functionName), 0, initBuffer, 1, functionName.Length);
    socket.GetStream().Write(initBuffer);

    int size = socket.GetStream().Read(initBuffer);
    if(size > 0){
      // handling response from server
      functionID = initBuffer[0];
    }

    msg.FunctionId = functionID;
    msgsSubs.Add(msg);
  }
  public void Remove(IMsg msg){
    msgsSubs.Remove(msg);
  }
  internal void Write(){
  }
  void BackgroundWorker(){
    if(!socket.Connected)
     return;

    byte[] streamBuffer = new byte[8000];
    byte[] outputBuffer = new byte[8000];

    while(true){
      if(socket.Available > 0){
        System.Console.WriteLine("Slave have a request");
        int bufferLength = socket.GetStream().Read(streamBuffer);
        if(bufferLength <= 0)
          continue;

        //- Get the function name. using a byte array.
        ushort type       = streamBuffer[0];
        ushort functionId = streamBuffer[1];

        //- Types of messages.
        const ushort MASTER = 255;
        const ushort SLAVE  = 125;

        //- WE NEED TO DO SOMETHING ELSE HERE.
      } else Thread.Sleep(250);
    }
  }

  List<IMsg> newSubscribers;
  List<IMsg> msgsSubs;
  TcpClient socket;
  Thread thread;

}