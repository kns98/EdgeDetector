using System;
using System.Drawing;
using System.IO;

class EdgeDetectionApp
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: EdgeDetectionApp <input image path> <output image path>");
            return;
        }

        string inputImagePath = args[0];
        string outputImagePath = args[1];

        try
        {
            using (Bitmap inputImage = new Bitmap(inputImagePath))
            {
                Bitmap outputImage = SobelEdgeDetection(inputImage);
                outputImage.Save(outputImagePath);
                Console.WriteLine($"Edge detection completed. Result saved to {outputImagePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static Bitmap SobelEdgeDetection(Bitmap image)
    {
        Bitmap grayScaleImage = ConvertToGrayscale(image);
        Bitmap edgeImage = new Bitmap(grayScaleImage.Width, grayScaleImage.Height);

        int[,] xKernel = {
            { -1, 0, 1 },
            { -2, 0, 2 },
            { -1, 0, 1 }
        };

        int[,] yKernel = {
            {  1,  2,  1 },
            {  0,  0,  0 },
            { -1, -2, -1 }
        };

        for (int y = 1; y < grayScaleImage.Height - 1; y++)
        {
            for (int x = 1; x < grayScaleImage.Width - 1; x++)
            {
                float xGradient = 0;
                float yGradient = 0;

                for (int ky = -1; ky <= 1; ky++)
                {
                    for (int kx = -1; kx <= 1; kx++)
                    {
                        Color pixel = grayScaleImage.GetPixel(x + kx, y + ky);
                        int pixelBrightness = pixel.R; // Assuming grayscale, R=G=B
                        xGradient += pixelBrightness * xKernel[ky + 1, kx + 1];
                        yGradient += pixelBrightness * yKernel[ky + 1, kx + 1];
                    }
                }

                int gradientMagnitude = (int)Math.Min(Math.Sqrt(xGradient * xGradient + yGradient * yGradient), 255);
                edgeImage.SetPixel(x, y, Color.FromArgb(gradientMagnitude, gradientMagnitude, gradientMagnitude));
            }
        }

        return edgeImage;
    }

    private static Bitmap ConvertToGrayscale(Bitmap image)
    {
        Bitmap grayscale = new Bitmap(image.Width, image.Height);

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Color originalColor = image.GetPixel(x, y);
                int grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));
                Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);
                grayscale.SetPixel(x, y, grayColor);
            }
        }

        return grayscale;
    }
}
