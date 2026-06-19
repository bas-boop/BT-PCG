using UnityEngine;

namespace UI
{
    public sealed class Spin : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject[] yes;

        private bool _toggle;
        
        private void Update()
        {
            Vector3 r = transform.rotation.eulerAngles;
            r.z += speed * Time.deltaTime;
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = r;
            transform.rotation = rotation;
        }

        public void Toggle()
        {
            _toggle = !_toggle;
            
            foreach (GameObject ye in yes)
            {
                ye.SetActive(_toggle);
            }
        }
    }
}