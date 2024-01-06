using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public static class PredefinedAssemblyUtil
    {
        enum AssemblyType
        {
            AssemblyCSharp,
            AssemblyCSharpEditor,
            AssemblyCSharpEditorFirstPass,
            AssemblyCSharpFirstPass,
            DARUMA,//Todo: how add custom assembly ?
        }

        static AssemblyType? GetAssemblyType(string assemblyName)
        {
            return assemblyName switch
            {
                "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
                "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
                "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
                "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
                "MazurkaStudio.DARUMA" => AssemblyType.DARUMA,
                _ => null
            };
        }

        static void AddTypesFromAssembly(Type[] assembly, ICollection<Type> types, Type targetType)
        {
            if (assembly == null) return;

            foreach (var type in assembly)
            {
                if (type != targetType && targetType.IsAssignableFrom(type)) types.Add(type);
            }
        }
        
        /// <summary>
        /// Iterate trough assembly (c# && firstpass) to find existing class of type
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static List<Type> GetTypes(Type targetType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblyTypes = new Dictionary<AssemblyType, Type[]>();
            var types = new List<Type>();

            foreach (var t in assemblies)
            {
                var assemblyType = GetAssemblyType(t.GetName().Name);
                if (assemblyType != null)  assemblyTypes.Add((AssemblyType) assemblyType, t.GetTypes());
            }

            if(assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharp, out var type)) AddTypesFromAssembly(type, types, targetType);
            if(assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharpFirstPass, out type)) AddTypesFromAssembly(type, types, targetType);
            if(assemblyTypes.TryGetValue(AssemblyType.DARUMA, out type)) AddTypesFromAssembly(type, types, targetType);
            return types;
        }
    }
}