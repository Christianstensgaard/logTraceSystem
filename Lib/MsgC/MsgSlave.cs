namespace MsgC;
public class MsgSlave : IDisposable, IMsg{
  public string FunctionName { get; set; }
    public ushort Type { get;set; }
    public ushort FunctionId { get; set; } = 0;

    public MsgSlave(string functionName){
    FunctionName = functionName;
    Type = 125;
    Msg_Engine.I.Add(this);
  }

  public delegate void RequestHandler();
  public event RequestHandler? OnRequest;

  public void Dispose()
  {
    Msg_Engine.I.Remove(this);
  }

  public void CallEvent()
  {
      OnRequest?.Invoke();
  }
}