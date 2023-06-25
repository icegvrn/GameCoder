using System;
using System.Collections.Generic;

internal static class ServiceLocator
{
    private static Dictionary<Type, object> listServices = new Dictionary<Type, object>();

    public static void RegisterService<T>(T service)
    {
        listServices[typeof(T)] = service;
    }

    public static T GetService<T>()
    {
        return (T)listServices[typeof(T)];
    }
}
