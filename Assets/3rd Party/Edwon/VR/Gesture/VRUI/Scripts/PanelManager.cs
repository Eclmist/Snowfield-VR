using UnityEngine;
using System.Collections.Generic;

namespace Edwon.VR.Gesture
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelManager : MonoBehaviour
    {
        [HideInInspector]
        public Panel currentPanel;

        public delegate void PanelFocusChanged(Panel panel);
        public static event PanelFocusChanged OnPanelFocusChanged;

        [HideInInspector]
        public List<Panel> panels;

        [HideInInspector]
        public CanvasGroup parentCanvasGroup;

        public void Awake()
        {
            InitPanels();
        }

        public void InitPanels()
        {
            // get the panels below me
            parentCanvasGroup = gameObject.GetComponent<CanvasGroup>();

            panels = new List<Panel>();
            Panel[] panelsTemp = transform.GetComponentsInChildren<Panel>();
            for (int i = 0; i < panelsTemp.Length; i++)
            {
                if (panelsTemp[i] != parentCanvasGroup)
                {
                    if (panelsTemp[i].transform.parent == transform)
                        panels.Add(panelsTemp[i]);
                }
            }
        }

        public void FocusPanel(string panelName)
        {
            if (panels == null || panels.Count == 0 || panels.Contains(null))
            {
                InitPanels();
            }

            // focus panel
            foreach (Panel panel in panels)
            {
                if (panel.gameObject.name == panelName)
                {
                    panel.TogglePanelVisibility(true);
                    currentPanel = panel;

                    // send event
                    if (OnPanelFocusChanged != null)
                    {
                        OnPanelFocusChanged(panel);
                    }
                }
                else
                {
                    panel.TogglePanelVisibility(false);
                }
            }

        }

        public void FocusNoPanels()
        {
            currentPanel = null;

            // hide all panels
            foreach (Panel panel in panels)
            {
                panel.TogglePanelVisibility(false);
            }

            // send event
            if (OnPanelFocusChanged != null)
            {
                OnPanelFocusChanged(null);
            }
        }

    }
}