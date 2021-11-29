using System;

namespace Ordering.Application.Exceptions
{
    public class NotFoundException<T> : ApplicationException        
    {
        public NotFoundException(string name, T key)
            : base($"entity {name} {key} not found" )
        {

        }
    }
}
