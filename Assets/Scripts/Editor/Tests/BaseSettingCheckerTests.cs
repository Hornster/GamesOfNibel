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

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceMultiAllCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
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

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceAllMultiCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkEmptyCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();

            Assert.False(baseSettingChecker.ChkBasesSetting(data));
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

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
        }

        //bad race cases
        [Test]
        public void ChkRaceNeutralNeutralCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceMultiLilyTeam()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceMultiLotusTeam()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceLilyMultiTeam()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceLotusMultiTeam()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceOnlyStartTeam()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));

            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceOnlyFinishTeam()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.RaceStart, GameplayModesEnum.Race));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.RaceFinish, GameplayModesEnum.Race));

            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }


        //CTF
        //[LilyCount, LotusCount, NeutralCount] - for example, [...]OneOneOneCase
        [Test]
        public void ChkRaceOneOneOneCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceOneOneThreeCase()
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

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkRaceThreeThreeOneCase()
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

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
        }
        //bad ctf cases
        [Test]
        public void ChkBadCtfMultiCase()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkBadCtfMultiCase3Bases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Multi, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkBadCtfLilyBases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkBadCtfLotusBases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Neutral, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }
        [Test]
        public void ChkBadCtfNoNeutralBases()
        {
            var baseSettingChecker = new BaseSettingChecker();
            var data = new List<BaseData>();
            data.Add(CreateData(Teams.Lotus, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            data.Add(CreateData(Teams.Lily, BaseTypeEnum.CtfDefault, GameplayModesEnum.CTF));
            Assert.False(baseSettingChecker.ChkBasesSetting(data));
        }

        //Mixed
        [Test]
        public void ChkBadMixedBases()
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

            Assert.True(baseSettingChecker.ChkBasesSetting(data));
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
