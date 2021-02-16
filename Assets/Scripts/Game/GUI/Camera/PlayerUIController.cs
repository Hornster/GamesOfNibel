using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.GUI.Camera
{
    public class PlayerUIController : MonoBehaviour
    {
        [Tooltip("Transform of gameobject that will be the parent of players' UI objects.")]
        [SerializeField]
        private Transform _playersUIParent;
        [Tooltip("Prefab of the player UI, used to spawn the UIs.")]
        [SerializeField]
        private GameObject _playerUIPrefab;


        public void CreatePlayerUI(UnityEngine.Camera playerCamera)
        {
            var playerUI = Instantiate(_playerUIPrefab, _playersUIParent);
            var uiCanvas = playerUI.GetComponentInChildren<Canvas>();
            uiCanvas.worldCamera = playerCamera;
        }
    }
}
