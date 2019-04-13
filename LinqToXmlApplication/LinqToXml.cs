using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace LinqToXmlApplication
{
    public static class LinqToXml
    {
        /// <summary>
        /// Creates hierarchical data grouped by category
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation (refer to CreateHierarchySourceFile.xml in Resources)</param>
        /// <returns>Xml representation (refer to CreateHierarchyResultFile.xml in Resources)</returns>
        public static string CreateHierarchy(string xmlRepresentation)
        {
            XElement root = XElement.Parse(xmlRepresentation);

            return 
                new XElement ("Root",
                root.Elements("Data").
                GroupBy(x => x.Element("Category").Value).
                Select(x => new XElement("Group", new XAttribute("ID", x.Key),
                x.Select(y => { y.Element("Category").Remove(); return y; })))).ToString();
           
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Get list of orders numbers (where shipping state is NY) from xml representation
        /// </summary>
        /// <param name="xmlRepresentation">Orders xml representation (refer to PurchaseOrdersSourceFile.xml in Resources)</param>
        /// <returns>Concatenated orders numbers</returns>
        /// <example>
        /// 99301,99189,99110
        /// </example>
        public static string GetPurchaseOrders(string xmlRepresentation)
        {
            XDocument xdoc = XDocument.Parse(xmlRepresentation);

            XNamespace ns = "http://www.adventure-works.com";

            return String.Join(",", 
                xdoc.Root.Elements(ns +"PurchaseOrder").
                Where(x => x.Descendants(ns + "Address").
                FirstOrDefault(y => y.Attribute(ns+ "Type").Value == "Shipping").
                Element(ns + "State").Value == "NY").
                Select(x => x.Attribute(ns + "PurchaseOrderNumber").Value));
                
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
                     
            return new XElement("Root", customers.Split(new[] { Environment.NewLine},StringSplitOptions.None).
                Select(x=> x.Split(new[] {','})).Select(x=>
                             
                new XElement("Customer",
                    new XAttribute("CustomerID", x[0]),
                    new XElement("CompanyName", x[1]),
                    new XElement("ContactName", x[2]),
                    new XElement("ContactTitle", x[3]),
                    new XElement("Phone", x[4]),
                    new XElement("FullAddress",
                        new XElement("Address", x[5]),
                        new XElement("City", x[6]),
                        new XElement("Region", x[7]),
                        new XElement("PostalCode", x[8]),
                        new XElement("Country", x[9])
                    )
                )
            )).ToString();

           
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            XDocument xdoc = XDocument.Parse(xmlRepresentation);
                  
            
            return xdoc.Root.Value; 
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            XElement root = XElement.Parse(xmlRepresentation);


            return new XElement ("Document", root.Elements().Select(x => {
                if (x.Name == "customer")
                    x.Name = "contact";
                return x;
            }
            )).ToString();

           
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            XElement root = XElement.Parse(xmlRepresentation);

            return root.Elements("channel").
                Where(x => x.Nodes().OfType<XComment>().Count(y=>y.Value=="DELETE")>0).
                Where(x=>x.Elements("subscriber").Count()>=2).
                                Select(x=>Int32.Parse(x.Attribute("id").Value));
                
                
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            XElement root = XElement.Parse(xmlRepresentation);
        

            return new XElement("Root",
                root.Elements().
                OrderBy(x => x.Descendants("Country").First().Value).
                ThenBy(x => x.Descendants("City").First().Value)).ToString();

           
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Gets XElement flatten string representation to save memory
        /// </summary>
        /// <param name="xmlRepresentation">XElement object</param>
        /// <returns>Flatten string representation</returns>
        /// <example>
        ///     <root><element>something</element></root>
        /// </example>
        public static string GetFlattenString(XElement xmlRepresentation)
        {
            
            return xmlRepresentation.ToString(SaveOptions.DisableFormatting);
                       
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Gets total value of orders by calculating products value
        /// </summary>
        /// <param name="xmlRepresentation">Orders and products xml representation (refer to GeneralOrdersFileSource.xml in Resources)</param>
        /// <returns>Total purchase value</returns>
        public static int GetOrdersValue(string xmlRepresentation)
        {
            XElement root = XElement.Parse(xmlRepresentation);

            var productCount = root.Element("Orders").Descendants("product").
               GroupBy(x => x.Value);

            return root.Element("products").Elements("product").
                Join(productCount, x => x.Attribute("Id").Value, x => x.Key, (x, y) =>
                 Int32.Parse(x.Attribute("Value").Value) * y.Select(z=>z).Count()).Sum();

            
            //throw new NotImplementedException();
        }
    }
}
