//- This is just a simple Demo. Showing how the Master work in Msg
using MsgC;
MsgMaster master = new MsgMaster("Master", 1024);

//- Enable the tracing and logging inside the Engine. 
Msg_Engine.I.EnableLogging = true;
Msg_Engine.I.EnableTracing = true;

master.OnReturnRequest += returnRequest;

while(true){
  master.Invoke("insertTrace");
}




//-- This function will be called then there is a response from the message server.
void returnRequest(int id){
    byte[]? dataStream = master.GetRequest(id);
}
