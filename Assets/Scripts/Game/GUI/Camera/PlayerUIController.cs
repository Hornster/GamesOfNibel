using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.UI;
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

        public void AddPlayerInGameUIs(List<GameObject> playerUIPrefab)
        {
            var canvases = _playersUIParent.GetComponentsInChildren<Canvas>();

            if (playerUIPrefab.Count != canvases.Length)
            {
                throw new Exception("Amount of player canvases and UIs provided does not match!");
            }

            for (int i = 0; i < playerUIPrefab.Count; i++)
            {
                var playerCamera = canvases[i].GetComponentInChildren<UnityEngine.Camera>();
                SetCamera(playerCamera, playerUIPrefab[i]);
                //Every player has the same game UI. They participate in the same type of gameplay after all.
                playerUIPrefab[i].transform.SetParent(canvases[i].transform);
                //Reset canvas scale to (1,1,1) since for some reason automatic scaling makes the scale go through the roof.
                //Hence "true"
                StartCoroutine(CanvasModifier.ChkForCanvas(playerUIPrefab[i], true));
            }
        }

        /// <summary>
        /// Tries to set the provided camera as world camera of provided GameObject's canvases.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="playerUI"></param>
        private void SetCamera(UnityEngine.Camera camera, GameObject playerUI)
        {
            var playerUICanvases = playerUI.GetComponentsInChildren<Canvas>();

            foreach (var playerUICanvas in playerUICanvases)
            {
                playerUICanvas.worldCamera = camera;
            }
        }
    }
}
