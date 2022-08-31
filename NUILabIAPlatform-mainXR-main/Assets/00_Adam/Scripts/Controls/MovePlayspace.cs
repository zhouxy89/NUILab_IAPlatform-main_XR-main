using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;


namespace Photon_IATK
{
    public class MovePlayspace : MonoBehaviour
    {
        //trigger sending values
        //trigger reciving values
        //trigger update

        public TMP_Dropdown playerDropDown;

        public TMP_InputField posX_TMP;
        public TMP_InputField posY_TMP;
        public TMP_InputField posZ_TMP;
        public TMP_InputField rotX_TMP;
        public TMP_InputField rotY_TMP;
        public TMP_InputField rotZ_TMP;

        public float posX;
        public float posY;
        public float posZ;
        public float rotX;
        public float rotY;
        public float rotZ;

        private void OnEnable()
        {

            setAxisDropdowns();
        }

        private void clearDropdownOptions()
        {
            playerDropDown.ClearOptions();
        }

        private void setAxisDropdowns()
        {
            clearDropdownOptions();

            List<TMP_Dropdown.OptionData> listDataDimensions = new List<TMP_Dropdown.OptionData>();

            listDataDimensions.Add(new TMP_Dropdown.OptionData() { text = "Undefined" });
            listDataDimensions.Add(new TMP_Dropdown.OptionData() { text = "FindPlayers" });

        }

        public void getplayers()
        {
            Debug.LogFormat(GlobalVariables.cRegister + "GetPlayers called{0}{1}{2}." + GlobalVariables.endColor + " {3}: {4} -> {5} -> {6}", "", "", "", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());


            setAxisDropdowns();
        }

        public void dropDownChanged()
        {
            if (playerDropDown.options[playerDropDown.value].text == "Undefined")
            {

            } else if (playerDropDown.options[playerDropDown.value].text == "FindPlayers")
            {
                getplayers();
            }
        }

        public void requestTrackerUpdate()
        {
            Debug.Log("button pressed");
            if (playerDropDown.options[playerDropDown.value].text == "Undefined")
            {
                getplayers();
            }
        }

        public void updateValues()
        {
            float.TryParse(posX_TMP.text, NumberStyles.Any, CultureInfo.CurrentCulture, out posX);
            float.TryParse(posY_TMP.text, NumberStyles.Any, CultureInfo.CurrentCulture, out posY);
            float.TryParse(posZ_TMP.text, NumberStyles.Any, CultureInfo.CurrentCulture, out posZ);
            float.TryParse(rotX_TMP.text, NumberStyles.Any, CultureInfo.CurrentCulture, out rotX);
            float.TryParse(rotY_TMP.text, NumberStyles.Any, CultureInfo.CurrentCulture, out rotY);
            float.TryParse(rotZ_TMP.text, NumberStyles.Any, CultureInfo.CurrentCulture, out rotZ);
        }

    }
}
