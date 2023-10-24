public sealed class ChatGroupInfo
{
	public string m_strId;

	public string m_strCreator;

	public string m_strBody;

	public long m_lCreation;

	public string m_strSubject;

	public long m_s_t;

	public string m_str_s_o;

	public string[] m_aryJids;

	public string[] m_aryErrorJids;

	public bool m_bGroup;

	public ChatGroupInfo(string strArg0)
	{
		this.m_strId = strArg0;
	}

	internal ChatGroupInfo(string strId, string strCreator, string strCreation, string strSubject, string str_s_t, string str_s_o, string strBody, string[] aryJids, string[] aryErrorJids, bool bGroup)
	{
		this.m_strId = strId;
		m_strBody = strBody;
		this.m_strCreator = strCreator;
		long.TryParse(strCreation, out this.m_lCreation);
		this.m_strSubject = strSubject;
		long.TryParse(str_s_t, out m_s_t);
		this.m_str_s_o = str_s_o;
		this.m_aryJids = aryJids;
		this.m_aryErrorJids = aryErrorJids;
		m_bGroup = bGroup;
	}
}
