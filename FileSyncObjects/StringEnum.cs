using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// This class handles strings attached to enum type values.
	/// </summary>
	[DataContract]
	public class StringEnum {
		
		/// <summary>
		/// Collection of string values already found, stored for quicker use.
		/// </summary>
		private static Hashtable cachedValues = new Hashtable();

		/// <summary>
		/// Gets a string attached to the specified enumeration member
		/// </summary>
		/// <param name="value">enumberation member</param>
		/// <returns>string property of the enum member</returns>
        public static string GetStringValue(Enum value) {
			string output = null;
			Type type = value.GetType();

			//Check first in our cached results...

			if (cachedValues.ContainsKey(value))
				output = (cachedValues[value] as StringValueAttribute).Value;
			else {
				//Look for our 'StringValueAttribute' 

				//in the field's custom attributes

				FieldInfo fi = type.GetField(value.ToString());
				StringValueAttribute[] attrs =
				   fi.GetCustomAttributes(typeof(StringValueAttribute),
										   false) as StringValueAttribute[];
				if (attrs.Length > 0) {
					cachedValues.Add(value, attrs[0]);
					output = attrs[0].Value;
				}
			}

			return output;
		}

	}
}
