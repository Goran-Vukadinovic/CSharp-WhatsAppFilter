using System;

internal sealed class ServerStaticPublicKeyParser
{
	public byte[] _data;

	public byte[] data_10;

	public byte[] data_18;

	public byte[] data_26;

	public byte[] data_34;

	public byte[] data_42;

	public ServerStaticPublicKeyParser(byte[] data)
	{
		data_10 = null;
		data_18 = null;
		data_26 = null;
		data_34 = null;
		data_42 = null;
		_data = data;
	}

	public bool Parse()
	{
		try
		{
			int num = 0;
			if (_data[0] != 10)
			{
				num += 2;
				data_10 = new byte[1] { 49 };
			}
			else
			{
				int num2 = _data[num + 1];
				data_10 = new byte[num2];
				Array.Copy(_data, 2, data_10, 0, _data[1]);
				num = _data[1] + 2;
			}
			if (_data[num] == 18)
			{
				int num3 = _data[num + 1];
				data_18 = new byte[num3];
				Array.Copy(_data, num + 2, data_18, 0, _data[num + 1]);
				num = num + _data[num + 1] + 2;
			}
			if (_data[num] == 26)
			{
				int num4 = _data[num + 1];
				data_26 = new byte[num4];
				Array.Copy(_data, num + 2, data_26, 0, _data[num + 1]);
				num = num + _data[num + 1] + 2;
			}
			if (_data[num] == 34)
			{
				int num5 = _data[num + 1];
				data_34 = new byte[num5];
				Array.Copy(_data, num + 2, data_34, 0, _data[num + 1]);
				num = num + _data[num + 1] + 2;
			}
			if (_data[num] == 42)
			{
				int num6 = _data[num + 1];
				data_42 = new byte[num6];
				Array.Copy(_data, num + 2, data_42, 0, _data[num + 1]);
			}
		}
		catch
		{
			return false;
		}
		return true;
	}
}
