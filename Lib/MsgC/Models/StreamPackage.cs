namespace MsgC.Models;

public class StreamPackage
{
    public string? functionName { get; set; } = null;
    public byte[] TraceInformation { get; set; } = null;
    public byte[] LoggingInformation { get; set; } = null;
    public byte? RequestType { get; set; } = 0xEE;
    public byte[]? payload { get; set; } = null;


    public const byte NewConnection = 0x00;
    public const byte SLAVE = 0xEF;
    public const byte MASTER = 0xFF;

    public void Master()
    {
        RequestType = 0xff;
    }

    public void Slave()
    {
        RequestType = 0xEE;
    }
}
