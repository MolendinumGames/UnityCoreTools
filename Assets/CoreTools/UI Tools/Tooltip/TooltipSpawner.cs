/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CoreTools.UI
{
    // Currently spawning only for 2D UI elements
    public class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Reusing the tooltip after creation does not mean the text cannot be changed.
        // The information the tooltip displays is updated everytime it is opened.
        [Tooltip("Prefab for tooltips. Will be resused after creation.")]
        [SerializeField]
        private GameObject tooltipPrefab = null;

        public string tooltipHeader = string.Empty;
        public string tooltipBody = string.Empty;

        // Note that the duration of the tooltip spawning animation is handled
        // by the tooltip animation controller.
        // The delay is left public to that system can be implemented in case
        // the player is meant to set the delay time in the settings
        /// <summary>
        /// The timeframe until the tooltip opens.
        /// </summary>
        [Range(0f, 2f)]
        public float delay = .5f;
        private float timeCounter = 0f;

        private bool courserIsHovering = false;
        private bool isOpen = false;

        // References to the created tooltip object
        GameObject tooltipInstance = null;
        TooltipController tooltipControllerInstance = null;

        private enum RelativeScreenPosition { UpLeft, UpRigth, DownLeft, DownRight }

        // public abstract bool CanSpawnToolTip();
        // public abstract void UpdateTooltip(GameObject tipObject);

        #region Unity Event Functions
        private void OnEnable()
        {
            ClearTooltip();
        }

        private void OnDisable()
        {
            ClearTooltip();
        }

        private void Update()
        {
            HandleDelayToOpen();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            courserIsHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }
        #endregion

        private void HandleDelayToOpen()
        {
            if (courserIsHovering && !isOpen)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter >= delay)
                {
                    OpenTooltip();
                }
            }
        }

        private void OpenTooltip()
        {
            if (!tooltipInstance)
            {
                if (tooltipPrefab)
                {
                    tooltipInstance = Instantiate(tooltipPrefab, GetComponentInParent<Canvas>().transform);
                    tooltipControllerInstance = tooltipInstance.GetComponent<TooltipController>();

                    // if the is no tooltip controller then do not display anything to avoid showing an empty UI element
                    if (tooltipControllerInstance == null)
                    {
                        tooltipInstance.SetActive(false);
                        Debug.LogWarning("Trying to show tooltip but the referenced tooltipPrefab had no controller attached. Not showing tooltip instead.");
                        return;
                    }
                }
                else
                {
                    // There is no reference to a current instance of a tooltip gameobject and no prefab is referenced
                    Debug.LogWarning("TooltipSpawner is not able to create a tooltip because the prefab is missing.");
                    return;
                }
            }

            tooltipInstance.SetActive(true);
            isOpen = true;

            UpdateTooltip();
            PositionTooltip(tooltipInstance);
        }


        public void UpdateTooltip()
        {
            // Feed the tooltip the data
            // This will also start any animations of the tooltip
            tooltipControllerInstance.Initialise(
                header: tooltipHeader,
                body: tooltipBody);
        }

        private void ClearTooltip()
        {
            courserIsHovering = false;
            timeCounter = 0f;
            isOpen = false;
            if (tooltipInstance)
            {
                tooltipInstance.SetActive(false);
            }
        }

        private void PositionTooltip(GameObject tooltip)
        {
            if (!tooltip)
                return;

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
