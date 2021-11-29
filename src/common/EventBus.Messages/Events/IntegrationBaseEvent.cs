using System;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
            : this(Guid.NewGuid(), DateTime.Now)
        { }
        

        public IntegrationBaseEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }

        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}
