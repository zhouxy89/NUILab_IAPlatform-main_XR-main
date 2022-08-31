using UnityEngine;

namespace Photon_IATK
{
    [DisallowMultipleComponent]
    public class GameSetupAuto : MonoBehaviour
    {

        private Btn_Functions_For_In_Scene_Scripts btn_functions;
        public string participantID;

        // Start is called before the first frame update
        void Start()
        {
            btn_functions = FindObjectOfType<Btn_Functions_For_In_Scene_Scripts>();

            setup();

        }

        static void enableVR()
        {
            UnityEngine.XR.XRSettings.enabled = true;
            UnityEngine.XR.XRSettings.LoadDeviceByName("OpenVR");
        }

        static void disableVR()
        {
            UnityEngine.XR.XRSettings.enabled = false;
            UnityEngine.XR.XRSettings.LoadDeviceByName("None");
        }



        private void setup()
        {
     
        // Debug.LogFormat(GlobalVariables.cCommon + "{0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "Seting up VIVE Environement", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

        //enableVR();

        //    Invoke("loadVis", 4f);

        //}
        //private void loadVis()
        //{
        //    GameObject vis;
        //    if(!HelperFunctions.FindGameObjectOrMakeOneWithTag("Vis", out vis, false, System.Reflection.MethodBase.GetCurrentMethod()))
        //    {
        //        btn_functions.LoadRemoveVis();
        //    }

        }

        


    }
}
