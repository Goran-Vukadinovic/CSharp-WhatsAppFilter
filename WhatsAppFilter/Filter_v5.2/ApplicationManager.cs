using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

internal static class ApplicationManager
{
	public static string version = "5.2";

	public static string strContact = "wasoft@pm.me";

	public static string strProductId = "WF";

	public static DateTime expireDate = new DateTime();

	public static string strUserId = "";

	public static string strLicense = "";

	public static string strHardwareId = "";
    public static void Run()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += new ThreadExceptionEventHandler(ThreadException);
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
        if (LicenseManager.CheckLicense())
        {
            Application.Run(new MainForm());
        }
        else
        {
            Application.Run(new LicenseForm());
        }
    }

	private static void ThreadException(object objArg0, ThreadExceptionEventArgs exceptionInfo)
	{
		string text = $"Error:\r\n{exceptionInfo.Exception.Message}\r\n{exceptionInfo.Exception.StackTrace}\r\nplease contact support.";
		MessageBox.Show(text, "Unexpected error");
	}

	private static void UnhandledException(object objArg0, UnhandledExceptionEventArgs exeptionInfo)
	{
		string text = $"Error:\r\n{((Exception)exeptionInfo.ExceptionObject).Message}\r\n{((Exception)exeptionInfo.ExceptionObject).StackTrace}\r\nplease contact support.";
		MessageBox.Show(text, "Unexpected error");
	}

}
