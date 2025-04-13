using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGallery.exception
{
    public class ArtWorkNotFoundException : Exception
    {
        public ArtWorkNotFoundException() : base() { }

        public ArtWorkNotFoundException(string message) : base(message) { }

        public ArtWorkNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
