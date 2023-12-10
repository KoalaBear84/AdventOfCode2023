using System.Diagnostics;

namespace Library;

public class TimerHelper
{
	public static double GetMilliseconds(Stopwatch stopwatch)
	{
		return (double)stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond;
	}
}
