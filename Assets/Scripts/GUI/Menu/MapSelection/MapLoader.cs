using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu.MapSelection
{
    /// <summary>
    /// Deserializes map data from JSON format.
    /// </summary>
    public class MapLoader
    {
        
        /// <summary>
        /// Deserializes map data.
        /// </summary>
        /// <param name="mapData">Maps data. One string per map.</param>
        /// <returns></returns>
        //public List<MapData> DeserializeMapsData(List<string> mapData)
        //{
        //    var deserializedMaps = new List<MapData>();
        //    foreach (var map in mapData)
        //    {
                
        //    }
        //}

        //private MapData DeserializeSingleMap(string map)
        //{
        //    var rawMapData = JsonUtility.FromJson<RawMapData>(map);

        //}
        /// <summary>
        /// Raw data of a map. Still needs validating the paths contained in the data.
        /// </summary>
        class RawMapData
        {
            public string ShownName { get; set; }
            public string PreviewImgPath { get; set; }
            public string ThumbnailImgPath { get; set; }
            public string[] Authors { get; set; }
            public int GameplayMode { get; set; }
            public string Description { get; set; }
            public string[] RequiredSkills { get; set; }
            public string SceneId { get; set; }
        }
    }
}
