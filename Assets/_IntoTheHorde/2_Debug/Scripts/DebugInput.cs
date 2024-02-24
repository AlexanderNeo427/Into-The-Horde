using UnityEngine;

namespace IntoThesdHorde
{
    public class DebugInput : MonoBehaviour
    {
        [SerializeField] Transform _transform;

        void Update()
        {
            if (Input.GetKeyDown( KeyCode.N ))
            {
                GetComponent<CharacterController>().enabled = false;
                transform.position = _transform.position;
                GetComponent<CharacterController>().enabled = true;
            }
        }
    }
}
