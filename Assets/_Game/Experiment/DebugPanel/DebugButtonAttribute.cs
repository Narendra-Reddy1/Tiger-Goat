using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DebugButtonAttribute : Attribute
{
    public string methodName;

    public DebugButtonAttribute(string methodName)
    {
        this.methodName = methodName;
    }

}
