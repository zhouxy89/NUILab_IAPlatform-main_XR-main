using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Photon_IATK
{
    /// <summary>
    /// This class is incharge of photon scene instantiation and destruction
    /// </summary>
    public class GeneralEventManager : MonoBehaviour
    {
        public static GeneralEventManager instance;
        public bool isElictationOnPC = false;

        #region Setup

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Debug.LogFormat(GlobalVariables.cCommon + "{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "GeneralEventManager Set", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
            }
            else if(instance != this)
            {
                Destroy(instance.gameObject);
                instance = this;
                Debug.LogFormat(GlobalVariables.cCommon + "{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "GeneralEventManager Destoryed then Set", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        /// <summary>
        /// Checks the relavance of the event then routes the event to the right funciton.
        /// Data = Object[]
        /// This will recive events that are not specific to the photon view of the sender
        /// </summary>
        //private void OnEvent(EventData photonEventData)
        //{
        //    byte eventCode = photonEventData.Code;

        //    //Check that the event was one we made, photon reserves 0, 200+
        //    if (eventCode == 0 || eventCode > 199) { return; }

        //    object[] data = (object[])photonEventData.CustomData;

        //    //route the event
        //    switch (eventCode)
        //    {
        //        case GlobalVariables.PhotonVisSceneInstantiateEvent:
        //            PhotonProcessVisSceneInstantiateEvent(data);
        //            break;
        //        case GlobalVariables.PhotonDeleteAllObjectsWithComponentEvent:
        //            PhotonProcessDeleteAllObjectsWithComponentEvent(data);
        //            break;
        //        case GlobalVariables.PhotonDeleteSingleObjectsWithViewIDEvent:
        //            PhotonProcessDeleteSingleObjectsWithViewEvent(data);
        //            break;
        //        case GlobalVariables.PhotonRequestLatencyCheckEvent:
        //            SendResponseToLatencyCheckEvent(data);
        //            break;
        //        case GlobalVariables.PhotonRequestLatencyCheckResponseEvent:
        //            PhotonProcessRequestLatencyCheckResponseEvent(data);
        //            break;
        //        case GlobalVariables.RequestElicitationSetupEvent:
        //            ElicitationSetUpEvent();
        //            Debug.Log("RequestElicitationSetupEvent");
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #region Send Events



        public void SendVisSceneInstantiateEvent()
        {
            GameObject obj;

                GameObject prefab = Resources.Load("Vis") as GameObject;
                GameObject prefabAnnotationStation = Resources.Load("AnnotationStationBtns") as GameObject;
                GameObject prefabTrashCube = Resources.Load("TrashCube") as GameObject;

                obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
                obj = Instantiate(prefabAnnotationStation, new Vector3(0, 0, 0), Quaternion.identity);
                obj = Instantiate(prefabTrashCube, new Vector3(0, 0, 0), Quaternion.identity);
        }

        public void SendDeleteAllObjectsWithComponentRequest(string className)
        {
                PhotonProcessDeleteAllObjectsWithComponentEvent(className);


        }

        public void SendDeleteSingleObjectRequest(GameObject obj)
        {
                SafeDestory(obj);
        }

        /// <summary>
        /// Using an RPC so that new clients recive the call on loggin in
        /// </summary>
        public void SendSetupElicitatoinPCRequest()
        {

                ElicitationSetUpEvent();

        }

        #endregion

        #region Receive Events


        public void ElicitationSetUpEvent()
        {
            // All clients now have this flagged.
            isElictationOnPC = true;
            RemoveVisItemsForElicitPC();
            DisableManipulations();

        }

        public void DisableManipulations()
        {
            var manipulatorScripts = FindObjectsOfType<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>();
            foreach (var script in manipulatorScripts)
            {
                script.ManipulationType = 0;
            }

            var manipulationControlsScripts = FindObjectsOfType<ManipulationControls>();
            foreach (var script in manipulationControlsScripts)
            {
                script.BoundingBoxActivation = ManipulationControls.BoundingBoxActivationType.ActivateManually;
            }
        }


        public void RemoveVisItemsForElicitPC()
        {
            //if exists find and remove
            //remove menu
            //remove platform
            GameObject[] taggedItems = GameObject.FindGameObjectsWithTag(GlobalVariables.pcElicitTag);
            foreach (GameObject obj in taggedItems)
            {
                Destroy(obj);
            }
        }

        private void PhotonProcessVisSceneInstantiateEvent(object[] data)
        {
            GameObject visObj;
            GameObject annotationObj;
            GameObject trashCube;


            visObj = Instantiate(Resources.Load("Vis") as GameObject);
            annotationObj = Instantiate(Resources.Load("AnnotationStation") as GameObject);
            trashCube = Instantiate(Resources.Load("TrashCube") as GameObject);
        }

        //delete vis
        private void PhotonProcessDeleteAllObjectsWithComponentEvent(string data)
        {
            string className = data;
            var type = Type.GetType(className);

            var obejectsWithComponent = FindObjectsOfType(type);

            if (obejectsWithComponent.Length == 0)
            {
                return;
            }

            foreach (Component _component in obejectsWithComponent)
            {
                SafeDestory(_component);
            }
        }

        private void PhotonProcessDeleteSingleObjectsWithViewEvent(object[] data)
        {

        }

        //instantiate annotation

        //delete annotation

        //save annotation

        //load annotation

        //delete all annotaitons

        #endregion

        private void SafeDestory(Component obj)
        {
                Debug.LogFormat(GlobalVariables.cOnDestory + "Offline Destorying: {0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", obj.gameObject.name, "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                Destroy(obj.gameObject);
        }

        private void SafeDestory(GameObject obj)
        {


                    Debug.LogFormat(GlobalVariables.cOnDestory + "Offline Destorying: {0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", obj.gameObject.name, "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                    Destroy(obj);
            }

        }
    }


