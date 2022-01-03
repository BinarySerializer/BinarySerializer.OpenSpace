using System;
using System.IO;

namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The data used for a file entry within a CNT archive
    /// </summary>
    public class CNT_File : BinarySerializable
    {
        #region Constructor

        public CNT_File() => FileXORKey = new byte[4];

        #endregion

        #region Public Properties

        public byte Pre_FileNameXORKey { get; set; }

        public int DirectoryIndex { get; set; } // -1 if it's root
        public string FileName { get; set; } // Including the extension
        public byte[] FileXORKey { get; set; }
        public uint FileChecksum { get; set; }
        public uint FileOffset { get; set; }
        public uint FileSize { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the full path for the file from the list of available directories
        /// </summary>
        /// <param name="directories">The directories</param>
        /// <returns>The full path of the file</returns>
        public string GetFullPath(string[] directories)
        {
            return Path.Combine(DirectoryIndex == -1 ? String.Empty : directories[DirectoryIndex], FileName);
        }

        public override void SerializeImpl(SerializerObject s)
        {
            DirectoryIndex = s.Serialize<int>(DirectoryIndex, name: nameof(DirectoryIndex));
            FileName = s.SerializeObject<CNT_String>(FileName, x => x.Pre_XORKey = Pre_FileNameXORKey, name: nameof(FileName));
            FileXORKey = s.SerializeArray<byte>(FileXORKey, 4, name: nameof(FileXORKey));
            FileChecksum = s.Serialize<uint>(FileChecksum, name: nameof(FileChecksum));
            FileOffset = s.Serialize<uint>(FileOffset, name: nameof(FileOffset));
            FileSize = s.Serialize<uint>(FileSize, name: nameof(FileSize));
        }

        public override string ToString()
        {
            return $"{DirectoryIndex}: {FileName}";
        }

        #endregion
    }
}