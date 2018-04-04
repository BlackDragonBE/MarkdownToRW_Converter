using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace DragonMarkdown.Utility
{
    public static class HtmlHelper
    {
        public static int RemoveNodesButKeepChildren(this HtmlNode rootNode, string xPath)
        {
            HtmlNodeCollection nodes = rootNode.SelectNodes(xPath);
            if (nodes == null)
                return 0;
            foreach (HtmlNode node in nodes)
                node.RemoveButKeepChildren();
            return nodes.Count;
        }

        public static void RemoveButKeepChildren(this HtmlNode node)
        {
            foreach (HtmlNode child in node.ChildNodes)
                node.ParentNode.InsertBefore(child, node);
            node.Remove();
        }

        public static void ReplaceChildHtml(this HtmlNode node, string newHtml)
        {
            node.ParentNode.ReplaceChild(HtmlNode.CreateNode(newHtml), node);
        }

    }
}
