using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InventorySystem.UI
{
    public class Dragger<T> : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler where T : class
    {
        private Canvas parentCanvas;

        private Vector3 startPos;
        private Transform originParent;

        private IDragSource<T> source;

        private void Awake()
        {
            parentCanvas = GetComponentInParent<Canvas>();
            source = GetComponentInParent<IDragSource<T>>();

        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            startPos = transform.position;
            originParent = transform.parent;
            transform.SetParent(parentCanvas.transform, true);
            GetComponent<CanvasGroup>().blocksRaycasts = false;

        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetDragging();
            if (EventSystem.current.IsPointerOverGameObject()) // Is mouse over UI
            {
                if (eventData.pointerEnter == null) return;

                IDragDestination<T> destination = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();

                var targetContainer = destination as IDragContainer<T>;
                var sourceContainer = source as IDragContainer<T>;
                if (targetContainer != null && sourceContainer != null && !ReferenceEquals(source, destination))
                {
                    AttemptSwap(sourceContainer, targetContainer);
                }
            }
        }
        private void ResetDragging()
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(originParent, true);
            transform.position = startPos;
        }
        private void AttemptSwap(IDragContainer<T> source, IDragContainer<T> destination)
        {

            var originItem = source.GetItem();
            int originAmount = source.GetAmount();
            var targetItem = destination.GetItem();
            int targetAmount = destination.GetAmount();

            if (originItem == null) // Empty slot was dragged
            {
                return;
            }
            else if (targetItem == null) // Switch into new slot
            {
                if (destination.MaxAcceptable(originItem) < 1)
                    return;

                destination.SetItem(originItem, originAmount);
                source.RemoveItems(originAmount);
            }
            else if (originItem == targetItem) // fill target slot
            {
                int acceptedAmount = destination.MaxAcceptable(originItem);
                if (acceptedAmount < 1) // target slot is full
                {
                    return;
                }
                else if (acceptedAmount < originAmount) // target slot doesnt accept enough
                {
                    int remainder = originAmount - acceptedAmount;
                    destination.SetItem(targetItem, acceptedAmount + destination.GetAmount());
                    source.SetItem(originItem, remainder);
                }
                else // fill into target slot completely
                {
                    destination.SetItem(targetItem, targetAmount + originAmount);
                    source.RemoveItems(originAmount);
                }
            }
            else // swap slot items
            {
                // Clear fields to make acceptance check possible
                source.RemoveItems(originAmount);
                destination.RemoveItems(targetAmount);

                if (source.MaxAcceptable(targetItem) < 1
                    || destination.MaxAcceptable(originItem) < 1)
                {
                    source.SetItem(originItem, originAmount);
                    destination.SetItem(targetItem, targetAmount);
                }
                else
                {
                    source.SetItem(targetItem, targetAmount);
                    destination.SetItem(originItem, originAmount);
                }
            }
        }
    }
}
