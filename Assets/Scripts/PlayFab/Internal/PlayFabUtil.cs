using PlayFab.Json;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace PlayFab.Internal
{
	internal static class PlayFabUtil
	{
		public static readonly string[] _defaultDateTimeFormats;

		public const int DEFAULT_UTC_OUTPUT_INDEX = 2;

		public const int DEFAULT_LOCAL_OUTPUT_INDEX = 9;

		public static DateTimeStyles DateTimeStyles;

		[ThreadStatic]
		private static StringBuilder _sb;

		[Obsolete("This field has moved to SimpleJsonInstance.ApiSerializerStrategy", false)]
		public static SimpleJsonInstance.PlayFabSimpleJsonCuztomization ApiSerializerStrategy => SimpleJsonInstance.ApiSerializerStrategy;

		public static string timeStamp => DateTime.Now.ToString(_defaultDateTimeFormats[9]);

		public static string utcTimeStamp => DateTime.UtcNow.ToString(_defaultDateTimeFormats[2]);

		static PlayFabUtil()
		{
			_defaultDateTimeFormats = new string[15]
			{
				"yyyy-MM-ddTHH:mm:ss.FFFFFFZ",
				"yyyy-MM-ddTHH:mm:ss.FFFFZ",
				"yyyy-MM-ddTHH:mm:ss.FFFZ",
				"yyyy-MM-ddTHH:mm:ss.FFZ",
				"yyyy-MM-ddTHH:mm:ssZ",
				"yyyy-MM-dd HH:mm:ssZ",
				"yyyy-MM-dd HH:mm:ss.FFFFFF",
				"yyyy-MM-dd HH:mm:ss.FFFF",
				"yyyy-MM-dd HH:mm:ss.FFF",
				"yyyy-MM-dd HH:mm:ss.FF",
				"yyyy-MM-dd HH:mm:ss",
				"yyyy-MM-dd HH:mm.ss.FFFF",
				"yyyy-MM-dd HH:mm.ss.FFF",
				"yyyy-MM-dd HH:mm.ss.FF",
				"yyyy-MM-dd HH:mm.ss"
			};
			DateTimeStyles = DateTimeStyles.RoundtripKind;
		}

		public static string Format(string text, params object[] args)
		{
			return (args.Length <= 0) ? text : string.Format(text, args);
		}

		public static string ReadAllFileText(string filename)
		{
			if (_sb == null)
			{
				_sb = new StringBuilder();
			}
			_sb.Length = 0;
			FileStream input = new FileStream(filename, FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(input);
			while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
			{
				_sb.Append(binaryReader.ReadChar());
			}
			return _sb.ToString();
		}
	}
}
