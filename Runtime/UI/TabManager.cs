using System;
using UnityEngine;

namespace Tofunaut.TofuUnity.UI
{
    public sealed class TabManager : MonoBehaviour
    {
        private Tab CurrentTab => tabs[_currentTabIndex];
        
        public Tab[] tabs;
        [Tooltip("Should we switch to the tab on the opposite side when reaching the end of the tab list")]
        public bool doCycle;

        private int _currentTabIndex;

        public void OnEnable()
        {
            _currentTabIndex = -1;
            OpenTab(0);
        }

        public void OpenTab(int index)
        {
            if (index > tabs.Length - 1)
                index = doCycle ? 0 : tabs.Length - 1;
            else if (index < 0)
                index = doCycle ? tabs.Length - 1 : 0;

            if (_currentTabIndex == index)
                return;

            _currentTabIndex = index;
            for(var i = 0; i < tabs.Length; i++)
                if (i == index && !tabs[i].IsOpen)
                    tabs[i].Open();
                else if(tabs[i].IsOpen)
                    tabs[i].Close();
        }

        public void GoLeft() => OpenTab(_currentTabIndex - 1);

        public void GoRight() => OpenTab(_currentTabIndex + 1);
    }
}