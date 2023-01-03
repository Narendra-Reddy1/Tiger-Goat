using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class DebugButtonAttributeHelper
{

    public static List<DebugMethodData> data = new List<DebugMethodData>();
    public const string requiredNameSpace = "Deftouch";
    //[MenuItem("Debug/GetAll")]
    public static void GetAllMethodsWithDebugButtonAttribute()
    {
        data.Clear();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        string currentNameSpace;
        foreach (Assembly assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();



            foreach (Type type in types)
            {
                currentNameSpace = type.Namespace;

                bool currentNameSpaceContainRequiredNameSpace = false;
                if (!string.IsNullOrEmpty(currentNameSpace))
                {
                    currentNameSpaceContainRequiredNameSpace = currentNameSpace.Contains(requiredNameSpace);
                }

                if (string.IsNullOrEmpty(currentNameSpace) || currentNameSpaceContainRequiredNameSpace)
                {

                    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                    MemberInfo[] memberInfos = type.GetMembers(flags);

                    foreach (MemberInfo memberInfo in memberInfos)
                    {
                        if (memberInfo.CustomAttributes.ToArray().Length > 0)
                        {
                            DebugButtonAttribute debugButton = memberInfo.GetCustomAttribute<DebugButtonAttribute>();

                            if (debugButton != null)
                            {
                                Debug.Log($" {type.Name} ,  {memberInfo.Name},      {debugButton.methodName}");
                                DebugMethodData debugMethodData = new DebugMethodData(type.Name, memberInfo.Name, debugButton.methodName);
                                data.Add(debugMethodData);
                            }
                        }
                    }
                }
            }
        }
    }

    public static void DebugTest()
    {
        for (int i = 0; i < data.Count; i++)
        {
            Debug.Log(data[i]);
        }
    }


    public static void CallMethod(DebugMethodData debugMethodData)
    {
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type type in types)
        {
            if (type.Name == debugMethodData.className)
            {
                object[] instances = UnityEngine.Object.FindObjectsOfType(type);

                foreach (object instance in instances)
                {
                    MethodInfo method = instance.GetType().GetMethod(debugMethodData.methodName);

                    method.Invoke(instance, null);
                }
            }
        }
    }

}


public class DebugMethodData
{
    public string className;
    public string methodName;
    public string displayName;

    public DebugMethodData(string className, string methodName, string displayName)
    {
        this.className = className;
        this.methodName = methodName;
        this.displayName = displayName;
    }

    public override string ToString()
    {
        return $"{className} , {methodName}, {displayName}";
    }
}