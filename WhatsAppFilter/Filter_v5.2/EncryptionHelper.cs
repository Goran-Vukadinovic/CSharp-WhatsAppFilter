using System;
using System.IO;
using System.Security.Cryptography;
using Security.Cryptography;

internal sealed class EncryptionHelper
{
	private byte[] m_hashChainData = null;

	private byte[] m_encryptKey = null;

	public static byte[] HashData_1 = new byte[36]
	{
		78, 111, 105, 115, 101, 95, 88, 88, 102, 97,
		108, 108, 98, 97, 99, 107, 95, 50, 53, 53,
		49, 57, 95, 65, 69, 83, 71, 67, 77, 95,
		83, 72, 65, 50, 53, 54
	};

	public static byte[] HashData_2 = new byte[32]
	{
		78, 111, 105, 115, 101, 95, 88, 88, 95, 50,
		53, 53, 49, 57, 95, 65, 69, 83, 71, 67,
		77, 95, 83, 72, 65, 50, 53, 54, 0, 0,
		0, 0
	};

	public static byte[] HashData_3 = new byte[32]
	{
		78, 111, 105, 115, 101, 95, 73, 75, 95, 50,
		53, 53, 49, 57, 95, 65, 69, 83, 71, 67,
		77, 95, 83, 72, 65, 50, 53, 54, 0, 0,
		0, 0
	};

	public static byte[] HashData_4 = new byte[4] { 87, 65, 5, 2 };

	public static byte[] aryHeader = new byte[4] { 69, 68, 0, 1 };

	private readonly Sha256HashChain m_hashChain;

	private int m_nIV;

	public EncryptionHelper(byte[] data, byte[] hashData)
	{
		m_hashChain = new Sha256HashChain(data);
		m_hashChainData = m_hashChain.m_sha256HashChain;
		m_hashChain.MakeHashChain(hashData);
	}

	public byte[] GetPublicKey(byte[] data)
	{
		m_hashChain.MakeHashChain(data);
		return data;
	}

	public static byte[] Get_IV(int v, int nSize)
	{
		byte[] array = new byte[nSize];
		byte[] bytes = BitConverter.GetBytes(v);
		Array.Reverse(bytes);
		Array.Copy(bytes, 0, array, 8, 4);
		return array;
	}

	public byte[] GetPubKeyData(byte[] keyData)
	{
		byte[] result;
		if (m_encryptKey != null)
		{
			int nIV = m_nIV;
			m_nIV++;
			result = WhatsAppDecrypt(keyData, m_encryptKey, Get_IV(nIV, 12), m_hashChain.m_sha256HashChain);
		}
		else
		{
			result = keyData;
		}
		m_hashChain.MakeHashChain(keyData);
		return result;
	}

	public byte[] SetChannelPubKey(byte[] data)
	{
		m_hashChain.MakeHashChain(data);
		return data;
	}

	public byte[] GetEncryptedData(byte[] data)
	{
		byte[] array;
		if (m_encryptKey != null)
		{
			int nIv = m_nIV;
			m_nIV++;
			array = WhatsAppEncrypt(data, m_encryptKey, Get_IV(nIv, 12), m_hashChain.m_sha256HashChain);
			m_hashChain.MakeHashChain(array);
			return array;
		}
		array = data;
		m_hashChain.MakeHashChain(array);
		return array;
	}

	public static byte[] WhatsAppDecrypt(byte[] data, byte[] decryptKey, byte[] ivData, byte[] authData)
	{
		byte[] array = new byte[data.Length - 16];
		byte[] array2 = new byte[16];
		Array.Copy(data, data.Length - 16, array2, 0, 16);
		Array.Copy(data, 0, array, 0, data.Length - 16);
		AuthenticatedAesCng authenticatedAesCng = new AuthenticatedAesCng
		{
			CngMode = CngChainingMode.Gcm,
			Key = decryptKey,
			IV = ivData,
			AuthenticatedData = authData,
			Tag = array2
		};
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, authenticatedAesCng.CreateDecryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(array, 0, array.Length);
		cryptoStream.FlushFinalBlock();
		return memoryStream.ToArray();
	}

	public static byte[] WhatsAppEncrypt(byte[] data, byte[] encryptKey, byte[] iv, byte[] authenticatedData)
	{
		AuthenticatedAesCng authenticatedAesCng = new AuthenticatedAesCng
		{
			CngMode = CngChainingMode.Gcm,
			Key = encryptKey,
			IV = iv
		};
		MemoryStream memoryStream;
		IAuthenticatedCryptoTransform authenticatedCryptoTransform;
		CryptoStream cryptoStream;
		byte[] tag;
		byte[] array;
		byte[] array2;
		if (authenticatedData != null)
		{
			authenticatedAesCng.AuthenticatedData = authenticatedData;			
		}
		memoryStream = new MemoryStream();
		authenticatedCryptoTransform = authenticatedAesCng.CreateAuthenticatedEncryptor();
		cryptoStream = new CryptoStream(memoryStream, authenticatedCryptoTransform, CryptoStreamMode.Write);
		cryptoStream.Write(data, 0, data.Length);
		cryptoStream.FlushFinalBlock();
		tag = authenticatedCryptoTransform.GetTag();
		array = memoryStream.ToArray();
		array2 = new byte[tag.Length + array.Length];
		Array.Copy(array, array2, array.Length);
		Array.Copy(tag, 0, array2, array.Length, tag.Length);
		return array2;
	}

	public byte[] GetEncryptKey()
	{
		byte[] data1 = new byte[0];
		byte[] data2 = m_hashChainData;
		return ParseKeyData(data1, data2, 64);
	}

	private byte[] ParseKeyData(byte[] data, byte[] chainData, int nLen)
	{
		if (chainData == null)
		{
			chainData = new byte[32];
		}
		byte[] hmac = BlasterUtils.GetHMacHash(chainData, data);
		data = new byte[0];
		byte[] array = new byte[0];
		int num = 1;
		while (data.Length < nLen)
		{
			byte[] data2 = ConcatArray(array, new byte[1] { (byte)num });
			array = BlasterUtils.GetHMacHash(hmac, data2);
			data = ConcatArray(data, array);
			num++;
		}
		byte[] array2 = new byte[64];
		Array.Copy(data, 0, array2, 0, 64);
		return array2;
	}

	private byte[] ConcatArray(byte[] data, byte[] data2)
	{
		byte[] array = new byte[data.Length + data2.Length];
		Array.Copy(data, 0, array, 0, data.Length);
		Array.Copy(data2, 0, array, data.Length, data2.Length);
		return array;
	}

	public void ParseKeyData(byte[] data)
	{
		byte[] sourceArray = ParseKeyData(data, m_hashChainData, 64);
		m_nIV = 0;
		m_hashChainData = new byte[32];
		m_encryptKey = new byte[32];
		Array.Copy(sourceArray, 0, m_hashChainData, 0, 32);
		Array.Copy(sourceArray, 32, m_encryptKey, 0, 32);
	}
}
