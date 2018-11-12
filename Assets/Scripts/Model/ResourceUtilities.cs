using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GQVRDemo
{
    public class ResourceUtilities : MonoBehaviour
    {

        #region enum

        public enum BUTTONTYPE
        {
            FORWARD,
            BACKWARD,
            LEFT,
            RIGHT,
            RESET
        }

		public enum PANELSTATE
		{
			NEEDSHOW,
			SHOWING,
			ISSHOWN,
			NEEDHIDE,
			HIDING,
			ISHIDDEN
		}
        #endregion

        #region fields

        #endregion
    }
}
