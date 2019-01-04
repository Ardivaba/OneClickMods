using OneClickMods.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OneClickMods.Config;

namespace OneClickMods
{
    class Program
    {
        static Config config;

        static void Main(string[] args)
        {
            Initialize();
            InjectHooks();
        }

        static void Initialize()
        {
            FileHelper.PrepareFolders();
            FileHelper.CreateConfigFile(typeof(Config.Settings));
            config = new Config(FileHelper.ConfigFile);
            FileHelper.CopyBootloaderToAssembliesDirectories(config.Values.HookedAssemblies);
        }

        static void InjectHooks()
        {
            FileHelper.BackupAssemblies(config.Values.HookedAssemblies);
            FileHelper.RestoreAssemblies(config.Values.HookedAssemblies);

            var bootLoader = FileHelper.LoadBootloader();
            Injector injector = new Injector(bootLoader);

            var assemblies = FileHelper.GetAssemblyDefinitions(config.Values.HookedAssemblies);
            var injections = config.Values.Injections;
            injector.Inject(assemblies, injections);
        }
    }
}
