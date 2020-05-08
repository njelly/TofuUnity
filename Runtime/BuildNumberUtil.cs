using System.IO;
using UnityEngine;

namespace Tofunaut.UnityUtils
{
    public static class BuildNumberUtil
    {
        private static string BuilNumberFilePath { get { return Path.Combine(Application.streamingAssetsPath, "BuildNumber.txt"); } }

        /// <summary>
        /// Gets the current build number by reading from BuildNumber.txt in the StreamingAssets folder.
        /// If the file does not exist it will be created with the build number set to 0.
        /// </summary>
        public static int ReadBuildNumber()
        {
            string path = BuilNumberFilePath;

            if (File.Exists(path))
            {
                string data = File.ReadAllText(path).Trim();
                if (!int.TryParse(data, out int toReturn))
                {
                    Debug.LogErrorFormat("could not parse {0}", data);
                }
                else
                {
                    return toReturn;
                }
            }
            else
            {
                Debug.LogFormat("Could not find file at {0}, so it will be created", path);

                // check to make sure the directory exists before trying to write a file to it
                if (!Directory.Exists(Application.streamingAssetsPath))
                {
                    Directory.CreateDirectory(Application.streamingAssetsPath);
                }

                File.WriteAllLines(path, new[] { "0" });
            }

            return 0;
        }

        /// <summary>
        /// Increments the build number by one and writes it to BuildNumber.txt
        /// </summary>
        public static void IncrementBuildNumber()
        {
            string path = BuilNumberFilePath;

            int newBuildNumber = ReadBuildNumber() + 1;
            string[] lines = { newBuildNumber.ToString() };
            File.WriteAllLines(path, lines);
        }
    }
}
