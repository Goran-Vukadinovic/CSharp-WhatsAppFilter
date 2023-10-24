using System;
using System.IO;

public sealed class WhatsAppInfo
{
	public static string strVersion = "2.20.47";

	public static string strUnknown = "armani";

	public static string strDevice = "Xiaomi";

	public static int nUnknown = 0;

	public static string strAndroidVersion = "4.4";

	public static string strPhone = "JLS36C";

	public static string strUserAgent = "WhatsApp/2.20.47 Android/4.4 Device/Xiaomi-HM_1SW";

	public static string strProcessor = "MSM8974";

	public static void LoadInfo(string strSaveFolder)
	{
		string path2 = Path.Combine(strSaveFolder, "constants.txt");
		if (File.Exists(path2))
		{
			Random random = new Random();
			try
			{
				string[] array = File.ReadAllLines(path2);
				int num = random.Next(0, array.Length);
				string[] array2 = array[num].Split(';');
				strVersion = array2[0];
				strUnknown = array2[1];
				strDevice = array2[2];
				nUnknown = int.Parse(array2[3]);
				strAndroidVersion = array2[4];
				strPhone = array2[5];
				strProcessor = array2[7];
				string[] array3 = new string[6];
				array3[0] = "WhatsApp/";
                array3[1] = strVersion;
				array3[2] = " Android/";
				array3[3] = strAndroidVersion;
				array3[4] = " Device/";
				array3[5] = array2[6];
				strUserAgent = string.Concat(array3);
			}
			catch (Exception)
			{
			}
		}
	}
}
