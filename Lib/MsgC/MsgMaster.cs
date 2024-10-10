using MsgC.Interfaces;

namespace MsgC;
public class MsgMaster : IMsg
{
  public MsgMaster(string Name, int buffer_size){
    FunctionName = Name;
    StreamBuffer = new byte[buffer_size];
    bufferAllocation = new int[10,2];
    allocationIndex = 0;
  }

    public string FunctionName {get;set;}
    public ushort FunctionId {get;set;}
    public ushort Type {get;set;}
    public byte TraceCallerID { get; set; }
    public byte[] StreamBuffer {get; protected set;}

    public delegate void requestReturn(int id);
    public event requestReturn OnReturnRequest;

    public int Invoke(string functionName){
      if(allocationIndex >= 10)
        allocationIndex = 0; // using ringbuffer design. 
      Msg_Engine.I.Write(this);
      return allocationIndex++;
    }
    public void Log(LogModel log){
      //- Log this to the logging system. 
    }


    public byte[]? GetRequest(int invokeId){
      if(invokeId >= 10)
        return null;

      int start = bufferAllocation[invokeId, 0];
      int end = bufferAllocation[invokeId, 1];
      int buffersize = end - start;

      if(buffersize <= 0)
        return null;
      byte[] buffer = new byte[buffersize];

      Array.Copy(StreamBuffer, start, buffer, 0, buffersize);
      return buffer;
    }
    public void CallEvent()
    {
        OnReturnRequest?.Invoke(id);
    }

    public void WriteToClass(byte[] data, int size)
    {
      if(bufferIndex >= StreamBuffer.Length + size)
        bufferIndex = 0; //ring buffer
      
      Array.Copy(StreamBuffer, bufferIndex,data, 0, size);
      bufferIndex += size;
    }
    int id;
    int allocationIndex;
    int bufferIndex;
    int[,] bufferAllocation;
}

public class LogModel
{
  public string Description { get; set; } = "";





}