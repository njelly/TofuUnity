using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tofunaut.TofuUnity
{
    public class AppState<T> where T : AppStateController<T>
    {
        public bool IsComplete => _stateController && _stateController.IsReady && _stateController.IsComplete;

        private readonly int _sceneIndex;

        private T _stateController;

        public AppState(int sceneIndex)
        {
            _sceneIndex = sceneIndex;
        }

        public virtual async Task Enter()
        {
            var loadTask = SceneManager.LoadSceneAsync(_sceneIndex);
            while (!loadTask.isDone)
                await Task.Yield();

            _stateController = Object.FindObjectOfType<T>();
            if(!_stateController)
                Debug.LogError($"no object found of type {nameof(T)}");
        }
    }
}