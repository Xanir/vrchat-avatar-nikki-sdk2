
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Moshiro.VRCThumbnailSelector
{

   
    public class VRCCamThmbnailOverlay : MonoBehaviour
    {

        
        public Texture2D texture;


      
        private Material material;

        
        void Start()
        {

        }

       
        void Update()
        {

        }


        
        public void SetTextuer(Texture2D texture)
        {
            if (null == texture)
            {
                Debug.Log("no texture");
                return;
            }
            this.texture = texture;

            
            if (null == material)
            {

                
                Shader overlayShader = Shader.Find("Moshiro/VRCCam");
                if (null == overlayShader)
                {
                    Debug.Log("VRCCamOverlayShader Not Found");
                    return;
                }

                
                material = new Material(overlayShader);
                if (null == material)
                {
                    Debug.Log("Material Create Faild");
                    return;
                }
            }

            
            material.SetTexture("_Overlay", texture);
        }

        
        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
           
            if (null == material)
            {

               
                return;
            }

           
            material.SetVector("_UV_Transform", new Vector4(1, 0, 0, 1));

          
            material.SetTexture("_Overlay", texture);

            
            Graphics.Blit(src, dest, material, 0);
        }
    }
}


