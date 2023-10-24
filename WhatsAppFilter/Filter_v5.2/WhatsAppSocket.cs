using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

public sealed class WhatsAppSocket
{
	private readonly int m_nTimeout;

	private Socket m_socket;

	public static Random m_random = new Random();

	public WhatsAppSocket(int nTimeout = 2000)
	{
		m_nTimeout = nTimeout;
	}

	public void Connect()
	{
		int[] array = new int[2] { 443, 5222 };
		if (!Connect("g.whatsapp.net", array[m_random.Next(0, 2)]))
		{
			throw new Exception("Cannot connect");
		}
		if (!m_socket.Connected)
		{
			throw new Exception("Cannot connect");
		}
	}

	private bool Connect(string strHost, int nPort)
	{
		try
		{
			m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			m_socket.Connect(strHost, nPort);
			m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, m_nTimeout);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public void Close()
	{
		if (m_socket != null)
		{
			m_socket.Close();
		}
	}

	public byte[] ReadPacket()
	{
		byte[] header = ReadData(3);
		if (header == null || header.Length == 0)
		{
			return null;
		}
		if (header.Length == 3)
		{
			int num = BlasterUtils.GetInt24(header);
			int num2 = num;
			List<byte> list = new List<byte>();
			do
			{
				byte[] array2 = ReadData(num2);
				list.AddRange(array2);
				num2 -= array2.Length;
			}
			while (num2 > 0);
			if (list.Count == num)
			{
				List<byte> list2 = new List<byte>();
				list2.AddRange(header);
				list2.AddRange(list.ToArray());
				return list2.ToArray();
			}
			throw new Exception("Read Next Tree error");
		}
		throw new Exception("Failed to read node header");
	}

	public byte[] ReadData(int nCnt)
	{
		if (m_socket == null)
		{
			return null;
		}
		if (!m_socket.Connected)
		{
			return null;
		}
		byte[] array = new byte[nCnt];
		int num = 0;
		int num2;
		byte[] array2;
		while (true)
		{
			num++;
			try
			{
				num2 = m_socket.Receive(array, 0, nCnt, SocketFlags.None);
			}
			catch (SocketException)
			{
				return null;
			}
			if (num2 <= 0)
			{
				Thread.Sleep(10);
				if (num > 15 || num2 > 0)
				{
					array2 = new byte[num2];
					if (num2 > 0)
					{
						Buffer.BlockCopy(array, 0, array2, 0, num2);
					}
					return array2;
				}
			}
			else if (num > 15 || num2 > 0)
			{
				break;
			}
		}
		array2 = new byte[num2];
		if (num2 > 0)
		{
			Buffer.BlockCopy(array, 0, array2, 0, num2);
		}
		return array2;
	}

	public void SendData(byte[] _data)
	{
		if (m_socket == null)
		{
			throw new Exception("Socket not connected");
		}
		if (!m_socket.Connected)
		{
			throw new Exception("Socket not connected");
		}
		m_socket.Send(_data);
	}
}
