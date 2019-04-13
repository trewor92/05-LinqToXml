using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace LinqToXmlApplication
{
    class Program
    {
        static void Main(string[] args)
        {
        XElement contacts =
            new XElement("Contacts",
                new XElement("Contact",
                    new XElement("Name", "Patrick Hines"),
                    new XElement("Phone", "206-555-0144"),
                    new XElement("Address",
                        new XElement("Street1", "123 Main St"),
                        new XElement("City", "Mercer Island"),
                        new XElement("State", "WA"),
                        new XElement("Postal", "68042")
                                 )
                             )
                          );

            var a = LinqToXml.GetFlattenString(contacts);

            
            Console.WriteLine(a);

            Console.ReadLine();
                        
        }
    }
}
