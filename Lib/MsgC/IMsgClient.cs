namespace MsgC;
public interface IMsg{
  public string FunctionName {get; set;}

  public void WriteToClass(byte[] data, int size);
  public ushort FunctionId {get;set;}
  public ushort Type {get; set;}
  public void CallEvent();
}