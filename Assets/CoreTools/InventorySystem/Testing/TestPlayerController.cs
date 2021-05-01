using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewNameSpace
{
	public class TestPlayerController : MonoBehaviour
	{
		public float speed = 1f;
	#region EventFunctions
		private void Awake()
		{
			
		}
        private void Update()
        {
			//float horInput = Input.GetAxisRaw("Horizontal");
			//float vertINput = Input.GetAxisRaw("Vertical");
			//transform.Translate(new Vector3(horInput, 0f, vertINput) * speed * Time.deltaTime);
        }
        #endregion
    }	
}
