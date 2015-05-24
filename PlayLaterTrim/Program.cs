using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PlayLaterTrim
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"C:\PlayLater";
            string[] fileEntries = Directory.GetFiles(directoryPath);
            foreach (string filePath in fileEntries)
                ProcessFile(filePath);
        }

        public static void ProcessFile(string filePath)
        {
            var inputFile = new MediaFile { Filename = filePath };
            var outputFile = new MediaFile { Filename = @"C:\PlayLater\test" + @"\" + Path.GetFileName(filePath) };

            using (var engine = new Engine())
            {
                engine.ConvertProgressEvent += ConvertProgressEvent;
                engine.ConversionCompleteEvent += engine_ConversionCompleteEvent;
                engine.GetMetadata(inputFile);
                if (inputFile.Metadata != null)
                {
                    var options = new ConversionOptions();
                    options.CutMedia(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(inputFile.Metadata.Duration.TotalSeconds) - TimeSpan.FromSeconds(10));
                    engine.Convert(inputFile, outputFile, options);
                }
            }
        }

        private static void ConvertProgressEvent(object sender, ConvertProgressEventArgs e)
        {
            Console.WriteLine("\n------------\nConverting...\n------------");
            Console.WriteLine("Bitrate: {0}", e.Bitrate);
            Console.WriteLine("Fps: {0}", e.Fps);
            Console.WriteLine("Frame: {0}", e.Frame);
            Console.WriteLine("ProcessedDuration: {0}", e.ProcessedDuration);
            Console.WriteLine("SizeKb: {0}", e.SizeKb);
            Console.WriteLine("TotalDuration: {0}\n", e.TotalDuration);
        }

        private static void engine_ConversionCompleteEvent(object sender, ConversionCompleteEventArgs e)
        {
            Console.WriteLine("\n------------\nConversion complete!\n------------");
            Console.WriteLine("Bitrate: {0}", e.Bitrate);
            Console.WriteLine("Fps: {0}", e.Fps);
            Console.WriteLine("Frame: {0}", e.Frame);
            Console.WriteLine("ProcessedDuration: {0}", e.ProcessedDuration);
            Console.WriteLine("SizeKb: {0}", e.SizeKb);
            Console.WriteLine("TotalDuration: {0}\n", e.TotalDuration);
        }
    }
}