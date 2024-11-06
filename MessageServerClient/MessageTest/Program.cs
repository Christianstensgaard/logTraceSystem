
using System.Net;
using MsgServerlib;
using MsPipeline;
using Service;
using Service.Core;

MsgBuilder msgBuilder = new MsgBuilder(IPAddress.Any, 20200);

byte[] demo = Package.CreatePackage(0xf0, "HelloWorld", "Functino", 10, [0xff, 0xff]);


System.Console.WriteLine(Package.EqualClientName(demo, demo));
System.Console.WriteLine(Package.EqualTaskName(demo,demo));



Thread runner = msgBuilder.Build();
runner.Start();
ServiceEngine.I.Start("localhost", 20200);


TaskBuilder.CreateTask("Update Account", null, ()=>{

  //- Get the Stream and parse it
  

  return true;
});







class ServiceDemo : TaskService
{
    public ServiceDemo() : base("ServiceDemo")
    {
      

      ErrorDefinitions.TTD = 10; // Time to die.
      ErrorDefinitions.CallOnConnectionLost = true;
    }

    public override void OnError(ErrorModel error)
    {
        throw new Exception("Error" + error.Message);
    }

    public override void OnInvoke(byte[] payload)
    {
      //- Called when invoked by the engine client...
      System.Console.WriteLine("Service Demo called!");
      
    }
}