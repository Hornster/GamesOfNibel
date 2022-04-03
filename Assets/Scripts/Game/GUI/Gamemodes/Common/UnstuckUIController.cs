using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.GUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.GUI.Gamemodes
{
    public class UnstuckUIController : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarController _progressBar;
        [SerializeField]
        private TMP_Text _unstuckText;

        public void ToggleUnstuckUI(KeyStateEnum keyState)
        {
            switch (keyState)
            {
                case KeyStateEnum.Down:
                    ShowUI();
                    break;
                case KeyStateEnum.Held:
                    return;//No need to do anything here
                case KeyStateEnum.Up:
                    HideUI();
                    break;
            }
        }
        private void ShowUI()
        {
            _progressBar.ShowProgressBar();
        }
        private void HideUI()
        {
            _progressBar.HideProgressBar();
            
        }
        private void ChangeProgress()
        {
            //TODO this, connect this to race controller, connect race controller
            //to input reader (or this directly). Setup the objects hierarchy.
        }
    }
}
}