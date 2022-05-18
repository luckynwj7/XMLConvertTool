using System;
using System.Windows;
using System.Xml;

namespace XMLConvertTool
{
    public static class XmlParser
    {
        public static XmlElement XmlParse(string xmlCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            // XML파일을 파싱하여 XmlDoc에 저장
            if (xmlCode != null)
            {
                try
                {
                    xmlDoc.LoadXml(xmlCode);
                }
                catch (System.Xml.XmlException)
                {
                    // XML 파일을 해석 할 수 없을 경우
                    xmlDoc = null;
                }
                XmlElement root = xmlDoc.DocumentElement;
                return root;
            }
            xmlDoc = null;
            return null;
        }

        public static void PrintAllXmlNodeListUsingDFS(XmlNode currentNode, int depth = 0)
        {
            // XmlNode내의 모든 요소를 메세지박스로 출력하고자 할 때 (DFS사용)
            if ((currentNode.HasChildNodes).Equals(true))
            {
                ++depth;
                foreach (XmlNode node_ChildNode in currentNode.ChildNodes)
                {
                    if (!(node_ChildNode.Name).Equals("#text"))
                    {
                        Console.WriteLine(node_ChildNode.Name + "\n깊이 : " + depth);

                    }

                    PrintAllXmlNodeListUsingDFS(node_ChildNode, depth);
                }

            }

        }

        public static void PrintAllXmlNodeListUsingDFS2(XmlNode currentNode, int depth = 0)
        {
            // XmlNode내의 모든 요소를 메세지박스로 출력하고자 할 때 (DFS사용)
            if ((currentNode.HasChildNodes).Equals(true))
            {
                ++depth;
                foreach (XmlNode node_ChildNode in currentNode.ChildNodes)
                {
                    if (!(node_ChildNode.Name).Equals("#text"))
                    {
                        foreach (XmlAttribute attr in node_ChildNode.Attributes)
                        {
                            Console.WriteLine(attr.Name + "\n값 : " + attr.Value + "\n깊이 :" + depth);
                        }

                    }

                    PrintAllXmlNodeListUsingDFS(node_ChildNode, depth);
                }

            }

        }
    }

}
