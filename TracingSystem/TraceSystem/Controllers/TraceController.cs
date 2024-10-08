
using MySql.Data.MySqlClient;
using TraceSystem.Models;
using TraceSystem.Controllers;

namespace TraceSystem.Controllers;
public class TraceController{

    public TraceController(DatabaseConnection connection){
        db = connection;
    }

    DatabaseConnection db;
    public void InsertTraceCall(TraceCallModel traceCall)
    {
        using (var connection = db.GetConnection())
        {
            connection.Open();
            var query = "INSERT INTO TraceCall (TraceID, PreviousCallID, ServiceName, MethodName, CallStartTime, CallEndTime, Status) " +
                        "VALUES (@TraceID, @PreviousCallID, @ServiceName, @MethodName, @CallStartTime, @CallEndTime, @Status);";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TraceID", traceCall.TraceID);
                command.Parameters.AddWithValue("@PreviousCallID", traceCall.PreviousCallID.HasValue ? (object)traceCall.PreviousCallID : DBNull.Value);
                command.Parameters.AddWithValue("@ServiceName", traceCall.ServiceName);
                command.Parameters.AddWithValue("@MethodName", traceCall.MethodName);
                command.Parameters.AddWithValue("@CallStartTime", traceCall.CallStartTime);
                command.Parameters.AddWithValue("@CallEndTime", traceCall.CallEndTime.HasValue ? (object)traceCall.CallEndTime.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Status", traceCall.Status);
                command.ExecuteNonQuery();
            }
        }
    }

    public void InsertTrace(TraceModel trace)
    {
        using (var connection = db.GetConnection())
        {
            connection.Open();
            var query = "INSERT INTO Trace (StartTime, EndTime, Status) VALUES (@StartTime, @EndTime, @Status);";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StartTime", trace.StartTime);
                command.Parameters.AddWithValue("@EndTime", trace.EndTime.HasValue ? (object)trace.EndTime.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Status", trace.Status);
                command.ExecuteNonQuery();
            }
        }
    }
}