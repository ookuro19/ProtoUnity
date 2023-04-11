using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

namespace Framework
{
    public class PBGenerator
    {
        // Example code for using protoc in Unity
        // Assumes protoc.exe is in the project's root directory
        // todo: serialize in project setting
        static string protocPath = Application.dataPath + "/../Tools/protoc-22.2-win64/bin/protoc.exe";
        static string protoFolderPath = Application.dataPath + "/Scripts/GenerateCode";
        static string outputDir = Application.dataPath + "/Scripts/GenerateCode";


        [MenuItem("Proto/Generate CS")]
        public static void GenerateCSFromProto()
        {
            if (!Directory.Exists(protoFolderPath))
            {
                Debug.Log($"指定目录不存在: {protoFolderPath}");
                return;
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(protoFolderPath);
            var fileInfos = directoryInfo.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (fileInfo.Extension == ".proto")
                {
                    Proto2PB(fileInfo);
                }
            }
        }

        public static void Proto2PB(FileInfo fileInfo)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = protocPath;
            startInfo.Arguments = $"--csharp_out={outputDir} --proto_path={protoFolderPath} {fileInfo.FullName}";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            Debug.Log(startInfo.FileName);
            Debug.Log(startInfo.Arguments);
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new System.Exception($"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}");
            }

            Debug.Log(output);
        }

    }

}