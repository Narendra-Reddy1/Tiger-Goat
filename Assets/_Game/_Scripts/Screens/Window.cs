using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SovereignStudios
{
    [System.Serializable]
    public enum Window
    {
        None,
        GenericPopup,
        TriangleBoardMap,
        MainMenu,
        GameplayScreen,
        GameOverScreen,
        SettingsScreen,
        ConsoleScreen,//Debug Screen
        AIModeSelectionScreen,

    }

    [System.Serializable]
    public class WindowObject
    {
        [SerializeField]
        private GameObject gameObjectRef;
        private GameObject gameObject;

        #region Public Methods
        public ref GameObject GetObj()
        {
            return ref gameObject;
        }
        public GameObject Instantiate(Transform parent, bool instantiateInWorldSpace)
        {
            gameObject = GameObject.Instantiate(gameObjectRef, parent, instantiateInWorldSpace);
            return gameObject;
        }
        public void Release()
        {
            GameObject.Destroy(gameObject);
        }
        #endregion
    }

    [System.Serializable]
    public class WindowAddressableObject
    {
        [SerializeField]
        private AssetReferenceGameObject assetRefObj;
        private AsyncOperationHandle<GameObject> asyncObj;
        [HideInInspector]
        public GameObject gameObject;
        [HideInInspector]
        public bool IsObjectExist = false;
        private bool IsObjectinstantiating = false;
        #region Public Methods
        public ref GameObject GetObj()
        {
            return ref gameObject;
        }
        public void Instantiate(Transform parent, bool instantiateInWorldSpace, Action<bool> OnComplete)
        {
            if (IsObjectinstantiating)
            {
                OnComplete.Invoke(false);
            }
            IsObjectinstantiating = true;
            if (IsObjectExist)
            {
                OnComplete.Invoke(true);
                return;
            }
            AddressableAssetLoader.Instance.Instantiate(assetRefObj, parent, instantiateInWorldSpace,
                (status, _asynobj) =>
                {
                    IsObjectinstantiating = false;
                    if (status)
                    {
                        asyncObj = _asynobj;
                        gameObject = asyncObj.Result;
                        IsObjectExist = true;
                        OnComplete.Invoke(true);
                    }
                    else
                    {
                        OnComplete.Invoke(false);
                        IsObjectExist = false;
                    }
                });
        }
        public void Release()
        {
            IsObjectinstantiating = false;
            if (IsObjectExist)
            {
                AddressableAssetLoader.Instance.Release(asyncObj);
                // assetRefObj.ReleaseAsset();

            }
            IsObjectExist = false;
            gameObject = null;
        }
        #endregion
    }
}