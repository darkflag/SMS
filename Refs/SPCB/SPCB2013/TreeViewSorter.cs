using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser
{
    public class TreeViewSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode node1 = (TreeNode)x;
            TreeNode node2 = (TreeNode)y;

            bool isNode1GeneralGroup = node1.Tag != null && node1.Tag.Equals(NodeLoadType.GeneralGroup);
            bool isNode2GeneralGroup = node2.Tag != null && node2.Tag.Equals(NodeLoadType.GeneralGroup);

            // Default sort based on alfabet
            if (node1.Parent != null &&
                node1.Parent.Tag != null &&
                node1.Parent.Tag.GetType().Equals(typeof(NodeLoadType)) &&
                !isNode1GeneralGroup &&
                !isNode2GeneralGroup)
            {
                return string.Compare(node1.Text, node2.Text);
            }
            // Group fields and content types
            else if (isNode1GeneralGroup || isNode2GeneralGroup)
            {
                if (isNode1GeneralGroup && isNode2GeneralGroup)
                    return string.Compare(node1.Text, node2.Text);

                if (isNode1GeneralGroup)
                    return -1; // Insert above
                else
                    return 1; // Insert below
            }
            // Sort folders and files
            else if (node1.Parent != null &&
                     node1.Parent.Tag != null &&
                     node1.Parent.Tag.GetType().Equals(typeof(SPClient.Folder)))
            {
                bool isNode1File = string.IsNullOrEmpty(System.IO.Path.GetExtension(node1.Text));
                bool isNode2File = string.IsNullOrEmpty(System.IO.Path.GetExtension(node2.Text));

                // Sort on alfabet
                if (isNode1File && isNode2File || !isNode1File && !isNode2File)
                    return string.Compare(node1.Text, node2.Text);

                if (isNode2File)
                    return 1; // If Node2 is file then insert below folder
                else
                    return -1; // If Node2 is folder then insert above file
            }
            // No sort
            else
            {
                return 0;
            }
        }
    }
}
