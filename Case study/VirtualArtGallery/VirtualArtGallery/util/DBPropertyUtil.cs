using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace VirtualArtGallery.util
{
    public static class DBPropertyUtil
    {
        public static string GetPropertyString()
        {
            // Directly returns the complete connection string from App.config
            return ConfigurationManager.AppSettings["ConnectionString"];
        }
    }
}
