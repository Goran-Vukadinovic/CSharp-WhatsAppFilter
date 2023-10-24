using System;

internal sealed class PublicKeyDataParser
{
	private readonly byte[] _data;

	public byte[] data_10;

	public byte[] data_18;

	public byte[] data_26;

	public PublicKeyDataParser(byte[] data)
	{
		data_10 = null;
		data_18 = null;
		data_26 = null;
		_data = data;
	}

	public bool Parse()
	{
		try
		{
			int num = 5;
			if (_data[2] > 127)
			{
				num++;
			}
			if (_data[num] != 10)
			{
				num += 2;
				data_10 = new byte[1] { 49 };
			}
			else
			{
				int num2 = _data[num + 1];
				data_10 = new byte[num2];
				Array.Copy(_data, num + 2, data_10, 0, num2);
				num = num + num2 + 2;
			}
			if (_data[num] == 18)
			{
				int num3 = _data[num + 1];
				data_18 = new byte[num3];
				Array.Copy(_data, num + 2, data_18, 0, num3);
				num = num + num3 + 2;
			}
			if (_data[num] == 26)
			{
				int num4 = _data[num + 1];
				if (num4 > 128)
				{
					num++;
				}
				data_26 = new byte[num4];
				Array.Copy(_data, num + 2, data_26, 0, num4);
				num = num + num4 + 2;
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}
}
