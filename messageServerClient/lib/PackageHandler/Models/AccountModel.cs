using PackageHandler.Interfaces;

namespace PackageHandler.Models;

public class AccountModel : IStreamable
{
  public int Id { get; set; }               = -1;
  public string AccountName { get; set; }   = string.Empty;
  public string PasswordHash { get; set; }  = string.Empty;
  public int ActiveState { get; set; }      = -1;

  public void Pack(BinaryWriter writer)
  {
    writer.Write(Id);
    writer.Write(AccountName);
    writer.Write(PasswordHash);
    writer.Write(ActiveState);
  }

  public void Unpack(BinaryReader reader)
  {
    Id = reader.ReadInt32();
    AccountName = reader.ReadString();
    PasswordHash = reader.ReadString();
    ActiveState = reader.ReadInt32();
  }
}
