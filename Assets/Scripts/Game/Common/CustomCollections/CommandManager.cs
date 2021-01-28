using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.CustomCollections
{
    /// <summary>
    /// Used to remember given events.
    /// </summary>
    /// <typeparam name="T">Remembered data.</typeparam>
    public class CommandManager<T>
    {
        /// <summary>
        /// Made actions are stored here.
        /// </summary>
        private Stack<T> _commands = new Stack<T>();

        /// <summary>
        /// Adds action as most recent one made.
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(T action)
        {
            _commands.Push(action);
        }
        /// <summary>
        /// Returns the most recent action that has been remembered.
        /// </summary>
        /// <returns></returns>
        public T GetLastAction()
        {
            return _commands.Pop();
        }
        /// <summary>
        /// Removes everything from the history. Permanently.
        /// </summary>
        public void ClearHistory()
        {
            _commands.Clear();
        }
    }
}
