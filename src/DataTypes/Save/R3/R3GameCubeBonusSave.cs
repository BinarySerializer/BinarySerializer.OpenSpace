namespace BinarySerializer.OpenSpace
{
    public class R3GameCubeBonusSave : BinarySerializable
    {
        public bool Unlocked2DNightmare { get; set; }
        public bool Unlocked2DMadness { get; set; }
        public uint UnlockedMinigames { get; set; }
        public bool[] CompletedMinigames { get; set; }
        public uint Uint_F0 { get; set; } // Unknown
        public uint Uint_F4 { get; set; } // Unknown
        public uint CompletedLevelsCount { get; set; }
        public uint StartGameState { get; set; } // 0-2, determines how to start the game

        public override void SerializeImpl(SerializerObject s)
        {
            Unlocked2DNightmare = s.SerializeBoolean<int>(Unlocked2DNightmare, name: nameof(Unlocked2DNightmare));
            Unlocked2DMadness = s.SerializeBoolean<int>(Unlocked2DMadness, name: nameof(Unlocked2DMadness));
            UnlockedMinigames = s.Serialize<uint>(UnlockedMinigames, name: nameof(UnlockedMinigames));
            CompletedMinigames = s.InitializeArray(CompletedMinigames, 12);
            s.DoArray(CompletedMinigames, (obj, _, name) => s.SerializeBoolean<int>(obj, name: name), name: nameof(CompletedMinigames));
            Uint_F0 = s.Serialize<uint>(Uint_F0, name: nameof(Uint_F0));
            Uint_F4 = s.Serialize<uint>(Uint_F4, name: nameof(Uint_F4));
            CompletedLevelsCount = s.Serialize<uint>(CompletedLevelsCount, name: nameof(CompletedLevelsCount));
            StartGameState = s.Serialize<uint>(StartGameState, name: nameof(StartGameState));
        }
    }
}