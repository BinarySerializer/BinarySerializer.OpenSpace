﻿using System;

namespace BinarySerializer.OpenSpace
{
    public class CNT : BinarySerializable
    {
        public bool IsXORUsed { get; set; }
        public bool IsChecksumUsed { get; set; }
        public byte StringsXORKey { get; set; }
        public string[] Directories { get; set; }
        public CNT_File[] Files { get; set; }

        public void ReadFile(CNT_File file, Action<BinaryDeserializer> readAction, bool logIfNotFullyRead = true)
        {
            BinaryDeserializer s = Context.Deserializer;

            // Get the file start offset
            Pointer startOffset = Offset + file.FileOffset;

            // Go to the file offset
            s.Goto(startOffset);

            // Read using the xor key
            s.DoXOR(new XORArrayCalculator(file.FileXORKey), () => readAction(s));

            if (logIfNotFullyRead && s.CurrentPointer != startOffset + file.FileSize)
                s.LogWarning($"File {file.FileName} was not fully read");
        }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize the directory and file array sizes
            Directories = s.SerializeArraySize<string, int>(Directories, name: nameof(Directories));
            Files = s.SerializeArraySize<CNT_File, int>(Files, name: nameof(Files));

            // Serialize header info
            IsXORUsed = s.Serialize<bool>(IsXORUsed, name: nameof(IsXORUsed));
            IsChecksumUsed = s.Serialize<bool>(IsChecksumUsed, name: nameof(IsChecksumUsed));
            StringsXORKey = s.Serialize<byte>(StringsXORKey, name: nameof(StringsXORKey));

            // Serialize the directory paths and the checksum afterwards
            s.DoChecksum<byte>(new Checksum8Calculator(), () =>
            {
                byte key = IsXORUsed ? StringsXORKey : (byte)0;

                for (int i = 0; i < Directories.Length; i++)
                    Directories[i] = s.SerializeObject<CNT_String>(Directories[i], x => x.Pre_XORKey = key, name: $"{nameof(Directories)}[{i}]");
            }, ChecksumPlacement.After, name: "DirectoriesChecksum");

            // Serialize the file info
            Files = s.SerializeObjectArray<CNT_File>(Files, Files.Length, x => x.Pre_FileNameXORKey = IsXORUsed ? StringsXORKey : (byte)0, name: nameof(Files));
        }
    }
}