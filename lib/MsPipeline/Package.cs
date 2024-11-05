using System.Drawing;
using System.Text;

namespace MsPipeline;
public class Package{

  //###########################################
  // - [0] Type                           byte
  // - [1] Padding                        byte
  // - [2] PackageSize                    int32
  // - [6] ClientName Start Position      uint16
  // - [8] TaskName Start position        uint16
  // - [10] Id start position             uint16
  // - [12] Payload start position        uint16

  const int defaultStartIndex = 14;

  public static byte[] CreatePackage(byte type, string clientName, string taskName, int id, byte[] payload){

    byte[] clientNameBuffer = Encoding.Unicode.GetBytes(clientName);
    byte[] taskNameBuffer = Encoding.Unicode.GetBytes(taskName);
    byte[] resultBuffer = new byte[clientNameBuffer.Length + taskNameBuffer.Length + payload.Length + 2 + sizeof(Int32)*4 + sizeof(UInt16)*4];

    resultBuffer[0] = type;
    resultBuffer[1] = 0xff;

    //Inserting the TotalPackage Size
    CopyInt32(resultBuffer.Length, resultBuffer, 2);

    int bufferPosition = defaultStartIndex;

    ClientNamePosition(resultBuffer, bufferPosition);
    CopyInt32(clientNameBuffer.Length, resultBuffer, bufferPosition);
    bufferPosition += sizeof(Int32);
    Array.Copy(clientNameBuffer, 0, resultBuffer, bufferPosition, clientNameBuffer.Length);
    bufferPosition += clientNameBuffer.Length;

    TaskNamePosition(resultBuffer, bufferPosition);
    CopyInt32(taskNameBuffer.Length, resultBuffer, bufferPosition);
    bufferPosition += sizeof(Int32);
    Array.Copy(taskNameBuffer,0,resultBuffer, bufferPosition, taskNameBuffer.Length);
    bufferPosition += taskNameBuffer.Length;

    IdPosition(resultBuffer, bufferPosition);
    CopyInt32(id, resultBuffer, bufferPosition);
    bufferPosition += sizeof(Int32);

    PayloadPosition(resultBuffer, bufferPosition);
    Array.Copy(payload, 0, resultBuffer, bufferPosition, payload.Length);

    return resultBuffer;
  }

  public static int GetPackageSize(byte[] stream){
    return BitConverter.ToInt32(stream, 2);
  }
  public static string GetClientName(byte[] stream){
    int position = BitConverter.ToUInt16(stream, 6);
    int size = BitConverter.ToInt32(stream, position);
    return Encoding.Unicode.GetString(stream, position + sizeof(Int32), size);
  }
  public static string GetTaskName(byte[] stream){
    int position = BitConverter.ToUInt16(stream, 8);
    int size = BitConverter.ToInt32(stream, position);

    return Encoding.Unicode.GetString(stream, position + sizeof(Int32), size);
  }
  public static int GetId(byte[] stream){
    int position = BitConverter.ToUInt16(stream, 10);
    return BitConverter.ToInt32(stream, position);
  }

  public static byte[] getPayload(byte[] stream){
    int position = BitConverter.ToUInt16(stream, 12);
    int size = GetPackageSize(stream) - position;

    System.Console.WriteLine(size);
    byte[] payload = new byte[size];
    Array.Copy(stream, position, payload, 0, size );
    return payload;
  }


  static void ClientNamePosition(byte[] buffer, int position){
    Array.Copy(BitConverter.GetBytes((UInt16) position), 0, buffer, 6, sizeof(UInt16));
  }
  static void TaskNamePosition(byte[] buffer, int position){
    CopyUInt16((UInt16)position, buffer, 8);
  }
  static void IdPosition(byte[] buffer, int position){
    CopyUInt16((UInt16)position, buffer, 10);
  }
  static void PayloadPosition(byte[] buffer, int position){
    CopyUInt16((UInt16)position, buffer, 12);
  }

  static void UpdatePositionByte(int index, int position, byte[] buffer){
    Array.Copy(BitConverter.GetBytes((UInt16) position), 0, buffer, index, sizeof(UInt16));
  }
  static void CopyInt32(int value, byte[] buffer, int index){
    Array.Copy(BitConverter.GetBytes(value),0,buffer,index, sizeof(Int32));
  }
  static void CopyUInt16(UInt16 value, byte[] buffer, int index){
    Array.Copy(BitConverter.GetBytes(value),0,buffer,index, sizeof(UInt16));
  }
}