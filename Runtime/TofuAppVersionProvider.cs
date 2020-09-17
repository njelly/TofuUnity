using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class TofuAppVersionProvider : MonoBehaviour, ITofuVersionProvider
    {
        public TofuVersion Version => _version;

        private TofuVersion _version;

        private void Awake()
        {
            _version = new TofuVersion($"{Application.version}{TofuVersion.Delimiter}{BuildNumberUtil.ReadBuildNumber()}");
        }
    }
}