using Service.Core;

namespace Service;
public abstract class TaskService{
  ServiceEngine engine = ServiceEngine.I;

  public string TaskName { get; set; } = "None";

  public void InvokeTask(string clientName, string taskName, byte[] payload, Func<bool>? callback){
    if(callback == null)
      engine.InvokeTask(clientName, taskName, payload);
    else engine.InvokeTask(clientName, taskName, payload, callback);
  }

  public abstract void OnInvoke(byte[] payload);
  public abstract void OnError();


}