using System;
using System.Collections.Generic;
using Softgames.Utilities;

namespace Softgames.Core.Services
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        
        //-----------------------------------------------------------------------
        public static void RegisterService<T>(T service) where T : class
        {
            var type = typeof(T);
            if (!_services.TryAdd(type, service))
            {
                Logging.LogWarning($"Service of type {type} is already registered.");
                _services[type] = service;
            }
        }
        
        //-----------------------------------------------------------------------
        public static T GetService<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }
            
            Logging.LogError($"Service of type {type} is not registered.");
            return null;
        }
        
        //-----------------------------------------------------------------------
        public static void Reset() => _services.Clear();
    }
}
