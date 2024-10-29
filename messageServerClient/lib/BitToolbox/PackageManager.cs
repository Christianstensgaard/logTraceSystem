//- Christian Leo Stensgaard Jørgensen
namespace BitToolbox;

public class PackageManager{
  public static byte[] Pack(byte[] header, byte[] payload){
    byte[] resultbuffer = new byte[header.Length + payload.Length + sizeof(Int32)];
    Array.Copy(BitConverter.GetBytes(header.Length + payload.Length), resultbuffer, sizeof(Int32));
    Array.Copy(header, 0, resultbuffer, sizeof(Int32), header.Length);
    Array.Copy(payload, 0, resultbuffer, header.Length + sizeof(Int32), payload.Length);

    return resultbuffer;
  }
  public static byte[] Unpack(byte[] stream, int start ){
    int packageSize = BitConverter.ToInt32(stream, start);
    byte[] package = new byte[packageSize];
    System.Console.WriteLine(packageSize);
    Array.Copy(stream, sizeof(Int32), package, 0, packageSize);

    return package;
  }

  public static byte[] Unpack(ByteArray byteArray){
    int packageSize = BitConverter.ToInt32(byteArray.Stream, byteArray.Start);
    byte[] package = new byte[packageSize];
    System.Console.WriteLine(packageSize);
    Array.Copy(byteArray.Stream, sizeof(Int32), package, 0, packageSize);

    return package;
  }


  public static byte[] GetPayload(byte[] stream){
    ushort FirstSize  = BitConverter.ToUInt16(stream, 2);
    ushort SecundSize = BitConverter.ToUInt16(stream, 2+sizeof(ushort));

    int payloadStart = FirstSize + SecundSize + 2;
    int arraySize = stream.Length - payloadStart;

    byte[] payloadBuffer = new byte[arraySize];
    Array.Copy(stream, 0, payloadBuffer, 0,arraySize);

    return payloadBuffer;
  }
}