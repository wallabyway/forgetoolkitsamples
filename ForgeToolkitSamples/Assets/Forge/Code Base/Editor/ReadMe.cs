﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Autodesk.ForgeToolkit
{
	[InitializeOnLoad]
	public class ReadMe : EditorWindow
	{
		public static readonly string kShowOnStart = "Autodesk.ForgeToolkit.ShowOnStart";
		public static readonly string kShownThisSession = "Autodesk.ForgeToolkit.ShownThisSession";
		public static readonly int kShowOnStartCookie = 1;

		GUIStyle _style;
		Vector2 _scroll;
		string _text;


		static ReadMe()
		{
			EditorApplication.delayCall += () =>
			{
				int cookie = EditorPrefs.GetInt(kShowOnStart, defaultValue: 0);
				if (cookie < kShowOnStartCookie && !SessionState.GetBool(kShownThisSession, defaultValue: false))
				{
					Show();
				}
			};
		}

		[MenuItem("Forge/About Forge Toolkit", false, -100)]
		public static new void Show()
		{
			((ReadMe)ScriptableObject.CreateInstance(typeof(ReadMe))).ShowUtility();
			SessionState.SetBool(kShownThisSession, true);
		}

		[PreferenceItem("Read Me")]
		static void OnPrefsGUI()
		{
			int cookie = EditorPrefs.GetInt(kShowOnStart, defaultValue: 0);
			bool showOnStart = cookie < kShowOnStartCookie;

			if (EditorGUILayout.Toggle("Show Forge Readme On Start", showOnStart) != showOnStart)
				EditorPrefs.SetInt(kShowOnStart, showOnStart ? kShowOnStartCookie : 0);
		}

		protected void OnEnable()
		{
			titleContent = new GUIContent("About");
			minSize = new Vector2(800, 600);
			maxSize = new Vector2(1280, 960);

			try
			{
				_text = ParseMarkdown("Assets/Forge/README.md");
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				_text = e.Message;
			}
		}

		protected void OnGUI()
		{
			if (_style == null)
			{
				_style = new GUIStyle(EditorStyles.textArea);
				_style.richText = true;
			}

			_scroll = EditorGUILayout.BeginScrollView(_scroll);
			GUILayout.TextArea(_text, _style, GUILayout.ExpandHeight(true));
			EditorGUILayout.EndScrollView();

			int cookie = EditorPrefs.GetInt(kShowOnStart, defaultValue: 0);
			bool showOnStart = cookie < kShowOnStartCookie;

			if (EditorGUILayout.ToggleLeft("Show On Start", showOnStart) != showOnStart)
				EditorPrefs.SetInt(kShowOnStart, showOnStart ? kShowOnStartCookie : 0);

			if (GUILayout.Button("OK", GUILayout.MinHeight(50)))
			{
				Close();
			}
		}

		static string ParseMarkdown(string path)
		{
			// strict markdown subset:
			var h1 = new Regex(@"^\=+\s*$");
			var h2 = new Regex(@"^\-+\s*$");
			var hr = new Regex(@"^\*+\s*$");
			var li = new Regex(@"^\-\s+(.*)$");

			var lines = new Queue<string>(File.ReadAllLines(path));
			var builder = new StringBuilder();

			bool list = true;
			Action endList = () =>
			{
				if (list)
					builder.Append('\n');
				list = false;
			};

			while (lines.Count > 0)
			{
				var line = lines.Dequeue();
				Match match;

				if (string.IsNullOrEmpty(line))
				{
					endList();
					continue;
				}
				else if (hr.IsMatch(line))
				{
					endList();
					builder.Append('\n');
					for (int i = 0; i < 20; ++i)
						builder.Append("\u2e3b");
					builder.Append("\n\n");
					continue;
				}
				else if ((match = li.Match(line)).Success)
				{
					builder.AppendFormat(" \u2022 {0}\n", match.Groups[1].Captures[0].Value);
					list = true;
					continue;
				}
				else if (lines.Count > 0)
					if (h1.IsMatch(lines.Peek()))
					{
						lines.Dequeue();
						endList();
						builder.AppendFormat("\n<size=24><b>{0}</b></size>\n\n", line);
						continue;
					}
					else if (h2.IsMatch(lines.Peek()))
					{
						lines.Dequeue();
						endList();
						builder.AppendFormat("\n<size=18><b>{0}</b></size>\n\n", line);
						continue;
					}

				builder.AppendFormat("{0}\n\n", line);
			}

			return builder.ToString();
		}
	}
}