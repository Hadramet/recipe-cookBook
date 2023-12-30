using System.Collections.Generic;
using System.Windows.Controls;

namespace CookBook.Extensions;

public static class TreeViewItemExtensions
{
    public static IEnumerable<TreeViewItem> ToTreeViewItems<T>(this IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            yield return new TreeViewItem { Header = item };
        }
    }

    public static void AddItemsToTreeView(this TreeViewItem treeView, IEnumerable<TreeViewItem> items)
    {
        foreach (var item in items)
        {
            treeView.Items.Add(item);
        }
    }
}