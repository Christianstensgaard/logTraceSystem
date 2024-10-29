using PackageHandler.Interfaces;

namespace PackageHandler.Models;
public class CommentModel : IStreamable {
  public int Id { get; set; }             = -1;
  public int PostId { get; set; }         = -1;
  public string Comment { get; set; }     = string.Empty;
  public int Likes { get; set; }          = -1;

    public void Pack(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(PostId);
        writer.Write(Comment);
        writer.Write(Likes);
    }

    public void Unpack(BinaryReader reader)
    {
      Id      = reader.ReadInt32();
      PostId  = reader.ReadInt32();
      Comment = reader.ReadString();
      Likes   = reader.ReadInt32();
    }
}
