using System.Linq.Expressions;
using System.Text;

namespace MsgC;


public class StreamPackage{
  public string? functionName {get;set;} = null;
  public byte[] TraceInformation {get;set;} = null;
  public byte[] LoggingInformation {get;set;} = null;
  public byte? RequestType {get;set;} = 0xEE;
  public byte[]? payload {get;set;} = null;


  public const byte NewConnection = 0x00;
  public const byte SLAVE = 0xEF;
  public const byte MASTER = 0xFF;

  public void Master(){
    RequestType = 0xff;
  }

  public void Slave(){
    RequestType = 0xEE;
  }
}


public class StreamConverter{

  public static StreamPackage? UnPackStream(byte[] streambuffer, int size){
    StreamPackage package = new StreamPackage();

    if(!IsValid(streambuffer))
      return null;

    package.RequestType = streambuffer[Global_validation_Size];
    package.functionName = GetFunctionName(streambuffer);

    if(package.functionName == null)
      return null;


    int payloadSize = size - Global_validation_Size - Global_functionName_size;

    if(payloadSize <= 0)
      return null;

    package.payload = new byte[size - 4 - Global_functionName_size];
    Array.Copy(streambuffer,  Global_validation_Size + Global_functionName_size, package.payload, 0, package.payload.Length);


    //- Load the Tracing into the Class. 


    return package;
  }

  public static byte[]? PackStream(StreamPackage package){

    if(package == null)
      return null;

    if(package.functionName == null)
      return null;

    if(package.RequestType == null)
      return null;

    if(package.payload == null)
      return null;


    byte[] outputBuffer = new byte[package.payload.Length + Global_functionName_size + Global_validation_Size];


    SignPackage(outputBuffer);
    PlaceFunctionName(package.functionName, outputBuffer);
    Array.Copy(package.payload, 0, outputBuffer,Global_functionName_size+ Global_validation_Size, package.payload.Length);

    if(outputBuffer.Length != ( package.payload.Length + Global_functionName_size + Global_validation_Size))
      return null;




    //- Pack Tracing into the Streambuffer.






    return outputBuffer;
  }

  static string? GetFunctionName(byte[] stream){
    if(stream.Length < ( Global_functionName_size + Global_validation_Size))
      return null;
    byte[] charBuffer = new byte[Global_functionName_size];
    int functionName_size = 0;
    for (int i = 0; i < Global_functionName_size; i++)
    {
      if(stream[i] == (byte)Package_padding)
      {
        functionName_size = i;
        break;
      }
    }

    if(functionName_size == 0)
      return null;

    Array.Copy(stream, 5, charBuffer, 0, functionName_size);
    return new string(Encoding.Unicode.GetChars(charBuffer));
  }
  static bool PlaceFunctionName(string functionName, byte[] stream)
  {
    if(functionName.Length > Global_functionName_size)
      return false;

    Array.Copy(Encoding.Unicode.GetBytes(functionName.ToCharArray()), 0 ,stream, Global_validation_Size, functionName.Length);

    if(functionName.Length < Global_functionName_size)
      for (int i = 0; i < Global_functionName_size - functionName.Length; i++)
      {
        stream[functionName.Length+i] = (byte) Package_padding;
      }

    return true;
  }
  static bool IsValid(byte[] stream){
    bool isValid = true;

    for (int i = 0; i < Global_validation_Size; i++)
    {
      if(stream[i] != Package_validation_code)
        isValid = false;
    }
    return isValid;
  }

  static bool SignPackage(byte[] stream)
  {
    if(stream.Length < Global_validation_Size)
      return false;
    for (int i = 0; i < Global_validation_Size; i++)
    {
      stream[i] = Package_validation_code;
    }
    return true;
  }


  static bool PlaceLoggingAndTracingIntoStream(LogTrace logTrace, byte[] stream)
  {



    return true;
  }





  const int Global_functionName_size = 55;
  const int Global_validation_Size = 4;

  const byte Package_validation_code = 0xEE;
  const char Package_padding = '\n';
}