/*using System;
using System.Collections.ObjectModel;
using DynamicData;

namespace Asv.Mavlink.Shell;

public class FtpEntry
{
    private readonly Node<IFtpEntry, string> _node;
    private readonly ReadOnlyObservableCollection<FtpEntry> _items;
    
    public FtpEntry(Node<IFtpEntry, string> node)
    {
        _node = node;
        _node.Children.Connect()
            .Transform(childNode => new FtpEntry(childNode))
            .Bind(out _items)
            .Subscribe();
        Key = node.Key;
        Depth = node.Depth;
        IsRoot = node.IsRoot;
        Item = node.Item;
    }

    public string Key { get; set; }
    public int Depth { get; set; }
    public bool IsRoot { get; set; }
    public IFtpEntry Item { get; set; }
    public Node<IFtpEntry, string> Node => _node;
    public ReadOnlyObservableCollection<FtpEntry> Items => _items;
}

public class FtpEntryModel : IFtpEntry
{
    public string ParentPath { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public FtpEntryType Type { get; set; }
}*/