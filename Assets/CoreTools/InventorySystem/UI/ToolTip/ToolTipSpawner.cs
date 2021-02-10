using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventorySystem
{
	public abstract class ToolTipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
        [Tooltip("Generic UIPrefab for tooltips. Will be resused after creation.")]
        [SerializeField] GameObject tooltipPrefab = null;

        // Child ref
        GameObject tooltip = null;

        private enum RelativeScreenPosition { UpLeft, UpRigth, DownLeft, DownRight }

        #region EventFunctions
        private void OnEnable() => ClearTooltip();

        private void OnDisable() => ClearTooltip();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanSpawnToolTip()) // Not allowed to show
            {
                ClearTooltip();
                return;
            }

            if (!tooltip) // No Child
            {
                if (tooltipPrefab)
                    tooltip = Instantiate(tooltipPrefab, GetComponentInParent<Canvas>().transform);
                else return;
            }
            tooltip.SetActive(true);
            UpdateTooltip(tooltip);
            PositionTooltip(tooltip);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }
        #endregion

        // PUBLIC ABSTRACT METHODS
        public abstract bool CanSpawnToolTip();
        public abstract void UpdateTooltip(GameObject tip);

        // PRIVATE METHODS
        private void ClearTooltip()
        {
            if (tooltip)
                Destroy(tooltip);
        }
        private void PositionTooltip(GameObject tooltip)
        {
            if (!tooltip) return;

            Canvas.ForceUpdateCanvases();

            // Find cornerpoints of this (parent) RectTransfrom
            Vector3[] slotCorners = new Vector3[4];
            ((RectTransform)transform).GetWorldCorners(slotCorners);
            Vector3 topLeftSlot     = slotCorners[1];
            Vector3 topRightSlot    = slotCorners[2];
            Vector3 bottomLeftSlot  = slotCorners[0];
            Vector3 bottomRightSlot = slotCorners[3];

            // Find croners of tooltip
            Vector3[] tooltipCorners = new Vector3[4];
            ((RectTransform)tooltip.transform).GetWorldCorners(tooltipCorners);
            Vector3 bottomLeftTooltip  = tooltipCorners[0];
            Vector3 topLeftTooltip     = tooltipCorners[1];
            Vector3 topRightTooltip    = tooltipCorners[2];
            Vector3 bottomRightTooltip = tooltipCorners[3];

            // Find position relative to screen
            RelativeScreenPosition relativePos;
            if (transform.position.x > Screen.width / 2f)
            {
                if (transform.position.y > Screen.height / 2f)
                    relativePos = RelativeScreenPosition.UpRigth;
                else
                    relativePos = RelativeScreenPosition.DownRight;
            }
            else
            {
                if (transform.position.y > Screen.height / 2f)
                    relativePos = RelativeScreenPosition.UpLeft;
                else
                    relativePos = RelativeScreenPosition.DownLeft;
            }

            // Set position of tooltip
            switch (relativePos)
            {
                case RelativeScreenPosition.UpLeft:
                    tooltip.transform.position = tooltip.transform.position + (bottomLeftSlot  - topLeftTooltip);
                    break;
                case RelativeScreenPosition.UpRigth:
                    tooltip.transform.position = tooltip.transform.position + (bottomRightSlot - topRightTooltip);
                    break;
                case RelativeScreenPosition.DownLeft:
                    tooltip.transform.position = tooltip.transform.position + (topLeftSlot     - bottomLeftTooltip);
                    break;
                case RelativeScreenPosition.DownRight:
                    tooltip.transform.position = tooltip.transform.position + (topRightSlot    - bottomRightTooltip);
                    break;
                default:
                    Debug.LogError("Tooltip couldn't determine relative screen position.");
                    tooltip.transform.position = transform.position;
                    break;
            }
        }
    }	
}
