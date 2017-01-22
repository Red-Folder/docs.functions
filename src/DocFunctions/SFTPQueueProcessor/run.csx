using System;

public static void Run(string sftpRequest, TraceWriter log)
{
    log.Info($"SFTP Queue triggered: {sftpRequest}");
}