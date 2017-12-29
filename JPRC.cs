using UnityEngine;
using ColossalFramework;
using ICities;
using System;
using System.Linq;
using System.Collections.Generic;

namespace JapaneseRailwayCrossings
{
	public class JPRC
	{
		// 更新用メモ
		// 1. メンバ変数を追加
		// 2. コンストラクタにアセットを追加
		// 3. (style追加の場合)ReturnStyleFromConfigに処理を追加
		// 4. ReplaceTLに処理を追加

		/// <summary>
		/// メンバ変数
		/// </summary>
		private static readonly JapaneseRailwayCrossingsConfiguration config;
		private static readonly PropInfo JPRC_s1_small_close,
										JPRC_s1_small_open,
										JPRC_s1_tiny_close,
										JPRC_s1_tiny_open,
										JPRC_s1_tiny8_close,
										JPRC_s1_tiny8_open,
										JPRC_s1_large_close,
										JPRC_s1_large_open,
										JPRC_s1_large24_close,
										JPRC_s1_large24_open,
										JPRC_s1_large38_close,
										JPRC_s1_large38_open;
		private enum style { s_default, s_null, s1_close, s1_open }
		private enum roadWidth { tiny, tiny8, small, large, large24, large38, wide, highway}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		static JPRC()
		{
			//1249000709.
			string workshopId = "1249000709.";
			JPRC_s1_small_close = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-small-close" + "_Data");
			JPRC_s1_small_open = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-small-open" + "_Data");
			JPRC_s1_tiny_close = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-tiny-close" + "_Data");
			JPRC_s1_tiny_open = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-tiny-open" + "_Data");
			JPRC_s1_tiny8_close = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-tiny8-close" + "_Data");
			JPRC_s1_tiny8_open = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-tiny8-open" + "_Data");
			JPRC_s1_large_close = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-large-close" + "_Data");
			JPRC_s1_large_open = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-large-open" + "_Data");
			JPRC_s1_large24_close = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-large24-close" + "_Data");
			JPRC_s1_large24_open = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-large24-open" + "_Data");
			JPRC_s1_large38_close = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-large38-close" + "_Data");
			JPRC_s1_large38_open = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPRC-s1-large38-open" + "_Data");

			if (JPRC_s1_small_close == null ||
			JPRC_s1_small_open == null ||
			JPRC_s1_tiny_close == null ||
			JPRC_s1_tiny_open == null ||
			JPRC_s1_tiny8_close == null ||
			JPRC_s1_tiny8_open == null ||
			JPRC_s1_large_close == null ||
			JPRC_s1_large_open == null ||
			JPRC_s1_large24_close == null ||
			JPRC_s1_large24_open == null ||
			JPRC_s1_large38_close == null ||
			JPRC_s1_large38_open == null)
			{
				Log.Display(Log.mode.error, "Prop Not Found", true);
				return;
			}

			config = Configuration<JapaneseRailwayCrossingsConfiguration>.Load();
		}

		/// <summary>
		/// 信号を置き換える。
		/// </summary>
		public static void ReplaceAllTL()
		{
			Log.Display(Log.mode.warning, $"ReplaceAllTL Init");

			NetInfo[] roads = GetRegisteredNetInfos();

			if (roads == null || roads.Length == 0)
			{
				Log.Display(Log.mode.error, "ReplaceAllTL - NetInfo[] is null.");
				return;
			}

			ReplaceTL(roads);
		}

		/// <summary>
		/// 全てのNetInfoを読み込み、配列で返す。
		/// </summary>
		public static NetInfo[] GetRegisteredNetInfos()
		{
			var allNetinfos = new List<NetInfo>();
			for (uint i = 0; i < PrefabCollection<NetInfo>.PrefabCount(); i++)
			{
				NetInfo info = PrefabCollection<NetInfo>.GetPrefab(i);
				if (info == null) continue;

				allNetinfos.Add(info);
			}
			return allNetinfos.ToArray();
		}

		/// <summary>
		/// NetInfoコレクションにある信号を置き換える。
		/// </summary>
		private static void ReplaceTL(IEnumerable<NetInfo> roads)
		{
			Log.Display(Log.mode.warning, "ReplaceTL Init");

			foreach (var road in roads)
			{
				foreach (var lane in road.m_lanes)
				{
					if (lane?.m_laneProps?.m_props == null)
					{
						continue;
					}

					foreach (var laneProp in lane.m_laneProps.m_props)
					{
						var prop = laneProp.m_finalProp;

						if (prop == null)
						{
							continue;
						}

						// 道路を判別してstyleとroadWidthを取得する
						StyleParams styleParam = ReturnStyleFromRoadname(road.name);
						style style = styleParam.style;
						roadWidth width = styleParam.width;

						// ここから置き換え
						switch (prop.name)
						{
							case "Railway Crossing Short":
							case "Railway Crossing Medium":
							case "Railway Crossing Long":
							case "Railway Crossing Very Long":
								
								switch (style)
								{
									case style.s_default:
										break;

									case style.s_null:
										laneProp.m_finalProp = null;
										laneProp.m_prop = null;
										break;

									case style.s1_close:
									case style.s1_open:
										switch(width)
										{
											case roadWidth.small:
												
												if (road.name.Contains("KT01"))
												{
													if (laneProp.m_position.x > 0) { laneProp.m_position.x = -2; }
													else { laneProp.m_position.x = 2; }
												}
												else if (road.name.Contains("Zonable Promenade"))
												{
													if (laneProp.m_position.x > 0) { laneProp.m_position.x = 3; }
													else { laneProp.m_position.x = -3; }
												}
												else if (
													road.name.Contains("Small Avenue") ||
													road.name.Contains("AsymRoadL1R3") ||
													road.name.Contains("Oneway4L") ||
													road.name.Contains("OneWay3L")
												)
												{
													if (laneProp.m_position.x > 0) { laneProp.m_position.x = -0.6f; }
													else { laneProp.m_position.x = 0.6f; }
												}
												else
												{
													if (laneProp.m_position.x > 0) { laneProp.m_position.x = -1; }
													else { laneProp.m_position.x = 1; }
												}
												
												if(style == style.s1_close)
												{
													laneProp.m_finalProp = JPRC_s1_small_close;
													laneProp.m_prop = JPRC_s1_small_close;
												}
												else
												{
													laneProp.m_finalProp = JPRC_s1_small_open;
													laneProp.m_prop = JPRC_s1_small_open;
												}
												break;

											case roadWidth.tiny:
												if (laneProp.m_position.x > 0) { laneProp.m_position.x = 0.6f; }
												else { laneProp.m_position.x = -0.6f; }
												
												if (style == style.s1_close)
												{
													laneProp.m_finalProp = JPRC_s1_tiny_close;
													laneProp.m_prop = JPRC_s1_tiny_close;
												}
												else
												{
													laneProp.m_finalProp = JPRC_s1_tiny_open;
													laneProp.m_prop = JPRC_s1_tiny_open;
												}
												break;

											case roadWidth.tiny8:
												if (road.name.Contains("Zonable Pedestrian"))
												{
													if (laneProp.m_position.x > 0) { laneProp.m_position.x = -4; }
													else { laneProp.m_position.x = 4; }
												}
												else
												{
													if (laneProp.m_position.x > 0) { laneProp.m_position.x = 1; }
													else { laneProp.m_position.x = -1; }
												}

												if (style == style.s1_close)
												{
													laneProp.m_finalProp = JPRC_s1_tiny8_close;
													laneProp.m_prop = JPRC_s1_tiny8_close;
												}
												else
												{
													laneProp.m_finalProp = JPRC_s1_tiny8_open;
													laneProp.m_prop = JPRC_s1_tiny8_open;
												}
												break;

											case roadWidth.large:

												if(road.name.Contains("Eight-Lane Avenue"))
												{
													if (laneProp.m_position.x > 0) { laneProp.m_position.x = -1; }
													else { laneProp.m_position.x = 1; }
												}
												else
												{
													if (laneProp.m_position.x > 0) { laneProp.m_position.x = -2; }
													else { laneProp.m_position.x = 2; }
												}

												if (style == style.s1_close)
												{
													laneProp.m_finalProp = JPRC_s1_large_close;
													laneProp.m_prop = JPRC_s1_large_close;
												}
												else
												{
													laneProp.m_finalProp = JPRC_s1_large_open;
													laneProp.m_prop = JPRC_s1_large_open;
												}
												break;

											case roadWidth.large24:
												if (laneProp.m_position.x > 0) { laneProp.m_position.x = -1.7f; }
												else { laneProp.m_position.x = 1.7f; }

												if (style == style.s1_close)
												{
													laneProp.m_finalProp = JPRC_s1_large24_close;
													laneProp.m_prop = JPRC_s1_large24_close;
												}
												else
												{
													laneProp.m_finalProp = JPRC_s1_large24_open;
													laneProp.m_prop = JPRC_s1_large24_open;
												}
												break;

											case roadWidth.large38:
												if (laneProp.m_position.x > 0) { laneProp.m_position.x = -1.4f; }
												else { laneProp.m_position.x = 1.4f; }

												if (style == style.s1_close)
												{
													laneProp.m_finalProp = JPRC_s1_large38_close;
													laneProp.m_prop = JPRC_s1_large38_close;
												}
												else
												{
													laneProp.m_finalProp = JPRC_s1_large38_open;
													laneProp.m_prop = JPRC_s1_large38_open;
												}
												break;
										}
										
										break;
								}
								break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コンフィグの値に対応したstyleを返す。
		/// </summary>
		private static style ReturnStyleFromConfig(int i)
		{
			switch (i)
			{
				case 0:
				default:
					return style.s_default;
				case 1:
					return style.s_null;
				case 2:
					return style.s1_close;
				case 3:
					return style.s1_open;
			}
		}


		struct StyleParams
		{
			internal style style;
			internal roadWidth width;
		}

		/// <summary>
		/// 道路名に対応したsyleとroadWidthを返す。
		/// </summary>
		private static StyleParams ReturnStyleFromRoadname(string name)
		{
			style style = ReturnStyleFromConfig(config.Global);

			// Konno Road
			if (name.Contains("KT01") && name.Contains("Medium"))
			{
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.MediumRoads),
					width = roadWidth.large24
				};
			}
			if (name.Contains("KT02") && name.Contains("Medium"))
			{
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.MediumRoads),
					width = roadWidth.large24
				};
			}
			if (name.Contains("KT01") && name.Contains("Large"))
			{
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.LargeRoads),
					width = roadWidth.large38
				};
			}

			// Tiny Roads
			if (
				name.Contains("Gravel Road") ||
				name.Contains("PlainStreet2L")
			) {
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.TinyRoads),
					width = roadWidth.tiny
				};
			}

			// Tiny Roads 8m
			if (
				name.Contains("Two-Lane Alley") ||
				name.Contains("Two-Lane Oneway") ||
				name.Contains("One-Lane Oneway With") ||
				name == "One-Lane Oneway"
			)
			{
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.TinyRoads),
					width = roadWidth.tiny8
				};
			}

			// Small Roads
			if (
				name.Contains("Basic Road") ||
				name.Contains("BasicRoadPntMdn") ||
				name.Contains("BasicRoadMdn") ||
				name.Contains("Oneway Road") ||
				name.Contains("Asymmetrical Three Lane Road") ||
				name.Contains("One-Lane Oneway with") ||
				name.Contains("Small Busway") ||
				name.Contains("Harbor Road") ||
				name.Contains("Tram Depot Road") ||
				name.Contains("Small Road")
			) {
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.SmallRoads),
					width = roadWidth.small
				};
			}

			// Small Heavy Roads
			if (
				name.Contains("BasicRoadTL") ||
				name.Contains("AsymRoadL1R2") ||
				name.Contains("Oneway3L") ||
				name.Contains("Small Avenue") ||
				name.Contains("AsymRoadL1R3") ||
				name.Contains("Oneway4L") ||
				name.Contains("OneWay3L")
			) {
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.SmallHeavyRoads),
					width = roadWidth.small
				};
			}

			// Medium Roads
			if (
				name.Contains("Medium Road") ||
				name.Contains("Avenue Large With") ||
				name.Contains("Medium Avenue") ||
				//name.Contains("FourDevidedLaneAvenue") ||
				name.Contains("AsymAvenueL2R4") ||
				name.Contains("AsymAvenueL2R3")
			) {
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.MediumRoads),
					width = roadWidth.large
				};
			}

			// Large Roads
			if (
				name.Contains("Large Road") ||
				name.Contains("Large Oneway") ||
				name.Contains("Eight-Lane Avenue") ||
				name.Contains("Six-Lane Avenue Median")
			) {
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.LargeRoads),
					width = roadWidth.large
				};
			}

			// Wide Roads
			if (
				name.Contains("WideAvenue")
			) {
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.WideRoads),
					width = roadWidth.wide
				};
			}

			// Highway
			if (
				name.Contains("Highway")
			) {
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.Highways),
					width = roadWidth.highway
				};
			}

			// Pedestrian Roads
			if (name.Contains("Zonable Pedestrian"))
			{
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.PedestrianRoads),
					width = roadWidth.tiny8
				};
			}

			// Promenade
			if (name.Contains("Zonable Promenade"))
			{
				return new StyleParams()
				{
					style = ReturnStyleFromConfig(config.PedestrianRoads),
					width = roadWidth.small
				};
			}

			return new StyleParams()
			{
				style = style,
				width = roadWidth.small
			};
		}
	}
}
