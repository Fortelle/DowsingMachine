using System;
using System.Collections.Generic;
using System.IO;

namespace PBT.DowsingMachine.FileFormats;

public class MatrixPack
{
    public byte[][] Entries;

    public MatrixPack(byte[] data, int itemSize, int itemCount, int offset)
    {
        if(itemCount == -1 ) itemCount = data.Length / itemSize;
        using var ms = new MemoryStream(data);
        using var br = new BinaryReader(ms);
        Read(br, itemSize, itemCount, offset);
    }

    public MatrixPack(byte[] data, int itemSize) : this(data, itemSize, data.Length / itemSize, 0)
    {
    }

    public MatrixPack(byte[] data, int itemSize, int itemCount) : this(data, itemSize, itemCount, 0)
    {
    }

    public MatrixPack(BinaryReader br, int itemSize) : this(br, itemSize, (int)(br.BaseStream.Length / itemSize), 0)
    {
    }

    public MatrixPack(BinaryReader br, int itemSize, int itemCount) : this(br, itemSize, itemCount, 0)
    {
    }

    public MatrixPack(BinaryReader br, int itemSize, int itemCount, int offset)
    {
        if (itemCount == -1) itemCount = (int)(br.BaseStream.Length / itemSize);
        Read(br, itemSize, itemCount, offset);
    }

    private void Read(BinaryReader br, int itemSize, int itemCount, int offset)
    {
        if(offset > 0)
        {
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        var entries = new List<byte[]>();
        for (int i = 0; i < itemCount; i++)
        {
            var bytes = br.ReadBytes(itemSize);
            entries.Add(bytes);
        }

        Entries = entries.ToArray();
    }

    public static byte[][] From(BinaryReader br, int itemSize, int itemCount)
    {
        return new MatrixPack(br, itemSize, itemCount).Entries;
    }
}
