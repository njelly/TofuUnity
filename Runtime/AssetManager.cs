using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Tofunaut.TofuUnity
{
    public class AssetManager
    {
        public delegate void LoadCompleteCallback<TObject>(bool successful, TObject payload) where TObject : UnityEngine.Object;
        public delegate void LoadListCompleteCallback<TObject>(bool successful, List<TObject> payload) where TObject : UnityEngine.Object;

        /// <summary>
        /// True if all load calls have completed.
        /// </summary>
        public bool Ready { get { return _numLoadCalls == _numCompletions; } }

        /// <summary>
        /// The current progress of all load calls.
        /// </summary>
        public float Progress { get { return (float)_numLoadCalls / _numCompletions; } } // TODO: This could be better.

        private int _numLoadCalls;
        private int _numCompletions;
        private readonly Dictionary<Type, Dictionary<string, UnityEngine.Object>> _loadedAssets = new Dictionary<Type, Dictionary<string, UnityEngine.Object>>();
        private readonly Dictionary<Type, HashSet<string>> _loadingAssets = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, Dictionary<string, List<Action>>> _typeToKeyToDuplicateLoadCallbacks = new Dictionary<Type, Dictionary<string, List<Action>>>();

        /// <summary>
        /// Load an asset asynchronously into memory.
        /// </summary>
        public void Load<TObject>(string key, LoadCompleteCallback<TObject> onComplete = null) where TObject : UnityEngine.Object
        {
            if (IsLoaded(typeof(TObject), key))
            {
                onComplete?.Invoke(true, Get<TObject>(key));
                return;
            }

            _numLoadCalls++;

            // when multiple identical load calls all happen at the same time, we don't want to execute them all,
            // we just need to load once and distribute the result to all the callbacks
            if (IsLoading(typeof(TObject), key))
            {
                if (!_typeToKeyToDuplicateLoadCallbacks.TryGetValue(typeof(TObject), out Dictionary<string, List<Action>> keyToDuplicateLoadCallbacks))
                {
                    keyToDuplicateLoadCallbacks = new Dictionary<string, List<Action>>();
                    _typeToKeyToDuplicateLoadCallbacks.Add(typeof(TObject), keyToDuplicateLoadCallbacks);
                }
                if (!keyToDuplicateLoadCallbacks.TryGetValue(key, out List<Action> duplicateLoadCallbacks))
                {
                    duplicateLoadCallbacks = new List<Action>();
                    keyToDuplicateLoadCallbacks.Add(key, duplicateLoadCallbacks);
                }

                duplicateLoadCallbacks.Add(() =>
                {
                    // by the time this executes, the asset should be loaded
                    TObject payload = Get<TObject>(key);
                    _numCompletions++;
                    onComplete?.Invoke(payload != null, payload);
                });

                // return, we'll execute the callback when the first load call completes
                return;
            }

            // record that we are already loading this asset
            if (!_loadingAssets.TryGetValue(typeof(TObject), out HashSet<string> loadingKeys))
            {
                loadingKeys = new HashSet<string>();
                _loadingAssets.Add(typeof(TObject), loadingKeys);
            }
            loadingKeys.Add(key);

            Addressables.LoadAssetAsync<TObject>(key).Completed += (AsyncOperationHandle<TObject> obj) =>
            {
                _numCompletions++;

                if (obj.Result == null)
                {
                    Debug.LogErrorFormat("the asset loaded with the key {0} is null", key);
                }
                else
                {
                    if (!_loadedAssets.ContainsKey(typeof(TObject)))
                    {
                        _loadedAssets.Add(typeof(TObject), new Dictionary<string, UnityEngine.Object>());
                    }

                    _loadedAssets[typeof(TObject)].Add(key, obj.Result as TObject);
                    loadingKeys.Remove(key);
                }

                onComplete?.Invoke(obj.Result != null, obj.Result as TObject);

                // now invoke any of the duplicate load complete callbacks
                if (_typeToKeyToDuplicateLoadCallbacks.ContainsKey(typeof(TObject)))
                {
                    if (_typeToKeyToDuplicateLoadCallbacks[typeof(TObject)].ContainsKey(key))
                    {
                        foreach (Action callback in _typeToKeyToDuplicateLoadCallbacks[typeof(TObject)][key])
                        {
                            callback?.Invoke();
                        }
                        _typeToKeyToDuplicateLoadCallbacks[typeof(TObject)][key].Clear();
                    }
                }
            };
        }

        /// <summary>
        /// Load a list of assets (like a list of sprites from a sprite image set to "multiple")
        /// </summary>
        public void LoadList<TObject>(string key, LoadListCompleteCallback<TObject> onComplete = null) where TObject : UnityEngine.Object
        {

            Addressables.LoadAssetAsync<IList<TObject>>(key).Completed += (AsyncOperationHandle<IList<TObject>> obj) =>
            {

                if (obj.Result == null)
                {
                    Debug.LogErrorFormat("the asset loaded with the key {0} is null", key);
                }
                else
                {
                    if (!_loadedAssets.ContainsKey(typeof(TObject)))
                    {
                        _loadedAssets.Add(typeof(TObject), new Dictionary<string, UnityEngine.Object>());
                    }

                    _loadedAssets[typeof(TObject)].Add(key, obj.Result as TObject);
                }

                onComplete?.Invoke(obj.Result != null, new List<TObject>(obj.Result));

                // now invoke any of the duplicate load complete callbacks
                if (_typeToKeyToDuplicateLoadCallbacks.ContainsKey(typeof(TObject)))
                {
                    if (_typeToKeyToDuplicateLoadCallbacks[typeof(TObject)].ContainsKey(key))
                    {
                        foreach (Action callback in _typeToKeyToDuplicateLoadCallbacks[typeof(TObject)][key])
                        {
                            callback?.Invoke();
                        }
                        _typeToKeyToDuplicateLoadCallbacks[typeof(TObject)][key].Clear();
                    }
                }
            };
        }

        /// <summary>
        /// Release an asset from memory.
        /// </summary>
        public void Release<TObject>(string key) where TObject : UnityEngine.Object
        {
            if (IsLoaded(typeof(TObject), key))
            {
                Addressables.Release(_loadedAssets[typeof(TObject)][key]);
                _loadedAssets[typeof(TObject)].Remove(key);
                if (_loadedAssets[typeof(TObject)].Count <= 0)
                {
                    _loadedAssets.Remove(typeof(TObject));
                }
            }
        }

        /// <summary>
        /// Returns a cached asset.
        /// </summary>
        public TObject Get<TObject>(string key) where TObject : UnityEngine.Object
        {
            if (IsLoaded(typeof(TObject), key))
            {
                return _loadedAssets[typeof(TObject)][key] as TObject;
            }

            Debug.LogErrorFormat("The asset {0} has not been loaded", key);
            return null;
        }

        /// <summary>
        /// Returns true if the asset of the type and key is loaded into memory.
        /// </summary>
        public bool IsLoaded(Type type, string key)
        {
            return _loadedAssets.TryGetValue(type, out Dictionary<string, UnityEngine.Object> dict) && dict.ContainsKey(key);
        }

        /// <summary>
        /// Returns true if the asset of the type and key has begun loading but has not yet completed.
        /// </summary>
        public bool IsLoading(Type type, string key)
        {
            return _loadingAssets.TryGetValue(type, out HashSet<string> loadingKeys) && loadingKeys.Contains(key);
        }
    }
}
