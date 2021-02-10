using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OldUnused
{
	public class SourceDataApplierWindow : EditorWindow
	{
		private enum SearchType { Selection, EntireScene, InSceneByTag };
		private SearchType searchType = SearchType.Selection;
		private GUIContent enumLabel = new GUIContent("Change all Audiosources in:");
		GUIContent dataLabel = new GUIContent("Data:");
		private AudioSourceData data;
		private string tag;
		private bool includeChildren = false;
		private bool logOnApplay = true;

		private readonly Color darkGrey = new Color(.17f, .17f, .17f, 1f);

		[MenuItem("MyTools/AudioSource Data Applier")]
		public static void OpenWindow()
        {
			var window = GetWindow<SourceDataApplierWindow>();
			window.titleContent = new GUIContent("AudioSource Data Applier");
			window.Show();
        }

        private void OnGUI()
        {
			DrawEnumSection();

			includeChildren = EditorGUILayout.Toggle("Include Children",includeChildren);
			logOnApplay = EditorGUILayout.Toggle("Log on apply", logOnApplay);

			EditorGUILayout.ObjectField(dataLabel, data, typeof(AudioSourceData), false);

			DrawLine(2, darkGrey, 6, 0);

			DrawApplyButton();

		}
		private void DrawEnumSection()
        {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(enumLabel);
			searchType = (SearchType)EditorGUILayout.EnumPopup(GUIContent.none, searchType);
			EditorGUILayout.EndHorizontal();
		}
		private void DrawApplyButton()
        {
			EditorGUI.BeginDisabledGroup(data == null);
			if (GUILayout.Button("Apply"))
			{
				ApplyData();
			}
			EditorGUI.EndDisabledGroup();
		}
		private void DrawLine(float height, Color color, float padding, int indent)
		{
			int oldIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = indent;

			EditorGUILayout.Space();
			Rect rect = EditorGUILayout.GetControlRect(false, 2 * padding);
			rect.height = height;
			rect.width *= 2;
			rect.position = new Vector2(0, rect.position.y);
			EditorGUI.DrawRect(rect, color);

			EditorGUI.indentLevel = oldIndent;
		}
		private void ApplyData()
        {
			List<AudioSource> targets = new List<AudioSource>();
			switch (searchType)
            {
				case SearchType.Selection:
					targets.AddRange(GetFromSelection());
					break;
				case SearchType.EntireScene:
					targets.AddRange(GetFromScene());
					break;
				case SearchType.InSceneByTag:
					targets.AddRange(GetFromTag(tag));
					break;
			}
			if (targets.Count > 0)
            {
                for (int i = 0; i < targets.Count; i++)
                {
					data.ApplyTo(targets[i]);
					if (logOnApplay)
						Debug.Log($"AudioSource Data applied to GameObject {targets[i].gameObject.name}");
                }
            }
        }
		private List<AudioSource> GetFromSelection()
        {
			List<AudioSource> result = new List<AudioSource>();

            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
				AudioSource s = Selection.gameObjects[i].GetComponentInChildren<AudioSource>();
				if (s != null)
					result.Add(s);
            }

			if (result.Count < 1)
				Debug.LogWarning("No Audiosources found in selected GameObjects and children.");
			else
				Debug.Log($"{result.Count} Audiosources found in GameObjects tagged {tag}.");

			return result;
        }
		private List<AudioSource> GetFromScene()
        {
			List<AudioSource> result = new List<AudioSource>();
			AudioSource[] foundSources = FindObjectsOfType<AudioSource>(true);

			for (int i = 0; i < foundSources.Length; i++)
			{
				if (foundSources[i].gameObject.scene.IsValid())
					result.Add(foundSources[i]);
            }

			if (result.Count < 1)
				Debug.LogWarning("No Audiosources found in the current scene.");
			else
				Debug.Log($"{result.Count} Audiosources found in GameObjects tagged {tag}.");

			return result;
		}
		private List<AudioSource> GetFromTag(string tag)
        {
			List<AudioSource> result = new List<AudioSource>();
			GameObject[] foundGO = GameObject.FindGameObjectsWithTag(tag);

            for (int i = 0; i < foundGO.Length; i++)
            {
				if (foundGO[i].scene.IsValid())
                {
					AudioSource target;
					if (includeChildren)
						target = foundGO[i].GetComponentInChildren<AudioSource>();
					else
						target = foundGO[i].GetComponent<AudioSource>();
					if (target != null)
						result.Add(target);
                }
            }

			if (result.Count < 1)
				Debug.LogWarning($"No Audiosources found in GameObjects tagged {tag}.");
			else
				Debug.Log($"{result.Count} Audiosources found in GameObjects tagged {tag}.");

			return result;
        }

	}	
}
