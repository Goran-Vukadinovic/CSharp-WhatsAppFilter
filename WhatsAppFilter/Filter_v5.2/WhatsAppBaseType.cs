using System;

public class WhatsAppBaseType
{
	public enum PrivacyCategory
	{
        PrivacyCategoryProfile,
        PrivacyCategoryStatus,
        PrivacyCategoryLast,
        PrivacyCategoryReadReceipts,
        PrivacyCategoryGroupAdd
    }

	public struct ContactInfo
	{
		public string strJid;

		public string strStatus;

		public string str_t;

		public bool bIsBusiness;

		public string[] arySubJid_DeviceList;

		public Business_Profile businessProfile;
	}

	public enum DataType
	{
		Full,
		Delta,
		Query,
		Chunked,
		Devices
	}

	public struct Business_Profile
	{
		public string address;

		public string description;

		public string email;

		public string verified_name;

		public string verified_level;
	}

	public struct MediaData
	{
		public byte[] hmacKey;

		public byte[] encryptedData;

		public byte[] privateKey;

		public byte[] dataHash;

		public byte[] encDataHash;

		public int dataLen;
	}

	public enum PrivacySetting
	{
        PrivacySettingNone,
        PrivacySettingContacts,
        PrivacySettingAll
    }

	public struct InteractiveButtonInfo
	{
		public string strCaption;

		public string strLink;

		public byte nType;
	}

	public enum DataContext
	{
		Interactive,
		Background,
		Registration
	}

	public enum StatusCode
	{
        Disconnected,
        Connected,
        Success
    }

	protected string GetPrivacyVisibilityString(PrivacySetting visibilityValue)
	{
        switch (visibilityValue)
        {
            default:
                {
                    throw new Exception("Invalid visibility setting");
                }
            case PrivacySetting.PrivacySettingNone:
                return "none";
            case PrivacySetting.PrivacySettingContacts:
                return "contacts";
            case PrivacySetting.PrivacySettingAll:
                return "all";
        }
    }

	protected string GetPrivacyCategoryString(PrivacyCategory privacyCategory)
	{
        switch (privacyCategory)
        {
            default:
                {
                    throw new Exception("Invalid privacy category");
                }
            case PrivacyCategory.PrivacyCategoryProfile:
                return "profile";
            case PrivacyCategory.PrivacyCategoryStatus:
                return "status";
            case PrivacyCategory.PrivacyCategoryLast:
                return "last";
            case PrivacyCategory.PrivacyCategoryReadReceipts:
                return "readreceipts";
            case PrivacyCategory.PrivacyCategoryGroupAdd:
                return "groupadd";
        }
    }

    protected PrivacyCategory ParsePrivacyCategory(string strVal)
    {
        PrivacyCategory result;
        if (!(strVal == "last"))
        {
            if (strVal == "status")
            {
                result = PrivacyCategory.PrivacyCategoryStatus;
            }
            else
            {
                string text5 = "profile";
                string text6 = text5;
                if (strVal == "profile")
                {
                    result = PrivacyCategory.PrivacyCategoryProfile;
                }
                else
                {
                    if (strVal == "readreceipts")
                    {
                        result = PrivacyCategory.PrivacyCategoryReadReceipts;
                    }
                    else
                    {
                        if (!(strVal == "groupadd"))
                        {
                            throw new Exception(string.Format("Could not parse {0} as privacy category", strVal));
                        }
                        result = PrivacyCategory.PrivacyCategoryGroupAdd;
                    }
                }
            }
            return result;
        }
        result = PrivacyCategory.PrivacyCategoryLast;
        return result;
    }

    protected PrivacySetting ParsePrivacySetting(string strVal)
	{
        PrivacySetting result;
        if (!(strVal == "none"))
        {
            if (!(strVal == "contacts"))
            {
                if (!(strVal == "all"))
                {
                    string text7 = "Cound not parse {0} as privacy setting";
                    string format = text7;
                    throw new Exception(string.Format(format, strVal));
                }
                result = PrivacySetting.PrivacySettingAll;
            }
            else
            {
                result = PrivacySetting.PrivacySettingContacts;
            }
            return result;
        }
        result = PrivacySetting.PrivacySettingNone;
        return result;
    }
}
