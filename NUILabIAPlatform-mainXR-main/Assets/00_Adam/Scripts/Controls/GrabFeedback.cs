using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon_IATK
{
    /// <summary>
    /// When attached this class will provide a material change event that can be triggered by adding it to the "on manipulation started/stopped" events in the GUI for the manipulation handler script.
    /// </summary>

    [DisallowMultipleComponent]
    public class GrabFeedback : MonoBehaviour
    {

        public Material grabbedMaterial;
        public Material grabbedHandleMaterial;
        private Dictionary<Renderer, Material> renderersAndMats;
        private bool isSecondAttempt = false;

        void Start()
        {
            if (grabbedMaterial == null)
            {
                grabbedMaterial = Resources.Load("GrabFeedback", typeof(Material)) as Material;
            }

            if (grabbedHandleMaterial == null)
            {
                grabbedHandleMaterial = Resources.Load("GrabHandleFeedback", typeof(Material)) as Material;
            }

            setupRenderers();
        }

        private void setupRenderers()
        {
            if (renderersAndMats == null)
            {
                renderersAndMats = new Dictionary<Renderer, Material>();

                Transform[] ts = this.transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts)
                {
                    if (t.GetComponent<GrabFeedbackTarget>() == null) continue;
                    Renderer renderer = t.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Material material = renderer.material;
                        if (material != null)
                        {
                            renderersAndMats.Add(renderer, renderer.material);
                        }
                    }
                }
            }

            if (renderersAndMats == null)
            {
                Transform[] ts = this.transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts)
                {
                    if (t.GetComponent<GrabFeedbackTarget>() == null) continue;
                    LineRenderer renderer = t.GetComponent<LineRenderer>();
                    if (renderer != null)
                    {
                        Material material = renderer.material;
                        if (material != null)
                        {
                            renderersAndMats.Add(renderer, renderer.material);
                        }
                    }
                }
            }
        }

        public void Grabbed()
        {

            _grabedEvent();
        }

        private void _grabedEvent()
        {
            _grabbed();
        }

        public void _grabbed()
        {
            foreach (Renderer key in renderersAndMats.Keys)
            {
                key.material = grabbedMaterial;
            }
        }

        public void GrabbedHandle()
        {
            _grabedHandleEvent();
        }

        private void _grabedHandleEvent()
        {
            _grabbedHandle();
        }

        public void _grabbedHandle()
        {
            foreach (Renderer key in renderersAndMats.Keys)
            {
                key.material = grabbedHandleMaterial;
            }
        }

        public void Released()
        {
            _ReleasedEvent();
        }

        private void _ReleasedEvent()
        {
            _released();
        }

        private void _released()
        {
            foreach (Renderer key in renderersAndMats.Keys)
            {
                key.material = renderersAndMats[key];
            }
        }

        public void ReleasedHandle()
        {
            _ReleasedHandleEvent();
        }

        private void _ReleasedHandleEvent()
        {
            _releasedHandle();
        }

        private void _releasedHandle()
        {
            foreach (Renderer key in renderersAndMats.Keys)
            {
                key.material = renderersAndMats[key];
            }
        }

        private static void ApplyMaterialToAllRenderers(GameObject root, Material material)
        {
            if (material != null)
            {
                Renderer[] renderers = root.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < renderers.Length; ++i)
                {
                    renderers[i].material = material;
                }
            }
        }
    }
}
