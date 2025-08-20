namespace BinarySerializer.OpenSpace
{
    // SAV2_MenuOptionHeader
    public class R3MenuOptionHeader : BinarySerializable
    {
        public float CenteringX { get; set; }
        public float CenteringY { get; set; }
        public bool WideScreen { get; set; }
        public bool Vibration { get; set; }
        public bool InversionHor { get; set; }
        public bool InversionVert { get; set; }

        public ushort[] KeyboardMapping { get; set; } // Uses DirectX key codes
        public ushort[] ControllerMapping { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            CenteringX = s.Serialize<float>(CenteringX, name: nameof(CenteringX));
            CenteringY = s.Serialize<float>(CenteringY, name: nameof(CenteringY));
            WideScreen = s.Serialize<bool>(WideScreen, name: nameof(WideScreen));
            Vibration = s.Serialize<bool>(Vibration, name: nameof(Vibration));
            InversionHor = s.Serialize<bool>(InversionHor, name: nameof(InversionHor));
            InversionVert = s.Serialize<bool>(InversionVert, name: nameof(InversionVert));

            if (settings.Platform == Platform.PC)
            {
                KeyboardMapping = s.SerializeArray<ushort>(KeyboardMapping, 13, name: nameof(KeyboardMapping));
                ControllerMapping = s.SerializeArray<ushort>(ControllerMapping, 13, name: nameof(ControllerMapping));
            }
        }
    }
}