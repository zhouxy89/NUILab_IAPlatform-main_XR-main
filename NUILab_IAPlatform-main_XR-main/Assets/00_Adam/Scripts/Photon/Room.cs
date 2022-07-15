
using UnityEngine;


namespace Photon_IATK
{
    public class Room : MonoBehaviour

    {
        [SerializeField] private GameObject photonUserPrefab = default;


        private void Start()
        {
            CreatePlayer();
        }


        public void CreatePlayer()
        {
            var player = Instantiate(photonUserPrefab);

        }
    }
}
