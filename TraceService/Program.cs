using MsPipeline;
using Service;
using Service.Core;
using TraceService.Services;

TaskBuilder.CreateTask("CreateTrace", new Trace(), ()=>{




  return true;
} );


public class UpdateTrace : TaskService
{
    public UpdateTrace(string taskName) : base("Update Task")
    {
      ErrorDefinitions.TTD = 10; //Time to die
      ErrorDefinitions.CallOnConnectionLost = true; // call error if connection is lost
    }


    public override void OnError(ErrorModel error)
    {
        throw new Exception(error.Message);
    }

    public override void OnInvoke(byte[] payload)
    {
       Trace trace = new Trace();
       UnPackStream(trace);



      //- ServiceEngine.I Will be removed, And wrapped inside the TaskService class
      if(trace.ID == 0)
        ServiceEngine.I.CallBackRequest(0xf4, "Logger", "Error", [0xff], ()=>{
            //- Response from the service
            return true;
        });
      else ServiceEngine.I.ReturnDone();
    }
}









public class Trace : ISerilize
{
    public bool IsValid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public int ID { get; set; }


    public byte Pack()
    {
        throw new NotImplementedException();
    }

    public void Unpack(byte[] stream)
    {
        throw new NotImplementedException();
    }

}






