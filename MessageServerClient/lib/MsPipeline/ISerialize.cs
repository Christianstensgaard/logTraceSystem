namespace MsPipeline;
public interface ISerilize{
  public void Unpack(byte[] stream);
  public bool IsValid { get; set; }
  public byte Pack();
}