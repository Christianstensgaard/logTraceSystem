namespace MsgC;
public class LogTrace{

  public static LogTrace CreateTrace(string functionName){
    return new LogTrace(){FunctionName = functionName, CallStack = new LinkedList<byte[]>(), TraceID = new byte[10]};
  }

  public static LogTrace? LoadTrace(byte[] streambuffer){
    return null;
  }


  public string FunctionName { get; set; }
  public byte[] TraceID { get; set; }
  public LinkedList<byte[]> CallStack { get; set; }


  public static readonly int TraceID_Size = 10;

}