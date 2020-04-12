
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moshiro.VRCThumbnailSelector
{
   
    public class VRCThumbnailSelector : MonoBehaviour {

     
        private bool bAddScript = false;

       
        public Texture2D texture;

       
        void Start () {
        }
	
	    
	    void Update () {

            
            if (false == bAddScript) {

              
                GameObject obj = GameObject.Find("VRCCam");
                if (null != obj)
                {
                    
                    bAddScript = true;

                    
                    obj.AddComponent<VRCCamThmbnailOverlay>();
                    VRCCamThmbnailOverlay script = obj.GetComponent<VRCCamThmbnailOverlay>();
                    if (null == script) {
                        Debug.Log("VRCCamOverlay Script not Found");
                        return;
                    }
                    
                    script.enabled = false;

                    
                    script.SetTextuer(texture);
                }
            }
        }
    }
}
