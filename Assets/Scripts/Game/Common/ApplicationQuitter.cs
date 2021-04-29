using UnityEngine;

namespace Assets.Scripts.Game.Common
{
    /// <summary>
    /// Quits the application
    /// </summary>
    public class ApplicationQuitter :SceneSingleton<ApplicationQuitter>
    {
        public void Quit()
        {
            Application.Quit();
        }
    }
}
