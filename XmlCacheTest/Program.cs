﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlCacheTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string results = CacheMan.Query(@"https://api.eveonline.com/server/ServerStatus.xml.aspx");
            
        }
    }
}
