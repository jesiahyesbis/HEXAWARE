using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGalleryNew.Exceptions
{
    public class GalleryNotFoundException:Exception
    {
        public GalleryNotFoundException() : base() { }
        public GalleryNotFoundException(string message) : base(message) { }
    }
}
