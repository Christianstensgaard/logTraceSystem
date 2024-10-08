namespace TraceSystem.Models;
public class TraceCallModel
{
    public int CallID { get; set; }
    public int TraceID { get; set; }
    public int? PreviousCallID { get; set; }
    public string ServiceName { get; set; }
    public string MethodName { get; set; }
    public DateTime CallStartTime { get; set; }
    public DateTime? CallEndTime { get; set; }
    public string Status { get; set; }
}