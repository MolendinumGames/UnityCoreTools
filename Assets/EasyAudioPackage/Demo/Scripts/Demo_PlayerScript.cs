using UnityEngine;

namespace EasyAudioSystem.Demo
{
    /* Only used for demo purposes and
     * Not needed for the EasyAudio workflow.
     */
    public class Demo_PlayerScript : MonoBehaviour
    {
        public Transform firePoint;
        public GameObject bulletPrefab;

        // Easy Audio reference:
        public EasyAudio gunShot;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject.Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                // Playing sounds with EasyAudio:
                gunShot.Play();
            }
        }
    }
}
