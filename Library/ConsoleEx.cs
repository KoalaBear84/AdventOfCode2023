using System.Drawing;
using System.Runtime.InteropServices;

namespace Library;

public static class ConsoleEx
{
	public static bool FullColorEnabled;

	public static void WriteLine(string text, ConsoleColor color)
	{
		Console.ForegroundColor = color;
		Console.WriteLine(text);
		Console.ResetColor();
	}

	public static void Write(string text, ConsoleColor color)
	{
		Console.ForegroundColor = color;
		Console.Write(text);
		Console.ResetColor();
	}

	public static void WriteRgb(string text, Color color)
	{
		if (!FullColorEnabled)
		{
			EnableFullColors();
		}

		Console.Write($"\x1b[38;2;{color.R};{color.G};{color.B}m");
		Console.Write(text);
		Console.ResetColor();
	}

	public static void WriteRgb(char c, Color color)
	{
		if (!FullColorEnabled)
		{
			EnableFullColors();
		}

		Console.Write($"\x1b[38;2;{color.R};{color.G};{color.B}m");
		Console.Write(c);
		Console.ResetColor();
	}

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool GetConsoleMode(IntPtr handle, out int mode);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr GetStdHandle(int handle);

	public static void EnableFullColors()
	{
		nint handle = GetStdHandle(-11);
		GetConsoleMode(handle, out int mode);
		SetConsoleMode(handle, mode | 0x4);
	}
}