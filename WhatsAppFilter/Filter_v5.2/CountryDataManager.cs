using System;
using System.IO;
using System.Linq;

public class CountryDataManager
{
	public string m_strCountryName;

	public string m_strAreaCode;

	public string m_strAreaNumber;

	public string m_strCountryId;

	public string m_strCountryLang;

	protected string m_strUnknownCode;

	protected string m_strUnknownCode2;

	public CountryDataManager(string strChannelNumber, string strBasepath)
	{
		if (!File.Exists(Path.Combine(strBasepath, "countries.csv")))
		{
			return;
		}
		string text = File.ReadAllText(Path.Combine(strBasepath, "countries.csv"));
		string[] array = text.Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
		string[] array2 = array;
		int num = 0;
		string[] array3;
		while (true)
		{
			if (num < array2.Length)
			{
				string text2 = array2[num];
				array3 = text2.Trim('\r').Split(',');
				if (strChannelNumber.StartsWith(array3[1]))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		m_strCountryName = array3[0].Trim('"');
		if (array3[1].StartsWith("1"))
		{
			array3[1] = "1";
		}
		m_strAreaCode = array3[1];
		m_strAreaNumber = strChannelNumber.Substring(m_strAreaCode.Length);
		m_strCountryId = array3[4].Trim('"');
		m_strCountryLang = array3[5].Trim('"');
		m_strUnknownCode = array3[2].Trim('"');
		m_strUnknownCode2 = array3[3].Trim('"');
		if (m_strUnknownCode.Contains('|'))
		{
			string[] array4 = m_strUnknownCode.Split('|');
			m_strUnknownCode = array4[0];
		}
	}

	public string GetPhoneNumber()
	{
		return m_strAreaCode + m_strAreaNumber;
	}

	public string GetFixSizeUnknownCode()
	{
		return m_strUnknownCode.PadLeft(3, (char)48);
	}

	public string GetFixSizeUnknownCode2()
	{
		return m_strUnknownCode2.PadLeft(3, (char)48);
	}
}
