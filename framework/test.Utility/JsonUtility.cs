using System;
using ServiceStack.Text;
using System.IO;

namespace test.Utility
{
	public static class JsonUtility
	{
		public static string Serialize<T> (T model)
		{
			return JsonSerializer.SerializeToString (model);
		}

		public static string Serialize (object model)
		{
			if (model == null) {
				return string.Empty;
			}

			return JsonSerializer.SerializeToString (model, model.GetType ());
		}

		public static T Deserialize<T> (string src)
		{
			if (string.IsNullOrEmpty (src)) {
				return default(T);
			}

			return JsonSerializer.DeserializeFromString<T> (src);
		}

		public static object Deserialize (string src, Type type)
		{
			if (string.IsNullOrEmpty (src)) {
				return null;
			}

			return JsonSerializer.DeserializeFromString (src, type);
		}

		public static void SerializeToStream<T> (T value, Stream writeStream)
		{
			JsonSerializer.SerializeToStream (value, writeStream);
		}

		public static object DeserializeFromStream (Type type, Stream stream)
		{
			var result = JsonSerializer.DeserializeFromStream (type, stream);
			return result;
		}
	}
}

