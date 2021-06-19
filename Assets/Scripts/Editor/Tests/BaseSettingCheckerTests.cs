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
        //RACE
        [Test]
        public void ChkRaceMultiMultiCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.True(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 1);
            Assert.True(result.correctGameModes[0] == GameplayModesEnum.Race);
        }
        [Test]
        public void ChkRaceMultiAllCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.True(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 1);
            Assert.True(result.correctGameModes[0] == GameplayModesEnum.Race);
        }
        [Test]
        public void ChkRaceAllAllCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.True(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 1);
            Assert.True(result.correctGameModes[0] == GameplayModesEnum.Race);
        }
        [Test]
        public void ChkRaceAllMultiCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.True(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 1);
            Assert.True(result.correctGameModes[0] == GameplayModesEnum.Race);
        }
        [Test]
        public void ChkEmptyCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkMultipleGoodCases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));


            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.True(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 1);
            Assert.True(result.correctGameModes[0] == GameplayModesEnum.Race);
        }

        //bad race cases
        [Test]
        public void ChkRaceNeutralNeutralCase()
        {
            LogAssert.ignoreFailingMessages = true;
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkRaceMultiLilyTeam()
        {
            LogAssert.ignoreFailingMessages = true;
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkRaceMultiLotusTeam()
        {
            LogAssert.ignoreFailingMessages = true;
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkRaceLilyMultiTeam()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            LogAssert.ignoreFailingMessages = true;
            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkRaceLotusMultiTeam()
        {
            LogAssert.ignoreFailingMessages = true;
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            LogAssert.ignoreFailingMessages = true;
            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkRaceOnlyStartTeam()
        {
            LogAssert.ignoreFailingMessages = true;
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkRaceOnlyFinishTeam()
        {
            LogAssert.ignoreFailingMessages = true;
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }


        //CTF
        //[LilyCount, LotusCount, NeutralCount] - for example, [...]OneOneOneCase
        [Test]
        public void ChkCtfOneOneOneCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.True(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 1);
            Assert.True(result.correctGameModes[0] == GameplayModesEnum.CTF);
        }
        [Test]
        public void ChkCtfOneOneThreeCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(new BaseData()
            {
                BaseType = BaseTypeEnum.CtfDefault,
                BaseTeam = Teams.Neutral,
                GameMode = GameplayModesEnum.CTF,
                ID = 1
            });
            data.Add(new BaseData()
            {
                BaseType = BaseTypeEnum.CtfDefault,
                BaseTeam = Teams.Lotus,
                GameMode = GameplayModesEnum.CTF,
                ID = 1
            });
            data.Add(new BaseData()
            {
                BaseType = BaseTypeEnum.CtfDefault,
                BaseTeam = Teams.Neutral,
                GameMode = GameplayModesEnum.CTF,
                ID = 1
            });
            data.Add(new BaseData()
            {
                BaseType = BaseTypeEnum.CtfDefault,
                BaseTeam = Teams.Lily,
                GameMode = GameplayModesEnum.CTF,
                ID = 1
            });
            data.Add(new BaseData()
            {
                BaseType = BaseTypeEnum.CtfDefault,
                BaseTeam = Teams.Neutral,
                GameMode = GameplayModesEnum.CTF,
                ID = 1
            });

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.True(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 1);
            Assert.True(result.correctGameModes[0] == GameplayModesEnum.CTF);
        }
        [Test]
        public void ChkCtfThreeThreeOneCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.True(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 1);
            Assert.True(result.correctGameModes[0] == GameplayModesEnum.CTF);
        }
        //bad ctf cases
        [Test]
        public void ChkBadCtfMultiCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkBadCtfMultiCase3Bases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkBadCtfLilyBases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkBadCtfLotusBases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }
        [Test]
        public void ChkBadCtfNoNeutralBases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.False(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 0);
        }

        //Mixed
        [Test]
        public void ChkMixedBases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));

            var result = baseSettingChecker.ChkBasesSetting(data);
            Assert.True(result.isSettingCorrect);
            Assert.True(result.correctGameModes.Length == 2);
        }


        private BaseData CreateData(Teams team, BaseTypeEnum baseType, GameplayModesEnum gameplayMode)
        {
            return new BaseData()
            {
                BaseType = baseType,
                BaseTeam = team,
                GameMode = gameplayMode,
                ID = 1
            };
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
