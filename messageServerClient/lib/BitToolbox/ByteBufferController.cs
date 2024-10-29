//- Christian Leo Stensgaard Jørgensen
using System.Net.Sockets;

namespace BitToolbox;
public class ByteBufferController{
  const int BufferSize = 10000;
  const int bufffermargin = 500;


  public ByteArray[] Convert(NetworkStream stream){
    List<ByteArray> byteArrays = new List<ByteArray>();
    ByteArray buffer = Allocate(500);
    int size = stream.Read(buffer.Stream, buffer.Start, 500);
    int position = 0;
    while(position + sizeof(Int32) <= size){
      int psize = BitConverter.ToInt32(globalBuffer, position);
      int start = position + sizeof(Int32);
      position += sizeof(Int32);
      position += psize;
      byteArrays.Add(new ByteArray(globalBuffer, start, psize + start));
    }
    return byteArrays.ToArray();
  }

  public int Copy(byte[] stream){
    return -1;
  }

  public void Free(ByteArray array){
    //- Not Needed atm...
  }

  public ByteArray? Copy(byte[] stream, int start, int size){
    int length = size;

    if(length <= 0)
      return null;

    Check(length);
    Array.Copy(stream,start,globalBuffer, currentBufferPosition, length);
    return new ByteArray(globalBuffer, currentBufferPosition, length + ScalePosition(length));
  }

  public ByteArray Allocate(byte[] stream){
    int size = stream.Length;
    Check(size);
    int start = ScalePosition(size);
    Array.Copy(stream,0,globalBuffer, start, size);
    return new ByteArray(globalBuffer, start, start+size);
  }

  public ByteArray Allocate(int size){
    Check(size);
    return new ByteArray(globalBuffer, currentBufferPosition, ScalePosition(size) + size);
  }
  private static readonly object bufferLock = new object();
  int ScalePosition(int size)
  {
      lock (bufferLock)
      {
          int current = currentBufferPosition;
          currentBufferPosition += size;
          return current;
      }
  }

  void ClearBuffer(){
    currentBufferPosition = 0;
  }

  void Check(int size){
    if(currentBufferPosition + size >= BufferSize - bufffermargin ){
      ClearBuffer();
    }
  }

  int currentBufferPosition = 0;
  byte[] globalBuffer = new byte[BufferSize];
}

public class ByteArray{
  public ByteArray(byte[] globalSource, int start, int end){
    pBuffer = globalSource;
    Start = start;
    End = end;
    Length = End - Start;
    Stream = pBuffer;
  }
  public int Start { get;  private set; }
  int End { get;  set; }
  public int Length { get; private set; }
  public byte[] Stream {get; private set;}

  public byte[] ToArray(){
    byte[] buffer = new byte[End - Start];
    Array.Copy(pBuffer, Start, buffer, 0, End-Start);
    return buffer;
  }

  public byte this[int index]
  {
    get
    {
      //- Missing OOB
      return pBuffer[Start+index];
    }
    set
    {
      //- Missing OOB
      pBuffer[Start+index] = value;
    }
  }
  byte[] pBuffer;
}