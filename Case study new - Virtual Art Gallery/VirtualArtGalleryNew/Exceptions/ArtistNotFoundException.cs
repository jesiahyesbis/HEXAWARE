using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGalleryNew.Exceptions
{
    public class ArtistNotFoundException:Exception
    {
      public ArtistNotFoundException() : base() { }
      public ArtistNotFoundException(string message) : base(message) { }
    }
}
