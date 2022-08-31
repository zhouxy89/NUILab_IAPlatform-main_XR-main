// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace Photon_IATK
{
    /// <summary>
    /// Mixed Reality standard shader utility class with commonly used constants, types and convenience methods.
    /// </summary>
    public static class LitShaderUtility
    {
        /// <summary>
        /// The string name of the Mixed Reality Toolkit/Standard shader which can be used to identify a shader or for shader lookups.
        /// </summary>
        public static readonly string HDRPShaderName = "00_Adam/Prefab";

        /// <summary>
        /// Returns an instance of the Mixed Reality Toolkit/Standard shader.
        /// </summary>
        public static Shader HDRPShader
        {
            get
            {
                if (hdrpShader == null)
                {
                    hdrpShader = Shader.Find(HDRPShaderName);
                }

                return hdrpShader;
            }

            private set
            {
                hdrpShader = value;
            }
        }

        private static Shader hdrpShader = null;

        /// <summary>
        /// Checks if a material is using the Mixed Reality Toolkit/Standard shader.
        /// </summary>
        /// <param name="material">The material to check.</param>
        /// <returns>True if the material is using the Mixed Reality Toolkit/Standard shader</returns>
        public static bool IsUsingHDRPShader(Material material)
        {
            return IsHDRPShader((material != null) ? material.shader : null);
        }

        /// <summary>
        /// Checks if a shader is the Mixed Reality Toolkit/Standard shader.
        /// </summary>
        /// <param name="shader">The shader to check.</param>
        /// <returns>True if the shader is the Mixed Reality Toolkit/Standard shader.</returns>
        public static bool IsHDRPShader(Shader shader)
        {
            return shader == HDRPShader;
        }



    }
}
