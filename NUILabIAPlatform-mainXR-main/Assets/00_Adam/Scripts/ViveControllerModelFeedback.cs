using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Connects to the selected input device type
/// </summary>
public class ViveControllerModelFeedback : MonoBehaviour
{
    [Header("Controller type")]
    public InputDeviceCharacteristics inputCharacteristics = InputDeviceCharacteristics.Right;

    [Header("Controller model parts")]
    public GameObject primaryButton;
    public GameObject leftGrip;
    public GameObject rightGrip;
    public GameObject trackpad;
    public GameObject trackpadTouch;
    public GameObject triggerModel;

    [Header("Instructions")]
    public GameObject tooltips;
    public GameObject loadMenu;

    [Header("Controller feedback")]
    public Material feedbackMaterialTouch;
    public Material feedbackMaterialPress;
    public Material baseMaterial;

    [Header("Controller")]
    private InputDevice thisInput;
    private string myDeviceName;

    [Header("Controller position")]
    private Quaternion deviceRotation; //Controller rotation
    private Vector3 devicePosition; //Controller position

    [Header("Controller inputs")]
    private bool triggerButton;
    private bool gripButton;
    private bool primary2DAxisTouch;
    private bool primary2DAxisClick;
    private bool primaryButtonPressed;
    private Vector2 primary2DAxis;
    private float trigger;

    private bool lastPrimaryButtonState = false;
    private bool lastTriggerButtonState = false;
    private bool lastGripButtonState = false;
    private bool lastPrimary2DAxisClickState = false;

    private bool isFirstToggle = true;

    // Start is called before the first frame update
    void Start()
    {
        setInputDevice(); //Try to set using existing inputs

        // register events
        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDestroy()
    {
        // stop listening to events
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
    }

    /// <summary>
    /// If active input is disconected log warning
    /// </summary>
    /// <param name="obj"></param>
    private void InputDevices_deviceDisconnected(InputDevice obj)
    {
        if (!thisInput.isValid) { return; }
        if (obj.name == myDeviceName)
        {
            Debug.LogWarning("Active input disconected, Name: " + myDeviceName);
            myDeviceName = "";
            setInputDevice(); //Try to set using existing inputs
        }
    }

    /// <summary>
    /// When a new input is connected and there is no input, try to register it
    /// </summary>
    /// <param name="obj"></param>
    private void InputDevices_deviceConnected(InputDevice obj)
    {
        if (thisInput.isValid) { return; }
        setInputDevice(); setInputDevice(); //Try to set using newly connected input
    }

    /// <summary>
    /// Sets the tracked input device based on the inputCharacteristics selected
    /// </summary>
    /// <returns>bool is input found</returns>
    private bool setInputDevice()
    {
        bool inputFound = false;

        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);

        if (devices.Count > 0)
        {
            thisInput = devices[0];
            myDeviceName = thisInput.name;
            inputFound = true;

            Debug.Log("Conncected to: " + thisInput.name + " : " + thisInput.characteristics);
        } else
        {
            Debug.LogError("No valid input devices were found, so none were set");
        }

        return inputFound;
    }

    // Update is called once per frame
    void Update()
    {
        if (!thisInput.isValid) { return; }
        checkForInputActions();
    }
    /// <summary>
    /// Do a check for each input action state, then when possible execute actions based on that state
    /// </summary>

    private void checkForInputActions()
    {
        //// Get values for device position, rotation
        //if (thisInput.TryGetFeatureValue(CommonUsages.devicePosition, out devicePosition))
        //{
        //    transform.localPosition = devicePosition;
        //}

        //if (thisInput.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation))
        //{
        //    transform.localRotation = deviceRotation * Quaternion.Euler(new Vector3(0f, 180f, 0f));
        //}

        // trigger buttons press and touch
        thisInput.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButton);
        if (thisInput.TryGetFeatureValue(CommonUsages.trigger, out trigger))
        {
            triggerAction();
        }

        // touchpad press and touch
        thisInput.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DAxis);
        thisInput.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out primary2DAxisClick);
        if(thisInput.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out primary2DAxisTouch)){
            touchpadAction();
        }

        // grip actions
        if (thisInput.TryGetFeatureValue(CommonUsages.gripButton, out gripButton))
        {
            gripAction();
        }

        // line menu button (on vive) actions
        if (thisInput.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed))
        {
            primaryButtonAction();
        }
    }

    private void triggerAction()
    {
        triggerModel.transform.localPosition = new Vector3(0.0f, (trigger * .020f * -1), (trigger * .0005f * -1));
        triggerModel.transform.localRotation = Quaternion.Euler(trigger * -1 * 25, 0.0f, 0.0f);

        if (trigger == 0)
        {
            lastTriggerButtonState = triggerButton;
            setMaterialBase(triggerModel);
        }
        else if (triggerButton)
        {
            lastTriggerButtonState = triggerButton;
            setMaterialPressed(triggerModel);
            hapticPulse();
        }
        else
        {
            setMaterialTouched(triggerModel);
        }
    }

    private void touchpadAction()
    {
        float x = 0;
        if (primary2DAxis.x > 0)
        {
            x = (primary2DAxis.x * .020f * -1);
        }
        else
        {
            x = (primary2DAxis.x * .022f * -1);
        }

        float y = 0;
        float z = 0;
        if (primary2DAxis.y > 0)
        {
            y = (primary2DAxis.y * .003f);
            z = (primary2DAxis.y * .020f * -1);
        }
        else
        {
            y = (primary2DAxis.y * .0015f);
            z = (primary2DAxis.y * .020f * -1);
        }

        trackpadTouch.transform.localPosition = new Vector3(x, y, z);

        if (primary2DAxisClick)
        {
            if (lastPrimary2DAxisClickState == primary2DAxisClick) { return; }
            lastPrimary2DAxisClickState = primary2DAxisClick;

            setMaterialPressed(trackpad);
            setMaterialAlpha(trackpadTouch, 1f);
            hapticPulse();
        }
        else if (primary2DAxisTouch)
        {
            setMaterialTouched(trackpad);
            setMaterialAlpha(trackpadTouch, .75f);
            lastPrimary2DAxisClickState = primary2DAxisClick;
        }
        else
        {
            setMaterialBase(trackpad);
            setMaterialAlpha(trackpadTouch, 0f);
            lastPrimary2DAxisClickState = primary2DAxisClick;
        }

    }

    private void gripAction()
    {
        if (lastGripButtonState == gripButton) { return; }
        lastGripButtonState = gripButton;

        if (gripButton)
        {
            setMaterialPressed(leftGrip);
            setMaterialPressed(rightGrip);
            hapticPulse();
        } else
        {
            setMaterialBase(leftGrip);
            setMaterialBase(rightGrip);
        }
    }


    private void primaryButtonAction()
    {
        if (lastPrimaryButtonState == primaryButtonPressed) { return; }
        lastPrimaryButtonState = primaryButtonPressed;

        if (primaryButtonPressed)
        {
            setMaterialPressed(primaryButton);
            hapticPulse();
            tooltips.SetActive(!tooltips.activeSelf);

            if (isFirstToggle)
            {
                Destroy(loadMenu);
                isFirstToggle = false;
            }

        }
        else
        {
            setMaterialBase(primaryButton);
        }
    }

    private void setMaterialAlpha(GameObject feedbackTarget, float value)
    {
        Renderer targetRenderer;
        if (!feedbackTarget.TryGetComponent<Renderer>(out targetRenderer)) { return; }
        targetRenderer.material.SetFloat("_Alpha", value);
    }

    private void setMaterialBase(GameObject feedbackTarget)
    {
        Renderer targetRenderer;
        if (!feedbackTarget.TryGetComponent<Renderer>(out targetRenderer)) { return; }

        if (targetRenderer.material != baseMaterial)
        {
            targetRenderer.material = baseMaterial;
        }
    }

    private void setMaterialTouched(GameObject feedbackTarget)
    {
        Renderer targetRenderer;
        if (!feedbackTarget.TryGetComponent<Renderer>(out targetRenderer)) { return; }

        if (targetRenderer.material != feedbackMaterialTouch)
        {
            targetRenderer.material = feedbackMaterialTouch;
        }
    }

    private void setMaterialPressed(GameObject feedbackTarget)
    {
        Renderer targetRenderer;
        if (!feedbackTarget.TryGetComponent<Renderer>(out targetRenderer)) { return; }

        if (targetRenderer.material != feedbackMaterialPress)
        {
            targetRenderer.material = feedbackMaterialPress;
        }
    }


    /// <summary>
    /// Send a haptic pulse to the active controller
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="amplitude"></param>
    /// <param name="duration">Only for occulus</param>
    private void hapticPulse(uint channel = 0, float amplitude = 0.5f, float duration = 1f)
    {
        UnityEngine.XR.HapticCapabilities capabilities;
        bool isHaptic = thisInput.TryGetHapticCapabilities(out capabilities);

        if (isHaptic)
        {
            thisInput.SendHapticImpulse(channel, amplitude, duration);
        }
    }


}
