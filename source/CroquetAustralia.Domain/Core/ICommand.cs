﻿using System;

namespace CroquetAustralia.Domain.Core
{
    public interface ICommand
    {
        Guid EntityId { get; set; }
    }
}