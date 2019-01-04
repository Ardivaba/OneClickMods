using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class Logger
{
    static string lastLog = "";
    static int sameLogCount = 0;

    static Logger()
    {
        File.WriteAllText(Bootloader.LogsFile, "OneClickMods started!");
    }

    public static void Log(object logObject)
    {
        var logEntry = logObject.ToString();

        if(logEntry != lastLog)
        {
            var logs = new List<string>(File.ReadAllLines(Bootloader.LogsFile));
            logs.Add(logEntry);
            File.WriteAllLines(Bootloader.LogsFile, logs.ToArray());
            sameLogCount = 0;
        }
        else
        {
            //var logs = File.ReadAllLines(Bootloader.LogsFile);
            //logs[logs.Length - 1] = $"{sameLogCount + 2}: {logEntry}";
            //File.WriteAllLines(Bootloader.LogsFile, logs);
            sameLogCount++;
        }

        lastLog = logEntry;
    }
}
