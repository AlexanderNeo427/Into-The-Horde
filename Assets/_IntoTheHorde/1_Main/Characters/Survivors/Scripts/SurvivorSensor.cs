using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( SphereCollider ))]
    public class SurvivorSensor : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] SurvivorController _survivorController;

       /* [Header("Customisations")]
        [SerializeField] bool  _visualiseSensor    = false;
        [SerializeField] Color _visualisationColor = Color.green;*/

        SphereCollider m_collider;

        void Awake()
        {
            if (_survivorController == null)
                Debug.LogError("SurvivorSensor.cs : Missing survivor controller reference");

            m_collider = GetComponent<SphereCollider>();
            m_collider.isTrigger = true;
        }

        void OnTriggerEnter(Collider other) => _survivorController.OnSensorEnter( other );

        void OnTriggerStay(Collider other) => _survivorController.OnSensorStay( other );

        void OnTriggerExit(Collider other) => _survivorController.OnSensorExit( other );

        /*void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            if (!_visualiseSensor)      return;

            float parentScaleX = _survivorController.gameObject.transform.localScale.x;
            float parentScaleY = _survivorController.gameObject.transform.localScale.y;
            float parentScaleZ = _survivorController.gameObject.transform.localScale.z;

            float scaleMultiplier = Mathf.Max( parentScaleX, parentScaleY );
            scaleMultiplier = Mathf.Max( scaleMultiplier, parentScaleZ );

            Gizmos.color = _visualisationColor;
            Gizmos.DrawSphere( transform.position, m_collider.radius * scaleMultiplier );
        }*/
    }
}
