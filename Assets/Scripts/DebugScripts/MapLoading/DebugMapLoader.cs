using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Mods.Maps;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DebugScripts.MapLoading
{
    public class DebugMapLoader : MonoBehaviour
    {
        [SerializeField] private Image _prevewImage;
        [SerializeField] private Image _thumbnailImage;

        [SerializeField] private AssetBundleLoader _assetBundleLoader;

        private void Start()
        {
            var images = _assetBundleLoader.LoadMapAssetBundle();

            _prevewImage.sprite = images[0].PreviewImg;
            _thumbnailImage.sprite = images[0].ThumbnailImg;

            _prevewImage.SetNativeSize();
            _thumbnailImage.SetNativeSize();
        }
    }  
}
