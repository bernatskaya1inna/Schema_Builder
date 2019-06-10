using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using System.Drawing.Imaging;

namespace BoxRender
{
    class Program
    {
        static void Main(string[] args)
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder(args[0]);
            schemaBuilder.Run();
        }
    }
}


