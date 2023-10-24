using System.Threading;

public static class TicketCounter
{
	private static int m_nCounter1 = 0;

	private static int m_nCounter2 = -1;

	public static int NextTicket_1()
	{
		return Interlocked.Increment(ref m_nCounter1);
	}

	public static string MakeId_1()
	{
		int num = NextTicket_1();
		string format = num.ToString("X").ToLower();
		return string.Format(format, new object[0]);
	}

	public static string MakeId_2()
	{
		int num = Interlocked.Increment(ref m_nCounter2);
		string format = num.ToString("X2").ToLower();
		return string.Format(format, new object[0]);
	}

	public static string GetRandHexString(int nLen)
	{
		NextTicket_1();
		return BlasterUtils.GenerateRandomHexString(nLen).ToUpper();
	}

	public static void Init()
	{
		m_nCounter1 = 0;
		m_nCounter2 = -1;
	}
}
