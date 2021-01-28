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
            _skillColorBeforeUnavailable = _skillReadyColor;
        }

        public void SkillWasUsed()
        {
            _skillImage.color = _skillUsedColor;
            _skillColorBeforeUnavailable = _skillUsedColor;
        }

        public void SkillWasReset()
        {
            _skillImage.color = _skillReadyColor;
            _skillColorBeforeUnavailable = _skillReadyColor;
        }

        public void SkillBecameUnavailable()
        {
            _skillImage.color = _skillUnavailableColor;
        }

        public void SkillBecameAvailable()
        {
            _skillImage.color = _skillColorBeforeUnavailable;
        }
    }
}
//TODO - can become unavailable only when is recharged and ready to use.