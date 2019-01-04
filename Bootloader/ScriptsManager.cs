using NLua;
using NLua.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

class ScriptsManager
{
    Dictionary<string, Lua> scripts = new Dictionary<string, Lua>();

    public ScriptsManager()
    {
        LoadScripts();
        StartWatchingScripts();
    }

    void StartWatchingScripts()
    {
        FileSystemWatcher watcher = new FileSystemWatcher(Bootloader.ScriptsFolder);
        watcher.Changed += WatchScripts;
        watcher.Created += WatchScripts;
        watcher.EnableRaisingEvents = true;
    }

    private void WatchScripts(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType == WatcherChangeTypes.Changed)
        {
            Logger.Log("Script changed: " + e.FullPath);
            ReloadScript(e.FullPath);
        }

        if (e.ChangeType == WatcherChangeTypes.Created)
        {
            Logger.Log("Script added: " + e.FullPath);
            LoadScript(e.FullPath);
        }
    }

    void LoadScripts()
    {
        var scriptFiles = Directory.GetFiles(Bootloader.ScriptsFolder);
        foreach (var scriptFile in scriptFiles)
        {
            LoadScript(scriptFile);
        }
    }

    void RestartScripts()
    {
        Logger.Log("Restarting scripts");
        foreach (var scriptFile in scripts.Keys)
        {
            scripts[scriptFile].Close();
            LoadScript(scriptFile);
        }
    }

    void LoadScript(string scriptFile)
    {
        try
        {
            Logger.Log("Loading script: " + scriptFile);
            var script = new Lua();
            var fileInfo = new FileInfo(scriptFile);
            scripts.Add(fileInfo.FullName, script);
            script.HookException += OnLuaException;
            script.LoadCLRPackage();
            LoadMethods(script);
            script.DoFile(scriptFile);
        }
        catch(Exception e)
        {
            Logger.Log(e);
        }
    }

    private void OnLuaException(object sender, HookExceptionEventArgs e)
    {
        Logger.Log(e.Exception);
    }

    void LoadMethods(Lua script)
    {
        var luaMethods = typeof(LuaMethods).GetMethods(BindingFlags.Public | BindingFlags.Static);
        foreach (var luaMethod in luaMethods)
        {
            script.RegisterFunction(luaMethod.Name, luaMethod);
        }
    }

    void ReloadScript(string scriptFile)
    {
        Logger.Log("Reloading script: " + scriptFile);

        try
        {
            scripts[scriptFile].DoFile(scriptFile);
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }
    }

    public void ReloadScripts()
    {
        foreach (var pair in scripts)
        {
            Logger.Log("Reloading script: " + pair.Key);
            pair.Value.DoFile(pair.Key);
        }
    }

    public void ExecuteFunction(string name, params object[] values)
    {
        foreach (var script in scripts.Values)
        {
            var luaFunction = (LuaFunction)script[name];
            if (luaFunction != null)
            {
                luaFunction.Call(values);
            }
        }
    }
}