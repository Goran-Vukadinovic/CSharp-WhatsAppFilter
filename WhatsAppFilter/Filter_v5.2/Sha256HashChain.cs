using System;
using System.Security.Cryptography;

internal sealed class Sha256HashChain
{
	public byte[] m_sha256HashChain;

	public Sha256HashChain(byte[] data)
	{
		if (data.Length > 32)
		{
			SHA256Managed sHA256Managed = new SHA256Managed();
			m_sha256HashChain = sHA256Managed.ComputeHash(data);
		}
		else
		{
			m_sha256HashChain = data;
		}
	}

	public void MakeHashChain(byte[] data)
	{
		byte[] array = new byte[m_sha256HashChain.Length + data.Length];
		Array.Copy(m_sha256HashChain, array, 32);
		Array.Copy(data, 0, array, 32, data.Length);
		SHA256Managed sHA256Managed = new SHA256Managed();
		m_sha256HashChain = sHA256Managed.ComputeHash(array);
	}
}
