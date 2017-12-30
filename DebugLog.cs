using UnityEngine;

namespace JapaneseRailwayCrossings
{
	public static class Log
	{
		public enum mode
		{
			normal,error,warning
		}

		public static void Display(mode mode, string message, bool alwaysDisplay = false)
		{
			if (alwaysDisplay)
			{
				DisplayLog(mode, message);
			}
			else
			{
				#if DEBUG
				DisplayLog(mode, message);
				#endif
			}
		}

		private static void DisplayLog(mode mode, string message)
		{
			switch (mode)
			{
				case mode.normal:
					Debug.Log(message);
					break;

				case mode.error:
					Debug.LogError(message);
					break;

				case mode.warning:
					Debug.LogWarning(message);
					break;
			}
		}

		/// <summary>
		/// 全てのNetInfoをテキストに出力する。
		/// </summary>
		public static void OutputRegisteredNetInfos()
		{
			System.IO.StreamWriter sw = new System.IO.StreamWriter("AllNetInfos.txt", false, System.Text.Encoding.GetEncoding("utf-8"));

			NetInfo[] ni = JPRC.GetRegisteredNetInfos();

			foreach (var s in ni)
			{
				sw.WriteLine(s.name);
			}

			sw.Close();
		}
	}
}
