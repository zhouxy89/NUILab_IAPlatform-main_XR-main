using UnityEngine;
using IATK;


namespace Photon_IATK
{
    public class VisualizationEvent_Calls : MonoBehaviour
    {
        public delegate void OnEventVisualisationUpdated(AbstractVisualisation.PropertyType propertyType);
        public static OnEventVisualisationUpdated RPCvisualisationUpdatedDelegate;

        public delegate void OnEventCVisualisationUpdateRequest(AbstractVisualisation.PropertyType propertyType);
        public static OnEventCVisualisationUpdateRequest RPCvisualisationUpdateRequestDelegate;

        private VisWrapperClass thisVis;

        public bool isWaitingForPhotonRequestStateEvent = false;

        public string[] loadedCSVHeaders { get; set; }

        public string xDimension {
            get 
            {
                return thisVis.xDimension.Attribute;
            }
        }
        public string yDimension
        {
            get
            {
                return thisVis.yDimension.Attribute;
            }
        }
        public string zDimension
        {
            get
            {
                return thisVis.zDimension.Attribute;
            }
        }
        public string colourDimension
        {
            get
            {
                return thisVis.colourDimension;
            }
        }
        public string sizeDimension
        {
            get
            {
                return thisVis.sizeDimension;
            }
        }

        public string axisKey
        {
            get
            {
                return thisVis.axisKey;
            }
        }


        private void Awake()
        {
            getVisWrapper();

            Debug.LogFormat(GlobalVariables.cRegister + "Registering visUpdatedListener{0}." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "visUpdatedListener", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
            VisWrapperClass.visualisationUpdatedDelegate += visUpdatedListener;

        }

        private void OnDestroy()
        {
            Debug.LogFormat(GlobalVariables.cRegister + "Unregistering visUpdatedListener{0}." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "visUpdatedListener", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            VisWrapperClass.visualisationUpdatedDelegate -= visUpdatedListener;
        }

        private void getVisWrapper()
        {
            if (thisVis == null)
            {
                thisVis = this.gameObject.GetComponent<VisWrapperClass>();
            }

            if (thisVis != null)
            {
                loadedCSVHeaders = thisVis.loadedCSVHeaders;
                return;
            }

            Debug.LogFormat(GlobalVariables.cError + "{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "No VisWrapper found.", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
        }

        private void visUpdatedListener(AbstractVisualisation.PropertyType propertyType)
        {
            if (thisVis == null)
                getVisWrapper();

            if (RPCvisualisationUpdatedDelegate != null)
                RPCvisualisationUpdatedDelegate(propertyType);
        }


        private void _RPCvisualisationUpdateRequestDelegate()
        {
            if (RPCvisualisationUpdateRequestDelegate != null)
            {
                Debug.Log("RPCvisualisationUpdateRequestDelegate from VisEventCalls");
                RPCvisualisationUpdateRequestDelegate(AbstractVisualisation.PropertyType.DimensionChange);
            }
        }

            //switch (eventCode)
            //{
            //    case GlobalVariables.PhotonChangeX_AxisEvent:
            //        _RPCvisualisationUpdateRequestDelegate();
            //        _changeXAxis((string)data[1]);
            //        Debug.Log("PhotonChangeX_AxisEvent");
            //        propertyType = AbstractVisualisation.PropertyType.X;
            //        break;
            //    case GlobalVariables.PhotonChangeY_AxisEvent:
            //        _RPCvisualisationUpdateRequestDelegate();
            //        _changeYAxis((string)data[1]);
            //        Debug.Log("PhotonChangeY_AxisEvent");
            //        propertyType = AbstractVisualisation.PropertyType.Y;
            //        break;
            //    case GlobalVariables.PhotonChangeZ_AxisEvent:
            //        _RPCvisualisationUpdateRequestDelegate();
            //        _changeZAxis((string)data[1]);
            //        propertyType = AbstractVisualisation.PropertyType.Z;
            //        Debug.Log("PhotonChangeZ_AxisEvent");
            //        break;
            //    case GlobalVariables.PhotonChangeColorDimensionEvent:
            //        _changeColorDimension((string)data[1]);
            //        propertyType = AbstractVisualisation.PropertyType.Colour;
            //        Debug.Log("PhotonChangeSizeDimensionEvent");
            //        break;
            //    case GlobalVariables.PhotonChangeSizeDimensionEvent:
            //        _changeSizeDimension((string)data[1]);
            //        propertyType = AbstractVisualisation.PropertyType.Size;
            //        Debug.Log("PhotonChangeColorDimensionEvent");
            //        break;
            //    case GlobalVariables.PhotonRequestStateEvent:
            //        _sendStateEvent();
            //        Debug.Log("PhotonRequestStateEvent");
            //        break;
            //    case GlobalVariables.PhotonRequestStateEventResponse:
            //        _processRequestStateEventResponse(data);
            //        Debug.Log("PhotonRequestStateEventResponse");
            //        break;
            //    default:
            //        break;
            //}


        public void RequestChangeXAxisEvent(string newAxisDimension)
        {
            _changeXAxis(newAxisDimension);
        }

        public void RequestChangeYAxisEvent(string newAxisDimension)
        {
            _changeYAxis(newAxisDimension);
        }

        public void RequestChangeZAxisEvent(string newAxisDimension)
        {
            _changeZAxis(newAxisDimension);
        }

        public void RequestChangeColorDimensionEvent(string newAxisDimension)
        {
            _changeColorDimension(newAxisDimension);
        }

        public void RequestChangeSizeDimensionEvent(string newAxisDimension)
        {
            _changeSizeDimension(newAxisDimension);
        }

        private void _changeXAxis(string newAxisDimension)
        {
            Debug.LogFormat(GlobalVariables.cCommon + "{0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "Updated Mapping", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), 
System.Reflection.MethodBase.GetCurrentMethod());
            thisVis.xDimension = newAxisDimension;
            thisVis.updateVisPropertiesSafe();
        }

        private void _changeYAxis(string newAxisDimension)
        {
            Debug.LogFormat(GlobalVariables.cCommon + "{0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "Updated Mapping", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            thisVis.yDimension = newAxisDimension;
            thisVis.updateVisPropertiesSafe();
        }

        private void _changeZAxis(string newAxisDimension)
        {
            Debug.LogFormat(GlobalVariables.cCommon + "{0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "Updated Mapping", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            thisVis.zDimension = newAxisDimension;
            thisVis.updateVisPropertiesSafe();
        }

        private void _changeColorDimension(string newColorDimension)
        {
            Debug.LogFormat(GlobalVariables.cCommon + "{0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "Updated Mapping", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            Color colorStart = Color.blue;
            Color colorEnd = Color.red;

            thisVis.dimensionColour = HelperFunctions.getColorGradient(colorStart, colorEnd);
            thisVis.colourDimension = newColorDimension;
            thisVis.updateVisPropertiesSafe();
        }

        private void _changeSizeDimension(string newAxisDimension)
        {
            Debug.LogFormat(GlobalVariables.cCommon + "{0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "Updated Mapping", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

            thisVis.sizeDimension = newAxisDimension;
            thisVis.updateVisPropertiesSafe();
        }

    }
}
