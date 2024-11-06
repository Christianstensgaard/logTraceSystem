//- Christian Leo Stensgaard Jørgensen
/*
  * For Future me!
    - Right now, reading the position of the id and payload, is done by readinging the
      2 and 3 int32 in the array, and add them to get to the id position, after that the payload
      are placed all that plus size of int32.

    In the furture, this should be changed so the package / header contains the location of each
    different types. This mean if i need to read the ID, i can read the x2 int in the stream, and get the
    position, also this way i can do different checks on if its null without reading the value.


  * Model could be something like
      PackageSize
      PackageType
      ClientNamePosition
      TaskNamePosition
      IdPosition
      PayloadPosition

    - makes it alot easyer to make changes to the underlaying parsin system.

  (|PackageType|PackageSize|) default package build-up

  Next are positions type(ushort)
  |ClientName|TaskName|Id|Payload|

  Package buildup.

  ¨ClientName"
  type - can be used to do some check on the datatype
  |type|Size(ushort)|value(string)|

*/

using System.Text;
namespace BitToolbox;
public class HeaderManager{
  public static byte[] CreateHeader(string ServiceClientName, string ServiceFunctionName){
    byte[] RootName = Encoding.Unicode.GetBytes(ServiceClientName);
    byte[] functionName = Encoding.Unicode.GetBytes(ServiceFunctionName);
    byte[] resultBuffer = new byte[RootName.Length + functionName.Length + ( 2 * sizeof(ushort) ) + 2];

    resultBuffer[0] = 0x01;
    resultBuffer[1] = 0xff;
    Array.Copy(BitConverter.GetBytes((ushort)RootName.Length),0, resultBuffer,2, sizeof(ushort));
    Array.Copy(BitConverter.GetBytes((ushort)functionName.Length),0, resultBuffer, sizeof(ushort)+2, sizeof(ushort));
    Array.Copy(RootName, 0, resultBuffer, sizeof(ushort) * 2 + 2, RootName.Length);
    Array.Copy(functionName, 0, resultBuffer, (sizeof(ushort) * 2) + RootName.Length + 2 , functionName.Length);

    return resultBuffer;
  }
  public static byte[] CreateHeader( byte headertype, string ServiceClientName, string ServiceFunctionName){
    byte[] RootName = Encoding.Unicode.GetBytes(ServiceClientName);
    byte[] functionName = Encoding.Unicode.GetBytes(ServiceFunctionName);
    byte[] resultBuffer = new byte[RootName.Length + functionName.Length + ( 2 * sizeof(ushort) ) + 2];

    resultBuffer[0] = headertype;
    resultBuffer[1] = 0xff;
    Array.Copy(BitConverter.GetBytes((ushort)RootName.Length),0, resultBuffer,2, sizeof(ushort));
    Array.Copy(BitConverter.GetBytes((ushort)functionName.Length),0, resultBuffer, sizeof(ushort)+2, sizeof(ushort));
    Array.Copy(RootName, 0, resultBuffer, sizeof(ushort) * 2 + 2, RootName.Length);
    Array.Copy(functionName, 0, resultBuffer, (sizeof(ushort) * 2) + RootName.Length + 2 , functionName.Length);

    return resultBuffer;
  }
  public static byte[] CreateResponseHeader(int id, string targetClientName){
  byte[] clientName = Encoding.Unicode.GetBytes(targetClientName);
    byte[] buffer = new byte[clientName.Length + sizeof(Int32) + sizeof(Int16) + 2];

    buffer[0] = 0x1f;
    buffer[1] = 0xff;

    Array.Copy(BitConverter.GetBytes((short) clientName.Length), 0 , buffer, 2, sizeof(Int16)); //string length
    Array.Copy(clientName, 0, buffer, 2 + sizeof(Int16), clientName.Length); // String data
    Array.Copy(BitConverter.GetBytes(id), 0, buffer, 2 + sizeof(Int16) + clientName.Length, sizeof(Int32)); // ID
    return buffer;
  }
  public static int GetCallbackId(ByteArray stream){
    if(stream[sizeof(Int32)] != 0x1f || stream[1+ sizeof(Int32)] != 0xff)
      return -1;
    ushort idPosition = BitConverter.ToUInt16(stream.Stream, sizeof(Int32)+2);

    if(idPosition <= 0)
      return -1;

    return BitConverter.ToInt32(stream.Stream, 2 + sizeof(Int32) + sizeof(Int16) + idPosition);
  }
  public static bool IsCallbackResponse(ByteArray stream){
    if(stream.Length < 4)
      return false;
    int start = sizeof(Int32);
    return stream[0+start] == 0x1f && stream[1+start] == 0xff;
  }
  public static string[] ConverToString(byte[] t1){
    if(t1.Length < sizeof(ushort)*2)
      return ["Error", "Error"];
    ushort ServiceClientName = BitConverter.ToUInt16(t1, 2);
    ushort FunctionName = BitConverter.ToUInt16(t1, sizeof(ushort) + 2);
    string clientName = Encoding.Unicode.GetString(t1, 2 + (sizeof(ushort) * 2), ServiceClientName);
    string functionName = Encoding.Unicode.GetString(t1, 2 + (sizeof(ushort) * 2) + ServiceClientName, FunctionName);

    return [clientName, functionName];
  }
  public static bool EqualServiceName(byte[] t1,  byte[] t2){
    ushort targetOneSize = BitConverter.ToUInt16(t1, 2);
    ushort targetTwoSize = BitConverter.ToUInt16(t2, 2);
    if(targetOneSize != targetTwoSize)
      return false;

    bool result = true;
    for (int i = sizeof(ushort) * 2 + 2; i < targetOneSize; i++)
    {
      if(t1[i] != t2[i]){
        result = false;
        break;
      }
    }
    return result;
  }
  public static bool EqualServiceName(byte[] sourceBuffer, int start,   byte[] t2){
    ushort targetOneSize = BitConverter.ToUInt16(sourceBuffer, 2 + start);
    ushort targetTwoSize = BitConverter.ToUInt16(t2, 2);
    if(targetOneSize != targetTwoSize)
      return false;

    bool result = true;
    for (int i = sizeof(ushort) * 2 + 2; i < targetOneSize; i++)
    {
      if(sourceBuffer[i+start] != t2[i]){
        result = false;
        break;
      }
    }
    return result;
  }
  public static bool EqualFunctionName(byte[] t1, byte[] t2){
    int startIndex       = BitConverter.ToInt16(t1,2);
    ushort targetOneSize = BitConverter.ToUInt16(t1, sizeof(ushort) + 2 );
    ushort targetTwoSize = BitConverter.ToUInt16(t2, sizeof(ushort) + 2 );

    if(targetOneSize != targetTwoSize)
      return false;

    bool result = true;
    for (int i = sizeof(ushort) * 2 + startIndex; i < targetOneSize + (sizeof(ushort) *2) + startIndex + 2; i++)
    {
      if(t1[i] != t2[i]){
        result = false;
        break;
      }
    }
    return result;
  }
  public static byte[] ExstractHeader(byte[] t1){
    ushort s1 = BitConverter.ToUInt16(t1, 2+ sizeof(Int32));
    ushort s2 = BitConverter.ToUInt16(t1, sizeof(ushort) + 2 + sizeof(Int32));
    byte[] resultBuffer = new byte[s1 + s2 + 2];

    resultBuffer[0] = 0x00;
    resultBuffer[1] = 0x01;
    Array.Copy(t1, 2, resultBuffer, 2, s1 + s2);
    return resultBuffer;
  }
}


public class PackageManager{


  //- Global Position for package informations
  const int position_PackageType_1  = 0; // [0] type [1] padding + 2
  const int position_PackageType_2  = 1; // [0] type [1] padding + 2
  const int Position_PackageSize    = 2; // [3]                  + sizeof(int32)
  const int position_Client         = sizeof(UInt16) * 1 + 2;  //  int32
  const int position_Task           = sizeof(UInt16) * 2 + 2;
  const int Position_Callback       = sizeof(UInt16) * 3 + 2;
  const int position_Payload        = sizeof(UInt16) * 4 + 2;

  public byte PackageType(ByteArray buffer){
    return buffer[position_PackageType_1];
  }
  public string ClientName(ByteArray buffer){
    int position = ReadPositionValue(position_Client, buffer);
    return ReadStringValue(position, buffer);
  }
  public string TaskName(ByteArray buffer){
    int position = ReadPositionValue(position_Task, buffer);
    return ReadStringValue(position, buffer);
  }
  public int CallbackId(ByteArray buffer){
    return ReadPositionValue(Position_Callback, buffer);
  }
  public bool EqualTask(ByteArray t1, ByteArray t2){
    return IsEqual(position_Task, ReadPositionValue(position_Task, t1), t1, t2);
  }
  public bool EqualClient(ByteArray t1, ByteArray t2){
    return IsEqual(position_Client, ReadPositionValue(position_Client, t1), t1, t2);
  }



  bool IsEqual(int position,int size, ByteArray t1, ByteArray t2){
    for (int i = 0; i < size; i++)
    {
      if(t1[i + position] != t2[i+position]){
        return false;
      }
    }
    return true;
  }
  int ReadPositionValue(int position, ByteArray stream){
    return BitConverter.ToUInt16(stream.Stream, position);
  }

  void InsertValueAtPosition(int position, ushort value, ByteArray buffer){
    Array.Copy(BitConverter.GetBytes(value), 0, buffer.Stream, position, sizeof(ushort));
  }


  string ReadStringValue(int position, ByteArray buffer){
    int stringSize = BitConverter.ToInt32(buffer.Stream, position);
    return Encoding.Unicode.GetString(buffer.Stream, position, stringSize);
  }


  ByteArray buffer;
}
