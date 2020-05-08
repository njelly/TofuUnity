using UnityEngine;
using UnityEditor;

namespace Tofunaut.TofuUnity.Editor
{
    public class DeletePlayerPrefs
    {
        [MenuItem("Tofunaut/Utilities/Delete PlayerPrefs")]
        public static void Delete()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs have been deleted!");
        }

    }
}