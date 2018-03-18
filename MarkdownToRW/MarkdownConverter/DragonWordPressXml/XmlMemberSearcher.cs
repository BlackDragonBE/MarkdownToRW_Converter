using System;
using System.Collections.Generic;
using HtmlAgilityPack;

// Most hacky solution known to man to get the members out of an xml rpc response.
// Turn back now if you have a weak stomache...

namespace DragonMarkdown.DragonWordPressXml
{
    public class XmlMemberSearcher
    {
        private Dictionary<string, string> MemberValues = new Dictionary<string, string>();

        private class NameAndValue
        {
            public string Name = null;
            public string Value = null;
        }

        public XmlMemberSearcher(string xml)
        {
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(xml);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(xml);

            HtmlNodeCollection members = doc.DocumentNode.SelectNodes("//member");
            foreach (HtmlNode member in members)
            {
                //Console.WriteLine("");
                //Console.WriteLine(member.Name);
                //Console.WriteLine("----");

                NameAndValue nameAndValue = new NameAndValue();

                foreach (HtmlNode node in member.ChildNodes)
                {
                    //Console.WriteLine(node.Name + " --- " + node.InnerText);

                    if (node.Name == "name")
                    {
                        nameAndValue.Name = node.InnerText;
                    }

                    if (node.Name == "value")
                    {
                        nameAndValue.Value = node.InnerText;
                        AddToDictionary(nameAndValue);
                    }
                }

                //Console.WriteLine("----");
            }

            //DebugDictionary();
        }

        private void DebugDictionary()
        {
            Console.WriteLine("");
            Console.WriteLine("DICT:");
            foreach (KeyValuePair<string, string> pair in MemberValues)
            {
                Console.WriteLine(pair.Key + " -- " + pair.Value);
            }
        }

        private void AddToDictionary(NameAndValue nameAndValue)
        {
            if (!MemberValues.ContainsKey(nameAndValue.Name))
            {
                MemberValues.Add(nameAndValue.Name, nameAndValue.Value);
            }
        }

        public string GetValueOfMember(string memberName)
        {
            //return "wait";
            return MemberValues[memberName];
        }
    }
}