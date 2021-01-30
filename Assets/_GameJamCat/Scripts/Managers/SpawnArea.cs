using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameJamCat
{
    public class SpawnArea : MonoBehaviour
    {
        
        [Title("Properties")]
        [SerializeField] private float _spawnAreaRadius = 5;
        
        /// <summary>
        /// Sphere radius of spawn area
        /// </summary>
        public float SpawnAreaRadius => _spawnAreaRadius;

        public Vector3 GetRandomUnitWithSphere()
        {
            var randomPosition = Random.insideUnitSphere * _spawnAreaRadius;
            randomPosition.z = randomPosition.y;
            randomPosition = transform.TransformPoint(randomPosition);
            randomPosition.y = 0;
            return randomPosition;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.4f);
            Gizmos.DrawSphere(transform.position, _spawnAreaRadius);
        }
    }
}
