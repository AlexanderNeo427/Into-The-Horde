using UnityEngine;

namespace IntoTheHorde
{
    public class testScript : MonoBehaviour
    {
        float timer = 0f;

        void Update()
        {
            timer += Time.deltaTime;

            if (timer > 5f)
                Destroy( gameObject );
        }
    }
}


