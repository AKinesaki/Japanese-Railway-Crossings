// source code from
// https://github.com/gansaku/CSLMapView
// by Gansaku

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseRailwayCrossings {
    /// <summary>
    /// 多言語化対応
    /// </summary>
    internal class Globalization {
        internal const string DEFAULT_LANGUAGE = "en";
        private readonly string[] supportedLanguages = new[] { "en", "ja" };
        private Dictionary<string, Dictionary<StringKeys, string>> strings;
        /// <summary>
        /// 文字列のキー
        /// </summary>
        internal enum StringKeys {
			/// <summary>
			/// ヘッダー
			/// </summary>
			HeaderText,
			/// <summary>
			/// スタイル
			/// </summary>
			s_default,
			s_null,
			s1_close,
			s1_open,
			/// <summary>
			/// グローバル
			/// </summary>
			GlobalText,
			/// <summary>
			/// 道路種別
			/// </summary>
			TinyRoadsText,
			SmallRoadsText,
			SmallHeavyRoadsText,
			MediumRoadsText,
			LargeRoadsText,
			WideRoadsText,
			HighwaysText,
			PedestrianRoadsText,
			BusText,
			MonorailText,
			GrassText,
			TreesText,
			/// <summary>
			/// オプションテキスト
			/// </summary>
			OptionStyleText,
			OptionEnableText
		}
        /// <summary>
        /// 現在の言語
        /// </summary>
        internal string Language { get; set; } = DEFAULT_LANGUAGE;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal Globalization() {
            strings = new Dictionary<string, Dictionary<StringKeys, string>>();

            strings.Add( "en", new Dictionary<StringKeys, string>() );
            initEn( strings["en"] );
            strings.Add( "ja", new Dictionary<StringKeys, string>() );
            initJa( strings["ja"] );
        }
        private void initJa( Dictionary<StringKeys, string> dic ) {
			dic.Add(StringKeys.HeaderText, "日本風踏切MODオプション - 変更はセーブデータの再読込後に適応されます。");
			dic.Add(StringKeys.OptionStyleText, "スタイル");
			dic.Add(StringKeys.s_default, "デフォルト");
			dic.Add(StringKeys.s_null, "非表示");
			dic.Add(StringKeys.s1_close, "下げ");
			dic.Add(StringKeys.s1_open, "上げ");
			dic.Add(StringKeys.GlobalText, "グローバル(新しい道路に適応されます)");
			dic.Add(StringKeys.TinyRoadsText, "Tiny Roads");
			dic.Add(StringKeys.SmallRoadsText, "小さい道路");
			dic.Add(StringKeys.SmallHeavyRoadsText, "Small Heavy Roads");
			dic.Add(StringKeys.MediumRoadsText, "中くらいの道路");
			dic.Add(StringKeys.LargeRoadsText, "広い道路");
			dic.Add(StringKeys.WideRoadsText, "Wide Roads");
			dic.Add(StringKeys.HighwaysText, "高速道路");
			dic.Add(StringKeys.PedestrianRoadsText, "Pedestrian Roads");
			dic.Add(StringKeys.OptionEnableText, "有効にする");
			dic.Add(StringKeys.BusText, "[Beta]バスレーンのある道路(いくつかの道路を除く)");
			dic.Add(StringKeys.MonorailText, "[Beta]モノレール軌道のある道路");
			dic.Add(StringKeys.GrassText, "[Beta]芝生のある道路(分離帯付きの中道路を除く)");
			dic.Add(StringKeys.TreesText, "[Beta]街路樹のある道路(分離帯付きの中道路を除く)");
		}
        private void initEn( Dictionary<StringKeys, string> dic ) {
            dic.Add( StringKeys.HeaderText, "Japanese Railway Crossings Options - Changes will apply after reload savedata.");
			dic.Add( StringKeys.OptionStyleText, "Style");
			dic.Add( StringKeys.s_default, "Default");
			dic.Add( StringKeys.s_null, "Invisible");
			dic.Add( StringKeys.s1_close, "Close State");
			dic.Add( StringKeys.s1_open, "Open State");
			dic.Add( StringKeys.GlobalText, "Global(It applies to new roads)");
			dic.Add( StringKeys.TinyRoadsText, "Tiny Roads");
			dic.Add( StringKeys.SmallRoadsText, "Small Roads");
			dic.Add( StringKeys.SmallHeavyRoadsText, "Small Heavy Roads");
			dic.Add( StringKeys.MediumRoadsText, "Medium Roads");
			dic.Add( StringKeys.LargeRoadsText, "Large Roads");
			dic.Add( StringKeys.WideRoadsText, "Wide Roads");
			dic.Add( StringKeys.HighwaysText, "Highways");
			dic.Add( StringKeys.PedestrianRoadsText, "Pedestrian Roads");
			dic.Add( StringKeys.OptionEnableText, "Enable");
			dic.Add( StringKeys.BusText, "[Beta]Road with Bus Lanes(except some roads)");
			dic.Add( StringKeys.MonorailText, "[Beta]Road with Monorail Track");
			dic.Add( StringKeys.GrassText, "[Beta]Road with Grass(except for Medium Roads with median)");
			dic.Add( StringKeys.TreesText, "[Beta]Road with Trees(except for Medium Roads with median)");
		}
        /// <summary>
        /// デフォルト言語での文字列を取得します。
        /// </summary>
        /// <param name="key">文字列キー</param>
        /// <returns></returns>
        internal string GetString( StringKeys key ) {
            return GetString( Language, key );
        }

        /// <summary>
        /// 指定した言語での文字列を取得します。
        /// </summary>
        /// <param name="lang">言語(en,ja)</param>
        /// <param name="key">文字列キー</param>
        /// <returns></returns>
        internal string GetString( string lang, StringKeys key ) {
            if( !supportedLanguages.Contains( lang ) ) {
                lang = DEFAULT_LANGUAGE;
            }
            Dictionary<StringKeys, string> dic = strings[lang];
            if( dic.ContainsKey( key ) ) {
                return dic[key];
            } else {
                if( lang == DEFAULT_LANGUAGE ) {
                    return null;
                }
                return GetString( DEFAULT_LANGUAGE, key );
            }
        }
    }
}
