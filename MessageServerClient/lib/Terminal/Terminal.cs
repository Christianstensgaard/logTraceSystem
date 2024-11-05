using System.Diagnostics;

namespace Terminal;
public class IO
{

  public struct TraceModel
  {
    public string Message;
    
  }

  public static void Print(string text){
    Console.WriteLine(text);
  }

  public static void Print(TraceModel info){
    Console.WriteLine(info.Message);
  }










  string[] TerminalBuffer = new string[60];
}
