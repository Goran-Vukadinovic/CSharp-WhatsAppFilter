using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;

//[DebuggerNonUserCode]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
internal sealed class AppResourceManager
{
	private static ResourceManager _instance;

	private static CultureInfo cultureInfo;

	internal AppResourceManager()
	{
	}

	internal static ResourceManager GetResourceManager()
	{
		if (_instance == null)
		{
			string baseName = "WhatsAppFilter.Properties.Resources";
			ResourceManager ins = new ResourceManager(baseName, typeof(AppResourceManager).Assembly);
			_instance = ins;
		}
		return _instance;
	}

	internal static CultureInfo GetCultureInfo()
	{
		return cultureInfo;
	}

	internal static void setCultureInfo(CultureInfo info)
	{
		cultureInfo = info;
	}

	internal static Bitmap GetWallpaperBitmap()
	{
		ResourceManager resourceManager = GetResourceManager();
		string name = "default_wallpaper";
		object @object = resourceManager.GetObject(name, cultureInfo);
		return (Bitmap)@object;
	}

	internal static Bitmap GetLogoBmp()
	{
		ResourceManager resourceManager = GetResourceManager();
		string name = "Logo";
		object @object = resourceManager.GetObject(name, cultureInfo);
		return (Bitmap)@object;
	}

	internal static Bitmap GetPaymentPattern1Bmp()
	{
		ResourceManager resourceManager = GetResourceManager();
		string name = "payment_pattern1";
		object @object = resourceManager.GetObject(name, cultureInfo);
		return (Bitmap)@object;
	}
}
