using BitToolbox;
using Service.Core;

namespace Service;
public static class ToolBox{
  public static void NewRequest(string ClientName, string functionName, byte[] payload)
  {
    ToolBox.RunTime.AddRequest(new RequestModel(){
      Header = HeaderManager.CreateHeader(ClientName, functionName),
      Payload = payload
    });
    System.Console.WriteLine("Request Added");
  }
  public static void Request(string ClientName, string functionName, byte[] payload){
    //- Missing
  }
  public static void Request(string functionName, byte[] payload){
    //- Missing
  }

  public static void Request(RequestState requestState){
    //- Handle that type of request
  }

  public static void AddService(ServiceFunction modelClass){
    RunTime.AddService(modelClass);
  }


  public static ServiceController RunTime => ServiceController.Runtime;
}

public enum RequestState{
  Finish,
  Error
}

public class RequestModel{
  public byte[] Header { get; set; }
  public byte[] Payload { get; set; }
}