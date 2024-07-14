namespace BinarySerializer.OpenSpace
{
    // SAV2_g_stList
    public class R3SaveList : BinarySerializable
    {
        public R3EnvironmentValues EnvironmentValues { get; set; }
        public uint[] ForKeyAlgo { get; set; } // For encoding Hall of Fame key
        public bool EndOfGameIsDone { get; set; }
        public uint MaxIndCurrentEnv { get; set; } // Max level

        // Always 0 and gets set to 0 when saving
        public uint Uint_68 { get; set; }
        public uint Uint_6C { get; set; }
        public bool Bool_70 { get; set; }

        public bool IsVibrationEnabled { get; set; }
        public bool IsHorizontalInversionEnabled { get; set; }
        public bool IsVerticalInversionEnabled { get; set; }

        public ushort[] KeyboardMapping { get; set; } // Uses DirectX key codes
        public ushort[] ControllerMapping { get; set; }

        public bool UserWarnedAboutAutoSave { get; set; }
        public bool RevisitMode { get; set; }
        public int RevisitEnvScoreAtBeginingOfLevel { get; set; }
        public int RevisitGlobalScoreAtBeginingOfLevel { get; set; }
        public int RevisitIndCurrentEnv { get; set; }
        public int RevisitIndEnvironementFromWhereWeCome { get; set; }

        public string LevelNameToSave { get; set; } // The current level to load, or "endgame" if the game has been finished
        public string LevelNameAfterReinitToSave { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            EnvironmentValues = s.SerializeObject<R3EnvironmentValues>(EnvironmentValues, name: nameof(EnvironmentValues));
            ForKeyAlgo = s.SerializeArray<uint>(ForKeyAlgo, 4, name: nameof(ForKeyAlgo));
            EndOfGameIsDone = s.SerializeBoolean<int>(EndOfGameIsDone, name: nameof(EndOfGameIsDone));
            MaxIndCurrentEnv = s.Serialize<uint>(MaxIndCurrentEnv, name: nameof(MaxIndCurrentEnv));
            Uint_68 = s.Serialize<uint>(Uint_68, name: nameof(Uint_68));
            Uint_6C = s.Serialize<uint>(Uint_6C, name: nameof(Uint_6C));
            Bool_70 = s.Serialize<bool>(Bool_70, name: nameof(Bool_70));
            IsVibrationEnabled = s.Serialize<bool>(IsVibrationEnabled, name: nameof(IsVibrationEnabled));
            IsHorizontalInversionEnabled = s.Serialize<bool>(IsHorizontalInversionEnabled, name: nameof(IsHorizontalInversionEnabled));
            IsVerticalInversionEnabled = s.Serialize<bool>(IsVerticalInversionEnabled, name: nameof(IsVerticalInversionEnabled));

            if (settings.Platform == Platform.PC)
            {
                KeyboardMapping = s.SerializeArray<ushort>(KeyboardMapping, 13, name: nameof(KeyboardMapping));
                ControllerMapping = s.SerializeArray<ushort>(ControllerMapping, 13, name: nameof(ControllerMapping));
            }

            UserWarnedAboutAutoSave = s.SerializeBoolean<int>(UserWarnedAboutAutoSave, name: nameof(UserWarnedAboutAutoSave));
            RevisitMode = s.SerializeBoolean<int>(RevisitMode, name: nameof(RevisitMode));
            RevisitEnvScoreAtBeginingOfLevel = s.Serialize<int>(RevisitEnvScoreAtBeginingOfLevel, name: nameof(RevisitEnvScoreAtBeginingOfLevel));
            RevisitGlobalScoreAtBeginingOfLevel = s.Serialize<int>(RevisitGlobalScoreAtBeginingOfLevel, name: nameof(RevisitGlobalScoreAtBeginingOfLevel));
            RevisitIndCurrentEnv = s.Serialize<int>(RevisitIndCurrentEnv, name: nameof(RevisitIndCurrentEnv));
            RevisitIndEnvironementFromWhereWeCome = s.Serialize<int>(RevisitIndEnvironementFromWhereWeCome, name: nameof(RevisitIndEnvironementFromWhereWeCome));

            LevelNameToSave = s.SerializeString(LevelNameToSave, 20, name: nameof(LevelNameToSave));
            LevelNameAfterReinitToSave = s.SerializeString(LevelNameAfterReinitToSave, 20, name: nameof(LevelNameAfterReinitToSave));
        }
    }
}