namespace BinarySerializer.OpenSpace
{
    public class IdObjInfo : BinarySerializable
    {
        public uint Pre_Version { get; set; }
        public Pointer Pre_BaseOffset { get; set; }

        public uint ClassNameLength { get; set; }
        public string ClassName { get; set; }
        public CUUID Id { get; set; }
        public Pointer ObjPointer { get; set; }
        public uint ObjSize { get; set; }
        public bool Unknown { get; set; }
        public RefObjectCont RefObjectCont { get; set; }

        public uint IdObjClassNameLength { get; set; }
        public string IdObjClassName { get; set; }
        public IdObj IdObj { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ClassNameLength = s.Serialize<uint>(ClassNameLength, name: nameof(ClassNameLength));
            ClassName = s.SerializeString(ClassName, length: ClassNameLength, name: nameof(ClassName));
            Id = s.SerializeObject<CUUID>(Id, name: nameof(Id));
            ObjPointer = s.SerializePointer(ObjPointer, anchor: Pre_BaseOffset, name: nameof(ObjPointer));
            ObjSize = s.Serialize<uint>(ObjSize, name: nameof(ObjSize));
            Unknown = s.SerializeBoolean<uint>(Unknown, name: nameof(Unknown));

            if (Pre_Version >= 2)
                RefObjectCont = s.SerializeObject<RefObjectCont>(RefObjectCont, x => x.Pre_Version = Pre_Version, name: nameof(RefObjectCont));

            s.DoAt(ObjPointer, () =>
            {
                IdObjClassNameLength = s.Serialize<uint>(IdObjClassNameLength, name: nameof(IdObjClassNameLength));
                IdObjClassName = s.SerializeString(IdObjClassName, length: IdObjClassNameLength, name: nameof(IdObjClassName));

                // TODO: Support remaining types
                switch (IdObjClassName)
                {
                    case PCWavResData.ClassName:
                        IdObj = s.SerializeObject<PCWavResData>((PCWavResData)IdObj, name: nameof(IdObj));
                        break;
                    
                    case EventResData.ClassName:
                        IdObj = s.SerializeObject<EventResData>((EventResData)IdObj, name: nameof(IdObj));
                        break;
                    
                    case RandomResData.ClassName:
                        IdObj = s.SerializeObject<RandomResData>((RandomResData)IdObj, name: nameof(IdObj));
                        break;
                    
                    case PCWaveFileIdObj.ClassName:
                        IdObj = s.SerializeObject<PCWaveFileIdObj>((PCWaveFileIdObj)IdObj, name: nameof(IdObj));
                        break;
                    
                    default:
                        s.SystemLogger?.LogWarning("Unsupported class type {0}", IdObjClassName);
                        IdObj = s.SerializeObject<RawIdObj>((RawIdObj)IdObj, x => x.Pre_Length = ObjSize - (4 + IdObjClassNameLength + 8), name: nameof(IdObj));
                        break;
                }

                // Verify object size
                long serializedSize = s.CurrentFileOffset - ObjPointer.FileOffset;
                if (serializedSize != ObjSize)
                {
                    s.SystemLogger?.LogWarning("Incorrect object length ({0} != {1})", serializedSize, ObjSize);

                    if (serializedSize < ObjSize)
                        s.SerializeArray<byte>(null, ObjSize - serializedSize, name: "RemainingData");
                }
            });

            s.Log($"Object with ID {Id} and class {ClassName}");
        }
    }
}