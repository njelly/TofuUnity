using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Tofunaut.TofuUnity.Editor
{
    public class BuildInfoUpdater : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            var buildInfos = AssetDatabase.FindAssets("t:BuildInfo").Select(x =>
                AssetDatabase.LoadAssetAtPath<BuildInfo>(AssetDatabase.GUIDToAssetPath(x)));

            foreach (var buildInfo in buildInfos)
            {
                buildInfo.buildNumber += 1;
                buildInfo.buildDate = DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss");
                buildInfo.buildMachineName = Environment.MachineName;
                
                PlayerSettings.iOS.buildNumber = buildInfo.buildNumber.ToString();
                PlayerSettings.macOS.buildNumber = buildInfo.buildNumber.ToString();
                PlayerSettings.tvOS.buildNumber = buildInfo.buildNumber.ToString();
                
                EditorUtility.SetDirty(buildInfo);
            }
            
            
            AssetDatabase.SaveAssets();
        }
    }
}