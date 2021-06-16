using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.MapEdit.Editor.Util;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Assets.Scripts.Editor.Tests
{
    public class BaseSettingCheckerTests
    {
        [Test]
        public void ChkMultiMultiCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(new BaseData()
            {
                BaseType = BaseTypeEnum.RaceStart,
                BaseTeam = Teams.Multi,
                GameMode = GameplayModesEnum.Race,
                ID = 1
            });
            data.Add(new BaseData()
            {
                BaseType = BaseTypeEnum.RaceFinish,
                BaseTeam = Teams.Multi,
                GameMode = GameplayModesEnum.Race,
                ID = 1
            });

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
        }
        //TODO the rest of cases
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BaseSettingCheckerTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
