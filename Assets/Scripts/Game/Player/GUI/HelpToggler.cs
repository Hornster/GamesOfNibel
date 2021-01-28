using UnityEngine;

namespace Assets.Scripts.Game.Player.GUI
{
    public class HelpToggler : MonoBehaviour
    {
        [SerializeField] private GameObject _helpObject;

        private bool _isActive = false;
        // Start is called before the first frame update
        void Start()
        {
            _helpObject.SetActive(_isActive);
            InputReader.RegisterHelpToggleHandler(ToggleHelpPage);
        }

        private void ToggleHelpPage()
        {
            _isActive = !_isActive;
            _helpObject.SetActive(_isActive);
        }
    }
}
