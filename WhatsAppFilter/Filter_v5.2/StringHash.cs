using System.Runtime.InteropServices;

internal sealed class StringHash
{
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 7)]
    public struct us_A63LKBUE4RAXWFB32Z5LUWH5NV4Y2EJ8K
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 36)]
	public struct us_PMUKQ8GCMVWTTZ4PGBAXK4R28JFHYC2V
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 32)]
    public struct us_VD6V9ABWEELPC9F8NTUFFX9SHRVQK9AH
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 3)]
    public struct us_XMTEXL6S7R6C6X63R4PDM668FC28838X
	{
	}

	//internal static readonly PMUKQ8GCMVWTTZ4PGBAXK4R28JFHYC2V LRGZC3W4LG6E5MFUFJQB9JKNDGPWVZM6/* Not supported: data(4E 6F 69 73 65 5F 58 58 66 61 6C 6C 62 61 63 6B 5F 32 35 35 31 39 5F 41 45 53 47 43 4D 5F 53 48 41 32 35 36) */;

	//internal static readonly int EVL6WVT8YMUUA9PYXSGYR37WYK7JCRWT/* Not supported: data(A0 02 DC 0F) */;

	//internal static readonly A63LKBUE4RAXWFB32Z5LUWH5NV4Y2EJ8K A8Z9FEP246X8SANPQPFEF8Q9LFAW92TEB/* Not supported: data(7A 05 78 00 80 01 00) */;

	//internal static readonly VD6V9ABWEELPC9F8NTUFFX9SHRVQK9AH JE9Z3X2HT3Q65UY5CA6EHWQ9QATMBRCZ/* Not supported: data(4E 6F 69 73 65 5F 58 58 5F 32 35 35 31 39 5F 41 45 53 47 43 4D 5F 53 48 41 32 35 36 00 00 00 00) */;

	//internal static readonly int PN9WKNWRWZ7LTNL2RBWANJGZBR3CC6NP/* Not supported: data(45 44 00 01) */;

	//internal static readonly XMTEXL6S7R6C6X63R4PDM668FC28838X BCJWR97BJ2VGA5T3JC9QWW5QQN839SXG/* Not supported: data(82 02 02) */;

	//internal static readonly int UXY8P2CEFHJJWBTSUVZAPWWS4CPT2Y8W/* Not supported: data(57 41 05 02) */;

	//internal static readonly int A6DSQYWC7RJY85LKW3PSBAVVNVT94LKBH/* Not supported: data(A8 02 C0 01) */;

	//internal static readonly XMTEXL6S7R6C6X63R4PDM668FC28838X QNSMT9QMC4G483AGW4LFFVL8VCXX443K/* Not supported: data(B8 01 01) */;

	//internal static readonly VD6V9ABWEELPC9F8NTUFFX9SHRVQK9AH M43DA2N67JP8C6N78ZTP234TUBSE47PF/* Not supported: data(4E 6F 69 73 65 5F 49 4B 5F 32 35 35 31 39 5F 41 45 53 47 43 4D 5F 53 48 41 32 35 36 00 00 00 00) */;

	//internal static readonly XMTEXL6S7R6C6X63R4PDM668FC28838X KYVF3KLUUB8UVCM9NFNK8M3EHW3Q92LC/* Not supported: data(80 01 01) */;

	internal static uint CalcHash(string str)
	{
		uint num = default(uint);
		if (str != null)
		{
			num = 2166136261u;
			for (int j = 0; j < str.Length; j++)
			{
				num = (str[j] ^ num) * 16777619;
			}
		}
		return num;
	}
}
