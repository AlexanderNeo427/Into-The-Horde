using UnityEngine;

namespace IntoTheHorde
{
    [System.Serializable]
    public class TransformInfo 
    {
        public Vector3 Position = Vector3.zero;
        public Vector3 Rotation = Vector3.zero;
        public Vector3 Scale    = new Vector3( 1f, 1f, 1f );
    }
}
