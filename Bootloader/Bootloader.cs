using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class Bootloader
{
    public const string CoreFolder = "OneClickMods";
    public const string ScriptsFolder = "OneClickMods/Scripts";
    public const string LogsFile = "OneClickMods/console.log";

    static ScriptsManager scriptsManager;
    static Bootloader()
    {
        try
        {
            SetupScripts();
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }
    }

    static void SetupScripts()
    {
        scriptsManager = new ScriptsManager();
    }

    public static void HookTarget(string methodName, object instance)
    {
        try
        {
            scriptsManager.ExecuteFunction(methodName, instance);
        }
        catch(Exception e)
        {
            Logger.Log(e);
        }
    }
}
