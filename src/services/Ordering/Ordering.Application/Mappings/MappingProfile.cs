using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace Ordering.Application.Mappings
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes().Where(t =>
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)));

            const string MappingMethodName = "Mapping";
            const string MappingInterfaceName = "IMapFrom`1";

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod(MappingMethodName) ??
                    type.GetInterface(MappingInterfaceName).GetMethod(MappingMethodName);

                methodInfo.Invoke(instance, new object[] { this } );
            }          
        }
    }
}
