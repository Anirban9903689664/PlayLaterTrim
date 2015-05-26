using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.IO;
using System.Diagnostics;

namespace PlayLaterTrim
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string directoryPath = @"C:\Users\Public\Videos\PlayLater\PlayLaterTrim";
                string[] fileEntries = Directory.GetFiles(directoryPath);
                foreach (string filePath in fileEntries)
                    ProcessFile(filePath);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public static void ProcessFile(string filePath)
        {
            try
            {
                using (var engine = new Engine())
                {
                    var inputFile = new MediaFile { Filename = filePath };
                    engine.GetMetadata(inputFile);
                    if (inputFile.Metadata != null)
                    {
                        string command = @"C:\ffmpeg\bin\ffmpeg.exe";
                        string args = String.Format(@"-i ""{0}"" -ss 00:00:05 -t {1} -vcodec copy -acodec copy ""{2}"" -y", filePath, (inputFile.Metadata.Duration - TimeSpan.FromSeconds(10)).ToString(@"hh\:mm\:ss"), @"C:\Users\Public\Videos\" + Path.GetFileName(filePath));
                        Log(args);

                        Process process = new Process();
                        process.StartInfo.FileName = command;
                        process.StartInfo.Arguments = args;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                        process.WaitForExit(600000);
                        File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public static void Log(string line)
        {
            string filePath = @"Log.txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(DateTime.Now.ToString() + Environment.NewLine);
                writer.WriteLine(line);
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
        
        public static void LogError(Exception ex)
        {
            string filePath = @"Error.txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
}