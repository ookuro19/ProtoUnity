using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Framework
{
    public class PBGenerator
    {
        // Example code for using protoc in Unity
        // Assumes protoc.exe is in the project's root directory

        static string protocPath = Application.dataPath + "/../Tools/protoc.exe";
        static string protoFolderPath = Application.dataPath + "/../ProtoMessage";
        static string protoFilePath = $"chat.proto";
        static string outputDir = Application.dataPath + "/Scripts/ProtoCS";


        [MenuItem("Proto/Generate CS")]
        public static void Proto2PB()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = protocPath;
            startInfo.Arguments = $"--csharp_out={outputDir} --proto_path={protoFolderPath} {protoFilePath}";
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