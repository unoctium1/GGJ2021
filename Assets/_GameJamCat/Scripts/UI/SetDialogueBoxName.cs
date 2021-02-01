using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameJamCat
{
    public class SetDialogueBoxName : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _catNameTextMeshPro;

        public void SetCatNameInDialogueBox(string catName)
        {
            _catNameTextMeshPro.text = catName;
        }

        public void ResetCatDialogueBox()
        {
            _catNameTextMeshPro.text = "?????";
        }

    }
}
