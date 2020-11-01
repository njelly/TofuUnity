using Tofunaut.TofuUnity.Interfaces;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class AppVersionProvider : MonoBehaviour, IVersionProvider
    {
        public Version Version => _version;

        private Version _version;

        private void Awake()
        {
            _version = new Version($"{Application.version}{Version.Delimiter}{BuildNumberUtil.ReadBuildNumber()}");
        }
    }
}