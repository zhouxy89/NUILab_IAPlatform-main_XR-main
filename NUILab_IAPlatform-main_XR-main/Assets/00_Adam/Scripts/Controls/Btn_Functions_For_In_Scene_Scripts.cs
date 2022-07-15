﻿using UnityEngine;
using System.Reflection;

namespace Photon_IATK
{
    public class Btn_Functions_For_In_Scene_Scripts : MonoBehaviour
    {

        #region Private variables

        private bool isMine;
        //public static Btn_Functions_For_In_Scene_Scripts Instance;

        #endregion

        #region Private Functions

        private void Awake()
        {
            //if (Instance == null)
            //{
            //    Debug.LogFormat(GlobalVariables.cSingletonSetting + "{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "Setting Btn_Functions_For_In_Scene_Scripts.Instance ", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            //    Instance = this;
            //}
            //else if (Instance != this && Instance != null)
            //{
            //    Debug.LogFormat(GlobalVariables.cSingletonSetting + "{0}, Destroying then setting Btn_Functions_For_In_Scene_Scripts.Instance." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", Instance.name, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            //    Destroy(Instance.gameObject);

            //    Instance = this;
            //} 
            //else
            //{
            //    Debug.LogFormat(GlobalVariables.cError + "{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "Failed to make instance", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
            //}
        }

        private bool checkSetViewOwnership()
        {
            isMine = true;
            return true;
        }

        private bool GetOrAddComponent<T>(out T component) where T : Component
        {
            component = null;

            if (!checkSetViewOwnership())
            {
                Debug.LogFormat(GlobalVariables.cError + "{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "Current View is not mine", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                return false;
            }

            component = FindObjectOfType(typeof(T)) as T;

            if (component != null)
            {
                Debug.LogFormat(GlobalVariables.cCommon + "Found {0}, returning it." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", component.GetType(), Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                return true;
            }
            else
            {
                component = gameObject.AddComponent<T>() as T;
                Debug.LogFormat(GlobalVariables.cComponentAddition + "Attaching {0} and returning it." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", component.GetType(), Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                return true;
            }
        }

        private T GetOrAddComponent<T>() where T : Component
        {
            T component;
            if (GetOrAddComponent<T>(out component))
            {
                if (component == null)
                {
                    Debug.LogFormat(GlobalVariables.cError + "Failed to get compenent: {0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", component.GetType(), Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                }
            }
            return component;
        }

        #endregion

        #region Btn_Calls

        #region sceneManager
        public void sceneManager_Load_Main()
        {
            GetOrAddComponent<MRTK_Scene_Manager>().load_00_EntryPoint();
        }
        public void sceneManager_Load_01_SetupMenu()
        {
            GetOrAddComponent<MRTK_Scene_Manager>().load_01_SetupMenu();
        }

        public void sceneManager_load_02_EnterPID()
        {
            GetOrAddComponent<MRTK_Scene_Manager>().load_02_EnterPID();
        }

        public void sceneManager_load_03_VuforiaSetup()
        {
            GetOrAddComponent<MRTK_Scene_Manager>().load_03_Vuforia_Setup();
        }

        public void sceneManager_load_04_TriVuforia()
        {
            GetOrAddComponent<MRTK_Scene_Manager>().load_04_TriVuforia();
        }
        public void sceneManager_unload_04_TriVuforia()
        {
            GetOrAddComponent<MRTK_Scene_Manager>().unload_04_TriVuforia();
        }
        #endregion

        #region Photon
        public void Lobby_Connect()
        {
            GetOrAddComponent<Room>().CreatePlayer();
        }
        #endregion

        #region Logs

        public void showVisInterface()
        {
            GameObject visMenu;
            if (HelperFunctions.FindGameObjectOrMakeOneWithTag(GlobalVariables.visInterfaceMenuTag, out visMenu, false, System.Reflection.MethodBase.GetCurrentMethod()))
            {
                Debug.LogFormat(GlobalVariables.cOnDestory + "Destorying VisMenu{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                Destroy(visMenu);
            }
            else
            {
                visMenu = Resources.Load<GameObject>("VisInterfaceMenu");
                visMenu = Instantiate(visMenu);
                visMenu.tag = GlobalVariables.visInterfaceMenuTag;
            }

            Debug.LogFormat(GlobalVariables.cCommon + "Loading VisMenu{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
  
        }

        public void showDebugLog()
        {
            GameObject debugLog;
            if (HelperFunctions.FindGameObjectOrMakeOneWithTag(GlobalVariables.debugLogTag, out debugLog, false, System.Reflection.MethodBase.GetCurrentMethod()))
            {
                Debug.LogFormat(GlobalVariables.cOnDestory + "Destorying Debug Log{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                Destroy(debugLog);
            }
            else
            {
                debugLog = Resources.Load<GameObject>("Log");
                debugLog = Instantiate(debugLog);
                debugLog.AddComponent<Debug_Log>();
                debugLog.tag = GlobalVariables.debugLogTag;

                Debug.LogFormat(GlobalVariables.cCommon + "Loading Debug Log{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        public void showAnchorMenu()
        {
            GameObject anchorMenu;
            if (HelperFunctions.FindGameObjectOrMakeOneWithTag(GlobalVariables.AnchorMenuTag, out anchorMenu, false, System.Reflection.MethodBase.GetCurrentMethod()))
            {
                Debug.LogFormat(GlobalVariables.cOnDestory + "Destorying anchorMenu{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                Destroy(anchorMenu);
            }
            else
            {
                anchorMenu = Resources.Load<GameObject>("MovePlayspaceMenu");
                anchorMenu = Instantiate(anchorMenu);

                Debug.LogFormat(GlobalVariables.cCommon + "Loading anchorMenu Log{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region VisIATK
        public void LoadRemoveVis()
        {
            GetOrAddComponent<NetworkedLoadVisualisation>().LoadVis();
        }

        public void CenterVis()
        {
            if (PlayspaceAnchor.Instance == null) { return; }

            Vector3 pos = PlayspaceAnchor.Instance.transform.position;

            GameObject[] viss = GameObject.FindGameObjectsWithTag(GlobalVariables.visTag);



            foreach (GameObject vis in viss)
            {
                vis.transform.position = pos;

                Vector3 lookPos = Camera.main.transform.position;
                Quaternion lookRot = Quaternion.LookRotation((lookPos - vis.transform.position).normalized);
                float eulerY = lookRot.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0, eulerY + 180, 0);

                vis.transform.rotation = rotation;

                //Quaternion lookPos = Camera.main.transform.rotation;
                //float eulerY = lookPos.eulerAngles.y;
                //Quaternion rotation = Quaternion.Euler(0, eulerY, 0);

                //vis.transform.rotation = rotation;
            }

            MoveToTagLocation[] moveToTagLocations = Object.FindObjectsOfType<MoveToTagLocation>();
            foreach (MoveToTagLocation move in moveToTagLocations)
            {
                move.moveToTag();
            }


        }
        #endregion

        #region Annotations
        public void SaveAnnotations()
        {
            GetOrAddComponent<AnnotationManagerSaveLoadEvents>().saveAnnotations();
        }

        public void LoadAnnotations()
        {
            GetOrAddComponent<AnnotationManagerSaveLoadEvents>().loadAnnotations();
        }
        #endregion

        #region Events
        public void ShowHideControllerModels()
        {
            Pun_Player_Event_Calls.Event_showHideControllerModels();
        }

        public void ShowHideExtras()
        {
            Pun_Player_Event_Calls.Event_HideExtras();
            Debug.LogFormat(GlobalVariables.cCommon + "{0}{1}{2}{3}" + GlobalVariables.endColor + " {4}: {5} -> {6} -> {7}", "Btn pressed", ": ", "Event_HideExtras", "", this.name, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void SetNickname()
        {
            Pun_Player_Event_Calls.Event_setNickName();
        }


        #endregion

        #region spawn items

        public void LoadSetNameBox()
        {
            GameObject[] IDBoxs = GameObject.FindGameObjectsWithTag("PIDEntry");

            if (IDBoxs.Length > 0)
            {
                foreach (GameObject IDBox in IDBoxs)
                {
                    Photon_Set_Nickname setNickName = IDBox.GetComponentInChildren<Photon_Set_Nickname>();
                    if (setNickName != null)
                    {
                        setNickName.SetPlayerName();
                    }

                    Destroy(IDBox);
                }
            } else
            {
                GameObject instance = Instantiate(Resources.Load("IDEntry", typeof(GameObject))) as GameObject;
            }
        }

        public void DestroySetNameBox()
        {
            GameObject[] IDBoxs = GameObject.FindGameObjectsWithTag("PIDEntry");

            foreach (GameObject IDBox in IDBoxs)
            {
                Destroy(IDBox);
            }
        }

        #endregion

        #endregion


    }
}
