﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;

namespace Assets.Scripts.GameModes.CTF
{
    public interface IFlagCarrier
    {
        Teams MyTeam { get; }
    }
}
