using System;
using System.Collections.Generic;
using System.Linq;

public sealed class ProtocolTreeNode
{
	private sealed class KeyValUtils
	{
		public string strKey;

		internal bool IsEqualKey(KeyValue keyval)
		{
			return keyval.GetKey().Equals(strKey);
		}
	}

	public string tag;

	public IEnumerable<KeyValue> attributeHash;

	public IEnumerable<ProtocolTreeNode> children;

	public byte[] data;

	public ProtocolTreeNode(string _tag, IEnumerable<KeyValue> _attributeHash, IEnumerable<ProtocolTreeNode> _children = null, byte[] _data = null)
	{
		tag = _tag ?? "";
		attributeHash = _attributeHash ?? new KeyValue[0];
		children = _children ?? new ProtocolTreeNode[0];
		data = new byte[0];
		if (_data != null)
		{
			data = _data;
		}
	}

	public ProtocolTreeNode(string _tag, IEnumerable<KeyValue> _attributeHash, ProtocolTreeNode _children = null)
	{
		if (_tag == null)
		{
			tag = "";
		}
		else
		{
			tag = _tag;
		}
		attributeHash = _attributeHash ?? new KeyValue[0];
		children = ((_children == null) ? new ProtocolTreeNode[0] : new ProtocolTreeNode[1] { _children });
		data = new byte[0];
	}

	public ProtocolTreeNode(string _tag, IEnumerable<KeyValue> _attributeHash, byte[] _data = null)
		: this(_tag, _attributeHash, new ProtocolTreeNode[0], _data)
	{
	}

	public ProtocolTreeNode(string _tag, IEnumerable<KeyValue> _attributeHash)
		: this(_tag, _attributeHash, new ProtocolTreeNode[0])
	{
	}

	public string NodeString(string indent)
	{
		string text5 = indent + "<" + tag;
		if (attributeHash != null)
		{
			foreach (KeyValue item in attributeHash)
			{
				text5 = text5 + string.Format(" {0}=\"{1}\"", item.GetKey(), item.GetValue());
			}
		}
		text5 = text5 + ">";
		if (data.Length != 0)
		{
			if (data.Length <= 256)
			{
				string text16 = BitConverter.ToString(data);
				text5 = text5 + text16.Replace("-", "");
			}
			else
			{
				text5 = text5 + string.Format("--{0} byte--", data.Length);
			}
		}
		if (children != null && children.Count() > 0)
		{
			foreach (ProtocolTreeNode item2 in children)
			{
				text5 = text5 + Environment.NewLine + item2.NodeString(indent + "  ");
			}
			text5 = text5 + Environment.NewLine + indent;
		}
        return text5 + "</" + tag + ">";
    }

    public string GetAttribute(string attribute)
	{
		KeyValUtils keyvalUtils = new KeyValUtils();
		keyvalUtils.strKey = attribute;
		return attributeHash.FirstOrDefault(keyvalUtils.IsEqualKey)?.GetValue();
	}

	public ProtocolTreeNode GetChild(string _tag)
	{
		if (children != null && children.Any())
		{
			foreach (ProtocolTreeNode item in children)
			{
				if (!(item.tag == _tag))
				{
					ProtocolTreeNode node = item.GetChild(_tag);
					if (node != null)
					{
						return node;
					}
					continue;
				}
				return item;
			}
		}
		return null;
	}

	public IEnumerable<ProtocolTreeNode> GetAllChildren(string _tag)
	{
		List<ProtocolTreeNode> list = new List<ProtocolTreeNode>();
		if (children != null && children.Any())
		{
			foreach (ProtocolTreeNode item in children)
			{
				if (_tag.Equals(item.tag, StringComparison.InvariantCultureIgnoreCase))
				{
					list.Add(item);
				}
				list.AddRange(item.GetAllChildren(_tag));
			}
		}
		return list.ToArray();
	}

	public IEnumerable<ProtocolTreeNode> GetAllChildren()
	{
		return children.ToArray();
	}

	public byte[] GetData()
	{
		return data;
	}
}
