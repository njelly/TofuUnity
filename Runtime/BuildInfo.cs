using UnityEngine;

namespace Tofunaut.TofuUnity
{
    [CreateAssetMenu(fileName = "new BuildInfo", menuName = "Tofunaut/BuildInfo")]
    public class BuildInfo : ScriptableObject
    {
        public int buildNumber;
        public string buildMachineName;
        public string buildDate;
    }
}