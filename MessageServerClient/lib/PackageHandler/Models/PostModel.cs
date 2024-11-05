using PackageHandler.Interfaces;

namespace PackageHandler.Models;
public class PostModel : IStreamable{
  public int Id { get; set; }

    public void Pack(BinaryWriter writer)
    {
      writer.Write(Id);
    }

    public void Unpack(BinaryReader reader)
    {
      Id = reader.ReadInt32();
    }
}