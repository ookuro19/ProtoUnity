using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Presets;
using System;
using System.Reflection;

namespace ProtoBuf.Editor
{
    public class ProtobufSettingsProvider : SettingsProvider
    {

        public ProtobufSettingsProvider() : base("Project/Protobuf", UnityEditor.SettingsScope.Project) { }

        [SettingsProvider]
        static SettingsProvider CreateProtobufSettingsProvider()
        {
            return new ProtobufSettingsProvider();
        }

        private ProtobufSettings _protoSettings => ProtobufSettings.instance;

        private GUIStyle buttonStyle;

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {

        }


        public override void OnDeactivate()
        {
            base.OnDeactivate();
            _protoSettings.SaveSettings();
        }


        public override void OnGUI(string searchContext)
        {
            EditorGUI.BeginChangeCheck();

            _protoSettings.Enable = EditorGUILayout.Toggle(new GUIContent("开启protobuf自动编译", ""), _protoSettings.Enable);

            EditorGUI.BeginDisabledGroup(!_protoSettings.Enable);
            EditorGUILayout.HelpBox(@"On Windows put the path to protoc.exe (e.g. C:\My Dir\protoc.exe), on macOS and Linux you can use ""which protoc"" to find its location. (e.g. /usr/local/bin/protoc)", MessageType.Info);

            // protoc路径相关
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Protoc路径", GUILayout.Width(100));
            _protoSettings.ProtocPath = EditorGUILayout.TextField(_protoSettings.ProtocPath, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Select File", GUILayout.Width(100)))
            {
                _protoSettings.ProtocPath = EditorUtility.OpenFilePanel("选择Protoc路径", Application.dataPath, "");
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button(new GUIContent("Force Compilation")))
            {
                ProtobufAssetsImporter.CompileAllProtos();
            }

            EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck())
            {
                _protoSettings.SaveSettings();
            }
        }

        public override void OnTitleBarGUI()
        {
            base.OnTitleBarGUI();
            var rect = GUILayoutUtility.GetLastRect();
            buttonStyle = buttonStyle ?? GUI.skin.GetStyle("IconButton");

            #region 绘制proto对应版本github
            var w = rect.x + rect.width;
            rect.x = w - 57;
            rect.y += 6;
            rect.width = rect.height = 18;
            var content = EditorGUIUtility.IconContent("_Help");
            content.tooltip = "目前使用的proto版本为v22.2";
            if (GUI.Button(rect, content, buttonStyle))
            {
                Application.OpenURL("https://github.com/protocolbuffers/protobuf/releases/tag/v22.2");
            }
            #endregion


            #region 绘制 Reset
            rect.x += 19;
            content = EditorGUIUtility.IconContent("pane options");
            content.tooltip = "Reset";
            if (GUI.Button(rect, content, buttonStyle))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Reset"), false, () =>
                {
                    Undo.RecordObject(_protoSettings, "Capture Value for Reset");
                    var dv = ScriptableObject.CreateInstance<ProtobufSettings>();
                    var json = EditorJsonUtility.ToJson(dv);
                    UnityEngine.Object.DestroyImmediate(dv);
                    EditorJsonUtility.FromJsonOverwrite(json, _protoSettings);
                    _protoSettings.SaveSettings();
                });
                menu.ShowAsContext();
            }
            #endregion
        }


    }
}
