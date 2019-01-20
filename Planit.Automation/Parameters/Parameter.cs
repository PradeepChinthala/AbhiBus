using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Planit.Automation.Parameters
{
    public static class Parameter
    {
        private static Dictionary<string, object> _parametersDictionary = new Dictionary<string, object>();

        // To collect the paramters from xml 
        public static void Collect(string fileName, List<string> collectionCriteria)
        {
            string filePath = string.Empty;
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Parameters\");
            filePath = directory + fileName;
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            foreach (var sectionXpath in collectionCriteria)
            {
                var xmlNode = xmlDoc.SelectSingleNode(sectionXpath);
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    Add(node.Name, node.InnerText.ToString());
                }
            }
        }

        //Adding Parameters to dictionry as key,value
        public static void Add<T>(string key, T value) where T : class
        {
            if (_parametersDictionary.ContainsKey(key))
                _parametersDictionary[key] = value;
            else
                _parametersDictionary.Add(key, value);
        }
        //Get Paramter from dictionary base on key
        public static T Get<T>(string key, bool shouldLog = false) where T : class
        {
            object value = null;
            _parametersDictionary.TryGetValue(key, out value);
            return value as T;
        }
    }
}
