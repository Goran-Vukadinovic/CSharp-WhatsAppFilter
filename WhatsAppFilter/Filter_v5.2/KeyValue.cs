using System;
using System.Diagnostics;

public sealed class KeyValue
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string Key;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string Value;

	public KeyValue(string key, string value)
	{
		if (value == null || key == null)
		{
			throw new NullReferenceException();
		}
		SetKey(key);
		SetValue(value);
	}

	public string GetKey()
	{
		return Key;
	}

	private void SetKey(string key)
	{
		Key = key;
	}

	public string GetValue()
	{
		return Value;
	}

	private void SetValue(string value)
	{
		Value = value;
	}
}
