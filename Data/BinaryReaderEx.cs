using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PBT.DowsingMachine.Data
{
    public class BinaryReaderEx : BinaryReader
    {
        public bool IsBigEndian { get; set; }
        public int Alignment { get; set; } = 1;

        public BinaryReaderEx(Stream input) : base(input)
        {
        }

        public BinaryReaderEx(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public BinaryReaderEx(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public void Align()
        {
            var remainder = BaseStream.Position % Alignment;
            if (remainder > 0)
            {
                BaseStream.Seek(Alignment - remainder, SeekOrigin.Current);
            }
        }

        public string PeekString(int length)
        {
            var chars = ReadChars(length);
            var text = new string(chars);
            BaseStream.Seek(-length, SeekOrigin.Current);
            return text;
        }

        public int ReadInt24()
        {
            var uint24 = ReadUInt24();
            if(uint24 > 0x7FFFFF)
            {
                return (int)(uint24 - 0x7FFFFF);
            }
            else
            {
                return (int)uint24;
            }
        }

        public uint ReadUInt24()
        {
            if (IsBigEndian)
            {
                return (uint)((ReadByte() << 0) | (ReadByte() << 8) | (ReadByte() << 16));
            }
            else
            {
                return (uint)((ReadByte() << 16) | (ReadByte() << 8) | (ReadByte() << 0));
            }
        }

        public override short ReadInt16()
        {
            return IsBigEndian
                ? BinaryPrimitives.ReadInt16BigEndian(ReadBytes(sizeof(short)))
                : base.ReadInt16();
        }

        public override int ReadInt32()
        {
            return IsBigEndian
                ? BinaryPrimitives.ReadInt32BigEndian(ReadBytes(sizeof(int)))
                : base.ReadInt32();
        }

        public override long ReadInt64()
        {
            return IsBigEndian
                ? BinaryPrimitives.ReadInt64BigEndian(ReadBytes(sizeof(long)))
                : base.ReadInt64();
        }

        public override ushort ReadUInt16()
        {
            return IsBigEndian
                ? BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(sizeof(ushort)))
                : base.ReadUInt16();
        }

        public override uint ReadUInt32()
        {
            return IsBigEndian
                ? BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(sizeof(uint)))
                : base.ReadUInt32();
        }

        public override ulong ReadUInt64()
        {
            return IsBigEndian
                ? BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(sizeof(ulong)))
                : base.ReadUInt64();
        }

    }
}
