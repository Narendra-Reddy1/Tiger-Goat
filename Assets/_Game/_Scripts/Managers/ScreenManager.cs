using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SovereignStudios
{
    public class ScreenManager : MonoBehaviour
    {
        // public static ScreenManager Instance;
        public static Action<Window> OnScreenChange = null;
        [SerializeField] Transform m_CanvasParent;
        [Space(10)]
        [SerializeField] ScreenSystemObjectDictionary m_Screens = null;

        //New Variables
        [HideInInspector] public List<Window> m_AdditiveScreenStack = new List<Window>();
        private Window m_CurrentReplaceScreenType = Window.None;
        private Window m_CurrentScreenType = Window.None;
        private Window m_PreviousScreenType = Window.None;
        private bool isReadyForNewScreen = true;
        private Action onComplete;
        private Action onCloseComplete;

        #region UnityMethods

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, Callback_On_Change_Screen_Requested);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_CLOSE_LAST_ADDITIVE_SCREEN, Callback_On_Close_Last_Additive_Screen_Requested);
            GlobalEventHandler.AddListener(EventID.EVENT_ON_REMOVE_ALL_SCREENS_REQUESTED, Callback_On_Remove_All_Screens_Requested);
            GlobalEventHandler.AddListener(EventID.EVENT_REQUEST_GET_CURRENT_SCREEN, Callback_On_Current_Screen_Requested);
            GlobalEventHandler.AddListener(EventID.EVENT_REQUEST_GE_PREVIOUS_SCREEN, Callback_On_Previous_Screen_Requested);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_CHANGE_SCREEN_REQUESTED, Callback_On_Change_Screen_Requested);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_CLOSE_LAST_ADDITIVE_SCREEN, Callback_On_Close_Last_Additive_Screen_Requested);
            GlobalEventHandler.RemoveListener(EventID.EVENT_ON_REMOVE_ALL_SCREENS_REQUESTED, Callback_On_Remove_All_Screens_Requested);
            GlobalEventHandler.RemoveListener(EventID.EVENT_REQUEST_GET_CURRENT_SCREEN, Callback_On_Current_Screen_Requested);
            GlobalEventHandler.RemoveListener(EventID.EVENT_REQUEST_GE_PREVIOUS_SCREEN, Callback_On_Previous_Screen_Requested);
        }
        #endregion

        #region Public Methods

        public ref Window GetCurrentScreen()
        {
            return ref m_CurrentScreenType;
        }

        public ref Window GetPreviousScreenScreen()
        {
            return ref m_PreviousScreenType;
        }

        /// <summary>
        /// Call this to remove all screens from MainMenu Canvas.
        /// This case is relevant if there is any other canvas displaying the required information.
        /// eg. in this case, Onboarding scene uses its own Canvas
        /// </summary>
        public void RemoveAllScreens()
        {
            //Removal of Additive Screens
            if (m_AdditiveScreenStack.Count > 0)
            {
                for (int i = 0; i < m_AdditiveScreenStack.Count; i++)
                {
                    if (m_AdditiveScreenStack[i] != Window.None)
                        DestroyScreenObject(m_Screens[m_AdditiveScreenStack[i]]);
                }
                m_AdditiveScreenStack.Clear();
            }
            if (m_CurrentReplaceScreenType != Window.None)
            {
                //Removal of Base screen        
                DestroyScreenObject(m_Screens[m_CurrentReplaceScreenType]);
            }
            m_CurrentScreenType = Window.None;
            m_PreviousScreenType = Window.None;
            m_CurrentReplaceScreenType = Window.None;
        }

        public void ChangeScreen(Window newScreen, ScreenType screenType = ScreenType.Replace, bool enableDelay = true, Action onComplete = null)
        {
            this.onComplete = onComplete;
            Debug.Log("ChangeScreen   " + newScreen.ToString() + "  ScreenType " + screenType.ToString());

            if (GetCurrentScreen() == newScreen)
            {
                Debug.LogError("Same active screen is trying to open   " + newScreen.ToString() + "chekout if same screen is already active in screen or not");
                return;
            }
            //Check to see if required screen exists
            if (!m_Screens.ContainsKey(newScreen))
            {
                Debug.LogError("Could not find " + newScreen.ToString() + " screen key/enum within Screen Manager inspector window");
                return;
            }

            if (m_Screens[newScreen] == null)
            {
                Debug.LogError("Could not find prefab for " + newScreen.ToString() + " screen you can assign the empty field through the Screen Manager inspector window");
                return;
            }
            //Replace / Additive Logic
            if (screenType == ScreenType.Replace)
            {
                List<Window> tempObjectList = new List<Window>();
                tempObjectList.Add(m_CurrentScreenType);
                //Reset the additive screen stack
                foreach (var screens in m_AdditiveScreenStack)
                {
                    tempObjectList.Add(screens);
                }
                if (!tempObjectList.Contains(m_CurrentReplaceScreenType))
                {
                    tempObjectList.Add(m_CurrentReplaceScreenType);
                }

                //Introducing a delay here as Destroy happens at the end of frame so if we are instantiating the same object then the new object gets destroyed
                DelayedInstantiate(newScreen, false, (screen) =>
                {

                    OnIsntantiateComplete(newScreen);
                    foreach (var screenItem in tempObjectList)
                    {
                        if (m_Screens.ContainsKey(screenItem))
                            DestroyScreenObject(m_Screens[screenItem]);
                    }
                    m_AdditiveScreenStack.Clear();
                }).Forget();
            }
            else if (screenType == ScreenType.Additive_Hide)
            {
                if (!m_AdditiveScreenStack.Contains(newScreen))
                {
                    Window tempScren = m_CurrentScreenType;
                    Debug.Log("Change Screen aditive hide " + newScreen.ToString());

                    //Introducing a delay here as Destroy happens at the end of frame so if we are instantiating the same object then the new object gets destroyed
                    DelayedAdditiveInstantiate(newScreen, enableDelay,
                        (screen) =>
                        {
                            OnIsntantiateComplete(newScreen);
                            if (m_Screens.ContainsKey(tempScren))
                            {
                                if (m_Screens[tempScren].GetObj() != null)
                                    m_Screens[tempScren].GetObj().SetActive(false);
                            }
                        }).Forget();
                }
            }
            else
            {
                //Additive Screens
                if (!isReadyForNewScreen)
                {
                    return;
                }

                isReadyForNewScreen = false;
                //Introducing a delay here as Destroy happens at the end of frame so if we are instantiating the same object then the new object gets destroyed
                DelayedAdditiveInstantiate(newScreen, enableDelay, (screen) =>
                {
                    OnIsntantiateComplete(newScreen);
                    isReadyForNewScreen = true;
                }).Forget();
            }
            m_PreviousScreenType = m_CurrentScreenType;
            m_CurrentScreenType = newScreen;
        }

        public void CloseLastAdditiveScreen(Action onComplete = null)
        {
            this.onCloseComplete = onComplete;
            if (m_AdditiveScreenStack.Count > 0)
            {
                var currWindow = m_AdditiveScreenStack[m_AdditiveScreenStack.Count - 1];
                if (m_Screens[currWindow].gameObject.GetComponent<WindowBase>() != null)
                {
                    var windowbase = m_Screens[currWindow].gameObject.GetComponent<WindowBase>();
                    if (windowbase is PopupBase)
                    {
                        var popupBase = windowbase as PopupBase;
                        popupBase.EndAnimation(() =>
                        {
                            CloseScreen();
                        });
                    }
                    else
                    {
                        CloseScreen();
                    }
                }
                else
                {
                    Debug.LogError("Couldn't find WindowBase, ScreenBase or PopupBase is missing in the gameobject " + m_Screens[currWindow].gameObject.name);
                }
            }
        }
        #endregion

        #region Private Methods
        // Reset is called when the user hits the Reset button in the Inspector's context menu or when adding the component the first time.
        // This function is only called in editor mode. Reset is most commonly used to give good default values in the Inspector.
        private void Reset()
        {
            m_Screens = new ScreenSystemObjectDictionary();
        }

        private void CloseScreen()
        {
            var previouslyClosedScreenType = Window.None;
            Debug.Log("CloseLastAdditiveScreen: " + m_AdditiveScreenStack[m_AdditiveScreenStack.Count - 1]);
            DestroyScreenObject(m_Screens[m_AdditiveScreenStack[m_AdditiveScreenStack.Count - 1]]);
            Debug.Log("CloseLastAdditiveScreen case1: " + m_AdditiveScreenStack.Count);
            previouslyClosedScreenType = m_AdditiveScreenStack[m_AdditiveScreenStack.Count - 1];

            m_AdditiveScreenStack.RemoveAt(m_AdditiveScreenStack.Count - 1);
            Debug.Log("CloseLastAdditiveScreen case2: " + m_AdditiveScreenStack.Count);

            if (m_AdditiveScreenStack.Count == 0)
            {
                if (m_CurrentReplaceScreenType != Window.None)
                {
                    if (m_Screens[m_CurrentReplaceScreenType].GetObj() != null)
                    {
                        m_Screens[m_CurrentReplaceScreenType].GetObj().SetActive(true);
                    }
                    m_PreviousScreenType = Window.None;
                    m_CurrentScreenType = m_CurrentReplaceScreenType;
                }
            }
            else
            {
                m_PreviousScreenType = m_AdditiveScreenStack[m_AdditiveScreenStack.Count - 1];
                if (m_PreviousScreenType != Window.None)
                {
                    if (m_Screens[m_PreviousScreenType].GetObj() != null)
                        m_Screens[m_PreviousScreenType].GetObj().SetActive(true);
                }
                m_CurrentScreenType = m_PreviousScreenType;
            }
            OnScreenChange?.Invoke(m_CurrentScreenType);
            onCloseComplete?.Invoke();
        }

        private void OnIsntantiateComplete(Window screen)
        {
            OnScreenChange?.Invoke(screen);
            onComplete?.Invoke();
        }
        private void DestroyScreenObject(WindowAddressableObject screenObject)
        {
            if (screenObject.IsObjectExist)
                screenObject.Release();
        }
        private async UniTaskVoid DelayedInstantiate(Window newScreen, bool enableDelay, Action<Window> Callback)
        {
            if (enableDelay)
            {
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            }
            if (ReferenceEquals(m_CanvasParent, null) ? false : (m_CanvasParent ? false : true))
                m_CanvasParent = GetCanvasTransform();

            m_Screens[newScreen].Instantiate(m_CanvasParent, false, (status) =>
            {
                if (status)
                    Callback?.Invoke(newScreen);
            });
            m_CurrentReplaceScreenType = newScreen;
        }

        private async UniTaskVoid DelayedAdditiveInstantiate(Window newScreen, bool enableDelay, Action<Window> Callback)
        {
            if (enableDelay)
            {
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            }
            if (ReferenceEquals(m_CanvasParent, null) ? false : (m_CanvasParent ? false : true))
                m_CanvasParent = GetCanvasTransform();
            m_Screens[newScreen].Instantiate(m_CanvasParent, false, (status) =>
            {
                if (status)
                    Callback.Invoke(newScreen);
            });
            if (!m_AdditiveScreenStack.Contains(newScreen))
                m_AdditiveScreenStack.Add(newScreen);
        }

        private Transform GetCanvasTransform()
        {
            return GameObject.FindGameObjectWithTag("Canvas").transform;
        }
        #endregion

        #region Callbacks
        private void Callback_On_Remove_All_Screens_Requested(object args) => RemoveAllScreens();
        private void Callback_On_Close_Last_Additive_Screen_Requested(object args)
        {
            if (args != null)
            {
                Tuple<Action> onComplete = args as Tuple<Action>;
                CloseLastAdditiveScreen(onComplete.Item1);
            }
            else
                CloseLastAdditiveScreen();
        }
        private object Callback_On_Previous_Screen_Requested(object args)
        {
            return GetPreviousScreenScreen();
        }
        private object Callback_On_Current_Screen_Requested(object args)
        {
            return GetCurrentScreen();
        }

        private void Callback_On_Change_Screen_Requested(object args)
        {
            try
            {
                var properties = args as ScreenChangeProperties;

                ChangeScreen(properties.window, properties.screenType, properties.enableDelay, properties.onComplete);
            }
            catch (Exception e)
            {
                SovereignUtils.Log($"Exception from ChangeScreen: {e.Message} ST: {e.StackTrace}");
            }
        }

        #endregion Callbacks
    }

    public enum ScreenType
    {
        Additive,//This mode retains the base layer and opens new UI on top of it eg. Popups
        Replace,//This replaces the Base screen eg. Dashboard/ Characters/ Social from ASC
                //Temp_Replace//This is like replace but does not destroy the old base layer, to be used in case where Base Layer needs to instantiated again and again.
        Additive_Hide//This mode retains the base layer but hides it, and opens new UI on top of it eg. Over Change on top of Scoreboard
    }
    [Serializable]
    public class ScreenChangeProperties
    {
        public Window window;
        public ScreenType screenType;
        public bool enableDelay;
        public Action onComplete;
        public ScreenChangeProperties(Window window, ScreenType screenType = ScreenType.Replace, bool enableDelay = true, Action onComplete = null)
        {
            this.window = window;
            this.screenType = screenType;
            this.enableDelay = enableDelay;
            this.onComplete = onComplete;
        }
    }

}