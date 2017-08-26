using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class ListItemExtentions
    {
        /// <summary>
        /// Gets the display item URL.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static string GetDisplayItemUrl(this ListItem item)
        {
            // <sitecollection|web>/Lists/Aankondigingen/DispForm.aspx?ID=1
            return string.Format("{0}/Forms/DispForm.aspx?ID={1}", 
                item.ParentList.GetListUrl(),
                item.Id);
        }

        /// <summary>
        /// Gets the edit item URL.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static string GetEditItemUrl(this ListItem item)
        {
            // <sitecollection|web>/Lists/Aankondigingen/EditForm.aspx?ID=1
            return string.Format("{0}/Forms/EditForm.aspx?ID={1}",
                item.ParentList.GetListUrl(),
                item.Id);
        }

        /// <summary>
        /// Determines whether the specified item is a record.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="ctx">The client context.</param>
        /// <returns>
        ///   <c>true</c> if the specified item is a record; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRecord(this ListItem item, ClientContext ctx)
        {
            bool isRecord = false;
#if CLIENTSDKV161UP
            isRecord = Microsoft.SharePoint.Client.RecordsRepository.Records.IsRecord(ctx, item).Value;
#endif
            return isRecord;
        }
    }
}
