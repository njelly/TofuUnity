using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class BuildNumberIncrementer
    {
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            int prevBuild = BuildNumberUtil.ReadBuildNumber();
            BuildNumberUtil.IncrementBuildNumber();

            Debug.Log($"Completed build {prevBuild}, next build will be {prevBuild + 1}");
        }
    }
}