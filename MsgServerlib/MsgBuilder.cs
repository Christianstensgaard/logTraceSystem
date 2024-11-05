using System.Net;
using System.Net.Sockets;
using MsPipeline;
using Terminal;
using static Terminal.IO;

namespace MsgServerlib;

public class MsgBuilder
{
  public MsgBuilder(IPAddress address, int port){

    //- Setting up Socket host information
    connectionInformation = new ConnectionInformation(){
      address = address,
      port = port,
    };

    //- Setting up connections
    connections = new LinkedConnections(){
      size = 100,
      tcpClients = new TcpClient[100],
      position = 0
    };

    //- Setting up the Task Engine
    taskRunner = new TaskRunner(){
      size = 100,
      Runners = new Func<bool>[100],
      position = 0,
    };
  }


  public Thread Build(){
    ServerThread = new Thread(Worker);
    return ServerThread;
  }

  public void RequestStop(){
    application.Running = false;
  }

  void Worker(){

    socket = new TcpListener(connectionInformation.address, connectionInformation.port);
    socket.Start();
    IO.Print("Server started");
    application.Running = true;


    while(application.Running){
      if(!socket.Pending()){
        IO.Print("No Client Pending");
        CreateTask(_lock, ()=>{
          IO.Print("Reading all Connected clients");
          for (int i = 0; i < connections.position; i++)
          {
            TcpClient socket = connections.tcpClients[i];

            if(socket.Available > 0){

              byte[] buffer = new byte[1025];
              int size = socket.GetStream().Read(buffer);
              if(size <= 0)
                continue;
              switch(buffer[0]){
                case 0xf0:
                System.Console.WriteLine("New Subscriber Connected");
                break;

                case 0xf1:
                System.Console.WriteLine("Response From Task");
                break;


                case 0xf2:
                System.Console.WriteLine("Request from Source");
                break;


                case 0xf3:
                System.Console.WriteLine("Invoke Task");
                System.Console.WriteLine(Package.GetClientName(buffer));
                System.Console.WriteLine(Package.GetId(buffer));
                System.Console.WriteLine("------------------------------");
                socket.GetStream().Write(buffer);
                socket.GetStream().Flush();
                break;

                case 0xf4:
                System.Console.WriteLine("Callback");
                System.Console.WriteLine(Package.GetClientName(buffer));
                System.Console.WriteLine(Package.GetId(buffer));
                System.Console.WriteLine("------------------------------");
                socket.GetStream().Write(buffer);
                socket.GetStream().Flush();
                break;

                default:
                break;
              }
            }
          }

          return true;
        });
        Thread.Sleep(1000);
        continue;
      }

      CreateTask(_lock, ()=>{
        HandleClientConnection(socket.AcceptTcpClient());
        return true;
      });

      Thread.Sleep(2000);
    }
    socket.Stop();
    IO.Print("Server Stopped");
  }

  TcpListener socket;
  Thread ServerThread;

  /// <summary>
  /// Socket Connection information
  /// </summary>
  struct ConnectionInformation
  {
    public IPAddress address;
    public int port;
  }
  
  /// <summary>
  /// Application State
  /// </summary>
  struct App{
    public bool Running;
  }
  
  /// <summary>
  /// Collection of connected clients
  /// </summary>
  struct LinkedConnections{
    public int size;
    public TcpClient[] tcpClients;
    public int position;
  }

  /// <summary>
  /// Task Runner Collection.
  /// </summary>
  struct TaskRunner{
    public int size;
    public Func<bool>[] Runners;
    public int position;
  }
  
  App application; //- Application State 
  ConnectionInformation connectionInformation; //- socket connection info
  TaskRunner taskRunner; //- Background Task Runners
  LinkedConnections connections;


  //- Create a Task, that will be run at some point.
  void CreateTask(object locker ,Func<bool> RunnerFunction){
    lock(locker){
      if(taskRunner.position >= taskRunner.size)
        taskRunner.position = 0;
      taskRunner.Runners[taskRunner.position++] = RunnerFunction;
    }
      RunnerFunction();
  }



  //- Thread Worker function.
  static object _taskLock = new object();
  void TaskWorker(){
    
  }




  static object _lock = new object();
  void HandleClientConnection(TcpClient socket){
    lock(_lock){
      IO.Print("Handling Connection");
      connections.tcpClients[connections.position++] = socket;
    }
  }

}
