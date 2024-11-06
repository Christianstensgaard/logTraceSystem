using System.Net.Sockets;
using BitToolbox;

namespace MessageServer;
public class ServiceController{
  public ServiceController(){

  }



  /// <summary>
  /// Adding a Socket to the list of connections.
  /// </summary>
  /// <param name="client"></param>
  public void AddNewClient(TcpClient client){

  }



  /// <summary>
  /// Running on it's own, doing all the different stuff in the background.
  /// </summary>
  void ServiceWorker(){

  }







  /// <summary>
  /// Private class to handle the single Tcp-socket connection.
  /// </summary>
  class ClientPool{
    TcpClient client {get; set;}
    byte[] connectionInformation {get; set;}

  }

}