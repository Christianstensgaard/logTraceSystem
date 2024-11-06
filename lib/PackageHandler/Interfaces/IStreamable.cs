namespace PackageHandler.Interfaces;
public interface IStreamable{
  public void Pack(BinaryWriter writer);
  public void Unpack(BinaryReader reader);
}