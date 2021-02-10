using UnityEngine;

namespace EasyAudioSystem.Demo
{
    /* Only used for demo purposes and
     * Not needed for the EasyAudio workflow.
     */
    public class Demo_BulletScript : MonoBehaviour
    {
        public EasyAudio explosionSound;
        public AudioSource audioSource;
        private const float speed = 10f;

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        private void OnCollisionEnter(Collision collision)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            explosionSound.Play(audioSource); // Example Implementation
            this.Invoke("Vanish", 2f);
        }
        private void Vanish() => Destroy(gameObject);
    }
}
