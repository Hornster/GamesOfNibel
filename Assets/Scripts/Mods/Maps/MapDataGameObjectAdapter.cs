using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Mods.Maps
{
    /// <summary>
    /// Allows for hooking the MapData object to gameobjects.
    /// </summary>
    public class MapDataGameObjectAdapter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textArea;
        [SerializeField] private Image _thumbnail;
        public MapData MapData { get; private set; }
        /// <summary>
        /// Sets the data to provided map data. Updates the title and thumbnail on the map tile in the process.
        /// </summary>
        /// <param name="mapData"></param>
        public void SetMapData(MapData mapData)
        {
            MapData = mapData;
            _textArea.text = mapData.ShownMapName;
            _thumbnail.sprite = mapData.ThumbnailImg;
        }

    }
}
