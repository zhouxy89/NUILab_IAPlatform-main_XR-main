using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Varjo.XR;

namespace VarjoExample
{
    public class Controller : MonoBehaviour
    {
        [Header("Select hand")]
        public XRNode XRNode = XRNode.LeftHand;

        [Header("Controllers parts")]
        public GameObject controller;
        public GameObject bodyGameobject;
        public GameObject touchPadGameobject;
        public GameObject menubuttonGameobject;
        public GameObject triggerGameobject;
        public GameObject systemButtonGameobject;
        public GameObject gripButtonGameobject;

        [Header("Controller material")]
        public Material controllerMaterial;

        [Header("Controller button highlight material")]
        public Material buttonPressedMaterial;
        public Material touchpadTouchedMaterial;

        [Header("Visible only for debugging")]
        public bool triggerButton;
        public bool gripButton;
        public bool primary2DAxisTouch;
        public bool primary2DAxisClick;
        public bool menuButton;
        public float trigger;
        public bool triggerValue;
        public bool primaryValue;

        private List<InputDevice> devices = new List<InputDevice>();
        private InputDevice device;

        private Quaternion deviceRotation; //Controller rotation
        private Vector3 devicePosition; //Controller position
        private Vector3 deviceAngularVelocity; // Controller angular velocity
        private Vector3 deviceVelocity; // Controller velocity
        private Vector3 triggerRotation; // Controller trigger rotation

        public bool TriggerButton { get { return triggerButton; } }

        public bool GripButton { get { return gripButton; } }

        public bool Primary2DAxisTouch { get { return primary2DAxisTouch; } }

        public bool Primary2DAxisClick { get { return primary2DAxisClick; } }

        //public bool menuButton { get {return menuButton;} }

        public Vector3 DeviceVelocity { get { return deviceVelocity; } }

        public Vector3 DeviceAngularVelocity { get { return deviceAngularVelocity; } }

        public float Trigger { get { return trigger; } }

        void OnEnable()
        {
            if (!device.isValid)
            {
                GetDevice();
            }
        }

        void Update()
        {
            if (!device.isValid)
            {
                GetDevice();
            }

            // Get values for device position, rotation and buttons.
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out devicePosition))
            {
                Debug.LogFormat("got  devicePosition");
                Debug.LogFormat("got CommonUsages.menuButton::::" + device.TryGetFeatureValue(CommonUsages.menuButton, out primaryValue));
                transform.localPosition = devicePosition;
            }

            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation))
            {
                
                transform.localRotation = deviceRotation;
            }

            if (device.TryGetFeatureValue(CommonUsages.trigger, out trigger))
            {
                Debug.LogFormat("got  trigger");
                ControllerInput();
            }

            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                Debug.LogFormat("got  triggerValue");
                ControllerInput();
            }

            if (device.TryGetFeatureValue(CommonUsages.gripButton, out gripButton))
            {
                ControllerInput();
            }

            if (device.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out primary2DAxisTouch))
            {
                ControllerInput();
            }

            if (device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out primary2DAxisClick))
            {
                ControllerInput();
            }

            Debug.Log("menuButton is ::::" + device.TryGetFeatureValue(CommonUsages.menuButton, out menuButton));
            Debug.Log("menuButton ::::" + menuButton);
            if (device.TryGetFeatureValue(CommonUsages.menuButton, out menuButton) && menuButton)
            {
                Debug.Log("menuButton is pressed.");
                Debug.LogFormat("got menuButton");
                ControllerInput();
            }

            device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out deviceAngularVelocity);

            device.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity);
        }

        void GetDevice()
        {
            InputDevices.GetDevicesAtXRNode(XRNode, devices);
            device = devices.FirstOrDefault();
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", device.name, device.role.ToString()));
        }

        void ControllerInput()
        {
            //Set trigger rotation from input
            triggerRotation.Set(trigger * -30f, 0, 0);
            triggerGameobject.transform.localRotation = Quaternion.Euler(triggerRotation);

            //Set controller button inputs
            if (!triggerValue)
            {
                triggerGameobject.GetComponent<MeshRenderer>().material = controllerMaterial;
            }
            else
            {
                triggerGameobject.GetComponent<MeshRenderer>().material = buttonPressedMaterial;
            }

            if (!gripButton)
            {
                gripButtonGameobject.GetComponent<MeshRenderer>().material = controllerMaterial;
            }
            else
            {
                gripButtonGameobject.GetComponent<MeshRenderer>().material = buttonPressedMaterial;
            }

            if (!primary2DAxisTouch)
            {
                touchPadGameobject.GetComponent<MeshRenderer>().material = controllerMaterial;
            }
            else if (primary2DAxisTouch && primary2DAxisClick)
            {
                touchPadGameobject.GetComponent<MeshRenderer>().material = buttonPressedMaterial;
            }
            else if (primary2DAxisTouch)
            {
                touchPadGameobject.GetComponent<MeshRenderer>().material = touchpadTouchedMaterial;
            }

            if (!menuButton)
            {
                Debug.LogFormat("not trigger menuButton");
                menubuttonGameobject.GetComponent<MeshRenderer>().material = controllerMaterial;
            }
            else
            {
                Debug.LogFormat("triggered menuButton");
                menubuttonGameobject.GetComponent<MeshRenderer>().material = buttonPressedMaterial;
            }
        }
    }
}
