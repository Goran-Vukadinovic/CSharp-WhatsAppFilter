using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class WhatsAppChannel : WhatsAppCallback
{
	public static ChannelInfo m_channelInfo;

	public static bool bDebug;

	public bool m_bOfflinePreview;

	public List<ProtocolTreeNode> m_aryNodesToNeedSend;

	public StatusCode m_eStatusCode;

	protected static string m_strChannelLogFolderPath;

	protected static string m_strBasePath;

	protected static BinTreeNodeReader binReader;

	protected static BinTreeNodeWriter binWriter;

	public ProtocolNodeAttribute m_nodeAttribute;

	protected int m_nTimeout = 300000;

	protected static WhatsAppSocket m_whatsAppSocket;

	public static readonly Encoding m_encoding = Encoding.UTF8;

	public static string m_strWhatsAppDomain = "s.whatsapp.net";

	public static bool bUnKnown = false;

	protected void Init(string strNumber, string staticKeyHash, string staticPrivateKey, string strNickName, string strLogFolder, string strBasePath, bool _bDebug)
	{
		m_nodeAttribute = new ProtocolNodeAttribute
		{
			nCount = -1,
			nAppdata = -1,
			nMessage = -1,
			nNotification = -1,
			nReceipt = -1
		};
		m_aryNodesToNeedSend = new List<ProtocolTreeNode>();
		m_bOfflinePreview = false;
		m_channelInfo.strNumber = strNumber;
		m_channelInfo.staticKeyHash = Convert.FromBase64String(staticKeyHash);
		m_channelInfo.static_private_key = Convert.FromBase64String(staticPrivateKey);
		m_channelInfo.strNickName = strNickName;
		m_strBasePath = strBasePath;
		WhatsAppInfo.LoadInfo(strLogFolder);
		m_strChannelLogFolderPath = Path.Combine(strLogFolder, m_channelInfo.strNumber);
		bDebug = _bDebug;
		binReader = new BinTreeNodeReader();
		m_eStatusCode = StatusCode.Disconnected;
        binWriter = new BinTreeNodeWriter();
		m_whatsAppSocket = new WhatsAppSocket(m_nTimeout);
	}

	public void ConnectToServer()
	{
		try
		{
			m_whatsAppSocket.Connect();
			m_eStatusCode = StatusCode.Connected;
            InvokeConnectSuccessDelegate();
		}
		catch (Exception ex)
		{
			InvokeConnectFailedDelegate(ex);
		}
	}

	public void Disconnect(Exception ex)
	{
		m_whatsAppSocket.Close();
		m_eStatusCode = StatusCode.Disconnected;
        InvokeDisconnectCompleteDelegate(ex);
	}

	public void SendNode(ProtocolTreeNode node)
	{
		try
		{
			m_whatsAppSocket.SendData(binWriter.Write(node, bEncrypt: true));
		}
		catch (Exception)
		{
			Disconnect(null);
		}
	}
}
