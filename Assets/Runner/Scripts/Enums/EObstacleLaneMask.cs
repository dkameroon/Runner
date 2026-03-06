using System;

[Flags]
public enum EObstacleLaneMask
{
    None = 0,
    Left = 1 << 0,
    Center = 1 << 1,
    Right = 1 << 2
}