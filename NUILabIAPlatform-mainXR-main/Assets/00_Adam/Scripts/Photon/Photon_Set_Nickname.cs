using UnityEngine;

namespace Photon_IATK
{
    [RequireComponent(typeof(TMPro.TMP_InputField))]
    public class Photon_Set_Nickname : MonoBehaviour
    {


        public TMPro.TMP_InputField InputFeild;

        private string playerNamePrefKey = GlobalVariables.PlayerPrefsKeys.ParticipantID.ToString();

        // Start is called before the first frame update
        void Start()
        {
            string defaultName = string.Empty;
            if (InputFeild != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    InputFeild.text = defaultName;
                }
            }

        }

        public void SetPlayerName()
        {

            string value = "";

            if (InputFeild != null)
            {
                value = InputFeild.text;
            }

            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogFormat(GlobalVariables.cError + "{0}" + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "Nickname Null", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());
                return;
            }

            PlayerPrefs.SetString(playerNamePrefKey, value);

        }
    }
}
