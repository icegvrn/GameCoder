using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private static ServiceLocator instance;

    private Dictionary<Type, object> servicesDictionary = new Dictionary<Type, object>();

    public static ServiceLocator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ServiceLocator();
            }
            return instance;
        }
    }

    public void RegisterService<T>(T service)
    {

        Type serviceType = typeof(T);
        if (!servicesDictionary.ContainsKey(serviceType))
        {
            servicesDictionary.Add(serviceType, service);
        }
        else
        {
            servicesDictionary[serviceType] = service;
        }
    }

    public T GetService<T>()
    {
        Type serviceType = typeof(T);
        if (servicesDictionary.ContainsKey(serviceType))
        {
            return (T)servicesDictionary[serviceType];
        }
        else 
        {
            Debug.LogWarning("Le service de type " + serviceType + " est manquant.");
        }
       return default(T);
    }

    public bool IsServiceRegistered<T>()
    {
        Type serviceType = typeof(T);
        return servicesDictionary.ContainsKey(serviceType);
    }


    public void UnregisterService<T>()
    {
        Type serviceType = typeof(T);
        if (servicesDictionary.ContainsKey(serviceType))
        {
            servicesDictionary.Remove(serviceType);
        }
    }


}
