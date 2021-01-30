using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace GameJamCat
{

    /// <summary>
    /// Scriptable Object for 
    /// </summary>
    [CreateAssetMenu]
    public class CatGenerator : ScriptableObject, ICatFactory
    {

        #region SHADER_PROPERTY_IDS
        private readonly int FurMapID = Shader.PropertyToID("_BaseMap");
        private readonly int FurColorID = Shader.PropertyToID("_MainColor");
        private readonly int EyeColorID = Shader.PropertyToID("_EyeColor");
        private readonly int EarColorID = Shader.PropertyToID("_EarColor");
        private readonly int FeetColorID = Shader.PropertyToID("_FeetColor");
        private readonly int FurOffsetID = Shader.PropertyToID("_FurOffset");
        private readonly int FurScaleID = Shader.PropertyToID("_FurScale");
        private readonly int NoiseWeightID = Shader.PropertyToID("_NoiseWeight");
        private readonly int NoiseScaleID = Shader.PropertyToID("_NoiseScale");
        #endregion


        [SerializeField]
        CatBehaviour _catPrefab;
        [SerializeField]
        Texture2D[] _furs;
        [SerializeField, ColorUsage(false)]
        Color[] _furColors, _feetColors, _earColors;
        [SerializeField, ColorUsage(false, true)]
        Color[] _eyeColors;

        [SerializeField, Range(0f, 1f), Tooltip("Percent of cats with textures, relative to solid colors")]
        float _percentTextured = 0.5f;
        [SerializeField, MinMaxSlider(-10.0f, 10.0f)]
        Vector2 _textureOffsetRange = new Vector2(-5f, 5f);
        [SerializeField, MinMaxSlider(0.0f, 5.0f)]
        Vector2 _textureScaleRange = new Vector2(1f, 5f);
        [SerializeField, MinMaxSlider(0.0f, 1.0f)]
        Vector2 _noiseWeightRange = new Vector2(0f, 1f);
        [SerializeField, MinMaxSlider(0.0f, 10f)]
        Vector2 _noiseScaleRange = new Vector2(0f, 5f);

        [SerializeField]
        bool _poolObjects = false;
        [System.NonSerialized]
        List<CatBehaviour> _pool = null;
        Scene _poolScene;

        public CatBehaviour GetRandomCat()
        {
            var newCat =  _poolObjects ? GetRandomCat_pooled() : GetRandomCat_unpooled();
            SetupCatMaterial(newCat.CatRenderer.material);
            
            return newCat;
        }

        public void Reclaim(CatBehaviour cat)
        {
            if (!_poolObjects)
            {
                Destroy(cat.gameObject);
                return;
            }
            if (_pool == null) CreatePool();
            cat.gameObject.SetActive(false);
            _pool.Add(cat);
        }

        private void SetupCatMaterial(Material m)
        {
            if(Random.value <= _percentTextured)
            {
                //Texture the cat
                // Todo: it would be neat to assign some color variance alongside the texture
                m.SetTexture(FurMapID, GetRandom(_furs));
                m.SetVector(FurOffsetID, new Vector2(GetRandom(_textureOffsetRange), GetRandom(_textureOffsetRange)));
                m.SetVector(FurScaleID, new Vector2(GetRandom(_textureScaleRange), GetRandom(_textureScaleRange)));
                m.SetColor(FurColorID, Color.white);
            }
            else
            {
                //Leave texture null and assign a color
                //m.SetTex
                m.SetColor(FurColorID, GetRandom(_furColors));
            }
            m.SetColor(FeetColorID, GetRandom(_feetColors));
            m.SetColor(EarColorID, GetRandom(_earColors));
            m.SetColor(EyeColorID, GetRandom(_eyeColors));
            m.SetFloat(NoiseScaleID, GetRandom(_noiseScaleRange));
            m.SetFloat(NoiseWeightID, GetRandom(_noiseWeightRange));
        }

        private CatBehaviour GetRandomCat_pooled()
        {
            CatBehaviour cat;
            if (_pool == null) CreatePool();
            int lastIndex = _pool.Count - 1;
            if (lastIndex <= 0)
            {
                cat = Instantiate(_catPrefab);
                cat.Factory = this;
                SceneManager.MoveGameObjectToScene(cat.gameObject, _poolScene);
            }
            else
            {
                cat = _pool[lastIndex];
                _pool.RemoveAt(lastIndex);
            }

            return cat;
        }

        private CatBehaviour GetRandomCat_unpooled()
        {
            return Instantiate(_catPrefab);
        }

        private void CreatePool()
        {
            _pool = new List<CatBehaviour>();
            Debug.Log("Got here");
#if UNITY_EDITOR //This just prevents errors if we recompile while in play mode
            _poolScene = SceneManager.GetSceneByName(name);
            if (_poolScene.isLoaded)
            {
                var objects = _poolScene.GetRootGameObjects();
                foreach(var go in objects)
                {
                    if (!go.activeSelf)
                    {
                        var cat = go.GetComponent<CatBehaviour>();
                        _pool.Add(cat);
                    }
                    
                }
                return;
            }
#endif
            _poolScene = SceneManager.CreateScene(name);
            Debug.Log("And here");
        }

        private T GetRandom<T>(T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        private float GetRandom(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }
    }
}
