using System;
using System.Collections.Generic;

namespace Entity.Domain.Process
{
    public class EntityPlaceRevocationProcess : Process
    {
        public EntityPlaceRevocationProcess(string requestNumber, Author author) : base(requestNumber, author)
        {
        }

        public override string Code => "PS02";

        public ProcessStatus Status { get; private set; }
        public Author ChangeAuthor { get; private set; }

        public void AddRevokee(EntityPlace Place)
        {
            if (Status != ProcessStatus.New)
                throw new InvalidOperationException("Wrong wf state");

            _revokees.Add(Place);
        }

        private HashSet<EntityPlace> _revokees = new HashSet<EntityPlace>();

        public void Complete(Author author)
        {
            if (Status != ProcessStatus.InProcess)
                throw new InvalidOperationException("Wrong wf state");

            ChangeAuthor = author;
            Status = ProcessStatus.Completed;
        }

        public void Cancel(Author author)
        {
            if (Status != ProcessStatus.InProcess)
                throw new InvalidOperationException("Wrong wf state");

            ChangeAuthor = author;
            Status = ProcessStatus.Cancelled;
        }
    }
} 
