using System;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

public sealed class LicenseManager
{
    public static bool CheckLicense()
    {
        ApplicationManager.strUserId = "testman0427";
        ApplicationManager.strLicense = "1234-2345-3456-4567-5678";
        ApplicationManager.strHardwareId = GetHardwareId();
        ApplicationManager.expireDate = Long2DateTime(2576980377L);
        return true;
    }
    private static DateTime Long2DateTime(long lTick)
	{
		int year = 1970;
		int month = 1;
		int day = 1;
		int hour = 0;
		int minute = 0;
		int second = 0;
		int millisecond = 0;
		DateTime dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
		return dateTime.AddSeconds(lTick);
	}

	private static string GetHardwareId()
	{
		ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher();
		StringBuilder stringBuilder = new StringBuilder();
		managementObjectSearcher.Query = new ObjectQuery("select * from Win32_Processor");
		foreach (ManagementObject item in managementObjectSearcher.Get())
		{
			PropertyDataCollection properties = item.Properties;
			stringBuilder.Append(GetPropertyValue(properties["ProcessorId"]));
			stringBuilder.Append(',');
		}
		managementObjectSearcher.Query = new ObjectQuery("select * from Win32_BaseBoard");
		foreach (ManagementObject item2 in managementObjectSearcher.Get())
		{
			PropertyDataCollection properties2 = item2.Properties;
			stringBuilder.Append(GetPropertyValue(properties2["SerialNumber"]));
			stringBuilder.Append(',');
		}
		stringBuilder.Append(Environment.UserName);
		stringBuilder.Append(',');
		stringBuilder.Append(GetMachineGuid());
		string text = GetMd5HexString(stringBuilder.ToString() + ApplicationManager.strProductId);
		text = text.Insert(16, "-");
		text = text.Insert(8, "-");
		string strProductId = ApplicationManager.strProductId;
		return strProductId + "-" + text;
	}

	public static string GetMachineGuid()
	{
		string strMachineGhid = "MachineGuid";
		using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
		using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography");
		if (registryKey2 == null)
		{
			return "Key Not Found!!!";
		}
		object value = registryKey2.GetValue(strMachineGhid);
		if (value != null)
		{
			string text4 = value.ToString().Trim();
			if (text4.Length != 36)
			{
				text4 = "Bad hwid!";
			}
			return text4;
		}
		return string.Format("Index Not Found: {0}", strMachineGhid);
	}

	private static string GetPropertyValue(PropertyData propertyData)
	{
		string text = string.Empty;
		if (propertyData.Value != null && !string.IsNullOrEmpty(propertyData.Value.ToString()))
		{
			string strType = propertyData.Value.GetType().ToString();
			if (strType == "System.String[]")
			{
				string[] array = (string[])propertyData.Value;
				foreach (string text4 in array)
				{
					text = text + text4 + " ";
				}
			}
			else
			{
				if (strType == "System.UInt16[]")
				{
					ushort[] array3 = (ushort[])propertyData.Value;
					foreach (ushort num in array3)
					{
						text = text + num.ToString() + " ";
					}
				}
				else
				{
					text = propertyData.Value.ToString();
				}
			}
		}
		return text;
	}

	public static string GetMd5HexString(string strData)
	{
		using MD5 mD = MD5.Create();
		byte[] bytes = Encoding.ASCII.GetBytes(strData);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < array.Length; j++)
		{
			ref byte reference = ref array[j];
			stringBuilder.Append(reference.ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static byte[] HexString2ByteArray(string strHex)
	{
		strHex = strHex.Replace("-", "");
		byte[] array = new byte[strHex.Length / 2];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = Convert.ToByte(strHex.Substring(j * 2, 2), 16);
		}
		return array;
	}

	private static byte[] GetSaltFromUserId()
	{
		byte[] array = new byte[8];
		SHA512CryptoServiceProvider sHA512CryptoServiceProvider = new SHA512CryptoServiceProvider();
		byte[] src = sHA512CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(ApplicationManager.strUserId));
		Buffer.BlockCopy(src, 0, array, 0, 8);
		return array;
	}

	private static byte[] EncryptData(byte[] data, byte[] pwd)
	{
		byte[] result = null;
		byte[] salt = GetSaltFromUserId();
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			try
			{
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(pwd, salt, 1000);
				rijndaelManaged.KeySize = 256;
				rijndaelManaged.BlockSize = 128;
				rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
				rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
				rijndaelManaged.Mode = CipherMode.CBC;
				CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write);
				try
				{
					cryptoStream.Write(data, 0, data.Length);
					cryptoStream.Close();
				}
				finally
				{
					((IDisposable)cryptoStream).Dispose();
				}
				result = memoryStream.ToArray();
			}
			finally
			{
				((IDisposable)rijndaelManaged).Dispose();
			}
		}
		finally
		{
			((IDisposable)memoryStream).Dispose();
		}
		return result;
	}
}
