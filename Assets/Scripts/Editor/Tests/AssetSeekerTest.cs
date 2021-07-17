using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.MapEdit.Editor.Data.Constants;
using Assets.Scripts.MapEdit.Editor.Data.ScriptableObjects;
using Assets.Scripts.MapEdit.Editor.Util;
using NUnit.Framework;

namespace Assets.Scripts.Editor.Tests
{
    public class AssetSeekerTest
    {
        [Test]
        public void SeekForBaseMarkerFactorySO()
        {
            var assetsSeeker = new AssetSeeker<BaseMarkerFactorySO>();
            var baseMarkersFactory = assetsSeeker.FindAsset(
                SGMapEditConstants.MapEditBaseMarkersSOsFolderPath, BaseMarkerFactorySO.BaseMarkerFactorySoName);
            Assert.True(baseMarkersFactory != null);
        }

        [Test]
        public void SeekForMapModAssemblerCacheSO()
        {
            var assetsSeeker = new AssetSeeker<MapModAssemblerCacheSO>();
            var mapModAssemblerCache = assetsSeeker.FindAsset(SGMapEditConstants.MapEditMapModAssemblerSOsFolderPath,
                MapModAssemblerCacheSO.MapModAssemblerCacheSoName);
            Assert.True(mapModAssemblerCache != null);
        }
    }
}
