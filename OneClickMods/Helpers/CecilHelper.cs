using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneClickMods.Helpers
{
    public static class CecilHelper
    {
        public static AssemblyDefinition ReadAssembly(string assemblyPath, string dependenciesDirectory)
        {
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(dependenciesDirectory);
            return AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters { AssemblyResolver = resolver });
        }
    }
}
