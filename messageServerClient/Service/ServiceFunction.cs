namespace  Service;
public abstract class ServiceFunction{

  public class FunctionConfig{
    public string FunctionName { get; set; } = "undefined";
  }

  public string ServiceClientName => ToolBox.RunTime.ServiceClientName;
  public string SerivceFunctionName => Settings.FunctionName;
  public FunctionConfig Settings { get; set; } = new FunctionConfig();

  public void Request(string clientName, string functionName, byte[] payload){
    ToolBox.NewRequest(clientName, functionName, payload);
  }

  public void Request(string functionName, byte[] payload){
    ToolBox.NewRequest(ServiceClientName, functionName, payload);
  }

  internal byte[]? FunctionHeader {get;set;} 

  public abstract void OnInit(FunctionConfig config);
  public abstract void OnRequest();

}