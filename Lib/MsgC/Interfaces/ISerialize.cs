namespace MsgC.Interfaces;

public interface ISerialize
{
    public byte[] payload { get; set; }
    public byte IsValid { get; set; }
    public void Serialize();
    public T Deserialize<T>();
}
