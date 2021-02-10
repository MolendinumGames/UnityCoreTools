using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using EasyAudioSystem.Core;

namespace EasyAudioSystem.UI
{
    /* The EasyAudioEditor handles how the inspector for the EasyAudioObj Scriptable Object is drawn.
     * It further contains Editor only functionality like finding references to a specific EasyAudioObj.
     * As this script only runs in the UnityEditor Enviroment it will not be included in any build of your project.
     * For more information on the EasyAudio workflow check out the documentation:
     * 
     * For more information on Editor scripting in Unity refer to the Unity Documentation:
     * https://docs.unity3d.com/ScriptReference/Editor.html
     */
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EasyAudioObj))]
    public class EasyAudioObjEditor : Editor
    {
        // For Preview
        private AudioSource previewSource;
        private protected EasyAudio easyAudioPlayer = new EasyAudio();
        private int previewIndex = 0;

        // Drawing
        private readonly Color darkGrey = new Color(.17f, .17f, .17f, 1f);
        private const float seperatorHeight = 2f;
        private readonly Rect curveConstraint = new Rect(0, 0, 1, 1);

        // Ref searching
        private string searchPath = "";
        private bool searchResources = true;
        private bool moveSelection = false;

        // Target
        private protected EasyAudioObj eA;
        private protected SerializedObject sO;

        // S.-Properties
        private SerializedProperty audioClips;
        private SerializedProperty looping;
        private SerializedProperty volumeRange;
        private SerializedProperty pitchRange;
        private SerializedProperty selectState;
        private SerializedProperty playState;
        private SerializedProperty specificId;
        private SerializedProperty delay;
        private SerializedProperty fadeCurve;
        private SerializedProperty enableSource;
        private SerializedProperty mixerGroup;

        // Labels
        GUIContent loopLabel         = new GUIContent( "Looping",
                                                       "Loop the current clip");
        GUIContent selectStateLabel  = new GUIContent( "Select clip by ",
                                                       "How a clip is being selected when Play is called");
        GUIContent playStateLabel    = new GUIContent( "Play clip ",
                                                       "Select how the clip should be played.");
        GUIContent specificIdLabel   = new GUIContent( "ID: ",
                                                       "Always play the clip with this specific ID (start at 0).");
        GUIContent delayLabel        = new GUIContent( "Delay In Seconds",
                                                       "Settings are applied, then after X seconds the clip(s) is played.");
        GUIContent searchResLabel    = new GUIContent( "Include Resources Folder",
                                                       "Search through resources and scene hierarchy.");
        GUIContent searchPathLabel   = new GUIContent( "Search path",
                                                       "Empty means entire resources folder. Restrict to a clearer path for better performance.");
        GUIContent fadeLabel         = new GUIContent( "Fading",
                                                       "Change volume over time based on a curve.");
        GUIContent enableSourceLabel = new GUIContent( "Auto enable AudioSource",
                                                       "Enable the AudioSource component (not GameObject) passed into Play() in case it is disabled.");
        GUIContent moveSelectLabel   = new GUIContent( "Select Result in Hierarchy",
                                                       "Select the found GameObjects from the hierarchy.");
        GUIContent mixerLabel        = new GUIContent( "AudioMixerGroup", 
                                                       "Assign this SFX to an AudioMixerGroup on Play.");

        // Info Boxes
        //private const string oneshotInfo = "Plays the clip on top of what is currently played by the Audiosource. Note that this will override the settings any currently played clip.";
        private const string fadeingInfo = "X is the length of the clip on a 0 to 1 Scale. Y the respective volume multiplier.";

        //private const string nextInfo = "Always starts with clip at Index 0.";
        //private const string allInRowInfo = "Always starts with clip at Index 0.";


        #region UnityCallBacks
        private void OnEnable()
        {
            eA = target as EasyAudioObj;
            GetProperties();
            CreatePreviewSource();
        }
        private void OnDisable()
        {
            DestroyImmediate(previewSource);
        }
        #endregion
        public override void OnInspectorGUI()
        {
            sO.Update();

            DrawHeaderSection();

            DrawGeneralSection();

            DrawLine(seperatorHeight, darkGrey, 8f, 0);

            DrawPlayStateSection();

            DrawLine(seperatorHeight, darkGrey, 8f, 0);

            DrawSelectionSection();

            DrawLine(seperatorHeight, darkGrey, 8f, 0);

            DrawClipList();

            DrawLine(seperatorHeight, darkGrey, 8f, 0);

            DrawPreviewSection();

            DrawLine(seperatorHeight, darkGrey, 8f, 0);

            DrawReferenceSection();

            serializedObject.ApplyModifiedProperties();
        }

        #region Header
        private void DrawHeaderSection()
        {
            EditorGUILayout.LabelField("Easy Audio Object", SetUpHeaderStyle());
        }
        private GUIStyle SetUpHeaderStyle()
        {
            GUIStyle headerStyle = new GUIStyle("WhiteLargeLabel");
            headerStyle.alignment = TextAnchor.MiddleCenter;
            return headerStyle;
        }

        #endregion

        #region GeneralSection
        private void DrawGeneralSection()
        {
            EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(mixerGroup, mixerLabel);

            EditorGUILayout.PropertyField(looping, loopLabel);

            EditorGUILayout.PropertyField(enableSource, enableSourceLabel);

            EditorGUILayout.Space();
            //EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * 0.5f);

            EditorGUILayout.PropertyField(volumeRange);

            EditorGUILayout.PropertyField(pitchRange);
        }
        #endregion

        #region AudioClips
        private void DrawClipList()
        {
            bool isEditingMult = sO.isEditingMultipleObjects;
            bool listIsEmpty = audioClips.arraySize < 1;

            EditorGUI.BeginDisabledGroup(isEditingMult || listIsEmpty);
            EditorGUILayout.PropertyField(audioClips, false);
#if UNITY_2019 || UNITY_2018 || UNITY_2017
            if (audioclips.isexpanded)
            {
                for (int i = 0; i < audioclips.arraysize; i++)
                {
                    editorguilayout.beginhorizontal();
                    editorguilayout.propertyfield(audioclips.getarrayelementatindex(i));
                    if (guilayout.button("x", guilayout.expandwidth(false)))
                    {
                        audioclips.deletearrayelementatindex(i);
                        gui.changed = true;
                    }
                    editorguilayout.endhorizontal();

                    rect rect = editorguilayout.getcontrolrect(false, 2);
                    rect.height = 1;
                    rect.width *= 0.9f;
                    rect.position = new vector2(35, rect.position.y);
                    editorgui.drawrect(rect, new color(.3f, .3f, .3f));
                }
                drawaddbutton();
            }
#endif
            EditorGUI.EndDisabledGroup();
        }
        private void DrawAddButton()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("");
            if (GUILayout.Button("Add Clip"))
            {
                EasyAudioObj obj = target as EasyAudioObj;
                obj.CreateNewClipShell();
                GUI.changed = true;
            }
            EditorGUILayout.EndHorizontal();
        }
#endregion

#region PlayState Section

        private void DrawPlayStateSection()
        {
            EditorGUILayout.LabelField("PlayState", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(playState, playStateLabel);

            switch (playState.enumValueIndex)
            {
                case 0: // Normal
                    break;
                case 1: // Oneshot
                    //EditorGUILayout.HelpBox(oneshotInfo, MessageType.Info);
                    break;
                case 2: // Delayed
                    EditorGUILayout.PropertyField(delay, delayLabel);
                    delay.floatValue = Mathf.Abs(delay.floatValue);
                    break;
                case 3: // Faded
                    fadeCurve.animationCurveValue = EditorGUILayout.CurveField(fadeLabel, fadeCurve.animationCurveValue, Color.green, curveConstraint);
                    EditorGUILayout.HelpBox(fadeingInfo, MessageType.Info);
                    break;
            }
        }

#endregion

#region SelectionSection
        private void DrawSelectionSection()
        {
            EditorGUILayout.LabelField("Clip Selection", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(selectState, selectStateLabel);

            switch (selectState.enumValueIndex)
            {
                default: // Fallback
                    break;
                case 0: // Random
                    break;
                case 1: // Next
                    break;
                case 2: // Specific by ID
                    EditorGUILayout.PropertyField(specificId, specificIdLabel);
                    specificId.intValue = Mathf.Clamp(specificId.intValue, 0, audioClips.arraySize - 1);
                    break;
                case 3: // All in row 
                    DrawCombinedClipLength();
                    break;
                case 4: // All in row shuffeled
                    DrawCombinedClipLength();
                    break;
                case 5: // All Overlapped
                    break;
            }
        }
#endregion

#region Preview
        private void DrawPreviewSection()
        {
            EditorGUILayout.LabelField("Preview Options", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(sO.isEditingMultipleObjects || !eA.AClipExists());

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Preview"))
            {
                if (previewSource == null)
                    CreatePreviewSource();
                easyAudioPlayer.easyAudio = target as EasyAudioObj;
                easyAudioPlayer.Play(previewSource);
            }
            if (GUILayout.Button("Stop"))
            {
                if (previewSource == null)
                    return;
                easyAudioPlayer.Stop(previewSource);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Preview clip at index: "))
            {
                if (previewSource == null)
                    CreatePreviewSource();
                easyAudioPlayer.easyAudio = target as EasyAudioObj;
                easyAudioPlayer.Play(previewSource, previewIndex);
            }
            previewIndex = EditorGUILayout.IntField(previewIndex, GUILayout.MaxWidth(EditorGUIUtility.singleLineHeight * 2));
            if (audioClips.arraySize > 0)
                previewIndex = Mathf.Clamp(previewIndex, 0, audioClips.arraySize - 1);

            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
        }
#endregion

#region References
        void DrawReferenceSection()
        {
            EditorGUILayout.LabelField("Utility", EditorStyles.boldLabel);
            eA.clearConsoleOnLog = EditorGUILayout.Toggle( "Clear console on log", eA.clearConsoleOnLog);
            searchResources = EditorGUILayout.Toggle(searchResLabel, searchResources);
            moveSelection = EditorGUILayout.Toggle(moveSelectLabel, moveSelection);

            EditorGUI.BeginDisabledGroup(!searchResources);
            searchPath = EditorGUILayout.TextField(searchPathLabel, searchPath);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Find References*"))
            {
                FindReferences();
            }

            EditorGUILayout.HelpBox("Finds all Gameobjects/Prefabs with components that reference this object and prints it to the console.", MessageType.Info);
        }
        private void FindReferences()
        {
            if (eA.clearConsoleOnLog)
                ClearConsole();

            List<GameObject> newSelection = new List<GameObject>();
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (obj.scene.name != null
                    && obj.hideFlags == HideFlags.None)
                {
                    if (FindReferenceInGameObject(obj))
                        newSelection.Add(obj);

                }
            }
            if (moveSelection)
                Selection.objects = newSelection.ToArray();
            
            if (searchResources)
            {
                foreach (GameObject go in Resources.LoadAll(searchPath, typeof(GameObject)))
                {
                    if (go.hideFlags == HideFlags.None)
                    {
                        FindReferenceInGameObject(go);
                    }
                }
            }
        }
        private bool FindReferenceInGameObject(GameObject go)
        {
            if (go == null) return false;
            bool found = false;
            foreach (Component component in go.GetComponents<Component>())
            {
                if (component == null) continue;
                FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Instance
                                                                   | BindingFlags.Public 
                                                                   | BindingFlags.NonPublic);

                foreach (FieldInfo fieldInfo in fields)
                {
                    System.Object fieldValue = fieldInfo.GetValue(component);
                    if (fieldValue != null
                        && fieldValue.GetType() == typeof(EasyAudio)
                        && ((EasyAudio)fieldValue).easyAudio != null
                        && ((EasyAudio)fieldValue).easyAudio.Equals(serializedObject.targetObject as EasyAudioObj))
                    {
                        string assetPath = AssetDatabase.GetAssetOrScenePath(go);
                        Debug.Log($"{eA.name} is being referenced by GameObject: '{go.name}'" +
                                  $" in the Component: '{component.GetType().Name}' at path: {assetPath}");
                        found = true;
                    }
                }
            }
            return found;
        }
        private static void ClearConsole()
        {
            Assembly.GetAssembly(typeof(UnityEditor.Editor))
                    .GetType("UnityEditor.LogEntries")
                    .GetMethod("Clear")
                    .Invoke(new object(), null);
        }
#endregion

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

        private void GetProperties()
        {
            sO = serializedObject;
            audioClips   = sO.FindProperty("audioClips");
            volumeRange  = sO.FindProperty("volumeRange");
            pitchRange   = sO.FindProperty("pitchRange");
            looping      = sO.FindProperty("isLooping");
            selectState  = sO.FindProperty("selectState");
            playState    = sO.FindProperty("playState");
            specificId   = sO.FindProperty("specificId");
            delay        = sO.FindProperty("delay");
            fadeCurve    = sO.FindProperty("fadeCurve");
            enableSource = sO.FindProperty("enableSource");
            mixerGroup   = sO.FindProperty("mixerGroup");
        }
        private void DrawCombinedClipLength()
        {
            EditorGUILayout.LabelField($"Combined Clip Length: {eA.GetCombinedClipLenght()} seconds.");
        }
        private void CreatePreviewSource()
        {
            previewSource = EditorUtility.CreateGameObjectWithHideFlags("AudioPreview",
                                                                        HideFlags.HideAndDontSave,
                                                                        typeof(AudioSource))
                                                                            .GetComponent<AudioSource>();
        }
    }
}
