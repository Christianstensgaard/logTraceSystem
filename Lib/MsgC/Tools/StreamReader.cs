using System.Text;
using Microsoft.VisualBasic;

namespace MsgC.Tools;
public class StreamReader : IDisposable
{

  const byte INT = 0x00;
  const byte STRING = 0x11;

  public enum Datatypes{
    Int = 0x00,
    Float = 0x10,
    String = 0x11,
  }

  public StreamReader(byte[] stream){
    StreamBuffer = stream;
  }

  public int? GetInt(){
    if(StreamBuffer[index] != (byte)Datatypes.Int)
      return null;
    index++;
    return BitConverter.ToInt32(StreamBuffer, Update(sizeof(Int32)));
  }

  public string? GetString(){
    if(StreamBuffer[index] != (byte)Datatypes.String)
      return null;
    index++;
    int stringSize = BitConverter.ToInt32(StreamBuffer, Update(sizeof(Int32)));

    if(stringSize <= 0)
      return null;
    char[] charbuffer = new char[stringSize];
    Encoding.Unicode.GetChars(StreamBuffer, Update(stringSize), stringSize);
    return new string(charbuffer);
  }
  public int index = 0;
  public byte[] StreamBuffer { get; private set; }


  int Update(int size){
    int currentIndex = index;
    index += size;
    return currentIndex;
  }










    public void Dispose()
    {
      StreamBuffer = null;
    }
}