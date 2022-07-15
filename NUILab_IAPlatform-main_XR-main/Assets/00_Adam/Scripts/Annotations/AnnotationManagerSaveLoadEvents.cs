using System.Collections.Generic;
using UnityEngine;
using System.IO;
using IATK;

using System.Runtime.Serialization.Formatters.Binary;
using System;


namespace Photon_IATK
{
    public class AnnotationManagerSaveLoadEvents : MonoBehaviour
    {
        public bool isWaitingForListOfAnnotationIDs = false;
        public int annotationsCreated = 0;
        public int lastMadeAnnotationPhotonViewID;
        public static AnnotationManagerSaveLoadEvents Instance;
        public bool isFirstLoad = true;

        #region Setup and Teardown

        private void Awake()
        {

            Instance = this;

        }

        private void OnEnable()
        {
            VisualizationEvent_Calls.RPCvisualisationUpdatedDelegate += UpdatedView;
            VisualizationEvent_Calls.RPCvisualisationUpdateRequestDelegate += UpdatedViewRequested;
        }

        private void OnDisable()
        {
            VisualizationEvent_Calls.RPCvisualisationUpdateRequestDelegate -= UpdatedViewRequested;
            VisualizationEvent_Calls.RPCvisualisationUpdatedDelegate -= UpdatedView;      
        }

        private void OnApplicationQuit()
        {
            saveAnnotations();
        }

        #endregion Setup and Teardown

        #region Delegates

        private void UpdatedView(AbstractVisualisation.PropertyType propertyType)
        {
            if (propertyType == AbstractVisualisation.PropertyType.DimensionChange || propertyType == AbstractVisualisation.PropertyType.VisualisationType) {
                if (isFirstLoad)
                {
                    Invoke("loadAnnotations", 1f);
                } else
                {
                    loadAnnotations();
                }
                
            }

        }

        private void UpdatedViewRequested(AbstractVisualisation.PropertyType propertyType)
        {
            //Save annotations handled by other class

            if (propertyType == AbstractVisualisation.PropertyType.DimensionChange || propertyType == AbstractVisualisation.PropertyType.VisualisationType) { saveAnnotations(); }

            //Delete annotations without marking delete but as safe
            if (true) {
                RespondToRequestAnnotationRemoval();
                //RequestAnnotationRemoval();
            }
        }

        #endregion Delegates

        #region Events

        //private void OnEvent(EventData photonEventData)
        //{
        //    byte eventCode = photonEventData.Code;

        //    //Check that the event was one we made, photon reserves 0, 200+
        //    if (eventCode == 0 || eventCode > 199) { return; }

        //    object[] data = (object[])photonEventData.CustomData;
        //    int callerPhotonViewID = (int)data[0];

        //    //make sure that this object is the same as the sender object
        //    if (photonView.ViewID != callerPhotonViewID) { return; }

        //    //route the event
        //    switch (eventCode)
        //    {
        //        case GlobalVariables.RequestEventAnnotationCreation:
        //            RespondToRequestAnnotationCreation(data);
        //            break;
        //        case GlobalVariables.RequestEventAnnotationRemoval:
        //            RespondToRequestAnnotationRemoval();
        //            break;
        //        case GlobalVariables.RequestEventAnnotationFileSystemDeletion:
        //            DeleteAnnotaitonFileSystem();
        //            break;
        //        case GlobalVariables.SendEventNewAnnotationID:
        //            setAnnotationIDEvent(data);
        //            break;
        //        case GlobalVariables.RequestSaveAnnotation:
        //            _saveAnnotations(data);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #region AnnotationCreation

        public void RequestAnnotationCreationTestTracker()
        {
            RequestAnnotationCreation(Annotation.typesOfAnnotations.TEST_TRACKER);
        }

        public void RequestAnnotationCreationCentralityMetricPlane()
        {
            RequestAnnotationCreation(Annotation.typesOfAnnotations.CENTRALITY);
        }

        public void RequestAnnotationCreationHighlightCube()
        {
            RequestAnnotationCreation(Annotation.typesOfAnnotations.HIGHLIGHTCUBE);
        }

        public void RequestAnnotationCreationHighlightSphere()
        {
            RequestAnnotationCreation(Annotation.typesOfAnnotations.HIGHLIGHTSPHERE);
        }

        public void RequestAnnotationCreationDetailsOnDemand()
        {
            RequestAnnotationCreation(Annotation.typesOfAnnotations.DETAILSONDEMAND);
        }

        public void RequestAnnotationCreationText()
        {
            RequestAnnotationCreation(Annotation.typesOfAnnotations.TEXT);
        }

        /// <summary>
        /// Sends request to master client to room instantiate an annotation object
        /// Data Sent = object[] { photonView.ViewID, annotationType.ToString() };
        /// Raises = GlobalVariables.RequestEventAnnotationCreation
        /// Reciver = ReceiverGroup.MasterClient
        /// </summary>
        public void RequestAnnotationCreation(Annotation.typesOfAnnotations annotationType)
        {
            GameObject annotationCollection;
            if (!HelperFunctions.FindGameObjectOrMakeOneWithTag("AnnotationCollection", out annotationCollection, false, System.Reflection.MethodBase.GetCurrentMethod()))
            {
                Debug.LogFormat(GlobalVariables.cError + "{0}." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "You are offline or there is no annotationcollection.", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
                return;
            }
            CreateAnnotation(annotationType);
  
        }

        private void CreateAnnotation(Annotation.typesOfAnnotations annotationType)
        {
            GameObject genericAnnotationObj;


            annotationsCreated++;
            genericAnnotationObj = Instantiate(Resources.Load("GenericAnnotation")) as GameObject;
            HelperFunctions.SetObjectLocalTransformToZero(genericAnnotationObj, System.Reflection.MethodBase.GetCurrentMethod());
            genericAnnotationObj.name = "NewAnnotation_" + annotationsCreated;


            Annotation annotation;
            if (HelperFunctions.GetComponentInChild<Annotation>(out annotation, genericAnnotationObj, System.Reflection.MethodBase.GetCurrentMethod()))
            {
                annotation.myAnnotationType = annotationType;
                annotation.myUniqueAnnotationNumber = annotationsCreated;
                annotation.wasLoaded = false;
                annotation.SetAnnotationObject();
                centerAnnotationOnSpawnPoint(annotation);
            }

        }

        private void centerAnnotationOnSpawnPoint(Annotation annotation)
        {
            if (annotation.myAnnotationType == Annotation.typesOfAnnotations.LINERENDER) {
                Debug.LogFormat(GlobalVariables.cTest + "Not centering annotation, is linerender.{0} {1} " + GlobalVariables.endColor + " {2}: {3} -> {4} -> {5}", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
                return; 
            } else
            {
                Debug.LogFormat(GlobalVariables.cTest + "Centering annotation Type is {0} {1} " + GlobalVariables.endColor + " {2}: {3} -> {4} -> {5}", annotation.myAnnotationType.ToString(), "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
            }

            GameObject spawnPoint;
            HelperFunctions.FindGameObjectOrMakeOneWithTag(GlobalVariables.spawnTag, out spawnPoint, false, System.Reflection.MethodBase.GetCurrentMethod());

            if (annotation.myAnnotationType == Annotation.typesOfAnnotations.CENTRALITY)
            {
                annotation.transform.rotation = spawnPoint.transform.rotation;
                annotation.transform.Rotate(new Vector3(-90f, 0, 0));

                Transform centerPoint;
                centerPoint = annotation.myObjectRepresentation.transform.GetChild(0);
                //Vector3 travelPathVector = centerPoint - fromTransform.position;
                annotation.transform.position = spawnPoint.transform.position;
                annotation.transform.position = annotation.transform.position + (annotation.transform.position - centerPoint.transform.position);

            } else
            {
                annotation.transform.position = spawnPoint.transform.position;
                annotation.transform.rotation = spawnPoint.transform.rotation;
            }
            

        }

        private void CreateAnnotation(SerializeableAnnotation serializeableAnnotation)
        {


            GameObject genericAnnotationObj;


            annotationsCreated++;

            if (serializeableAnnotation.isDeleted)
            {
                return;
            }

            genericAnnotationObj = Instantiate(Resources.Load("GenericAnnotation")) as GameObject;
            HelperFunctions.SetObjectLocalTransformToZero(genericAnnotationObj, System.Reflection.MethodBase.GetCurrentMethod());
            genericAnnotationObj.name = "LoadedAnnotation_" + serializeableAnnotation.myAnnotationNumber;



            Annotation annotation;
            if (HelperFunctions.GetComponentInChild<Annotation>(out annotation, genericAnnotationObj, System.Reflection.MethodBase.GetCurrentMethod()))
            {
                annotation.SetUpFromSerializeableAnnotation(serializeableAnnotation);
                annotation.wasLoaded = true;
                annotation.SetAnnotationObject();

            }
        }



        #endregion Annotation Creation

        #region AnnotaitonRemoval

        /// <summary>
        /// Sends request to master client to remove and not mark deleted on all annotaitons
        /// Data Sent = object[] { photonView.ViewID};
        /// Raises = GlobalVariables.RequestEventAnnotationRemoval
        /// Reciver = ReceiverGroup.MasterClient
        /// </summary>
        public void RequestAnnotationRemoval()
        {
            //PhotonNetwork.RaiseEvent(GlobalVariables.RequestEventAnnotationRemoval, content, raiseEventOptions, GlobalVariables.sendOptions);
            RespondToRequestAnnotationRemoval();
        }

        private void RespondToRequestAnnotationRemoval()
        {

            var annotations = GameObject.FindGameObjectsWithTag("Annotation");

            foreach (GameObject annotation in annotations)
            {

                Debug.LogFormat(GlobalVariables.cOnDestory + "Destorying {0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", annotation.name, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
                Destroy(annotation);
            }
        }
        
        public void RequestAnnotationFileSystemDeletion()
        {

            DeleteAnnotaitonFileSystem();
        }

        private void DeleteAnnotaitonFileSystem()
        {
            string mainFolderName = GlobalVariables.annotationSaveFolder;
            string mainFolderPath = Path.Combine(Application.persistentDataPath, mainFolderName);
            if (Directory.Exists(mainFolderPath)) { Directory.Delete(mainFolderPath, true); }
        }

        #endregion AnnotaitonRemoval

        #region AnnotationSaveing


        private void _saveAnnotations(object[] data)
        {
            Debug.LogFormat(GlobalVariables.cCommon + "Annotation reviced, saving now: {0}." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            string jsonAnnotation = (string)data[1];
            SerializeableAnnotation serializeableAnnotation = JsonUtility.FromJson<SerializeableAnnotation>(jsonAnnotation);
            _saveAnnotations(serializeableAnnotation);
        }

        public void saveAnnotations()
        {
            bool saveWasSuccessfull = false;

            //find all annotations and convert to serilizable annotation
            List<SerializeableAnnotation> listOfAnnotations = _getAllAnnotationsAndConvertToSerializeableAnnotations(out saveWasSuccessfull);
            if (!saveWasSuccessfull) { return; };

            //save with axis title
            _saveAnnotations(listOfAnnotations);

            Debug.LogFormat(GlobalVariables.cCommon + "{0} {1} {2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "Annotations uccessfully saved", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
        }

        private List<SerializeableAnnotation> _getAllAnnotationsAndConvertToSerializeableAnnotations(out bool wasSuccessfull)
        {
            int countOfAnnotationsFound = 0;

            List<SerializeableAnnotation> listOfAnnotations = new List<SerializeableAnnotation>();
            GameObject[] annotationHolderObjects = GameObject.FindGameObjectsWithTag("Annotation");

            if (annotationHolderObjects.Length == 0)
            {
                Debug.LogFormat(GlobalVariables.cAlert + "{0} {1} {2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "No annotation holders found. Nothing saved", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
                wasSuccessfull = false;
            }
            else
            {
                foreach (GameObject annotationHolder in annotationHolderObjects)
                {
                    Annotation annotation = annotationHolder.GetComponent<Annotation>();
                    if (annotation != null)
                    {
                        listOfAnnotations.Add(annotation.GetSerializeableAnnotation());
                        countOfAnnotationsFound++;
                    }
                }
            }

            Debug.LogFormat(GlobalVariables.cCommon + "{0} {1} {2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", countOfAnnotationsFound, " Annotations Found.", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            wasSuccessfull = true;
            return listOfAnnotations;
        }

        private void _saveAnnotations(List<SerializeableAnnotation> listOfSerializeableAnnotations)
        {

            string subfolderPath = _getFolderPath();

            foreach (SerializeableAnnotation serializeableAnnotation in listOfSerializeableAnnotations)
            {
                serializeableAnnotation.wasLoaded = true;

                string filename = serializeableAnnotation.myAnnotationNumber.ToString("D3");
                filename += "_" + serializeableAnnotation.myAnnotationType.ToString();
                filename += "_" + _getParentVisAxisKey() + ".json";

                string jsonFormatAnnotion = JsonUtility.ToJson(serializeableAnnotation, true);

                string fullFilePath = Path.Combine(subfolderPath, filename);

                Debug.LogFormat(GlobalVariables.cFileOperations + "Saving {0}, full path: {1} " + GlobalVariables.endColor + " {2}: {3} -> {4} -> {5}", filename, fullFilePath, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
                System.IO.File.WriteAllText(fullFilePath, jsonFormatAnnotion);
            }

            Debug.LogFormat(GlobalVariables.cFileOperations + "Annotations saved for {0}, full path: {1} " + GlobalVariables.endColor + " {2}: {3} -> {4} -> {5}", _getParentVisAxisKey(), subfolderPath, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
        }

        private void _saveAnnotations(SerializeableAnnotation serializeableAnnotation)
        {

            string subfolderPath = _getFolderPath();

            serializeableAnnotation.wasLoaded = true;

            string filename = serializeableAnnotation.myAnnotationNumber.ToString("D3");
            filename += "_" + serializeableAnnotation.myAnnotationType.ToString();
            filename += "_" + _getParentVisAxisKey() + ".json";

            string jsonFormatAnnotion = JsonUtility.ToJson(serializeableAnnotation, true);

            string fullFilePath = Path.Combine(subfolderPath, filename);

            Debug.LogFormat(GlobalVariables.cFileOperations + "Saving {0}, full path: {1} " + GlobalVariables.endColor + " {2}: {3} -> {4} -> {5}", filename, fullFilePath, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
            System.IO.File.WriteAllText(fullFilePath, jsonFormatAnnotion);

            Debug.LogFormat(GlobalVariables.cFileOperations + "Annotation saved for {0}, full path: {1} " + GlobalVariables.endColor + " {2}: {3} -> {4} -> {5}", _getParentVisAxisKey(), subfolderPath, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
        }

        private string _getParentVisAxisKey()
        {
            VisualizationEvent_Calls visualisationEvent_Calls;
            if (!HelperFunctions.GetComponent<VisualizationEvent_Calls>(out visualisationEvent_Calls, System.Reflection.MethodBase.GetCurrentMethod())) { return "NoVisEventCallsFound"; }

            return visualisationEvent_Calls.axisKey;
        }

        private string _getFolderPath()
        {
            //Annotations are saved per VisState in a folder with the names of that vis axis
            string mainFolderName = GlobalVariables.annotationSaveFolder;
            string mainFolderPath = Path.Combine(Application.persistentDataPath, mainFolderName);

#if UWP
            Windows.Storage.StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

            mainFolderPath = Path.Combine(installedLocation, mainFolderName);
#endif


            //_checkAndMakeDirectory(mainFolderPath);

            string date = System.DateTime.Now.ToString("yyyyMMdd");
            //string parentVisAxisKey = _getParentVisAxisKey();
            //string subFolderName = date + "_" + parentVisAxisKey;
            string subFolderName = date;
            string subfolderPath = Path.Combine(mainFolderPath, subFolderName);
            _checkAndMakeDirectory(subfolderPath);

            return subfolderPath;
        }

        private void _checkAndMakeDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Debug.LogFormat(GlobalVariables.cFileOperations + "Makeing new folder{0}, full path: {1} " + GlobalVariables.endColor + " {2}: {3} -> {4} -> {5}", "", directory, Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                Directory.CreateDirectory(directory);
            }
        }

#endregion AnnotationSaveing

#region AnnotationLoading

        public void loadAnnotations()
        {
            isFirstLoad = false;

            //get file path
            string getFolderPath = _getFolderPath();

            string[] filePaths = Directory.GetFiles(getFolderPath, "*.json");

            Debug.LogFormat(GlobalVariables.cFileOperations + "{0} .json annotation records found in {1}, {2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", filePaths.Length, getFolderPath, "Loading annotations now.", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            foreach (string jsonPath in filePaths)
            {
                //Load file
                if (jsonPath.Contains(_getParentVisAxisKey()))
                {
                    SerializeableAnnotation serializeableAnnotation = JsonUtility.FromJson<SerializeableAnnotation>(File.ReadAllText(jsonPath));
                    CreateAnnotation(serializeableAnnotation);
                }
            }

        }


#endregion AnnotationLoading

#endregion Events
    }
}

