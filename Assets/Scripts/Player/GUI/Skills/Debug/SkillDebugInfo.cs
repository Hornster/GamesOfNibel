using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player.GUI.Skills.Debug
{
    public class SkillDebugInfo : MonoBehaviour, ISkillDebugInfo
    {
        private Image _skillImage;
        [SerializeField]
        private Color _skillReadyColor;
        [SerializeField]
        private Color _skillUsedColor;
        [SerializeField]
        private Color _skillUnavailableColor;

        private Color _skillColorBeforeUnavailable;

        private void Start()
        {
            _skillImage = GetComponent<Image>();
        }

        public void SkillWasUsed()
        {
            var imageMaterial = _skillImage.material;
            imageMaterial.color = _skillUsedColor;
            _skillImage.material = imageMaterial;
        }

        public void SkillWasReset()
        {
            var imageMaterial = _skillImage.material;
            imageMaterial.color = _skillReadyColor;
            _skillImage.material = imageMaterial;
        }

        public void SkillBecameUnavailable()
        {
            var imageMaterial = _skillImage.material;
            _skillColorBeforeUnavailable = imageMaterial.color;
            imageMaterial.color = _skillUnavailableColor;
            _skillImage.material = imageMaterial;
        }

        public void SkillBecameAvailable()
        {
            var imageMaterial = _skillImage.material;
            imageMaterial.color = _skillColorBeforeUnavailable;
            _skillImage.material = imageMaterial;
        }
    }
}
//TODO - can become unavailable only when is recharged and ready to use.