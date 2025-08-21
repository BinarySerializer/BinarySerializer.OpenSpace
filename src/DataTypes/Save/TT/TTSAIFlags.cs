using System;

namespace BinarySerializer.OpenSpace
{
    [Flags]
    public enum TTSAIFlags : ushort
    {
        None = 0,

        // Init type
        InitWhenNewGameStart = 1 << 0,
        InitWhenMapLoaded = 1 << 1,
        InitWhenPlayerGameSavedLoaded = 1 << 2,
        InitWhenLevelGameSavedLoaded = 1 << 3,
        InitWhenReinitTheMap = 1 << 4,
        InitWhenPlayerDead = 1 << 5,

        // Save type
        PlayerSaveTableValue = 1 << 8,
        PlayerSaveCurrentValue = 1 << 9,
        LevelSaveTableValue = 1 << 10,
        LevelSaveCurrentValue = 1 << 11,
    }
}