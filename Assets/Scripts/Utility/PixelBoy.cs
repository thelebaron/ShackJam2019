//PIXELBOY BY @WTFMIG EAT A BUTT WORLD BAHAHAHAHA POOP MY PANTS
// - edited by @Nothke to use screen height for #LOWREZJAM

using UnityEngine;

namespace Utility
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/PixelBoy")]
    public class PixelBoy : MonoBehaviour
    {
        public int h = 64;
        private int _w;
        protected void Start()
        {
            /*if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }*/
        }

        private void Update()
        {
            if (Camera.main == null) 
                return;
        
            var ratio = ((float)Camera.main.pixelWidth) / (float)Camera.main.pixelHeight;
            _w = Mathf.RoundToInt(h * ratio);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            source.filterMode = FilterMode.Point;
            var buffer = RenderTexture.GetTemporary(_w, h, -1);
            buffer.filterMode = FilterMode.Point;
            Graphics.Blit(source, buffer);
            Graphics.Blit(buffer, destination);
            RenderTexture.ReleaseTemporary(buffer);
        }
    }
}