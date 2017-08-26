using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class ChangeCollectionExtentions
    {
        /// <summary>
        /// Creates a <see cref="System.Collections.Generic.List&lt;T&gt;"/> from an <see cref="ChangeCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="collection">Extended collection of changes.</param>
        /// <returns>A <see cref="System.Collections.Generic.List&lt;T&gt;"/> that contains elements from the input sequence</returns>
        public static List<T> ToChangeList<T>(this ChangeCollection collection)
        {
            List<T> changes = new List<T>();

            foreach (Change change in collection)
            {
                changes.Add((T)(object)change);
            }

            return changes;
        }
    }
}
