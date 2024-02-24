using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class GuidancePointsManager : MonoBehaviour
    {
        [Header("Place the guidance points in order")]
        [SerializeField] List<GuidancePoint>    _guidancePoints;
        [SerializeField] GuidancePointIndicator _guidancePointIndicator;
        [SerializeField] Camera                 _mainCam;

        int m_index = 0;

        void Start()
        {
            foreach (GuidancePoint point in _guidancePoints)
            {
                point.SetOwner( this );
            }
        }

        void Update()
        {
            Vector3 guidePointPos = _guidancePoints[m_index].transform.position;
            float distanceToPoint = Vector3.Distance( _mainCam.transform.position, guidePointPos );

            Vector3 dir = (guidePointPos - _mainCam.transform.position).normalized;
            float dotProd = Vector3.Dot( _mainCam.transform.forward, dir );

            if (dotProd > -0.25f)
                _guidancePointIndicator.SetInfo( guidePointPos, distanceToPoint );
        }

        public void NextPoint()
        {
            if (m_index + 1 < _guidancePoints.Count)
                ++m_index;
        }
    }
}
