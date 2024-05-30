using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chackers3D.Assets.Scripts
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        private Dictionary<Type, object> services = new Dictionary<Type, object>();

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceLocator();
                }
                return _instance;
            }
        }

        public void RegisterService<T>(T service)
        {
            services[typeof(T)] = service;
        }

        public T GetService<T>()
        {
            return (T)services[typeof(T)];
        }
    }
}