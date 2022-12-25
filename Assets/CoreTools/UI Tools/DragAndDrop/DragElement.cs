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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CoreTools.UI
{
    public abstract class DragElement<T> : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler where T : class
    {
        Canvas parentCanvas = null;
        Vector3 startPos = Vector3.zero;
        Transform originParent = null;
        IDragContainer<T> originContainer;

        private void Awake()
        {
            StoreReferences();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SetupTransformState();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RestoreTransformState();
            if (IsOverUIObject(eventData))
                FindContainerInteraction(eventData);
        }

        private void FindContainerInteraction(PointerEventData eventData)
        {
            IDragContainer<T> destinationContainer = eventData.pointerEnter.GetComponentInParent<IDragContainer<T>>();
            if (originContainer == null || destinationContainer == null) return;

            if (!ReferenceEquals(originContainer, destinationContainer))
            {
                HandleContainerInteraction(originContainer, destinationContainer);
            }
        }

        private void HandleContainerInteraction(IDragContainer<T> originContainer, IDragContainer<T> destinationContainer)
        {
            T originItem = originContainer.GetItem();
            T destinationItem = destinationContainer.GetItem();

            if (originItem == null)
            {
                return;
            }
            else if (destinationItem == null || originItem == destinationItem)
            {
                FillItemsIntoSlot(originContainer, destinationContainer);
            }
            else
            {
                SwapItemsOfSlots(originContainer, destinationContainer);
            }
        }

        private void StoreReferences()
        {
            parentCanvas = GetComponentInParent<Canvas>();
            originContainer = GetComponentInParent<IDragContainer<T>>();
        }

        private void SwapItemsOfSlots(IDragContainer<T> originContainer, IDragContainer<T> destinationContainer)
        {
            T orgiginItem = originContainer.GetItem();
            int originAmount = originContainer.GetAmount();
            originContainer.SetItem(destinationContainer.GetItem(), destinationContainer.GetAmount());
            destinationContainer.SetItem(orgiginItem, originAmount);
        }

        private void FillItemsIntoSlot(IDragContainer<T> originContainer, IDragContainer<T> destinationContainer)
        {
            T originItem = originContainer.GetItem();
            int maxAcceptable = destinationContainer.MaxAcceptable(originItem);
            if (maxAcceptable == 0)
            {
                return;
            }
            else
            {
                int originAmount = originContainer.GetAmount();
                int remainder = destinationContainer.TryAddAmount(originItem, originAmount);
                int movedAmount = originAmount - remainder;
                originContainer.RemoveAmount(movedAmount);
            }
        }

        private void SetupTransformState()
        {
            startPos = transform.position;
            originParent = transform.parent;
            transform.SetParent(parentCanvas.transform, true);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        private void RestoreTransformState()
        {
            transform.SetParent(originParent, true);
            transform.position = startPos;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        private bool IsOverUIObject(PointerEventData eventData)
        {
            return eventData.pointerEnter != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}
