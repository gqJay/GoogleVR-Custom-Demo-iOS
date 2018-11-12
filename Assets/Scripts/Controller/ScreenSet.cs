using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GQVRDemo
{
    public class ScreenSet : MonoBehaviour
    {

		#region fields
        float m_Timer;

		const int TIMETHRESHOLD = 8;
		#endregion

        #region Unity Event
        // Use this for initialization
        void Start()
        {
            //SetCamerasCullingMask();		
        }

        // Update is called once per frame
        void Update()
        {
//            if (m_Timer >= TIMETHRESHOLD)
//            {
//                Camera[] cameras = FindObjectsOfType<Camera>();
//
//                for (int i = 0; i < cameras.Length; i++)
//                {
//                    Debug.LogError("gq" + cameras[i]);
//
//					Debug.LogError("gq" + cameras[i].cullingMask);
//                }
//
//				m_Timer = 0;
//            }
        }
        #endregion

		#region custom functions
        void SetCamerasCullingMask()
        {
            VRDemoManipulator manipulator = VRDemoManipulator.GetInstance();
            if (manipulator)
            {
                if (manipulator.Camera)
                {
                    Camera[] cameras = manipulator.Camera.GetComponentsInChildren<Camera>();
                    for (int i = 0; i < cameras.Length; i++)
                    {
                        if (i == 0)
                        {
                            cameras[i].cullingMask = ~(1 << LayerMask.NameToLayer("Left"));
                        }
                        else
                        {
                            cameras[i].cullingMask = ~(1 << LayerMask.NameToLayer("Right"));
                        }
                    }
                }
            }
        }
		#endregion
    }
}
