using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Editor.Util
{
    public class MapEditReporter
    {
        public static void ReportError(string message)
        {
            Debug.LogError(message);
        }
    }
}
