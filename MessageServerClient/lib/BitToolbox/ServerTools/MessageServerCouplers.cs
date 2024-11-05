namespace BitToolbox.ServerTools;

public interface IServerTask{
  public bool Begin();

  public void OnError();
  public void OnFinish();

}