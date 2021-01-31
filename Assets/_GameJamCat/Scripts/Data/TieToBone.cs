using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    /// <summary>
    /// Sets the transform to the specified cat bone. This is just to make the cat hierarchy less cluttered
    /// </summary>
    public class TieToBone : MonoBehaviour
    {
        [SerializeField] Transform bone;
        // Start is called before the first frame update
        void Start()
        {
            this.transform.parent = bone;
        }

        
    }
}
