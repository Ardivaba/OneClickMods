using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace OneClickMods
{
    public class Config
    {
        public enum InjectType { Hook, Add };
        public enum InjectPosition { Start, End };

        public class Injection
        {
            public InjectType Type = InjectType.Hook;
            public InjectPosition Position = InjectPosition.End;
            public string MethodPath;
        }

        public class Settings
        {
            public string[] HookedAssemblies = new string[] { "../Assembly-CSharp.dll" };
            public Injection[] Injections = new Injection[] { new Injection(){
                MethodPath = "CheatMgr.OnGUI"
            }};
        }

        public Settings Values;

        public Config(string configPath)
        {
            ReadConfig(configPath);
        }

        void ReadConfig(string configPath)
        {
            var settings = File.ReadAllText("config.json");
            Values = JsonConvert.DeserializeObject<Settings>(settings);
        }
    }
}
