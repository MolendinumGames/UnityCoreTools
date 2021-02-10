using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
	public class Pickup : MonoBehaviour
	{
		public InventoryItem item;
		public int amount;
        [SerializeField] Mesh standardMesh;
        [SerializeField] Material standardMat;
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;

        public void SetUp(InventoryItem item, int amount)
        {
            this.item = item;
            this.amount = amount;
            ExistenceCheck();
            SetVisual();
        }
        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
        }
        private void Start()
        {
            ExistenceCheck();

            SetUp(item, amount);

        }
        private int AddItemsToInventories(List<Inventory> inv, int toAdd)
        {
            int remainder = toAdd;
            for (int i = 0; i < inv.Count; i++)
            {
                if (inv[i].TryAddItem(item, remainder, out remainder))
                    break;
            }
            return remainder;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                AddItemToTarget(other.gameObject);
        }
        private bool HasItem() => item != null && amount > 0;
        private void ExistenceCheck()
        {
            if (!HasItem())
                gameObject.SetActive(false);
        }

        private void AddItemToTarget(GameObject target)
        {
            Inventory[] inventoriesToCheck = target.GetComponents<Inventory>();
            if (inventoriesToCheck.Length == 0) return;
            List<Inventory> matchingInventories = new List<Inventory>();
            List<Inventory> unrestrictedInventories = new List<Inventory>();

            for (int i = 0; i < inventoriesToCheck.Length; i++)
            {
                if (inventoriesToCheck[i].HasRestriction())
                {
                    if (inventoriesToCheck[i].ItemIsCorrectType(item))
                        matchingInventories.Add(inventoriesToCheck[i]);
                }
                else unrestrictedInventories.Add(inventoriesToCheck[i]);
            }
            int remainder = amount;
            if (remainder > 0 && matchingInventories.Count > 0)
                remainder = AddItemsToInventories(matchingInventories, remainder);
            if (remainder > 0 && unrestrictedInventories.Count > 0)
                remainder = AddItemsToInventories(unrestrictedInventories, remainder);
            amount = remainder;
            ExistenceCheck();
        }
        private void SetVisual()
        {
            if (item == null || item.GetMesh() == null || item.GetMaterial() == null)
            {
                meshFilter.sharedMesh = standardMesh;
                meshRenderer.sharedMaterial = standardMat;
            }
            else
            {
                meshFilter.sharedMesh = item.GetMesh();
                meshRenderer.sharedMaterial = item.GetMaterial();
            }
        }
	}	
}
