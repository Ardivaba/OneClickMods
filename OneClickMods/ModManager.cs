using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OneClickMods
{
    public class ScriptsManager
    {
        private const string ScriptsFolder = "./Scripts";
        public ScriptsManager()
        {

        }

        public string[] FindScripts()
        {
            return Directory.GetFiles(ScriptsFolder).ToArray();
        }
        
        public void ParseHooks()
        {

        }
    }
}
