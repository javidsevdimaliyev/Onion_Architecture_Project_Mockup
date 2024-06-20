using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;
using ZXing;
using ZXing.Common;
using ZXing.OneD;
using ZXing.QrCode;
using ZXing.Rendering;

namespace SolutionName.Application.Utilities.Utility;

public class BarcodeQrcodeGenerator
{
    public static byte[] CreateQRCodeWithQrCoder(string text)
    {
        QRCodeGenerator generator = new();
        QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new(data);
        byte[] byteGraphic = qrCode.GetGraphic(10, new byte[] { 84, 99, 71 }, new byte[] { 240, 240, 240 });
        return byteGraphic;
    }

    public static SvgRenderer.SvgImage CreateBarcode(string data, int width = 74, int height = 40)
    {
        try
        {
            if (!string.IsNullOrEmpty(data))
                try
                {
                    var writer = new BarcodeWriterSvg
                    {
                        Format = BarcodeFormat.CODABAR,
                        Encoder = new CodaBarWriter(),
                        Options = new EncodingOptions
                        {
                            Height = width,
                            Width = height,
                            PureBarcode = true
                        }
                    };
                    var result = writer.Write(data);
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }

            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }
 
    public static SvgRenderer.SvgImage CreateQrCode(string data, int width = 100, int height = 100)
    {
        if (string.IsNullOrEmpty(data)) return null;

        try
        {
            var writer = new BarcodeWriterSvg
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions()
                {
                    Height = height,
                    Width = width
                }
            };
            var result = writer.Write(data);


            return result;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public static byte[] CreateBarcodeImage(string data, int width = 74, int height = 30)
    {
        try
        {
            if (!string.IsNullOrEmpty(data))
            {
                var writer = new BarcodeWriterGeneric
                {
                    Format = BarcodeFormat.CODABAR,
                    Encoder = new CodaBarWriter(),
                    Options = new EncodingOptions
                    {
                        Height = height,
                        Width = width,
                        PureBarcode = true
                    }
                };
                var matrix = writer.Encode(data);
                var result = BitMatrixToBitMap(matrix);
                using (var stream = new MemoryStream())
                {
                    result.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }

            return new byte[0];
        }
        catch (Exception e)
        {
            return new byte[0];
        }
    }

    public static byte[] CreateQrCodeImage(string data, int width = 100, int height = 100)
    {
        try
        {
            Console.WriteLine();
            if (!string.IsNullOrEmpty(data))
            {
                var writer = new BarcodeWriterGeneric
                {
                    Format = BarcodeFormat.QR_CODE,
                    Encoder = new QRCodeWriter(),
                    Options = new QrCodeEncodingOptions
                    {
                        Height = height,
                        Width = width
                    }
                };
                var matrix = writer.Encode(data);
                var result = BitMatrixToBitMap(matrix);
                using (var stream = new MemoryStream())
                {
                    result.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }


            return new byte[0];
        }
        catch (Exception e)
        {
            return new byte[0];
        }
    }





    public static string CreateBarcodeNumber(string year, string officeCode, string registerTypeNumber,
        string registerNumber)
    {
        var regNumber = registerNumber.PadLeft(6, '0');
        var regTypeNumber = registerTypeNumber.PadLeft(2, '0');
        var code = officeCode.PadLeft(3, '0');
        return year + code + regTypeNumber + regNumber;
    }

    public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
    {
        var result = new Bitmap(width, height);
        using (var g = Graphics.FromImage(result))
        {
            g.DrawImage(bmp, 0, 0, width, height);
        }

        return result;
    }

    public static Bitmap BitMatrixToBitMap(BitMatrix matrix)
    {
        var width = matrix.Width;
        var height = matrix.Height;

        var bmp = new Bitmap(width, height);
        var gg = Graphics.FromImage(bmp);
        gg.Clear(Color.White);

        for (var x = 0; x < width - 1; x++)
            for (var y = 0; y < height - 1; y++)
                if (matrix[x, y])
                    gg.FillRectangle(Brushes.Black, x, y, 1, 1);
                else
                    gg.FillRectangle(Brushes.White, x, y, 1, 1);

        return bmp;
    }


    public static byte[] ReadToEnd(Stream stream)
    {
        long originalPosition = 0;

        if (stream.CanSeek)
        {
            originalPosition = stream.Position;
            stream.Position = 0;
        }

        try
        {
            var readBuffer = new byte[4096];

            var totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    var nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        var temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            var buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }

            return buffer;
        }
        finally
        {
            if (stream.CanSeek) stream.Position = originalPosition;
        }
    }


    public static int GetFileSizeInKBs(byte[] contentBytes)
    {
        return contentBytes.Length / 1024 + (contentBytes.Length % 1024 == 0 ? 0 : 1);
    }

    public static bool WriteToFile<T>(T obj, string fileName)
    {
        try
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            var json = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(fileName, json);
        }
        catch (Exception e)
        {
            throw new Exception("It was not possible to write to the file", e);
        }

        return true;
    }

    public static T ReadFromFile<T>(string fileName)
    {
        try
        {
            var json = File.ReadAllText(fileName);
            var myObject = JsonSerializer.Deserialize<T>(json);
            return myObject;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("It was not possible to read the file.");
        }
    }

    public static void WriteTextFile(string fileName, string text)
    {
        if (File.Exists(fileName))
            using (var sw = File.AppendText(fileName))
            {
                sw.WriteLine(text);
            }
        else
            using (var sw = File.CreateText(fileName))
            {
                sw.WriteLine(text);
            }
    }
}