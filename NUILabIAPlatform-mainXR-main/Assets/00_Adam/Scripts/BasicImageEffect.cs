/*
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
If you have any issues, feel free to reach out: contact@samuelvanallen.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ImageEffectAllowedInSceneView]
public class BasicImageEffect : MonoBehaviour
{
    [Header("Resources")]
    [Tooltip("Shader for desired image effect.")]
    [SerializeField] protected Shader imageEffectShader;

    private Material mat;

    protected void Start()
    {
        if (!TryCreateMaterial())
        {
            Debug.LogError("[BasicImageEffect] Unable to create material from shader! Script will self destruct.");
            Destroy(this);
        }
    }

    protected void OnEnable()
    {
        TryCreateMaterial();
    }

    protected bool TryCreateMaterial()
    {
        if (imageEffectShader == null)
            return false;

        if (mat == null)
            mat = new Material(imageEffectShader);

        return (mat != null);
    }

    protected void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}