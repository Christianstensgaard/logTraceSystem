//- Here we have the IO API for the Tracing database. 

using MsgC;
using TraceSystem.Controllers;
using TraceSystem.Models;


Msg_Engine.I.EnableLogging = true;
Msg_Engine.I.EnableTracing = true;

MsgSlave trace       = new("createTrace", 10024);     //- Create The Slave for Inserting Trace,       with a buffer size of 1024 bytes
MsgSlave traceCall   = new("insertTraceCall", 10024); //- Create the Slave for Inserting Trace Call,  --//--




//- Creating the event for the functions.
trace.OnRequest     += InsertTrace;
traceCall.OnRequest += InsertTraceCall;


while(true){
  if(Console.ReadLine() == "exit")
    break;
}

void InsertTrace(){
  TraceModel model = new TraceModel();
  System.Console.WriteLine("Called by Master");


  if(!true){
    trace.Log("Error Happened Under something", MsgSlave.LoggingTypes.Warning);
  }


  //- Read the Stream. 
  //- Unpack the Stream.


  using TraceController tc = new TraceController();
  tc.InsertTrace(model);
}

void InsertTraceCall(){
  TraceCallModel model = new TraceCallModel();
  System.Console.WriteLine("Called by Master");
  //Do what ever :)
}

