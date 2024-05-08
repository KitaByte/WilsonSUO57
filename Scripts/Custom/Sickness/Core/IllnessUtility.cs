using System;
using System.Text.RegularExpressions;

namespace Server.Engines.Sickness
{
	public static class IllnessUtility
	{
		// Takes a string and returns a new string where the
		// first character of each word is in uppercase and the rest
		// of the characters are in lowercase
		public static string SplitCamelCase(string word)
		{
			return Regex.Replace(word, @"(\B[A-Z])", " $1");
		}

		// Print text to console in n color and reset after
		public static void ToConsole(string msg, ConsoleColor mainC, bool arg = true)
		{
			Console.ForegroundColor = arg ? mainC : ConsoleColor.Red;

			Console.WriteLine($"Engine: Sickness System: {msg}");

			Console.ResetColor();
		}
	}
}
