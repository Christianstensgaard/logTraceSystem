CREATE TABLE Trace (
    TraceID INT AUTO_INCREMENT PRIMARY KEY,  -- Unique ID for the trace
    StartTime TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- Time when the trace started
    EndTime TIMESTAMP NULL,  -- Time when the trace ended, can be null initially
    Status VARCHAR(50) DEFAULT 'in-progress'  -- Status of the trace (e.g., in-progress, completed)
);

-- Create the TraceCall table to track each function/network call
CREATE TABLE TraceCall (
    CallID INT AUTO_INCREMENT PRIMARY KEY,  -- Unique ID for the call
    TraceID INT,  -- Foreign key linking to the Trace table
    PreviousCallID INT NULL,  -- ID of the previous call (for chaining)
    ServiceName VARCHAR(100),  -- Name of the service or function that made the call
    MethodName VARCHAR(100),  -- Name of the method or endpoint that was called
    CallStartTime TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- Time the call started
    CallEndTime TIMESTAMP NULL,  -- Time the call ended
    Status VARCHAR(50) DEFAULT 'in-progress',  -- Status of the call
    FOREIGN KEY (TraceID) REFERENCES Trace(TraceID),  -- Link back to the trace
    FOREIGN KEY (PreviousCallID) REFERENCES TraceCall(CallID)  -- Link to the previous call
);

