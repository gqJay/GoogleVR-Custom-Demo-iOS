using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace GQVRDemo
{
    public class ButtonCtrl : MonoBehaviour
    {

        #region fields

        [SerializeField]
        ResourceUtilities.BUTTONTYPE m_ButtonType;

        Button m_Button;

		Tweener moveTweener;

        #endregion

        #region Unity event

        // Use this for initialization
        void Awake()
        {
            m_Button = GetComponent<Button>();

            if (!m_Button)
            {
                Debug.LogError("Current button component is null");
            }
            else
            {
                m_Button.onClick.AddListener(new UnityAction(OnClick));
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region callback functions

        void OnClick()
        {
            Debug.Log(string.Format("{0} is clicked", m_ButtonType));

			VRDemoManipulator manipulator = VRDemoManipulator.GetInstance();
            GameObject player = manipulator.Player;
			GameObject camera = manipulator.Camera;

            if (!player)
            {
                return;
            }

			Vector3 desPos = Vector3.zero;

            switch (m_ButtonType)
            {
                case ResourceUtilities.BUTTONTYPE.FORWARD:	
                    desPos = player.transform.position + camera.transform.forward * manipulator.moveStep;
                    break;
                case ResourceUtilities.BUTTONTYPE.BACKWARD:
                    desPos = player.transform.position + -camera.transform.forward * manipulator.moveStep;
                    break;
                case ResourceUtilities.BUTTONTYPE.LEFT:
                    desPos = player.transform.position + -camera.transform.right * manipulator.moveStep;
                    break;
                case ResourceUtilities.BUTTONTYPE.RIGHT:
                    desPos = player.transform.position + camera.transform.right * manipulator.moveStep;
                    break;
                case ResourceUtilities.BUTTONTYPE.RESET:
                    desPos = manipulator.oriPos;
                    break;
                default:
                    break;
            }

			if(moveTweener != null && moveTweener.IsPlaying())
			{
				moveTweener.Kill();
			}

			//Only move in plane.
			desPos.y = manipulator.oriPos.y;
			moveTweener = player.transform.DOMove(desPos, manipulator.AnimationTime).SetEase(Ease.OutExpo);
        }

        #endregion
    }
}
