using Mono.Cecil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OneClickMods.Helpers
{
    public static class FileHelper
    {
        public const string ScriptsDirectory = "Scripts";
        public const string BackupDirectory = "Backup";
        public const string ConfigFile = "config.json";
        public const string BootloaderPath = "Bootloader.dll";

        public static void PrepareFolders()
        {
            Directory.CreateDirectory(ScriptsDirectory);
            Directory.CreateDirectory(BackupDirectory);
        }

        public static void CreateConfigFile(Type settingsType)
        {
            if (!File.Exists(ConfigFile))
            {
                var instance = Activator.CreateInstance(settingsType);
                var config = JsonConvert.SerializeObject(instance, Formatting.Indented);
                File.WriteAllText(ConfigFile, config);
            }
        }

        public static AssemblyDefinition LoadBootloader()
        {
            return AssemblyDefinition.ReadAssembly(BootloaderPath);
        }

        public static void BackupAssemblies(string[] assemblyPaths)
        {
            foreach (var assemblyPath in assemblyPaths)
            {
                BackupAssembly(assemblyPath);
            }
        }

        public static void BackupAssembly(string assemblyPath)
        {
            var fileInfo = new FileInfo(assemblyPath);
            if (!fileInfo.Exists)
                throw new Exception($"Assembly: {assemblyPath} not found");

            var backupPath = $"{BackupDirectory}/{fileInfo.Name}";
            if(!File.Exists(backupPath))
                File.Copy(assemblyPath, backupPath);
        }

        public static void RestoreAssemblies(string[] assemblyPaths)
        {
            foreach(var assemblyPath in assemblyPaths)
            {
                RestoreAssembly(assemblyPath);
            }
        }

        public static void RestoreAssembly(string assemblyPath)
        {
            var fileInfo = new FileInfo(assemblyPath);
            File.Copy($"{BackupDirectory}/{fileInfo.Name}", assemblyPath, true);
        }

        static Dictionary<AssemblyDefinition, string> assembliesPaths = new Dictionary<AssemblyDefinition, string>();
        public static AssemblyDefinition[] GetAssemblyDefinitions(string[] assemblyPaths)
        {
            List<AssemblyDefinition> assemblyDefinitions = new List<AssemblyDefinition>();
            foreach(var assemblyPath in assemblyPaths)
            {
                var fileInfo = new FileInfo(assemblyPath);

                var resolver = new DefaultAssemblyResolver();
                resolver.AddSearchDirectory(fileInfo.Directory.FullName);
                resolver.AddSearchDirectory("Dependencies");
                var assemblyDefinition = 
                    AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters { AssemblyResolver = resolver });

                assemblyDefinitions.Add(assemblyDefinition);
                assembliesPaths.Add(assemblyDefinition, assemblyPath);
            }

            return assemblyDefinitions.ToArray();
        }

        public static void CopyBootloaderToAssembliesDirectories(string[] assemblyPaths)
        {
            foreach(var assemblyPath in assemblyPaths)
            {
            }
        }

        public static string GetAssemblyLocation(AssemblyDefinition assembly)
        {
            return assembliesPaths[assembly];
        }
    }
}
