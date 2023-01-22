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
using UnityEngine.UI;

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

        // Tooltip Date
        public string tooltipHeader = string.Empty;
        [TextArea(1, 10)]
        public string tooltipBody = string.Empty;

        // Left public so that a system can be implemented in case
        // the player is meant to set the delay time in the settings
        /// <summary>
        /// The timeframe until the tooltip opens. Does not affect animations.
        /// </summary>
        [Range(0f, 2f)]
        public float delay = .5f;
        private float timeCounter = 0f;

        private bool courserIsHovering = false;
        private bool isOpen = false;

        // References to the created tooltip object
        GameObject tooltipInstance = null;
        TooltipController tooltipControllerInstance = null;

        private enum ReparentingOption { Spawner, Canvas, Other }
        [Space(10)]
        [Header("Reparenting")]
        [SerializeField]
        private ReparentingOption reparentingOption = ReparentingOption.Spawner;
        [Tooltip("Will only be parented to NewParent if option 'Other' is selected as ReparentingOption")]
        [SerializeField]
        private GameObject newParent = null;

        private enum RelativeScreenPosition { UpLeft, UpRigth, DownLeft, DownRight }

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
                    // Create the tooltip gameobject and set it up correctly
                    tooltipInstance = Instantiate(tooltipPrefab, GetComponentInParent<Canvas>().transform);
                    DeactivateAllRaycastTargets(tooltipInstance);
                    HandleParenting(tooltipInstance);

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

        private void HandleParenting(GameObject tooltipInstance)
        {
            tooltipInstance.transform.SetParent(reparentingOption switch
            {
                ReparentingOption.Spawner => this.gameObject.transform,
                ReparentingOption.Canvas => this.gameObject.GetComponentInParent<Canvas>().transform,
                ReparentingOption.Other => newParent ? newParent.transform : this.gameObject.GetComponentInParent<Canvas>().transform,
                _ => throw new NotImplementedException("Unkown case of ReparentingOption.")
            });
        }

        private void DeactivateAllRaycastTargets(GameObject tooltipInstance)
        {
            foreach (Graphic target in tooltipInstance.transform.GetComponentsInChildren<Graphic>())
            {
                target.raycastTarget = false;
            }
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

            RectTransform parentRectTransform = (RectTransform)transform;
            RectTransform tooltipRectTransform = (RectTransform)tooltip.transform;
            Rect parentRect = parentRectTransform.rect;
            Rect tooltipRect = tooltipRectTransform.rect;

            // Find position relative to screen of the spawner
            RelativeScreenPosition relativePos = GetRelativeScreenPosition(parentRectTransform.anchoredPosition);

            // Set position of tooltip
            Vector2 offSetMovement = GetOffsetMovement(parentRect, tooltipRect, relativePos);
            Vector2 relativePositioningToParent = GetRelativePositionToParent(); 
            Vector2 newTooltipPosition = relativePositioningToParent + offSetMovement;

            tooltipRectTransform.anchoredPosition = newTooltipPosition;
        }

        
        RelativeScreenPosition GetRelativeScreenPosition(Vector3 position)
        {
            float halfWidth  = Screen.width  * .5f;
            float halfHeight = Screen.height * .5f;

            return (position.x > halfWidth, position.y > halfHeight) switch
            {
                (  true,  true) => RelativeScreenPosition.UpRigth,
                (  true, false) => RelativeScreenPosition.DownRight,
                ( false,  true) => RelativeScreenPosition.UpLeft,
                ( false, false) => RelativeScreenPosition.DownLeft
            };
        }

        Vector2 GetOffsetMovement(Rect spawnerRect, Rect tooltipRect, RelativeScreenPosition relativePos)
        {
            float combinedHalfWidth = 0.5f * (spawnerRect.width + tooltipRect.width);
            Vector2 leftMovement    = Vector2.left  * combinedHalfWidth;
            Vector2 rightMovement   = Vector2.right * combinedHalfWidth;
            Vector2 upMovement      = Vector2.up    * 0.5f * (tooltipRect.height - spawnerRect.height);
            Vector2 downMovement    = Vector2.down  * 0.5f * (tooltipRect.height - spawnerRect.height);

            Vector2 offsetMovement = relativePos switch
            {
                RelativeScreenPosition.UpLeft    => rightMovement + downMovement,
                RelativeScreenPosition.UpRigth   => leftMovement  + downMovement,
                RelativeScreenPosition.DownLeft  => rightMovement + upMovement,
                RelativeScreenPosition.DownRight => leftMovement  + upMovement,
                _ => throw new NotImplementedException("Tooltip couldn't determine relative screen position.")
            };

            return offsetMovement;
        }

        Vector2 GetRelativePositionToParent()
        {
            Vector2 relativePos = reparentingOption switch
            {
                ReparentingOption.Spawner => Vector2.zero,
                ReparentingOption.Canvas  => throw new NotImplementedException(),
                ReparentingOption.Other   => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };

            return relativePos;
        }
    }	
}
