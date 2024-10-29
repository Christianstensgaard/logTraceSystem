//- Christian Leo Stensgaard Jørgensen
using System.Text;
namespace BitToolbox;
public class HeaderManager{
  public static byte[] CreateHeader(string ServiceClientName, string ServiceFunctionName){
    byte[] RootName = Encoding.Unicode.GetBytes(ServiceClientName);
    byte[] functionName = Encoding.Unicode.GetBytes(ServiceFunctionName);
    byte[] resultBuffer = new byte[RootName.Length + functionName.Length + ( 2 * sizeof(ushort) ) + 2];

    resultBuffer[0] = 0x01;
    resultBuffer[1] = 0xff;
    Array.Copy(BitConverter.GetBytes(RootName.Length),0, resultBuffer,2, sizeof(ushort));
    Array.Copy(BitConverter.GetBytes(functionName.Length),0, resultBuffer, sizeof(ushort)+2, sizeof(ushort));
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
    Array.Copy(BitConverter.GetBytes(RootName.Length),0, resultBuffer,2, sizeof(ushort));
    Array.Copy(BitConverter.GetBytes(functionName.Length),0, resultBuffer, sizeof(ushort)+2, sizeof(ushort));
    Array.Copy(RootName, 0, resultBuffer, sizeof(ushort) * 2 + 2, RootName.Length);
    Array.Copy(functionName, 0, resultBuffer, (sizeof(ushort) * 2) + RootName.Length + 2 , functionName.Length);

    return resultBuffer;
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
    ushort s1 = BitConverter.ToUInt16(t1, 2);
    ushort s2 = BitConverter.ToUInt16(t1, sizeof(ushort) + 2);
    byte[] resultBuffer = new byte[s1 + s2 + 2];
    
    resultBuffer[0] = 0x00;
    resultBuffer[1] = 0x01;
    Array.Copy(t1, 2, resultBuffer, 2, s1 + s2);
    return resultBuffer;
  }
}
