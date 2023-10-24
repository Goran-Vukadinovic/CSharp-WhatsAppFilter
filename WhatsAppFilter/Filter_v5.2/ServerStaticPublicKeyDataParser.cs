using System;

internal sealed class ServerStaticPublicKeyDataParser
{
	public byte[] _data;

	public byte[] data_10;

	public byte[] data_18;

	public ServerStaticPublicKeyDataParser(byte[] data)
	{
		_data = data;
	}

	public bool Parse()
	{
		try
		{
			int num = 0;
			if (_data[0] == 10)
			{
				int num2 = _data[num + 1];
				data_10 = new byte[num2];
				Array.Copy(_data, 2, data_10, 0, _data[1]);
			}
			num = _data[1] + 2;
			if (_data[num] == 18)
			{
				int num3 = _data[num + 1];
				data_18 = new byte[num3];
				Array.Copy(_data, num + 2, data_18, 0, _data[num + 1]);
			}
		}
		catch
		{
			return false;
		}
		return true;
	}
}
