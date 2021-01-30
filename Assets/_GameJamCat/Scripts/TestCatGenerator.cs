using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    public class TestCatGenerator : MonoBehaviour
    {
        [SerializeField] CatGenerator catGen;

        [SerializeField] bool spawn = true;

        // Update is called once per frame
        IEnumerator Start()
        {
            while (true)
            {
                while (spawn)
                {
                    yield return new WaitForSeconds(1f);
                    Vector3 randomPos = Random.insideUnitSphere;
                    randomPos.z = randomPos.y;
                    randomPos = transform.TransformPoint(randomPos);
                    randomPos.y = 0;
                    var cat = catGen.GetRandomCat();
                    cat.transform.position = randomPos;
                    cat.Initialize();
                }
                yield return null;
            }
        }
    }
}
