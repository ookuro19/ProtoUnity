using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace ProtoBuf.Editor
{
    public class ProtobufAssetsImporter : AssetPostprocessor
    {
        static string[] AllProtoFiles
        {
            get
            {
                string[] protoFiles = Directory.GetFiles(Application.dataPath, "*.proto", SearchOption.AllDirectories);
                return protoFiles;
            }
        }


        /// <summary>
        /// proto文件后缀名
        /// </summary>
        static string Proto_Asset_Extension = ".proto";

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            // 自动编译未开启时不需自动导入
            if (ProtobufSettings.instance.Enable == false)
            {
                return;
            }

            bool anyChanges = false;
            string fileName;
            foreach (string importAsset in importedAssets)
            {
                if (Path.GetExtension(importAsset) == Proto_Asset_Extension)
                    continue;

                fileName = Path.Combine(Application.dataPath.Replace("Assets", ""), importAsset);
                if (CompileProtobufSingleFile(fileName))
                {
                    anyChanges = true;
                }
            }

            if (anyChanges)
            {
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 编译所有proto
        /// </summary>
        internal static void CompileAllProtos()
        {
            Debug.Log("======= Protobuf Compile Begin ======");

            foreach (string file in AllProtoFiles)
            {
                Debug.Log("Protobuf Compiling : " + file);
                CompileProtobufSingleFile(file);
            }
            AssetDatabase.Refresh();
            Debug.Log("======= Protobuf Compile Finish ======");
        }

        /// <summary>
        /// 处理单个文件
        /// </summary>
        /// <param name="protoFilePath"></param>
        private static bool CompileProtobufSingleFile(string protoFilePath)
        {
            if (Path.GetExtension(protoFilePath) != Proto_Asset_Extension)
                return false;

            Debug.Log($"------ Protobuf Compile Begin: {protoFilePath} ------");
            string outputPath = Path.GetDirectoryName(protoFilePath);

            string protoFolderPath = Path.GetDirectoryName(protoFilePath);
            string args = $"\"{protoFilePath}\" --csharp_out \"{outputPath}\" --proto_path \"{protoFolderPath}\" ";

            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = ProtobufSettings.instance.ProtocPath, Arguments = args };

            Process proc = new Process() { StartInfo = startInfo };
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();

            string output = proc.StandardOutput.ReadToEnd();
            string error = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            if (output != "")
            {
                Debug.Log("Protobuf Output : " + output);
            }

            if (error != "")
            {
                Debug.LogError("Protobuf Error: " + error);
            }
            Debug.Log($"------ Protobuf Compile Finish: {protoFilePath} ------");
            return true;
        }
    }
}