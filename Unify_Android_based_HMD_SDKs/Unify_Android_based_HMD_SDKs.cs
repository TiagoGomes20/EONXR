using UnityEngine;

public class HMDManager : MonoBehaviour
{
    
    public enum HMDType
    {
        Oculus,
        Vive,
        GoogleVR,
       
    }

   
    private IHMDSDK hmdSDK;

   
    public void InitializeHMD(HMDType hmdType)
    {
        switch (hmdType)
        {
            case HMDType.Oculus:
                hmdSDK = new OculusSDK();
                break;
            case HMDType.Vive:
                hmdSDK = new ViveSDK();
                break;
            case HMDType.GoogleVR:
                hmdSDK = new GoogleVRSDK();
                break;
           
        }

        
        if (hmdSDK != null)
        {
            hmdSDK.Initialize();
        }
        else
        {
            Debug.LogError("Unsupported HMD type: " + hmdType);
        }
    }

    
    public Quaternion GetHMDRotation()
    {
        if (hmdSDK != null)
        {
            return hmdSDK.GetRotation();
        }
        else
        {
            Debug.LogError("HMD not initialized.");
            return Quaternion.identity;
        }
    }

    
    private interface IHMDSDK
    {
        void Initialize();
        Quaternion GetRotation();
        
    }

   
    private class OculusSDK : IHMDSDK
    {
        public void Initialize()
        {
          
            if (!OVRManager.isHMDPresent)
            {
                Debug.LogError("Oculus Rift headset is not connected.");
                return;
            }

           
            OVRManager.Initialize();
        }

        public Quaternion GetRotation()
        {
          
            return OVRManager.tracker.GetPose().orientation;
        }
    }

    
    private class ViveSDK : IHMDSDK
    {
        public void Initialize()
        {
            
            SteamVR.Initialize();
        }

        public Quaternion GetRotation()
        {
            
            return SteamVR_Input._default.trackingSpace.localRotation;
        }
    }

    
    private class GoogleVRSDK : IHMDSDK
    {
        public void Initialize()
        {
            
            GvrCardboardHelpers.Recenter();
        }

        public Quaternion GetRotation()
        {
           
            return GvrController.Orientation;
        }
    }
}
