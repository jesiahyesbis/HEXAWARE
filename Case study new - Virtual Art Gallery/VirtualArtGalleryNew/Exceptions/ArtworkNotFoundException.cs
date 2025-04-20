using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGalleryNew.Exceptions
{
    public class ArtworkNotFoundException:Exception
    {
        public ArtworkNotFoundException() : base() { }
        public ArtworkNotFoundException(string message) : base(message) { }
    }
}
