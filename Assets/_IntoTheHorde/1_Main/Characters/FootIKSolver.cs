using UnityEngine;

/*
 *  TODO : This is not quite working yet
 */
namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Animator ))]
    public class FootIKSolver : MonoBehaviour
    {
        [SerializeField] [Range(0f, 10f)]
        float _distanceToGround = 0.25f;

        [SerializeField] LayerMask _floorLayer;

        Animator m_animator;

        void Awake()
        {
            m_animator = GetComponent<Animator>();

            if ( !m_animator.isHuman )
                Debug.LogError("FootIKSolver.cs : animator rig must be of type 'Humanoid'");
        }

        void OnAnimatorIK(int layerIndex)
        {
            /*m_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,  m_animator.GetFloat("IKLeftFootWeight"));
            m_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot,  m_animator.GetFloat("IKLeftFootWeight"));
            m_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, m_animator.GetFloat("IKRightFootWeight"));
            m_animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, m_animator.GetFloat("IKRightFootWeight"));*/

            m_animator.SetIKPositionWeight( AvatarIKGoal.LeftFoot,  0.8f );
            m_animator.SetIKRotationWeight( AvatarIKGoal.LeftFoot,  0.8f );
            m_animator.SetIKPositionWeight( AvatarIKGoal.RightFoot, 0.8f );
            m_animator.SetIKRotationWeight( AvatarIKGoal.RightFoot, 0.8f );

            SolveFootIK( AvatarIKGoal.LeftFoot, _distanceToGround, _floorLayer );
            SolveFootIK( AvatarIKGoal.RightFoot, _distanceToGround, _floorLayer );
        }

        void SolveFootIK( AvatarIKGoal footIK, float distToGround, LayerMask floorLayer )
        {
            Ray ray = new Ray(m_animator.GetIKPosition( footIK ) + Vector3.up, Vector3.down);

            if (Physics.Raycast( ray, out RaycastHit hitInfo, distToGround + 1f, floorLayer))
            {
                Vector3 footPosition = hitInfo.point; 
                footPosition.y += distToGround;   
                m_animator.SetIKPosition( footIK, footPosition );
                m_animator.SetIKRotation( footIK, Quaternion.LookRotation( transform.forward, hitInfo.normal ));
            }
        }
    }
}
