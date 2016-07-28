using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CroquetAustralia.QueueProcessor.Email
{
    // EmailAttachments is used to simplify YAML serialization
    public class EmailAttachments : IEnumerable<FileInfo>
    {
        private readonly IEnumerable<FileInfo> _attachments;

        public EmailAttachments(IEnumerable<FileInfo> attachments)
        {
            _attachments = attachments;
        }

        public IEnumerator<FileInfo> GetEnumerator()
        {
            return _attachments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}