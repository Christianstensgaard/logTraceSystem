using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using BitToolbox;
//- Used to test the server and client.
byte[] TargetPayload = [0xff, 0xff, 0xff, 0x01, 0x02, 0x03, 0x04, 0xee, 0xee, 0xee, 0xff, 0xff, 0xff];



//- Testing the bitTool lib
const byte InitHeader     = 0xee;
const byte DefaultHeader = 0x01;
const byte PaddingHeader  = 0xff;

const string ClientName   = "TestApplication";
const string FunctionName = "TestFunction";


//- Testing Header init request
Print("InitHeaderByte - Testing first byte id", ByteEqual(HeaderManager.CreateHeader(0xee, ClientName, FunctionName)[0], InitHeader));


//- Testing Header default byte
byte[] targetHeader = HeaderManager.CreateHeader(ClientName, FunctionName);
Print("Default Header - Testing first byte id",    ByteEqual(DefaultHeader,    targetHeader[0]));
Print("headerpadding - Testing secound byte padding", ByteEqual(PaddingHeader, targetHeader[1]));

//- Testing ClientName
Print("ClientName - testing against it self", HeaderManager.EqualServiceName(targetHeader, targetHeader));
//- Testing FunctionName
Print("FunctionName - testing against it self", HeaderManager.EqualFunctionName(targetHeader, targetHeader));

byte[] swappedHeader = HeaderManager.CreateHeader(FunctionName, ClientName);
Print("WrongClientName - testing equal header check", !HeaderManager.EqualServiceName(targetHeader, swappedHeader));
Print("WrongFunctionName - testing equal header check", !HeaderManager.EqualFunctionName(targetHeader, swappedHeader));

//- Testing Unpacking Header to String array
string[] resultConvertHeader = HeaderManager.ConverToString(targetHeader);
Print("ClientName - Converted", StringEqual(resultConvertHeader[0], ClientName));
Print("FunctionName - Converted", StringEqual(FunctionName, resultConvertHeader[1]));


//- Testing BufferController lib
ByteBufferController byteBuffer = new ByteBufferController();
System.Console.WriteLine("#####");
System.Console.WriteLine("|-< Testing Byte buffer lib                                            |");
System.Console.WriteLine("");
var buffer = byteBuffer.Allocate(new byte[100]);
Print("Buffer length", buffer.Length == 100);
Print("Buffer start index", buffer.Start == 0);

buffer = byteBuffer.Allocate([0xff, 0xff,0x00,0x00,0x00,0x00,0xff,0xff]);
Print ("Buffer append index", buffer.Start == 100);
Print("index [0] data", buffer[0] == 0xff);
Print("index[last] data", buffer[^1] == 0xff);

Print("Correct data placement", buffer.Stream[100+3] == 0x00 && buffer.Stream[100+4] == 0x00 && buffer.Stream[100+4] == 0x00);


//- Testing payload Copy
byteBuffer = new ByteBufferController(); //- Resetting buffer
buffer = byteBuffer.Allocate(TargetPayload);

bool result = true;
for (int i = 0; i < TargetPayload.Length; i++)
{
  if(buffer[i] != TargetPayload[i])
    result = false;
}
Print("copy of byte[]", result);

byteBuffer = new ByteBufferController(); //- Resetting buffer
buffer = byteBuffer.Allocate(new byte[10000]);
buffer = byteBuffer.Allocate(new byte[100]);

Print("Ringbuffer reset", buffer.Start == 0);

buffer = byteBuffer.Allocate(new byte[100000]);
Print("Buffer overflow", buffer.Start == -1);


//- Testing the Converter. 
List<byte> bytes = new List<byte>();
bytes.AddRange(PackageManager.Pack(targetHeader, TargetPayload));
bytes.AddRange(PackageManager.Pack(targetHeader, TargetPayload));
bytes.AddRange(PackageManager.Pack(targetHeader, TargetPayload));
bytes.AddRange(PackageManager.Pack(targetHeader, TargetPayload));

byte[] packageStream = bytes.ToArray();

TcpListener listener = new TcpListener(IPAddress.Any, 20200);
listener.Start();

TcpSocket(packageStream);

while(!listener.Pending())
  Thread.Sleep(200);

TcpClient connection = listener.AcceptTcpClient();
while(connection.Available <= 0)
  Thread.Sleep(200);


ByteArray[] packages = byteBuffer.Convert(connection.GetStream());
Print("ByteBuffer.Convert()", packages.Length == 4);

result = true;
for (int i = 0; i < packages.Length; i++)
{
  byte[] p = PackageManager.Unpack(packages[i]);
  string[] strings = HeaderManager.ConverToString(p);

  if(strings[0] != ClientName || strings[1] != FunctionName)
    result = false;

}

Print("PackageManager.Unpacking", result);










//PRIVATE
void Print(string title, bool input){
  if(input)
    System.Console.WriteLine   ($"OK!     | {title}");
  else System.Console.WriteLine($"Failed! | {title}");
  System.Console.WriteLine("------------------------------------------------------------------------");
}

bool StringEqual(string t1, string t2){
  return t1 == t2;
}

bool ByteEqual(byte t1, byte t2){

  return t1 == t2;
}

void TcpSocket(byte[] sendStream){
  TcpClient tcpClient = new TcpClient("localhost", 20200);
  if(tcpClient.Connected){
    tcpClient.GetStream().Write(sendStream);
    tcpClient.GetStream().Flush();
  }
}