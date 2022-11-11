namespace PBT.DowsingMachine.Utilities;

public class ColorUtil
{
    public record HSV(float H, float S, float V);
    public record HSL(float H, float S, float L);

    public static HSV RgbToHsv(Color color)
    {
        return new HSV(color.GetHue(), color.GetSaturation(), color.GetBrightness());
    }

    public static HSL RgbToHsl(Color rgb)
    {
        float h, s, l = 0;

        float r = rgb.R / 255.0f;
        float g = rgb.G / 255.0f;
        float b = rgb.B / 255.0f;

        float min = Math.Min(Math.Min(r, g), b);
        float max = Math.Max(Math.Max(r, g), b);
        float delta = max - min;

        l = (max + min) / 2;

        if (delta == 0)
        {
            h = 0.0f;
            s = 0.0f;
        }
        else
        {
            s = (l <= 0.5) ? (delta / (max + min)) : (delta / (2 - max - min));

            float hue;

            if (r == max)
            {
                hue = ((g - b) / 6) / delta;
            }
            else if (g == max)
            {
                hue = (1.0f / 3) + ((b - r) / 6) / delta;
            }
            else
            {
                hue = (2.0f / 3) + ((r - g) / 6) / delta;
            }

            while (hue < 0) hue += 1;
            while (hue > 1) hue -= 1;
            h = hue * 360;
        }

        return new HSL(h, s, l);
    }

    public static HSL HsvToHsl(HSV hsv)
    {
        var l = (2 - hsv.S / 100) * hsv.V / 2;
        var s = hsv.S * hsv.V / (l < 50 ? l * 2 : 200 - l * 2);
        return new HSL(hsv.H, s, l);
    }

    public static Color HsvToRgb(HSV hsv)
    {
        return HsvToRgb(hsv.H, hsv.S, hsv.V);
    }

    public static Color HsvToRgb(double hue, double saturation, double value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0)
            return Color.FromArgb(255, v, t, p);
        else if (hi == 1)
            return Color.FromArgb(255, q, v, p);
        else if (hi == 2)
            return Color.FromArgb(255, p, v, t);
        else if (hi == 3)
            return Color.FromArgb(255, p, q, v);
        else if (hi == 4)
            return Color.FromArgb(255, t, p, v);
        else
            return Color.FromArgb(255, v, p, q);
    }

    public static Color HslToRgb(HSL hsl)
    {
        return HslToRgb(hsl.H, hsl.S, hsl.L);
    }

    public static Color HslToRgb(double hue, double saturation, double lightness)
    {
        double red, green, blue;

        var h = hue / 360.0f;
        var s = saturation;
        var l = lightness;

        if (Math.Abs(s - 0.0) < double.Epsilon)
        {
            red = l;
            green = l;
            blue = l;
        }
        else
        {
            double var2;

            if (l < 0.5)
            {
                var2 = l * (1.0 + s);
            }
            else
            {
                var2 = l + s - s * l;
            }

            var var1 = 2.0 * l - var2;

            red = Hue2Rgb(var1, var2, h + 1.0 / 3.0);
            green = Hue2Rgb(var1, var2, h);
            blue = Hue2Rgb(var1, var2, h - 1.0 / 3.0);
        }

        var nRed = Convert.ToInt32(red * 255.0);
        var nGreen = Convert.ToInt32(green * 255.0);
        var nBlue = Convert.ToInt32(blue * 255.0);

        return Color.FromArgb(nRed, nGreen, nBlue);
    }

    private static double Hue2Rgb(
        double v1,
        double v2,
        double vH)
    {
        if (vH < 0.0)
        {
            vH += 1.0;
        }
        if (vH > 1.0)
        {
            vH -= 1.0;
        }
        if (6.0 * vH < 1.0)
        {
            return v1 + (v2 - v1) * 6.0 * vH;
        }
        if (2.0 * vH < 1.0)
        {
            return v2;
        }
        if (3.0 * vH < 2.0)
        {
            return v1 + (v2 - v1) * (2.0 / 3.0 - vH) * 6.0;
        }

        return v1;
    }

    public static Color Lighten(Color color, double scale)
    {
        var hsl = RgbToHsl(color);
        var l = hsl.L + (1 - hsl.L) * scale;

        var light = HslToRgb(hsl.H, hsl.S, l);
        return light;
    }

}
