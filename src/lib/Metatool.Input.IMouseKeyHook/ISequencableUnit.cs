﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Metatool.Input
{
    public interface ISequenceUnit: IHotkey
    {
        ICombination ToCombination();
    }
}
