﻿using System.Numerics;

namespace Common.Players
{
    /// <summary>
    /// Interface defining a Player.
    /// </summary>
    public interface IPlayer
    {
        ulong Base { get; }
        string Name { get; }
        ref Vector3 Position { get; }
        Vector2 Rotation { get; }
        Skeleton Skeleton { get; }
    }
}
