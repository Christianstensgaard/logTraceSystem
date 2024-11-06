using MsPipeline;
using Service.Core;

namespace Service;
public class TaskBuilder{
  public static void CreateTask(string taskName, ISerilize serilize, Func<bool> onHandle){
      ServiceEngine.I.AddTask(taskName, [0xff, 0xff], onHandle);
      




  }







}