using System.Runtime.InteropServices;
using System.Text;

public sealed class KeyIniFileManager
{
	public string m_strFilePath;

	public KeyIniFileManager(string strFilePath)
	{
		m_strFilePath = strFilePath;
	}

	[DllImport("kernel32", EntryPoint = "WritePrivateProfileString")]
	private static extern long WritePrivateProfileString(string strAppName, string strKeyName, string strData, string strFilePath);

	[DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
	private static extern int GetPrivateProfileString(string strAppName, string strKeyName, string strDefault, StringBuilder sbReturnString, int nSize, string strFilePath);

	public void WriteSetting(string strAppName, string strKeyName, string strData)
	{
		WritePrivateProfileString(strAppName, strKeyName, strData, m_strFilePath);
	}

	public string ReadSetting(string strAppName, string strKeyName)
	{
		StringBuilder stringBuilder = new StringBuilder(255);
		GetPrivateProfileString(strAppName, strKeyName, "", stringBuilder, 255, m_strFilePath);
		return stringBuilder.ToString();
	}
}
