using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamCat
{
    public class DialogueBoxViewBehaviour : ViewBehaviour
    {
        [SerializeField] private DialogueBoxMenuView _dialogueBoxMenuView = null;

        public override void Initialize()
        {
            _menuView = _dialogueBoxMenuView;
            base.Initialize();
        }

        public void SetCat(CatBehaviour cat)
        {
            _dialogueBoxMenuView.SetupDialogueAnswers(cat);
            
        }

        public override void CleanUp()
        {
            
        }

    }
}
