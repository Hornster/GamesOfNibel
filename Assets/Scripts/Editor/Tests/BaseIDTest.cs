using System.Collections;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.MapEdit;
using Assets.Scripts.MapEdit.Editor.Data.Constants;
using Assets.Scripts.MapEdit.Editor.Data.ScriptableObjects;
using Assets.Scripts.MapEdit.Editor.Util;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Scripts.Editor.Tests
{
    public class BaseIDTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void BaseIDTestSimplePasses()
        {
            var assetsSeeker = new AssetSeeker<BaseMarkerFactorySO>();
            var baseMarkersFactory = assetsSeeker.FindAsset(
                SGMapEditConstants.MapEditScriptableObjectsPath, BaseMarkerFactorySO.BaseMarkerFactorySoName);
            Assert.True(baseMarkersFactory != null);

            var baseObject = baseMarkersFactory.CreateBaseMarker(Teams.Lotus, BaseTypeEnum.CtfDefault, Vector3.zero);
            int currentBaseMarkerID = baseMarkersFactory.LastUsedMarkerID;
            int baseMarkerID = baseObject.GetComponent<BaseMarkerData>().ID;

            Object.DestroyImmediate(baseObject);

            Assert.True(baseMarkerID == currentBaseMarkerID);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BaseIDTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
