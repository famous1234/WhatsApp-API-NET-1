using System;
using System.Globalization;

namespace WhatsAppApi.Settings
{
    /// <summary>
    /// Holds constant information used to connect to whatsapp server
    /// </summary>
    public class WhatsConstants
    {
        #region ServerConstants

        /// <summary>
        /// The whatsapp host
        /// </summary>
        public const string WhatsAppHost = "c3.whatsapp.net";

        /// <summary>
        /// The whatsapp XMPP realm
        /// </summary>
        public const string WhatsAppRealm = "s.whatsapp.net";

        /// <summary>
        /// The whatsapp server
        /// </summary>
        public const string WhatsAppServer = "s.whatsapp.net";

        /// <summary>
        /// The whatsapp group chat server
        /// </summary>
        public const string WhatsGroupChat = "g.us";

        /// <summary>
        /// The whatsapp version the client complies to
        /// </summary>
		//public const string WhatsAppVer = "2.13.21";
        public const string WhatsAppVer = "2.12.440";

        /// <summary>
        /// The port that needs to be connected to
        /// </summary>
        public const int WhatsPort = 443;

        /// <summary>
        /// iPhone device
        /// </summary>
        //	public const string Device = "S40";
        public const string Device = "Android";

        /// <summary>
        /// manufacturer
        /// </summary>
        public const string Manufacturer = "Xiaomi";

        /// <summary>
        /// OS Version
        /// </summary>
        public const string OS_Version = "4.3";

        /// <summary>
        /// The useragent used for http requests
        /// </summary>
		//public const string UserAgent = "WhatsApp/2.13.21 S40Version/14.26 Device/Nokia302";
        public const string UserAgent = "WhatsApp/2.12.440 Android/4.3 Device/Xiaomi-HM_1SW";

        #endregion ServerConstants

        #region ParserConstants

        /// <summary>
        /// The number style used
        /// </summary>
        public static NumberStyles WhatsAppNumberStyle = (NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

        /// <summary>
        /// Unix epoch DateTime
        /// </summary>
        public static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        #endregion ParserConstants

        public static String WhatsAppCheckHost = "v.whatsapp.net/v2/exist";
        public static String WhatsAppRegisterHost = "v.whatsapp.net/v2/register";
        public static String WhatsAppRequestHost = "v.whatsapp.net/v2/code";
    }
}