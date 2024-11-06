using BitToolbox;
using MsPipeline;
using Service.Core;
using static Service.Core.ServiceEngine;

namespace Service;
public abstract class TaskService{
  protected TaskService(string taskName){

  }
  ServiceEngine engine = I;
  public string TaskName { get; set; } = "None";
  public ErrorDefinition ErrorDefinitions { get; set; } = new();
  public ISerilize? OnParsing { get; set; } = null;
  RequestBuilder RequestBuilder => engine.CreateRequest;

  public ByteArray? BaseStream { get; set; } = null;



  //- Definition of Error
  public class ErrorDefinition{
    public int TTD { get; set; }
    public bool CallOnConnectionLost { get; set; } = false;
    public bool CallOnConnectionChanged { get; set; } = false;
  }

  //- Error message model
  public class ErrorModel{
    public string Message { get; set; }
    public string Function {get; set;}
  }


  //- Unpacking stream
  public bool UnPackStream(ISerilize serilize){
    if(serilize == null || BaseStream == null)
      return false;
      
    serilize.Unpack(BaseStream.ToArray());
    return serilize.IsValid;
  }


  public abstract void OnInvoke(byte[] payload);
  public abstract void OnError(ErrorModel error);
}