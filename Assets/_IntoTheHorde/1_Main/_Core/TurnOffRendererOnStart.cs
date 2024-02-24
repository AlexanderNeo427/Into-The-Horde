using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class TurnOffRendererOnStart : MonoBehaviour
    {
        void Start() => GetComponent<Renderer>().enabled = false;
    }
}
