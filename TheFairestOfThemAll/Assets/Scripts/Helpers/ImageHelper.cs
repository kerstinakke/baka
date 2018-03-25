/* made by Raimond-Hendrik Tunnel */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

public static class ImageHelper
{

    /// <summary>
    /// Converts a 24-bit texture to an EmguCV image of type T.
    /// </summary>
    public static Image<T, byte> ToImage<T>(Texture2D tex) where T: struct, IColor
    {
        return new Image<T, byte>(ToImageData(tex));
    }

    /// <summary>
    /// Converts a 32-bit texture to an EmguCV image of type T.
    /// </summary>
    public static Image<T, byte> ToImageA<T>(Texture2D tex) where T : struct, IColor
    {
        return new Image<T, byte>(ToImageData(tex, 4));
    }

    /// <summary>
    /// Converts a 24-bit texture to an EmguCV image of type Bgr.
    /// </summary>
    public static Image<Bgr, byte> ToImage(Texture2D tex)
    {
        return ToImage<Bgr>(tex);
    }

    /// <summary>
    /// Converts a 32-bit texture to an EmguCV image of type Bgra.
    /// </summary>
    public static Image<Bgra, byte> ToImageA(Texture2D tex)
    {
        return ToImageA<Bgra>(tex);
    }

    private static byte[,,] ToImageData(Texture2D tex, int depth = 3)
    {
        if (tex.mipmapCount > 1)
        {
            Debug.LogWarning(tex.name + " - Can not convert to an image a texture with mipmaps!");
        }
        if (tex.format != TextureFormat.RGB24 && tex.format != TextureFormat.RGBA32)
        {
            Debug.LogWarning(tex.name + " - Can not convert to an image a texture with a format " + tex.format + "! (only RGB24 is supported)");
            /*if (tex.alphaIsTransparency)
            {
                Debug.LogWarning(tex.name + " -Transparenct not supported!");
            }*/
        }

        var data = new byte[tex.width, tex.height, depth];
        byte[] dataLin = tex.GetRawTextureData();

        if (dataLin.GetLength(0) != data.Length)
        {
            Debug.LogWarning(tex.name + " -Texture (" + dataLin.GetLength(0) + ") and image (" + data.Length + ") byte counts do not match. Conversion failed.");
            return null;
        }

        for (int i = 0; i < dataLin.GetLength(0); i += depth)
        {
            int x = (i / depth) % tex.width;
            int y = (i / depth) / tex.width;
            data[x, y, 0] = dataLin[i + 0];
            data[x, y, 1] = dataLin[i + 1];
            data[x, y, 2] = dataLin[i + 2];
            if (depth > 3)
            {
                data[x, y, 3] = dataLin[i + 3];
            }
        }

        return data;
    }

    /// <summary>
    /// Converts a EmguCV image of type Bgr to a 24-bit texture.
    /// </summary>
    public static Texture2D ToTexture(Image<Bgr, byte> img, TextureFormat format = TextureFormat.RGB24, bool hasMipmaps = false)
    {
        if (img == null)
        {
            Debug.LogWarning("Can not convert 'null' to a texture!");
            return null;
        };

        Texture2D texCopy = new Texture2D(img.Data.GetLength(0), img.Data.GetLength(1), format, hasMipmaps);
        texCopy.LoadRawTextureData(ToTextureData(img.Data));

        return texCopy;
    }

    /// <summary>
    /// Converts a EmguCV image of type Bgr to a 32-bit texture.
    /// </summary>
    public static Texture2D ToTexture(Image<Bgra, byte> img, TextureFormat format = TextureFormat.RGBA32, bool hasMipmaps = false)
    {
        if (img == null)
        {
            Debug.LogWarning("Can not convert 'null' to a texture!");
            return null;
        };
        Debug.Log("aa");

        Texture2D texCopy = new Texture2D(img.Data.GetLength(0), img.Data.GetLength(1), format, hasMipmaps);
		//texCopy.alphaIsTransparency = true;
        texCopy.LoadRawTextureData(ToTextureData(img.Data));

        return texCopy;

    }

    private static byte[] ToTextureData(byte[,,] data)
    {
        byte[] dataLin = new byte[data.GetLength(0) * data.GetLength(1) * data.GetLength(2)];

        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                int i = (y * data.GetLength(2)) * data.GetLength(0) + (x * data.GetLength(2));
                dataLin[i + 0] = data[x, y, 0];
                dataLin[i + 1] = data[x, y, 1];
                dataLin[i + 2] = data[x, y, 2];
                if (data.GetLength(2) > 3)
                {
                    dataLin[i + 3] = data[x, y, 3];
                    Debug.Break();
                }
            }
        }

        return dataLin;
    }


}
