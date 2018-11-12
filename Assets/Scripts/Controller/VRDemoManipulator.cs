using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GQVRDemo
{
    public class VRDemoManipulator : MonoBehaviour
    {

        #region fields

        #region items
        [Tooltip("Player gameobject move item.")]
        [SerializeField]
        GameObject m_Player;
        public GameObject Player
        {
            get
            {
                if (!m_Player)
                {
                    Debug.LogError("VRDemoManipulator player is null.");
                }

                return m_Player;
            }
        }

        [Tooltip("Camera gameobject")]
        [SerializeField]
        GameObject m_Camera;
        public GameObject Camera
        {
            get
            {
                if (!m_Camera)
                {
                    Debug.LogError("VRDemoManipulator camera is null.");
                }

				return m_Camera;
            }
        }

        [Tooltip("ControllerPanel gameobject")]
        [SerializeField]
        Canvas m_CtrlPanel;

        CanvasGroup m_CanvasGroup;

        #endregion

        #region configs

        [Tooltip("The threshold of the angle of the show controller panel.")]
        [SerializeField]
        float m_ShowAngle;

        [Tooltip("Show and hide time.")]
        [SerializeField]
        float m_AnimationTime;
		public float AnimationTime
		{
			get{
				return m_AnimationTime;
			}
		}

        [Tooltip("The step of the player moving.")]
        public float moveStep;
        #endregion

        #region states

        ResourceUtilities.PANELSTATE m_CurPanelState = ResourceUtilities.PANELSTATE.ISHIDDEN;

        Tweener m_CurAnimation;
        #endregion

        #region value

        [HideInInspector]
        public Vector3 oriPos;

        #endregion
        static VRDemoManipulator s_Instance;

        #endregion

        #region Unity event

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (m_CtrlPanel)
            {
                m_CtrlPanel.enabled = false;

                m_CanvasGroup = m_CtrlPanel.GetComponent<CanvasGroup>();
                if (m_CanvasGroup)
                {
                    m_CanvasGroup.alpha = 0;
                }
                else
                {
                    Debug.LogError("VRDemoManipulator Awake canvasGroup is null.");
                }
            }

            if (m_Player)
            {
                oriPos = m_Player.transform.position;
            }

            s_Instance = this;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            ListenCtrlPanelState();
        }

        #endregion

        #region custom function

        public static VRDemoManipulator GetInstance()
        {
            if (!s_Instance)
            {
                Debug.LogError("VRDemoManipulator instane is null.");
            }

            return s_Instance;
        }

        void ListenCtrlPanelState()
        {
            if (!m_CtrlPanel)
            {
                Debug.LogError("VRDemoManipulator ShowCtrlPanel controller Panel is null.");
            }
            else if (!m_Player)
            {
                Debug.LogError("VRDemoManipulator ShowCtrlPanel player is null.");
            }
            else if (!m_Camera)
            {
                Debug.LogError("VRDemoManipulator ShowCtrlPanel camera is null.");
            }

            switch (m_CurPanelState)
            {
                case ResourceUtilities.PANELSTATE.NEEDSHOW:
                    ShowCtrlPanel();
                    break;
                case ResourceUtilities.PANELSTATE.SHOWING:
                case ResourceUtilities.PANELSTATE.ISSHOWN:
                    if (m_Camera.transform.localEulerAngles.x < m_ShowAngle)
                    {
                        m_CurPanelState = ResourceUtilities.PANELSTATE.NEEDHIDE;
                    }
                    break;
                case ResourceUtilities.PANELSTATE.NEEDHIDE:
                    HideCtrlPanel();
                    break;
                case ResourceUtilities.PANELSTATE.HIDING:
                case ResourceUtilities.PANELSTATE.ISHIDDEN:
                    if (m_Camera.transform.localEulerAngles.x >= m_ShowAngle &&
                    m_Camera.transform.localEulerAngles.x < 180)
                    {
                        m_CurPanelState = ResourceUtilities.PANELSTATE.NEEDSHOW;
                    }
                    break;
                default:
                    break;
            }
        }

        void ShowCtrlPanel()
        {
            if (m_CurAnimation != null && m_CurAnimation.IsPlaying())
            {
                m_CurAnimation.Kill();
            }

            m_CurAnimation = DOTween.To(() => m_CanvasGroup.alpha,
             x => m_CanvasGroup.alpha = x, 1, m_AnimationTime).OnStart(
                 () =>
                 {
                     m_CtrlPanel.enabled = true;

                     //Update the position of the controller panel at the begining.
                     float distance = Vector3.Distance(m_CtrlPanel.transform.localPosition,
                     m_Camera.transform.localPosition);
                     m_CtrlPanel.transform.localPosition = m_Camera.transform.localPosition +
                     m_Camera.transform.forward * distance;
                     m_CtrlPanel.transform.LookAt(m_Camera.transform);
                     m_CtrlPanel.transform.Rotate(new Vector3(0, 180, 0), Space.Self);

                     m_CurPanelState = ResourceUtilities.PANELSTATE.SHOWING;
                 }
             ).OnComplete(() =>
             {
                 m_CurPanelState = ResourceUtilities.PANELSTATE.ISSHOWN;
             }).SetEase(Ease.InExpo);
        }

        void HideCtrlPanel()
        {
            if (m_CurAnimation != null && m_CurAnimation.IsPlaying())
            {
                m_CurAnimation.Kill();
            }

            m_CurAnimation = DOTween.To(() => m_CanvasGroup.alpha,
            x => m_CanvasGroup.alpha = x, 0, m_AnimationTime).OnStart(
                () =>
                {
                    m_CurPanelState = ResourceUtilities.PANELSTATE.HIDING;
                }
            ).OnComplete(() =>
            {
                m_CtrlPanel.enabled = false;
                m_CurPanelState = ResourceUtilities.PANELSTATE.ISHIDDEN;
            }).SetEase(Ease.OutExpo);
        }
        #endregion
    }
}
