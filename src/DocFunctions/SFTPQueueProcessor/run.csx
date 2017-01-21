using System;

public static void Run(string sftpRequest, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {sftpRequest}");
}