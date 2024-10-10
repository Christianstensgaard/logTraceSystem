using MsgC.Interfaces;

namespace MsgC;
public class MsgSlave : IDisposable, IMsg{
  public string FunctionName { get; set; } = string.Empty;
    public ushort Type { get;set; } = 0;
    public ushort FunctionId { get; set; } = 0;

    public MsgSlave(string functionName, int size){
    FunctionName = functionName;
    Type = 125;
    StreamBuffer = new byte[size];
    Buffer_size = size;
    Msg_Engine.I.Add(this);
  }

  public delegate void RequestHandler();
  public event RequestHandler? OnRequest;
  public int Buffer_size {get; private set;}
  public byte[] StreamBuffer {get; private set;}

  public Msg_Engine Settings { get; private set; } = Msg_Engine.I;

  public void Dispose()
  {
    Msg_Engine.I.Remove(this);
  }
  public void CallEvent()
  {
      OnRequest?.Invoke();
  }
  public enum LoggingTypes{
    Debug,
    Warning,
    Error
  }
  public void Log(string desciption, LoggingTypes type ){
      //- Log something
  }
  public void WriteToClass(byte[] data, int size)
  {
      //- Write to the class in the background.
  }
}