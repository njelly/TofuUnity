using UnityEngine;

namespace Tofunaut.TofuUnity.UI
{
    public class Tab : MonoBehaviour
    {
        public bool IsOpen => content.activeInHierarchy;
        
        public GameObject content;
        
        

        public void Open()
        {
            content.SetActive(true);
            
            OnOpen();
        }
        
        protected virtual void OnOpen() { }

        public void Close()
        {
            content.SetActive(false);
            OnClose();
        }
        
        protected virtual void OnClose() { }
    }
}