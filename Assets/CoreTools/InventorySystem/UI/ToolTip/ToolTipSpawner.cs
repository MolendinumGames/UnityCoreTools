using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CoreTools.InventorySystem
{
	public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
        [Tooltip("Generic UIPrefab for tooltips. Will be resused after creation.")]
        [SerializeField]
        GameObject tooltipPrefab = null;

        // Child ref
        GameObject tooltip = null;

        private enum RelativeScreenPosition { UpLeft, UpRigth, DownLeft, DownRight }

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

        public abstract bool CanSpawnToolTip();
        public abstract void UpdateTooltip(GameObject tipObject);

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
            Vector3 pos = transform.position;
            float halfWidth = Screen.width * .5f;
            float halfHeight = Screen.height * .5f;
            RelativeScreenPosition relativePos = 
                (pos.x > halfWidth, pos.y > halfHeight) switch
            {
                (true,  true)  => RelativeScreenPosition.UpRigth,
                (true,  false) => RelativeScreenPosition.DownRight,
                (false, true)  => RelativeScreenPosition.UpLeft,
                (false, false) => RelativeScreenPosition.DownLeft
            };

            // Set position of tooltip
            tooltip.transform.position = relativePos switch
            {
                RelativeScreenPosition.UpLeft    => tooltip.transform.position + (bottomLeftSlot - topLeftTooltip),
                RelativeScreenPosition.UpRigth   => tooltip.transform.position + (bottomRightSlot - topRightTooltip),
                RelativeScreenPosition.DownLeft  => tooltip.transform.position + (topLeftSlot - bottomLeftTooltip),
                RelativeScreenPosition.DownRight => tooltip.transform.position + (topRightSlot - bottomRightTooltip),
                _ => throw new NotImplementedException("Tooltip couldn't determine relative screen position.")
            };
        }
    }	
}
