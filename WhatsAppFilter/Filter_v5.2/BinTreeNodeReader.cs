using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZLibNet;

public class BinTreeNodeReader
{
	[Serializable]
	private sealed class Byte2HexStringUtils
	{
		public static readonly Byte2HexStringUtils _instance = new Byte2HexStringUtils();

		public static Func<byte, string> fnByte2HexString;

		internal string Byte2HexString(byte v)
		{
			return v.ToString("X2");
		}
	}

	public byte[] m_encryptionKey;

	private List<byte> m_buffer;

	public int m_nIV_Counter;

	public BinTreeNodeReader()
	{
		m_encryptionKey = null;
		m_nIV_Counter = 0;
	}

	public void SetEncryptKey(byte[] encryptkey)
	{
		m_encryptionKey = encryptkey;
		m_nIV_Counter = 0;
	}

	public ProtocolTreeNode NextTree(byte[] data)
	{
		if (m_encryptionKey != null)
		{
			m_buffer = new List<byte>();
			m_buffer.AddRange(data);
			int num = BlasterUtils.GetInt24(m_buffer.ToArray());
			m_buffer.RemoveRange(0, 3);
			data = EncryptionHelper.WhatsAppDecrypt(m_buffer.ToArray(), m_encryptionKey, EncryptionHelper.Get_IV(m_nIV_Counter++, 12), null);
			m_buffer = new List<byte>();
			m_buffer.AddRange(data);
			bool flag = (data[0] & 2) != 0;
			m_buffer.RemoveAt(0);
			if (flag)
			{
				data = DecompressData(m_buffer.ToArray());
				m_buffer = new List<byte>();
				m_buffer.AddRange(data.ToArray());
			}
			if (num <= 0)
			{
				return null;
			}
			ProtocolTreeNode node = NextTreeInternal();
			if (node != null && WhatsAppChannel.bDebug)
			{
				OnWriteNodeDelegateMgr _delegateMgr = OnWriteNodeDelegateMgr.GetInstance();
				_delegateMgr.InvokeDelegate(Environment.NewLine + node.NodeString("rx "));
			}
			return node;
		}
		throw new Exception("Received encrypted message, encryption key not set");
	}

	private static byte[] DecompressData(byte[] data)
	{
		MemoryStream stream = new MemoryStream(data);
		ZLibStream zLibStream = new ZLibStream(stream, CompressionMode.Decompress, leaveOpen: true);
		MemoryStream memoryStream = new MemoryStream();
		zLibStream.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}

	protected byte[] GetTokenBytes(int nToken1Idx)
	{
		string strToken = null;
		int nToken2Idx = -1;
		new TokenMap().GetToken(nToken1Idx, ref nToken2Idx, ref strToken);
		if (strToken == null)
		{
			nToken1Idx = ReadInt8();
			new TokenMap().GetToken(nToken1Idx, ref nToken2Idx, ref strToken);
			if (strToken == null)
			{
				throw new Exception("BinTreeNodeReader->getToken: Invalid token/length in getToken " + nToken1Idx);
			}
		}
		return WhatsAppChannel.m_encoding.GetBytes(strToken);
	}

	protected byte[] GetTokenBytes2(int nRow, int nCol)
	{
		string strToken = null;
		int nToken2Idx = 1;
		int nToken1Idx = nCol + nRow * 256;
		new TokenMap().GetToken(nToken1Idx, ref nToken2Idx, ref strToken);
		if (strToken == null)
		{
			throw new Exception("BinTreeNodeReader->getToken: Invalid token  " + nToken1Idx);
		}
		return WhatsAppChannel.m_encoding.GetBytes(strToken);
	}

	protected string ReadEncodedString()
	{
		return WhatsAppChannel.m_encoding.GetString(ReadString(ReadInt8()));
	}

	protected byte[] ReadString(int token)
	{
		byte[] result = new byte[0];
		if (token != -1)
		{
			if (token > 2 && token < 236)
			{
				return GetTokenBytes(token);
			}
			if (token > 235 && token < 240)
			{
				return GetTokenBytes2(token - 236, ReadInt8());
			}
			switch (token)
			{
			default:
				return result;
			case 247:
			{
				int num = ReadInt8();
				int num2 = ReadInt8() & 0xFF;
				string text4 = ReadEncodedString();
				string text6 = "s.whatsapp.net";
				if ((num & 0xFF) == 1)
				{
					text6 = "lid";
				}
				if (string.IsNullOrEmpty(text4))
				{
					text4 = text6;
				}
				else
				{
					string[] obj = new string[7] { text4, null, null, null, null, null, null };
					obj[1] = ".";
					obj[2] = num.ToString();
					obj[3] = ":";
					obj[4] = num2.ToString();
					obj[5] = "@";
					obj[6] = text6;
					text4 = string.Concat(obj);
				}
				return WhatsAppChannel.m_encoding.GetBytes(text4);
			}
			case 248:
				return result;
			case 249:
				return result;
			case 250:
			{
				string text = ReadEncodedString();
				string text2 = ReadEncodedString();
				if (string.IsNullOrEmpty(text))
				{
					return WhatsAppChannel.m_encoding.GetBytes(text2);
				}
				return WhatsAppChannel.m_encoding.GetBytes(text + "@" + text2);
			}
			case 251:
				return ReadBytes(token);
			case 252:
				return ReadBytes2(ReadInt8());
			case 253:
				return ReadBytes2(ReadInt24());
			case 254:
				return ReadBytes2(ReadInt32());
			case 255:
				return ReadBytes(token);
			}
		}
		throw new Exception("BinTreeNodeReader->readString: Invalid token " + token);
	}

	private byte[] ReadBytes(int token)
	{
		int num = ReadInt8();
		int count = 0;
		if (((uint)num & 0x80u) != 0 && token == 251)
		{
			count = 1;
		}
		num &= 0x7F;
		byte[] array = new byte[num];
		Buffer.BlockCopy(m_buffer.ToArray(), 0, array, 0, num);
		m_buffer.RemoveRange(0, num);
		string text2 = "";
		Func<byte, string> byte2HexString = Byte2HexStringUtils.fnByte2HexString;
		string text3 = ((byte2HexString != null) ? string.Concat(array.Select(byte2HexString)) : string.Concat(array.Select(Byte2HexStringUtils.fnByte2HexString = Byte2HexStringUtils._instance.Byte2HexString)));
		for (int j = 0; j < text3.Length; j++)
		{
			byte b = Convert.ToByte("0x" + text3[j], 16);
			if (j == text3.Length - 1 && b > 11 && token != 251)
			{
				break;
			}
			text2 += (char)Hex2Char(token, b);
		}
		text2 = text2.Remove(text2.Length - 1, count);
		return WhatsAppChannel.m_encoding.GetBytes(text2);
	}

	private byte Hex2Char(int token, byte b)
	{
		switch (token)
		{
		case 251:
		{
			byte result;
			switch (b)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				result = (byte)(b + 48);
				break;
			default:
			{
				throw new Exception("bad hex " + b);
			}
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
				result = (byte)(b + 55);
				break;
			}
			return result;
		}
		default:
		{
			throw new Exception("bad packed type " + token);
		}
		case 255:
		{
			byte result;
			switch (b)
			{
			case 10:
			case 11:
				result = (byte)(b + 35);
				break;
			default:
			{
				throw new Exception("bad nibble " + b);
			}
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				result = (byte)(b + 48);
				break;
			}
			return result;
		}
		}
	}

	protected byte[] ReadBytes2(int nArg0)
	{
		byte[] array = new byte[nArg0];
		if (m_buffer.Count < nArg0)
		{
			throw new Exception();
		}
		Buffer.BlockCopy(m_buffer.ToArray(), 0, array, 0, nArg0);
		m_buffer.RemoveRange(0, nArg0);
		return array;
	}

	protected IEnumerable<KeyValue> ReadAttributes(int nSize)
	{
		List<KeyValue> list = new List<KeyValue>();
		int num = (nSize - 2 + nSize % 2) / 2;
		for (int j = 0; j < num; j++)
		{
			string key = ReadEncodedString();
			string value = ReadEncodedString();
			list.Add(new KeyValue(key, value));
		}
		return list;
	}

	protected ProtocolTreeNode NextTreeInternal()
	{
		int size = readListSize(ReadInt8());
		int token2 = ReadInt8();
		if (token2 == 1)
		{
			token2 = ReadInt8();
		}
		if (token2 != 2)
		{
			string tag = WhatsAppChannel.m_encoding.GetString(ReadString(token2));
			IEnumerable<KeyValue> tmpAttributes = ReadAttributes(size);
			if (size % 2 == 1)
			{
				return new ProtocolTreeNode(tag, tmpAttributes);
			}
			int token3 = ReadInt8();
			if (isListTag(token3))
			{
				return new ProtocolTreeNode(tag, tmpAttributes, ReadList(token3));
			}
			return token3 switch
			{
				251 => new ProtocolTreeNode(tag, tmpAttributes, null, ReadBytes(token3)), 
				252 => new ProtocolTreeNode(tag, tmpAttributes, null, ReadBytes2(ReadInt8())), 
				253 => new ProtocolTreeNode(tag, tmpAttributes, null, ReadBytes2(ReadInt24())), 
				254 => new ProtocolTreeNode(tag, tmpAttributes, null, ReadBytes2(ReadInt32())), 
				255 => new ProtocolTreeNode(tag, tmpAttributes, null, ReadBytes(token3)), 
				_ => new ProtocolTreeNode(tag, tmpAttributes, null, ReadString(token3)), 
			};
		}
		return null;
	}

	protected bool isListTag(int token)
	{
		return token switch
		{
			0 => true, 
			248 => true, 
			_ => token == 249, 
		};
	}

	protected List<ProtocolTreeNode> ReadList(int token)
	{
		int num = readListSize(token);
		List<ProtocolTreeNode> list = new List<ProtocolTreeNode>();
		for (int j = 0; j < num; j++)
		{
			list.Add(NextTreeInternal());
		}
		return list;
	}

	protected int readListSize(int token)
	{
		switch (token)
		{
		case 0:
			return 0;
		case 248:
			return ReadInt8();
		default:
		{
			throw new Exception("BinTreeNodeReader->readListSize: invalid list size in readListSize: token " + token);
		}
		case 249:
			return ReadInt16();
		}
	}

	private int ReadInt32()
	{
		int result = 0;
		if (m_buffer.Count >= 4)
		{
			result = m_buffer[0] << 24;
			result |= m_buffer[1] << 16;
			result |= m_buffer[2] << 8;
			result |= m_buffer[3];
			m_buffer.RemoveRange(0, 4);
		}
		return result;
	}

	protected int ReadInt24()
	{
		int result = 0;
		if (m_buffer.Count >= 3)
		{
			result = m_buffer[0] << 16;
			result |= m_buffer[1] << 8;
			result |= m_buffer[2];
			m_buffer.RemoveRange(0, 3);
		}
		return result;
	}

	protected int ReadInt16()
	{
		int result = 0;
		if (m_buffer.Count >= 2)
		{
			result = m_buffer[0] << 8;
			result |= m_buffer[1];
			m_buffer.RemoveRange(0, 2);
		}
		return result;
	}

	protected int ReadInt8()
	{
		int result = 0;
		if (m_buffer.Count >= 1)
		{
			result = m_buffer[0];
			m_buffer.RemoveAt(0);
			return result;
		}
		return result;
	}
}
