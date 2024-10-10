# Global Project Information
> [!WARNING]
> Project is still under contruction, and therefore alot of the elements are for demo view only and do not work



# UPDATES: <11-10-2024>
  ## Patch note
  ### Redesign of the msg engine
  - right now the engine, has been alright. but after working around. a redesign on the message engine, is needed to make it more simple for the future.

  - the Slave and Master: both classes will be redesigned to create a more simple interface for the user


# UPDATES: < 09-10-2024 >
## StreamWriter.cs & StreamReader.cs has been created:
  - Added a Stream reader & writer to the libary, to make reading and writing more easy for the user.

  >[!NOTE]
  > at the current time the reader and writer use the not right. in the future, the Master or Slave class should pass the class directly for ID reff insteadof the buffer

## TraceController.cs has been updated:
  - This includes more simple visualization of how the Slave will handle the request.

  > [!NOTE]
  > devnote_ working on the TraceController, the enable of the tracing and logging should be done in the program.cs instead right now it is done inside the functionController class.
