namespace BinarySerializer.OpenSpace
{
    // TODO: Support other platforms - the format is very similar

    /// <summary>
    /// The data for a Rayman 3 save file
    /// </summary>
    public class R3SaveFile : BinarySerializable
    {
        /// <summary>
        /// The total amount of cages
        /// </summary>
        public int TotalCages { get; set; }

        /// <summary>
        /// The total score
        /// </summary>
        public int TotalScore { get; set; }

        /// <summary>
        /// The data for each of the available levels. Count is always 9.
        /// </summary>
        public R3SaveLevelItem[] Levels { get; set; }

        public uint ForKeyAlgo { get; set; }
        public uint Uint_54 { get; set; } // Dword_7FE504
        public uint Uint_58 { get; set; } // Dword_7FE508
        public uint Uint_5C { get; set; } // Dword_7FE50C
        public int EndOfGameIsDone { get; set; }
        public uint MaxIndCurrentEnv { get; set; }

        // Always 0 and gets set to 0 when saving
        public uint Uint_68 { get; set; }
        public uint Uint_6C { get; set; }

        /// <summary>
        /// Indicates if controller vibration is enabled
        /// </summary>
        public bool IsVibrationEnabled { get; set; }

        /// <summary>
        /// Indicates if the horizontal axis is inverted
        /// </summary>
        public bool IsHorizontalInversionEnabled { get; set; }

        /// <summary>
        /// Indicates if the vertical axis is inverted
        /// </summary>
        public bool IsVerticalInversionEnabled { get; set; }

        public ushort[] KeyboardMapping { get; set; } // Uses DirectX key codes
        public ushort[] ControllerMapping { get; set; }

        public int UserWarnedAboutAutoSave { get; set; }
        public int RevisitMode { get; set; }
        public int RevisitEnvScoreAtBeginingOfLevel { get; set; }
        public int RevisitGlobalScoreAtBeginingOfLevel { get; set; }
        public int RevisitIndCurrentEnv { get; set; }
        public int RevisitIndEnvironementFromWhereWeCome { get; set; }

        /// <summary>
        /// The current level to load, or "endgame" if the game has been finished
        /// </summary>
        public string LevelNameToSave { get; set; }

        public string LevelNameAfterReinitToSave { get; set; }

        public byte[] Bytes_E8 { get; set; }

        public int SetVolumeSound { get; set; }
        public int SetVolumeVoice { get; set; }
        public int SetVolumeMusic { get; set; }
        public int SetVolumeAmbiance { get; set; }
        public int SetVolumeMenu { get; set; }

        // Contains data like the current player health etc.
        public R3SaveItem[] Items { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            TotalCages = s.Serialize<int>(TotalCages, name: nameof(TotalCages));
            TotalScore = s.Serialize<int>(TotalScore, name: nameof(TotalScore));

            Levels = s.SerializeObjectArray<R3SaveLevelItem>(Levels, 9, name: nameof(Levels));

            ForKeyAlgo = s.Serialize<uint>(ForKeyAlgo, name: nameof(ForKeyAlgo));
            Uint_54 = s.Serialize<uint>(Uint_54, name: nameof(Uint_54));
            Uint_58 = s.Serialize<uint>(Uint_58, name: nameof(Uint_58));
            Uint_5C = s.Serialize<uint>(Uint_5C, name: nameof(Uint_5C));
            EndOfGameIsDone = s.Serialize<int>(EndOfGameIsDone, name: nameof(EndOfGameIsDone));
            MaxIndCurrentEnv = s.Serialize<uint>(MaxIndCurrentEnv, name: nameof(MaxIndCurrentEnv));
            Uint_68 = s.Serialize<uint>(Uint_68, name: nameof(Uint_68));
            Uint_6C = s.Serialize<uint>(Uint_6C, name: nameof(Uint_6C));

            s.SerializePadding(1, logIfNotNull: true);
            IsVibrationEnabled = s.Serialize<bool>(IsVibrationEnabled, name: nameof(IsVibrationEnabled));
            IsHorizontalInversionEnabled = s.Serialize<bool>(IsHorizontalInversionEnabled, name: nameof(IsHorizontalInversionEnabled));
            IsVerticalInversionEnabled = s.Serialize<bool>(IsVerticalInversionEnabled, name: nameof(IsVerticalInversionEnabled));

            KeyboardMapping = s.SerializeArray<ushort>(KeyboardMapping, 13, name: nameof(KeyboardMapping));
            ControllerMapping = s.SerializeArray<ushort>(ControllerMapping, 13, name: nameof(ControllerMapping));

            UserWarnedAboutAutoSave = s.Serialize<int>(UserWarnedAboutAutoSave, name: nameof(UserWarnedAboutAutoSave));
            RevisitMode = s.Serialize<int>(RevisitMode, name: nameof(RevisitMode));
            RevisitEnvScoreAtBeginingOfLevel = s.Serialize<int>(RevisitEnvScoreAtBeginingOfLevel, name: nameof(RevisitEnvScoreAtBeginingOfLevel));
            RevisitGlobalScoreAtBeginingOfLevel = s.Serialize<int>(RevisitGlobalScoreAtBeginingOfLevel, name: nameof(RevisitGlobalScoreAtBeginingOfLevel));
            RevisitIndCurrentEnv = s.Serialize<int>(RevisitIndCurrentEnv, name: nameof(RevisitIndCurrentEnv));
            RevisitIndEnvironementFromWhereWeCome = s.Serialize<int>(RevisitIndEnvironementFromWhereWeCome, name: nameof(RevisitIndEnvironementFromWhereWeCome));

            LevelNameToSave = s.SerializeString(LevelNameToSave, 20, name: nameof(LevelNameToSave));
            LevelNameAfterReinitToSave = s.SerializeString(LevelNameAfterReinitToSave, 20, name: nameof(LevelNameAfterReinitToSave));

            Bytes_E8 = s.SerializeArray<byte>(Bytes_E8, 16, name: nameof(Bytes_E8));

            SetVolumeSound = s.Serialize<int>(SetVolumeSound, name: nameof(SetVolumeSound));
            SetVolumeVoice = s.Serialize<int>(SetVolumeVoice, name: nameof(SetVolumeVoice));
            SetVolumeMusic = s.Serialize<int>(SetVolumeMusic, name: nameof(SetVolumeMusic));
            SetVolumeAmbiance = s.Serialize<int>(SetVolumeAmbiance, name: nameof(SetVolumeAmbiance));
            SetVolumeMenu = s.Serialize<int>(SetVolumeMenu, name: nameof(SetVolumeMenu));

            Items = s.SerializeObjectArrayUntil(Items, x => x.KeyLength == 0, () => new R3SaveItem(), name: nameof(Items));
        }
    }
}