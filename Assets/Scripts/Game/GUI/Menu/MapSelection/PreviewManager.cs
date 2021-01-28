using System.Collections.Generic;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.GUI.Menu.MapSelection
{
    /// <summary>
    /// Manages data shown in the preview field of the map selection menu.
    /// </summary>
    public class PreviewManager : MonoBehaviour
    {
        /// <summary>
        /// Description part of the map.
        /// </summary>
        [Header("Description field")]
        [SerializeField] private TMP_Text _mapName;
        [SerializeField] private TMP_Text _mapAuthors;
        [SerializeField] private TMP_Text _mapMode;
        [SerializeField] private TMP_Text _description;

        /// <summary>
        /// The map big preview image.
        /// </summary>
        [Header("Preview image")] 
        [SerializeField] private Image _previewImage;
        /// <summary>
        /// Put in the place of the preview image when it is not available for previewed map.
        /// </summary>
        [SerializeField] private Sprite _noPreviewPlaceholder;

        /// <summary>
        /// Main gameobject which has all the skill toggling controls as its children. Used to retrieve the
        /// skill controls managers.
        /// </summary>
        [Header("SkillControls")]
        [SerializeField] private GameObject _skillControlsContainer;
        private readonly Dictionary<SkillType, SkillControlManager> _skillControls = new Dictionary<SkillType, SkillControlManager>();

        /// <summary>
        /// Used when corresponding piece of data from the map could not be loaded.
        /// </summary>
        [Header("Default values")] 
        [SerializeField]
        private MapSelectionDefaults _selectionDefaults;

        private void Start()
        {
            var controls = _skillControlsContainer.GetComponentsInChildren<SkillControlManager>();
            foreach (var skill in controls)
            {
                _skillControls.Add(skill.SkillType, skill);
            }
        }
        /// <summary>
        /// Updates map details.
        /// </summary>
        /// <param name="mapData"></param>
        private void UpdateMapDetails(MapData mapData)
        {
            _mapName.text = mapData?.ShownMapName?.Length > 0 ? mapData.ShownMapName : _selectionDefaults.DefaultMapName;
            _mapAuthors.text = mapData?.Authors?.Count > 0 ? StringManipulator.JoinStrings(", ", mapData.Authors) : _selectionDefaults.DefaultMapAuthors;
            _mapMode.text = mapData != null ? mapData.GameplayMode.ToString() : _selectionDefaults.DefaultMapMode;
            _description.text = mapData?.Description?.Length > 0 ? mapData.Description : _selectionDefaults.DefaultMapDescription;
        }
        private void UpdatePreviewImg(Sprite mapPreview)
        {
            _previewImage.sprite = mapPreview != null ? mapPreview : _selectionDefaults.DefaultMapPreview;
        }
        /// <summary>
        /// Updates all skills accordingly to required for currently PREVIEWED MAP required skill set.
        /// </summary>
        /// <param name="mapData"></param>
        private void UpdateSkillsState(MapData mapData)
        {
            //First reset all skills to not required.
            foreach (var skill in _skillControls.Values)
            {
                skill.UpdateSkillRequirementState(false);
            }

            //Then mark all required skills properly.
            foreach (var skill in mapData.RequiredSkills)
            {
                if (_skillControls.TryGetValue(skill, out var skillControl))
                {
                    skillControl.UpdateSkillRequirementState(true);
                }
            }
        }
        /// <summary>
        /// Forces update of entire preview using provided map data.
        /// </summary>
        /// <param name="mapData">Data used to update the preview.</param>
        public void UpdatePreview(MapData mapData)
        {
            UpdatePreviewImg(mapData?.PreviewImg);
            UpdateMapDetails(mapData);
            UpdateSkillsState(mapData);
        }
    }
}
