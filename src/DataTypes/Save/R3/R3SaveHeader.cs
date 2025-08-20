namespace BinarySerializer.OpenSpace
{
    // SAV2_tdstHeader
    public class R3SaveHeader : BinarySerializable
    {
        public R3EnvironmentValues EnvironmentValues { get; set; }
        public uint[] ForKeyAlgo { get; set; } // For encoding Hall of Fame key
        public bool EndOfGameIsDone { get; set; }
        public int MaxIndCurrentEnv { get; set; } // Max level
        public R3MenuOptionHeader MenuOptions { get; set; }
        public bool WarnedUserAboutAutoSave { get; set; }
        public bool RevisitMode { get; set; }
        public int RevisitEnvScore { get; set; }
        public int RevisitGlobalScore { get; set; }
        public int RevisitEnvironment { get; set; }
        public int RevisitOriginEnvironment { get; set; }
        public string LevelName { get; set; } // The current level to load, or "endgame" if the game has been finished
        public string LevelNameAfterRevisit { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            EnvironmentValues = s.SerializeObject<R3EnvironmentValues>(EnvironmentValues, name: nameof(EnvironmentValues));
            ForKeyAlgo = s.SerializeArray<uint>(ForKeyAlgo, 4, name: nameof(ForKeyAlgo));
            EndOfGameIsDone = s.SerializeBoolean<int>(EndOfGameIsDone, name: nameof(EndOfGameIsDone));
            MaxIndCurrentEnv = s.Serialize<int>(MaxIndCurrentEnv, name: nameof(MaxIndCurrentEnv));
            MenuOptions = s.SerializeObject<R3MenuOptionHeader>(MenuOptions, name: nameof(MenuOptions));
            WarnedUserAboutAutoSave = s.SerializeBoolean<uint>(WarnedUserAboutAutoSave, name: nameof(WarnedUserAboutAutoSave));
            RevisitMode = s.SerializeBoolean<int>(RevisitMode, name: nameof(RevisitMode));
            RevisitEnvScore = s.Serialize<int>(RevisitEnvScore, name: nameof(RevisitEnvScore));
            RevisitGlobalScore = s.Serialize<int>(RevisitGlobalScore, name: nameof(RevisitGlobalScore));
            RevisitEnvironment = s.Serialize<int>(RevisitEnvironment, name: nameof(RevisitEnvironment));
            RevisitOriginEnvironment = s.Serialize<int>(RevisitOriginEnvironment, name: nameof(RevisitOriginEnvironment));
            LevelName = s.SerializeString(LevelName, 20, name: nameof(LevelName));
            LevelNameAfterRevisit = s.SerializeString(LevelNameAfterRevisit, 20, name: nameof(LevelNameAfterRevisit));
        }
    }
}