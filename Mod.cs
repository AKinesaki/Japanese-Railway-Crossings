using UnityEngine;
using ColossalFramework;
using ICities;
using System;

namespace JapaneseRailwayCrossings
{
	public class Mod : IUserMod
    {
        public string Name => "Japanese Railway Crossings";
        public string Description => "Replaces railway crossings with more Japanese-looking versions";

		public void OnSettingsUI(UIHelperBase helper)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			// 多言語対応
			Globalization globalText = new Globalization();
			SavedString currentLang = new SavedString("localeID", "gameSettings");

			string displayLang;
			if(currentLang.value.Contains("ja")){ displayLang = "ja"; }
			else{ displayLang = currentLang.value; }

			string style = globalText.GetString(displayLang, Globalization.StringKeys.OptionStyleText);
			string enable = globalText.GetString(displayLang, Globalization.StringKeys.OptionEnableText);

			string[] styles = new string[] {
				globalText.GetString(displayLang, Globalization.StringKeys.s_default),
				globalText.GetString(displayLang, Globalization.StringKeys.s_null),
				globalText.GetString(displayLang, Globalization.StringKeys.s1_close),
				globalText.GetString(displayLang, Globalization.StringKeys.s1_open)
			};

			string[] styles2 = new string[] {
				globalText.GetString(displayLang, Globalization.StringKeys.s_default),
				globalText.GetString(displayLang, Globalization.StringKeys.s_null)
			};

			// オプションページ
			UIHelperBase GroupHeader = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.HeaderText));

			UIHelperBase GroupGlobal = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.GlobalText));
			GroupGlobal.AddDropdown(style, styles2, config.Global, Global);

			UIHelperBase GroupTinyRoads = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.TinyRoadsText));					
			GroupTinyRoads.AddDropdown(style, styles, config.TinyRoads, TinyRoads);

			UIHelperBase GroupSmallRoads = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.SmallRoadsText));
			GroupSmallRoads.AddDropdown(style, styles, config.SmallRoads, SmallRoads);

			UIHelperBase GroupSmallHeavyRoads = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.SmallHeavyRoadsText));
			GroupSmallHeavyRoads.AddDropdown(style, styles, config.SmallHeavyRoads, SmallHeavyRoads);

			UIHelperBase GroupMediumRoads = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.MediumRoadsText));
			GroupMediumRoads.AddDropdown(style, styles, config.MediumRoads, MediumRoads);

			UIHelperBase GroupLargeRoads = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.LargeRoadsText));
			GroupLargeRoads.AddDropdown(style, styles, config.LargeRoads, LargeRoads);

			UIHelperBase GroupWideRoads = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.WideRoadsText));
			GroupWideRoads.AddDropdown(style, styles2, config.WideRoads, WideRoads);

			UIHelperBase GroupHighways = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.HighwaysText));
			GroupHighways.AddDropdown(style, styles2, config.Highways, Highways);

			UIHelperBase GroupPedestrianRoads = helper.AddGroup(globalText.GetString(displayLang, Globalization.StringKeys.PedestrianRoadsText));
			GroupPedestrianRoads.AddDropdown(style, styles, config.PedestrianRoads, PedestrianRoads);
		}

		private void Global(int i)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			config.Global = i;
			Configuration<JapaneseRailwayCrossingsConfiguration>.Save();
		}

		private void TinyRoads(int i)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			config.TinyRoads = i;
			Configuration<JapaneseRailwayCrossingsConfiguration>.Save();
		}

		private void PedestrianRoads(int i)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			config.PedestrianRoads = i;
			Configuration<JapaneseRailwayCrossingsConfiguration>.Save();
		}

		private void SmallRoads(int i)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			config.SmallRoads = i;
			Configuration<JapaneseRailwayCrossingsConfiguration>.Save();
		}

		private void SmallHeavyRoads(int i)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			config.SmallHeavyRoads = i;
			Configuration<JapaneseRailwayCrossingsConfiguration>.Save();
		}

		private void MediumRoads(int i)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			config.MediumRoads = i;
			Configuration<JapaneseRailwayCrossingsConfiguration>.Save();
		}

		private void LargeRoads(int i)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			config.LargeRoads = i;
			Configuration<JapaneseRailwayCrossingsConfiguration>.Save();
		}

		private void WideRoads(int i)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			config.WideRoads = i;
			Configuration<JapaneseRailwayCrossingsConfiguration>.Save();
		}

		private void Highways(int i)
		{
			var config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();

			config.Highways = i;
			Configuration<JapaneseRailwayCrossingsConfiguration>.Save();
		}
	}

	[ConfigurationPath("JapaneseRailwayCrossings.xml")]
	public class JapaneseRailwayCrossingsConfiguration
	{
		public int Global { get; set; } = 0;
		public int TinyRoads { get; set; } = 2;
		public int PedestrianRoads { get; set; } = 2;
		public int SmallRoads { get; set; } = 2;
		public int SmallHeavyRoads { get; set; } = 2;
		public int MediumRoads { get; set; } = 2;
		public int LargeRoads { get; set; } = 2;
		public int WideRoads { get; set; } = 0;
		public int Highways { get; set; } = 0;
	}
}
