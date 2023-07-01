﻿using System;
using EventArguments;

namespace Services
{
    public interface IMovesLeftService
    {
        int MovesLeft { get; }
        event EventHandler<MovesLeftChangedEventArgs> OnMovesLeftChanged;
    }
}