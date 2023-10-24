using System;
using System.Diagnostics;
using System.Threading;

public class OnWriteNodeDelegateMgr
{
	public delegate void DelegateWriteData(object strData);

	protected static OnWriteNodeDelegateMgr _instance;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private DelegateWriteData delegateWriteData;

	public static OnWriteNodeDelegateMgr GetInstance()
	{
		if (_instance == null)
		{
			_instance = new OnWriteNodeDelegateMgr();
			return _instance;
		}
		return _instance;
	}

	public void AddDelegate(DelegateWriteData _delegate)
	{
		DelegateWriteData d1 = delegateWriteData;
		DelegateWriteData d2;
		do
		{
			d2 = d1;
			DelegateWriteData value = (DelegateWriteData)Delegate.Combine(d2, _delegate);
			d1 = Interlocked.CompareExchange(ref delegateWriteData, value, d2);
		}
		while ((object)d1 != d2);
	}

	public void RemoveDelegate(DelegateWriteData _delegate)
	{
		DelegateWriteData d1 = delegateWriteData;
		DelegateWriteData d2;
		do
		{
			d2 = d1;
			DelegateWriteData value = (DelegateWriteData)Delegate.Remove(d2, _delegate);
			d1 = Interlocked.CompareExchange(ref delegateWriteData, value, d2);
		}
		while ((object)d1 != d2);
	}

	internal void InvokeDelegate(object _delegate)
	{
		delegateWriteData?.Invoke(_delegate);
	}
}
