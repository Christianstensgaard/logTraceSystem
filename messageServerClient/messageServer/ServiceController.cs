using System.Net.Sockets;
using BitToolbox;

public class ServiceController{
  public ServiceController(){
    bufferController = new ByteBufferController();
    connections = new List<SlaveConnection>();
  }

  public void NewConnection(TcpClient socket){
      connections.Add(new SlaveConnection(){
        Socket = socket
      });
  }








    class SlaveConnection{
    public byte[] Header {get;set;}
    public TcpClient? Socket {get; set; }
  }
    readonly List<SlaveConnection> connections;
    readonly ByteBufferController bufferController;
}