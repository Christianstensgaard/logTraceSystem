using MsgC;
using TraceSystem.Controllers;

namespace TraceSystem.FunctionControllers;
public class TraceController{
  TraceController(){
    //-- Setup Logging and Tracing for the Class
    Msg_Engine.I.EnableLogging = true;
    Msg_Engine.I.EnableTracing = true;

    connection = new MsgSlave("createTrace", 4000);
    connection.OnRequest += OnRequest;
    connection.Log("Created Slave", MsgSlave.LoggingTypes.Debug);
  }

  void OnRequest(){
    System.Console.WriteLine("On Request!");

    using MsgC.Tools.StreamReader reader = new MsgC.Tools.StreamReader(connection.StreamBuffer); // <--- Stream buffer is the raw input, should be changed to only habdle the payload.
    int? traceID      = reader.GetInt();
    string? startTime = reader.GetString();
    string? endTime   = reader.GetString();
    string? status    = reader.GetString();

    if(traceID == null || startTime == null || endTime == null || status == null){
      connection.Log("Error parsing MessageStream", MsgSlave.LoggingTypes.Error);
      return;
    }

    using TraceContext tc = new TraceContext();
    tc.InsertTrace(new TraceModel(){
      TraceID   = (int) traceID,
      StartTime = DateTime.Parse(startTime),
      EndTime   = DateTime.Parse(endTime),
      Status    = status
    });
  }

  MsgSlave connection;
}
