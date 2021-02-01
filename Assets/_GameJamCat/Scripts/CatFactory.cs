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
    public class CatFactory : ScriptableObject, ICatFactory
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
        private CatBehaviour _catPrefab;

        [SerializeField]
        private DialogueOptions _dialogueOptions;

        [SerializeField]
        private Texture2D[] _furs;
        [SerializeField, ColorUsage(false)]
        private Color[] _furColors, _feetColors, _earColors;
        [SerializeField, ColorUsage(false, true)]
        private Color[] _eyeColors;

        [SerializeField, Range(0f, 1f), Tooltip("Percent of cats with textures, relative to solid colors")]
        private float _percentTextured = 0.5f;
        [SerializeField, MinMaxSlider(-10.0f, 10.0f)]
        private Vector2 _textureOffsetRange = new Vector2(-5f, 5f);
        [SerializeField, MinMaxSlider(0.0f, 5.0f)]
        private Vector2 _textureScaleRange = new Vector2(1f, 5f);
        [SerializeField, MinMaxSlider(0.0f, 1.0f)]
        private Vector2 _noiseWeightRange = new Vector2(0f, 1f);
        [SerializeField, MinMaxSlider(0.0f, 10f)]
        private Vector2 _noiseScaleRange = new Vector2(0f, 5f);

        [SerializeField]
        private bool _poolObjects = false;
        [System.NonSerialized]
        private List<CatBehaviour> _pool = null;
        private Scene _poolScene;

        public CatBehaviour GetRandomCat()
        {
            var newCat =  _poolObjects ? GetRandomCatPooled() : GetRandomCatUnpooled();
            SetupCatMaterial(newCat.CatRenderer.material);
            bool validCat = _dialogueOptions.GetRandomCat(out CatCustomisation catOptions);
            newCat.CatDialogue = catOptions;
            newCat.IsValidCat = validCat;

            return newCat;
        }

        public void DestroyCat(CatBehaviour cat)
        {
            _dialogueOptions.RecycleCat(cat.CatDialogue);
            if (!_poolObjects)
            {
                Destroy(cat.gameObject);
                return;
            }

            if (_pool == null)
            {
                CreatePool();
            }

            if (cat != null)
            {
                cat.gameObject.SetActive(false);
                _pool.Add(cat);
            }
        }

        private void SetupCatMaterial(Material m)
        {
            if(Random.value <= _percentTextured)
            {
                //Texture the cat
                // Todo: it would be neat to assign some color variance alongside the texture
                m.SetTexture(FurMapID, Utilities.GetRandom(_furs));
                m.SetVector(FurOffsetID, new Vector2(Utilities.GetRandom(_textureOffsetRange), Utilities.GetRandom(_textureOffsetRange)));
                m.SetVector(FurScaleID, new Vector2(Utilities.GetRandom(_textureScaleRange), Utilities.GetRandom(_textureScaleRange)));
                m.SetColor(FurColorID, Color.white);
            }
            else
            {
                //Leave texture null and assign a color
                //m.SetTex
                m.SetColor(FurColorID, Utilities.GetRandom(_furColors));
            }
            m.SetColor(FeetColorID, Utilities.GetRandom(_feetColors));
            m.SetColor(EarColorID, Utilities.GetRandom(_earColors));
            m.SetColor(EyeColorID, Utilities.GetRandom(_eyeColors));
            m.SetFloat(NoiseScaleID, Utilities.GetRandom(_noiseScaleRange));
            m.SetFloat(NoiseWeightID, Utilities.GetRandom(_noiseWeightRange));
        }

        private CatBehaviour GetRandomCatPooled()
        {
            CatBehaviour cat;
            if (_pool == null) CreatePool();
            int lastIndex = _pool.Count - 1;
            if (lastIndex <= 0)
            {
                cat = Instantiate(_catPrefab, Vector3.zero, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                SceneManager.MoveGameObjectToScene(cat.gameObject, _poolScene);
            }
            else
            {
                cat = _pool[lastIndex];
                _pool.RemoveAt(lastIndex);
            }

            return cat;
        }

        private CatBehaviour GetRandomCatUnpooled()
        {
            return Instantiate(_catPrefab);
        }

        private void CreatePool()
        {
            _pool = new List<CatBehaviour>();
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
        }
    }
}
