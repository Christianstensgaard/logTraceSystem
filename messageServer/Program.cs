//- Message Server

using System.Net.Sockets;
using BitToolbox;
using MessageServer;


NetworkController network = new NetworkController();
network.Start();
bool Running = true;
while(Running){
  Thread.Sleep(2000);
  //- If something else is needed in the future!
}
