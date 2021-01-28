using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Mods.Maps;
using Assets.Scripts.Game.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game.DebugScripts.MapLoading
{
    public class DebugMapLoader : MonoBehaviour
    {
        [SerializeField] private Image _prevewImage;
        [SerializeField] private Image _thumbnailImage;

        [SerializeField] private MapAssetBundleLoader _mapAssetBundleLoader;

        private MapData _loadedMap;

        private void Start()
        {
            var images = _mapAssetBundleLoader.LoadMapAssetBundle();

            _prevewImage.sprite = images[0].PreviewImg;
            _thumbnailImage.sprite = images[0].ThumbnailImg;
            _loadedMap = images[0];

            _prevewImage.SetNativeSize();
            _thumbnailImage.SetNativeSize();

            InputReader.RegisterGameEnd(AppExit);
            InputReader.RegisterJumpHandler(LoadMap);
        }

        void LoadMap()
        {
            var sceneBundle = _mapAssetBundleLoader.LoadMapSceneBundle(_loadedMap);
            SceneManager.LoadScene(_loadedMap.ScenePath);
        }
        void AppExit()
        {
            Application.Quit();
        }
    }  
}
