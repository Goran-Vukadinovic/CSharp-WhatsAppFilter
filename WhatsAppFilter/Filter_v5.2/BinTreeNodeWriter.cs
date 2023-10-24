using System;
using System.Collections.Generic;
using System.Linq;

public class BinTreeNodeWriter
{
	private List<byte> m_buffer;

	public byte[] m_encryptionkey;

	private int m_nIV_Counter;

	public BinTreeNodeWriter()
	{
		m_buffer = new List<byte>();
		m_nIV_Counter = 0;
	}

	public void SetEncryptKey(byte[] key)
	{
		m_encryptionkey = key;
		m_nIV_Counter = 0;
	}

	public byte[] EncryptData(byte[] _data, bool bEncrypt)
	{
		int len = _data.Length;
		if (bEncrypt)
		{
			_data = EncryptionHelper.WhatsAppEncrypt(_data, m_encryptionkey, EncryptionHelper.Get_IV(m_nIV_Counter++, 12), null);
			len = _data.Length;
		}
		List<byte> list = new List<byte>();
		list.AddRange(BlasterUtils.GetInt24Data(len));
		list.AddRange(_data);
		return list.ToArray();
	}

	public byte[] WriteUnknownData()
	{
		List<byte> list = new List<byte>();
		int item = 1;
		list.Add((byte)item);
		m_buffer = list;
		FlushBuffer((byte)1 != 0);
		m_buffer.AddRange(EncryptionHelper.HashData_4);
		byte[] result = m_buffer.ToArray();
		m_buffer = new List<byte>();
		return result;
	}

	public byte[] Write(ProtocolTreeNode node, bool bEncrypt)
	{
		if (node == null)
		{
			m_buffer.Add(0);
		}
		else
		{
			if (WhatsAppChannel.bDebug)
			{
				OnWriteNodeDelegateMgr onWriteDelegate = OnWriteNodeDelegateMgr.GetInstance();
				onWriteDelegate.InvokeDelegate(Environment.NewLine + node.NodeString("tx "));
			}
			WriteInternal(node);
		}
		return FlushBuffer(bEncrypt);
	}

	protected void WriteInternal(ProtocolTreeNode _node)
	{
		int len = 1;
		if (_node.attributeHash != null)
		{
			len += _node.attributeHash.Count() * 2;
		}
		if (_node.children.Any())
		{
			len++;
		}
		if (_node.data.Length != 0)
		{
			len++;
		}
		WriteListStart(len);
		WriteString(_node.tag, bEncrypt: false);
		WriteAttributes(_node.attributeHash.ToArray());
		if (_node.data.Length != 0)
		{
			writeBytes(_node.data, bEncrypt: false);
		}
		if (_node.children == null || !_node.children.Any())
		{
			return;
		}
		WriteListStart(_node.children.Count());
		foreach (ProtocolTreeNode item in _node.children)
		{
			WriteInternal(item);
		}
	}

	private int Alphabet2Int(byte v)
	{
		if ((uint)(v - 48) > 9u)
		{
			return ((uint)(v - 65) > 5u) ? (-1) : ((byte)(v - 55));
		}
		return (byte)(v - 48);
	}

	private int NumChar2Int(byte v)
	{
		if ((uint)(v - 45) > 1u)
		{
			return ((uint)(v - 47) <= 10u) ? ((byte)(v - 48)) : (-1);
		}
		return (byte)(v - 35);
	}

	protected byte[] FlushBuffer(bool encrypt)
	{
		byte[] array = m_buffer.ToArray();
		byte[] result = null;
		if (encrypt && m_encryptionkey != null)
		{
			byte[] array2 = new byte[array.Length + 1];
			Array.Copy(array, 0, array2, 1, array.Length);
			result = EncryptData(array2, encrypt);
		}
		m_buffer = new List<byte>();
		return result;
	}

	protected void WriteToken(int v)
	{
		if (v <= 255 && v >= 0)
		{
			m_buffer.Add((byte)v);
		}
	}

	protected void WriteInt8(int v)
	{
		m_buffer.Add((byte)v);
	}

	protected void WriteInt16(int v)
	{
		m_buffer.Add((byte)(v >> 8));
		m_buffer.Add((byte)v);
	}

	protected void WriteInt24(int v)
	{
		m_buffer.Add((byte)(v >> 16));
		m_buffer.Add((byte)(v >> 8));
		m_buffer.Add((byte)v);
	}

	protected void WeiteInt32(int v)
	{
		m_buffer.Add((byte)(v >> 24));
		m_buffer.Add((byte)(v >> 16));
		m_buffer.Add((byte)(v >> 8));
		m_buffer.Add((byte)v);
	}

	protected void WriteStringEx(string strData, bool bEncrypt)
	{
		writeBytes(WhatsAppChannel.m_encoding.GetBytes(strData), bEncrypt);
	}

	protected void writeBytes(byte[] bytes, bool bEncrypt)
	{
		int num = bytes.Length;
		if (num > 1048576)
		{
			m_buffer.Add(254);
			WeiteInt32(num);
		}
		else if (num >= 256)
		{
			m_buffer.Add(253);
			WriteInt24(num);
		}
		else
		{
			byte[] array = null;
			if (bEncrypt && num < 128)
			{
				array = WriteStringBuffer(byte.MaxValue, bytes);
				if (array == null)
				{
					array = WriteStringBuffer(251, bytes);
				}
			}
			if (array != null)
			{
				bytes = array;
			}
			else
			{
				m_buffer.Add(252);
				WriteInt8(num);
			}
		}
		m_buffer.AddRange(bytes);
	}

	protected void WriteString(string _tag, bool bEncrypt)
	{
		int nTokenIdx = -1;
		bool bIsToken2 = false;
		if (new TokenMap().GetValue(_tag, ref bIsToken2, ref nTokenIdx))
		{
			if (bIsToken2)
			{
				int num = nTokenIdx / 256;
				nTokenIdx %= 256;
				WriteToken(236 + num);
			}
			WriteToken(nTokenIdx);
			return;
		}
		int num2 = _tag.IndexOf('@');
		if (num2 > 1)
		{
			string subTxt = _tag.Substring(num2 + 1);
			string text = _tag.Substring(0, num2);
			if (text.IndexOf(":") > 0)
			{
				WriteString2(text);
			}
			else
			{
				WriteJid(text, subTxt);
			}
		}
		else if (!bEncrypt)
		{
			WriteStringEx(_tag, bEncrypt: false);
		}
		else
		{
			WriteStringEx(_tag, bEncrypt: true);
		}
	}

	private void WriteString2(string strUser)
	{
		m_buffer.Add(247);
		string text3 = strUser.Remove(0, strUser.IndexOf(":") + 1);
		string text6 = strUser.Remove(0, strUser.IndexOf(".") + 1);
		text6 = text6.Remove(text6.IndexOf(":"));
		strUser = strUser.Remove(strUser.IndexOf("."));
		m_buffer.Add(byte.Parse(text6));
		m_buffer.Add(byte.Parse(text3));
		WriteString(strUser, bEncrypt: true);
	}

	protected void WriteAttributes(KeyValue[] attributes)
	{
		if (attributes != null)
		{
			foreach (KeyValue item in attributes)
			{
				WriteString(item.GetKey(), bEncrypt: false);
				WriteString(item.GetValue(), bEncrypt: true);
			}
		}
	}

	private int Str2Int(int nType, byte v)
	{
		return nType switch
		{
			255 => NumChar2Int(v), 
			251 => Alphabet2Int(v), 
			_ => -1, 
		};
	}

	private byte[] WriteStringBuffer(byte byVal, byte[] _data)
	{
		int num = _data.Length;
		if (num < 128)
		{
			byte[] array = new byte[(num + 1) / 2];
			for (int j = 0; j < num; j++)
			{
				int num2 = Str2Int(byVal, _data[j]);
				if (num2 != -1)
				{
					int num3 = j / 2;
					array[num3] |= (byte)(num2 << 4 * (1 - j % 2));
					continue;
				}
				array = null;
				break;
			}
			if (array != null)
			{
				if (num % 2 == 1)
				{
					array[array.Length - 1] |= 15;
				}
				m_buffer.Add(byVal);
				WriteInt8((num % 2 << 7) | array.Length);
				return array;
			}
			return null;
		}
		return null;
	}

	protected void WriteListStart(int nLen)
	{
		if (nLen == 0)
		{
			m_buffer.Add(0);
		}
		else if (nLen < 256)
		{
			m_buffer.Add(248);
			WriteInt8(nLen);
		}
		else
		{
			m_buffer.Add(249);
			WriteInt16(nLen);
		}
	}

	protected void WriteJid(string strUser, string strServer)
	{
		m_buffer.Add(250);
		if (strUser.Length > 0)
		{
			WriteString(strUser, bEncrypt: true);
		}
		else
		{
			WriteToken(0);
		}
		WriteString(strServer, bEncrypt: false);
	}
}
