using Entity.Domain.Process;
using System;
using System.Collections.Generic;

namespace Entity.ApplicationServices
{
    public class ProcessCompletedNotificationEvent
    {
        public ProcessCompletedNotificationEvent(string requestNumber, Author author, EntityPlaceRevocationInfo[] PlacesToRevoke)
        {
            RequestNumber = requestNumber ?? throw new ArgumentNullException(nameof(requestNumber));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            if (PlacesToRevoke == null || PlacesToRevoke.Length == 0)
                throw new ArgumentException(nameof(PlacesToRevoke));

            PlacesToRevoke = PlacesToRevoke;
        }

        public string RequestNumber { get; }
        public Author Author { get; }

        public IReadOnlyList<EntityPlaceRevocationInfo> PlacesToRevoke { get; }
    }
}