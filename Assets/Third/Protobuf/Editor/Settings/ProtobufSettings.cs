using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

namespace ProtoBuf.Editor
{
    [FilePath("ProjectSettings/ProtobufSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class ProtobufSettings : ScriptableSingleton<ProtobufSettings>
    {
        [Header("开启默认导入")]
        public bool Enable = true;

        [Header("protoc路径")]
        public string ProtocPath = "";

        public void SaveSettings()
        {
            Save(true);
        }
    }
}

