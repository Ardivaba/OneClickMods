using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Inject;
using OneClickMods.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static OneClickMods.Config;

namespace OneClickMods
{
    public class Injector
    {
        private AssemblyDefinition bootAssembly;

        public struct SearchResult
        {
            public TypeDefinition TypeDefinition;
            public MethodDefinition MethodDefinition;
        }

        public Injector(AssemblyDefinition bootAssembly)
        {
            this.bootAssembly = bootAssembly;
        }

        public void Inject(AssemblyDefinition[] assemblies, Injection[] injections)
        {
            foreach (var injection in injections)
            {
                Inject(assemblies, injection);
            }

            foreach(var assembly in assemblies)
            {
                assembly.Write(FileHelper.GetAssemblyLocation(assembly));
            }
        }

        void Inject(AssemblyDefinition[] assemblies, Injection injection)
        {
            foreach(var assembly in assemblies)
            {
                var method = FindInjectionMethod(assembly, injection.MethodPath);
                if(method != null)
                {
                    InjectHook(assembly, method, injection);
                    return;
                }
            }

            throw new Exception($"Method with path: {injection.MethodPath} not found in any of the specified assemblies");
        }

        MethodDefinition FindInjectionMethod(AssemblyDefinition assembly, string fullName)
        {
            var nameSplit = fullName.Split('.');
            var className = fullName.Substring(0, fullName.Substring(0, fullName.Length - 1).LastIndexOf('.'));
            var methodName = nameSplit.Last();

            var typeDefinition = assembly.MainModule.Types.FirstOrDefault(x => x.Name == className || x.FullName == className);
            if(typeDefinition != null)
            {
                var methodDefinition = typeDefinition.GetMethod(methodName);
                if(methodDefinition != null)
                {
                    return methodDefinition;
                }
            }

            return null;
        }

        void Hello()
        {
            Meme(new object[] { "Hello", this });
        }

        void Meme(params object[] args)
        {
            Console.WriteLine(args.Length);
        }

        void InjectHook(AssemblyDefinition assembly, MethodDefinition method, Injection injection)
        {
            var il = method.Body.GetILProcessor();

            il.Body.Instructions.Insert(0, il.Create(OpCodes.Ldstr, method.Name));
            il.Body.Instructions.Insert(1, il.Create(OpCodes.Ldarg_0));
            il.Body.Instructions.Insert(2, il.Create(OpCodes.Call, assembly.MainModule.Import(FindHookTargetMethod())));
        }

        MethodDefinition FindHookTargetMethod()
        {
            return bootAssembly.MainModule.Types[1].GetMethod("HookTarget");
        }
    }
}
