﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator
{
    class GameboyUninitializedException: ApplicationException
    { }

    class UnknownOpcodeException : ApplicationException
    {
        public UnknownOpcodeException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
