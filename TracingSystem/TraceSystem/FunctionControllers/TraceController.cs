using MsgC;

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
    


    
  }







  MsgSlave connection;
}
