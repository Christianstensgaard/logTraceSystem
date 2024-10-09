using System.Text;

namespace MsgC.Tools;
public class StreamWriter: IDisposable{
  const byte INT = 0x00;
  const byte STRING = 0x11;

  public StreamWriter(){
    size = 6000;
    index = 0;
    StreamBuffer = new byte[size];
  }

  public void Write(int value){
    StreamBuffer[index++] = INT;
    Array.Copy(BitConverter.GetBytes(value), 0, StreamBuffer, Update(sizeof(Int32)), sizeof(Int32));
  }

  public void Write(string value){
    int size = value.Length;
    StreamBuffer[index++] = STRING;
    Array.Copy(Encoding.Unicode.GetBytes(value.ToCharArray()), 0, StreamBuffer, Update(size), size);
  }

  int size;
  int index;
  public byte[] StreamBuffer { get; private set; }
  public void Dispose(){
    StreamBuffer = null;
  }

  int Update(int size){
    int currentIndex = index;
    index += size;
    return currentIndex;
  }
}