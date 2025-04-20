using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGalleryNew.Exceptions
{
    public class DuplicateEntryException:Exception
    {
        public DuplicateEntryException() : base() { }
        public DuplicateEntryException(string message) : base(message) { }
    }
}
