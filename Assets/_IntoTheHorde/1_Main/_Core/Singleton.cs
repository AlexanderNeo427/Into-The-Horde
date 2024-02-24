using UnityEngine;

namespace IntoTheHorde
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T m_instance = null;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = GameObject.FindObjectOfType<T>();

                    if (m_instance == null)
                        m_instance = new GameObject("Instance of " + typeof( T )).AddComponent<T>();

                    // Uncomment if you want persistant singleton
                    // DontDestroyOnLoad( m_instance );
                }

                return m_instance;
            }
        }

        public static bool HasValidInstance() => m_instance != null;

/*        protected void Awake()
        {
            if (m_instance != null)
                Destroy( this.gameObject );
        }*/
    }
}
