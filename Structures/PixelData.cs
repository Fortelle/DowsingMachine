using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PBT.DowsingMachine.Structures
{
    public class PixelData
    {
        public byte[] Source;
        public int Width;
        public int Height;

        public PixelData(byte[] source, int stroke)
        {
            Source = source;
            Width = stroke;
            Height = source.Length / 4 / stroke;
        }

        public PixelData(byte[] source, int width, int height)
        {
            Source = source;
            Width = width;
            Height = height;
        }

        public PixelData(Bitmap bmp)
        {
            var bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int byteCount = bitmapData.Stride * bmp.Height;
            var pixels = new byte[byteCount];
            Marshal.Copy(bitmapData.Scan0, pixels, 0, pixels.Length);
            bmp.UnlockBits(bitmapData);

            Source = pixels;
            Width = bmp.Width;
            Height = bmp.Height;
        }

        public int GetOffset(int x, int y)
        {
            return (y * Width + x) * 4;
        }

        public Color GetColor(int x, int y)
        {
            var i = GetOffset(x, y);
            var color = Color.FromArgb(Source[i + 3], Source[i + 2], Source[i + 1], Source[i + 0]);
            return color;
        }

        public static IEnumerable<int> Iterator(Rectangle rect)
        {
            for (var y = rect.Top; y < rect.Bottom; y++)
            {
                for (var x = rect.Left; x < rect.Right; x++)
                {
                    yield return (y * rect.Width + x) * 4;
                }
            }
        }

        public IEnumerable<int> Iterator()
        {
            for (int i = 0; i < Source.Length; i += 4)
            {
                yield return i;
            }
        }

        public IEnumerable<TResult> ColorIterator<TResult>(Func<byte, byte, byte, byte, TResult> selector)
        {
            for (int i = 0; i < Source.Length; i += 4)
            {
                yield return selector(Source[i + 2], Source[i + 1], Source[i], Source[i + 3]);
            }
        }

        public IEnumerable<TResult> ColorIterator<TResult>(Func<byte, byte, byte, TResult> selector)
        {
            for (int i = 0; i < Source.Length; i += 4)
            {
                yield return selector(Source[i + 2], Source[i + 1], Source[i]);
            }
        }

        public void ColorIterator(Action<byte, byte, byte> selector)
        {
            for (int i = 0; i < Source.Length; i += 4)
            {
                selector(Source[i + 2], Source[i + 1], Source[i]);
            }
        }

        public void ColorIterator(Action<byte, byte, byte, byte> selector)
        {
            for (int i = 0; i < Source.Length; i += 4)
            {
                selector(Source[i + 2], Source[i + 1], Source[i], Source[i + 3]);
            }
        }

        public static PixelData FromBitmap(Bitmap bmp)
        {
            var bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int byteCount = bitmapData.Stride * bmp.Height;
            var pixels = new byte[byteCount];
            Marshal.Copy(bitmapData.Scan0, pixels, 0, pixels.Length);
            bmp.UnlockBits(bitmapData);

            var data = new PixelData(pixels, bmp.Width, bmp.Height);
            return data;
        }

        public Bitmap ToBitmap()
        {
            var bmp = new Bitmap(this.Width, this.Height);
            var bitmapData = bmp.LockBits(new Rectangle(0, 0, this.Width, this.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            int byteCount = bitmapData.Stride * bitmapData.Height;
            Marshal.Copy(this.Source, 0, bitmapData.Scan0, byteCount);
            bmp.UnlockBits(bitmapData);
            return bmp;
        }
    }
}
