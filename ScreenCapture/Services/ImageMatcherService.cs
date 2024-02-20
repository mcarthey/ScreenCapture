using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ScreenCapture.Extensions;

namespace ScreenCapture.Services;

public class ImageMatcherService
{
    /// <summary>
    /// Finds the location of a target image within a source image.
    /// </summary>
    /// <param name="sourceBitmap">The source image to search within.</param>
    /// <param name="targetBitmap">The target image to find within the source image.</param>
    /// <returns>The top-left corner of the area in the source image that best matches the target image, or null if no match is found.</returns>
    public static Point? FindTargetInSource(Bitmap sourceBitmap, Bitmap targetBitmap)
    {
        // Convert the source and target bitmaps to Image<Bgr, byte> objects, which are used by the Emgu.CV library for image processing.
        using (Image<Bgr, byte> sourceImage = sourceBitmap.ToImage())
            using (Image<Bgr, byte> targetImage = targetBitmap.ToImage())
            {
                // Perform template matching to find the location in the source image that best matches the target image.
                using (Image<Gray, float> matchResult = sourceImage.MatchTemplate(targetImage, TemplateMatchingType.CcorrNormed))
                {
                    // Initialize arrays to hold the minimum and maximum matching scores and their locations.
                    double[] minMatchValues, maxMatchValues;
                    Point[] minMatchLocations, maxMatchLocations;

                    // Find the minimum and maximum matching scores and their locations.
                    matchResult.MinMax(out minMatchValues, out maxMatchValues, out minMatchLocations, out maxMatchLocations);
                    matchResult.Save("matchResult.png");

                    // If the maximum matching score is above a certain threshold, return the location of the maximum score.
                    // This location is the top-left corner of the area in the source image that best matches the target image.
                    double matchingThreshold = 0.8; // Threshold for matching, can be adjusted
                    if (maxMatchValues[0] > matchingThreshold)
                    {
                        // Draw a rectangle around the area in the source image that best matches the target image.
                        Rectangle matchRect = new Rectangle(maxMatchLocations[0], targetBitmap.Size);
                        using (Graphics g = Graphics.FromImage(sourceBitmap))
                        {
                            g.DrawRectangle(new Pen(Color.LimeGreen, 2), matchRect);
                        }
                        sourceBitmap.Save("bestMatch.png");

                        return maxMatchLocations[0];
                    }
                }
            }

        // If no match with a score above the threshold is found, return null.
        return null;
    }

    public static Point? FindTargetInSourceOrb(Bitmap sourceBitmap, Bitmap targetBitmap)
    {
        // Convert the source and target bitmaps to Image<Gray, byte> objects.
        using (Image<Bgr, byte> sourceImage = sourceBitmap.ToImage())
        using (Image<Bgr, byte> targetImage = targetBitmap.ToImage())
        {
            // Initialize the ORB detector and the BFMatcher.
            using (var orbDetector = new ORB())
            using (var matcher = new BFMatcher(DistanceType.Hamming))
            {
                // Detect keypoints and compute descriptors for both images.
                var sourceKeyPoints = new VectorOfKeyPoint();
                var targetKeyPoints = new VectorOfKeyPoint();
                var sourceDescriptors = new Mat();
                var targetDescriptors = new Mat();

                orbDetector.DetectAndCompute(sourceImage, null, sourceKeyPoints, sourceDescriptors, false);
                orbDetector.DetectAndCompute(targetImage, null, targetKeyPoints, targetDescriptors, false);

                // Match descriptors between the source and target images.
                var matches = new VectorOfDMatch();
                matcher.Match(targetDescriptors, sourceDescriptors, matches);

                // Find the best match (you can also use more sophisticated methods to find a better match or validate matches).
                if (matches.Size > 0)
                {
                    var bestMatch = matches[0];
                    var bestMatchIndex = bestMatch.TrainIdx;
                    var bestMatchPoint = sourceKeyPoints[bestMatchIndex].Point;

                    // Draw a rectangle around the best match in the source image.
                    Rectangle matchRect = new Rectangle(new Point((int)bestMatchPoint.X, (int)bestMatchPoint.Y), targetBitmap.Size);
                    using (Graphics g = Graphics.FromImage(sourceBitmap))
                    {
                        g.DrawRectangle(new Pen(Color.LimeGreen, 2), matchRect);
                    }
                    sourceBitmap.Save("bestMatch.png");

                    return new Point((int)bestMatchPoint.X, (int)bestMatchPoint.Y);
                }
            }
        }

        // If no match is found, return null.
        return null;
    }

}