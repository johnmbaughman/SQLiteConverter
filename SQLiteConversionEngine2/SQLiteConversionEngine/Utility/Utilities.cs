#region

// -----------------------------------------------------
// MIT License
// Copyright (C) 2012 John M. Baughman (jbaughmanphoto.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------

#endregion

using System;
using System.Collections.Generic;

namespace SQLiteConversionEngine.Utility {
	public class Utilities {

		public static string ConvertListToInClause(List<string> list, string columnName, bool isString = true) {
			try {
				if (list.Count == 0) {
					return string.Empty;
				}
				string returnString = string.Format(" {0} IN (|", columnName);
				foreach (var item in list) {
					if (!returnString.Contains(item)) {
						if (isString) {
							returnString += string.Format(", '{0}'", item);
						}
						else {
							returnString += string.Format(", {0}", item);
						}
					}
				}
				returnString += ")";
				returnString = returnString.Replace("|,", "");

				return returnString;
			}
			catch (Exception ex) {
				//Logging.Log(LogLevel.Error, string.Format("ConvertListToInClause exception: {0}", LoggingEngine.FileLogger.GetInnerException(ex).ToString()));
				return string.Empty;
			}
		}

		public static List<string> ConvertListToCase(List<string> list, bool upperCase) {
			for (int i = 0; i < list.Count; i++) {
				list[i] = upperCase ? list[i].ToUpper() : list[i].ToLower();
			}
			return list;
		}

		/// <summary>
		/// Parse strings into true or false bools using relaxed parsing rules
		/// Source: http://www.dotnetperls.com/bool-parse
		/// </summary>
		public static class BoolParser {

			/// <summary>
			/// Get the boolean value for this string
			/// </summary>
			public static bool GetValue(string value) {
				return IsTrue(value);
			}

			/// <summary>
			/// Determine whether the string is not True
			/// </summary>
			public static bool IsFalse(string value) {
				return !IsTrue(value);
			}

			/// <summary>
			/// Determine whether the string is equal to True
			/// </summary>
			public static bool IsTrue(string value) {
				try {
					// 1
					// Avoid exceptions
					if (value == null) {
						return false;
					}

					// 2
					// Remove whitespace from string
					value = value.Trim();

					// 3
					// Lowercase the string
					value = value.ToLower();

					// 4
					// Check for word true
					if (value == "true") {
						return true;
					}

					// 5
					// Check for letter true
					if (value == "t") {
						return true;
					}

					// 6
					// Check for one
					if (value == "1") {
						return true;
					}

					// 7
					// Check for word yes
					if (value == "yes") {
						return true;
					}

					// 8
					// Check for letter yes
					if (value == "y") {
						return true;
					}

					// 9
					// It is false
					return false;
				}
				catch {
					return false;
				}
			}
		}
	}
}