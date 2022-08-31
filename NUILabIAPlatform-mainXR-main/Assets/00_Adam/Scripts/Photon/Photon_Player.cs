using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

namespace Photon_IATK
{
    public class Photon_Player : MonoBehaviour
    {


        #region Public Fields

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public bool isMine = false;

        public Photon_Cammera_Manager _Cammera_Manager;
        public TMPro.TextMeshPro txtNickName;

        public Vector3 CurrentTransform;
        public Vector3 NewTransform;

        #endregion

        #region Private Fields
        private bool isSetup = false;
        #endregion

        #region MonoBehaviour CallBacks
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized

            isMine = true;

            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
            txtNickName.text = "towo";
        }

        public void OnEnable()
        {
            Debug.LogFormat(GlobalVariables.cRegister + "Photon_Player registering OnEvent.{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            setup();
            //unity has predefined tags "Player" is one
            this.tag = "Player";
        }

        private void OnDestroy()
        {

        }

        #endregion

        #region Events

        public void RequestNicknameChangeEvent()
        {
            setNickname();
        }

        public void RequestHideControllerModelsEvent()
        {
                showHideControllerModels();
        }

        public void RequestHideExtrasEvent()
        {
                showHideExtras();
        }




        void setNickname()
        {
            txtNickName.text = "test";

        }

        public void showHideControllerModels()
        {
            Debug.LogFormat(GlobalVariables.cCommon + "{0}{1}{2}{3}" + GlobalVariables.endColor + " {4}: {5} -> {6} -> {7}", "Hiding Controller Models","","","", this.name, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
        }

        public void showHideExtras()
        {
            GameObject[] extras = GameObject.FindGameObjectsWithTag(GlobalVariables.ExtraTag);

            foreach (GameObject extra in extras)
            {
                bool currentState = extra.transform.GetChild(0).gameObject.activeSelf;

                if (currentState)
                {
                    extra.transform.GetChild(0).gameObject.SetActive(false);

                    Debug.LogFormat(GlobalVariables.cCommon + "Hiding {0}, current state: {1}, settting to: {2}, parent: {3}" + GlobalVariables.endColor + " {4}: {5} -> {6} -> {7}", extra.name, "True", "False", extra.transform.parent.name, this.name, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
                } else
                {
                    extra.transform.GetChild(0).gameObject.SetActive(true);

                    Debug.LogFormat(GlobalVariables.cCommon + "Hiding {0}, current state: {1}, settting to: {2}, parent: {3}" + GlobalVariables.endColor + " {4}: {5} -> {6} -> {7}", extra.name, "False", "True", extra.transform.parent.name, this.name, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
                }
                
            }
        }

        #endregion
        #region Custom


#if VIVE
        private void setup(){
            Debug.Log(GlobalVariables.green + "VIVE Setup... " + GlobalVariables.endColor + this.GetType().Name.ToString());
            if (true)
            {
                if (!isSetup)
                {
                    isSetup = true;

                    this.gameObject.AddComponent<PrimaryButtonWatcher>();
                    this.gameObject.AddComponent<PenButtonWatcher>();

                    // disable hand ray pointer
                    PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

                    Debug.LogFormat(GlobalVariables.cComponentAddition + "{0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "Adding Left Controller", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                    LoadControllerModels loadControllerModelsLeft = this.gameObject.AddComponent<LoadControllerModels>();
                    loadControllerModelsLeft.isLeft = true;
                    loadControllerModelsLeft.setUp();

                    Debug.LogFormat(GlobalVariables.cComponentAddition + "{0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "Adding Right Controller", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                    LoadControllerModels loadControllerModelsRight = this.gameObject.AddComponent<LoadControllerModels>();
                    loadControllerModelsRight.isLeft = false;
                    loadControllerModelsRight.setUp();

                    //this.gameObject.AddComponent<ButtonListeners>();

                }
            }
        }

        void Update()
        {
            this.gameObject.transform.position = Camera.allCameras[0].transform.position;
            this.gameObject.transform.rotation = Camera.allCameras[0].transform.rotation;
        }

#endif

        #endregion

    }
}