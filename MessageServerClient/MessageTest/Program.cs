
using System.Net;
using MsgServerlib;
using MsPipeline;
using Service.Core;

MsgBuilder msgBuilder = new MsgBuilder(IPAddress.Any, 20200);

Thread runner = msgBuilder.Build();
runner.Start();
ServiceEngine.I.Start("localhost", 20200);






class ServiceDemo{

}