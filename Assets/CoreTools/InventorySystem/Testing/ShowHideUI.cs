using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] private KeyCode key;
        [SerializeField] private GameObject TargetPanel;
        #region EventFunctions
        private void Awake()
        {

        }
        private void Update()
        {
            //ReadInput();
        }
        #endregion
        private void ReadInput()
        {
            if (Input.GetKeyDown(key))
            {
                OpenCloseUI();
            }
        }
        private void OpenCloseUI()
        {
            if (TargetPanel != null)
                TargetPanel.SetActive(!TargetPanel.activeSelf);
        }
    }
}
