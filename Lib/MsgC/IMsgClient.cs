namespace MsgC;
public interface IMsg{
  public string FunctionName {get; set;}
  public ushort FunctionId {get;set;}
  public ushort Type {get; set;}
  public void CallEvent();
}