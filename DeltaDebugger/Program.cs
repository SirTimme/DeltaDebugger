using System.Diagnostics;

namespace DeltaDebugger;

public class Program
{
   private const string ParserPath = "../../../WCNFParser.exe";
   private const string TEST_FILE_PATH = "../../../TestInput.txt";

   public static void Main(string[] args)
   {
      // save file for rollback purposes
      var testFile = File.ReadAllLines(TEST_FILE_PATH);

      // start first iteration with the whole file
      var parserProcess = Process.Start(ParserPath, TEST_FILE_PATH);
      parserProcess.WaitForExit();
      var programExitCode = parserProcess.ExitCode;

      // execute parser as long as errors occurres
      while (programExitCode != 0)
      {
         Console.ForegroundColor = ConsoleColor.White;
         Console.WriteLine("> Attempt to reduce the input file");

         // try first half of file
         var firsthalf = testFile.Take(testFile.Length / 2);

         // file already consists of a single line
         if (!firsthalf.Any())
         {
            break;
         }

         // save first half to disk for the parser to read
         File.WriteAllLines(TEST_FILE_PATH, firsthalf);

         parserProcess = Process.Start(ParserPath, TEST_FILE_PATH);
         parserProcess.WaitForExit();
         programExitCode = parserProcess.ExitCode;

         // error still occurres, so reduction is successful
         if (programExitCode != 0)
         {            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"> File successfully reduced. New file length: {testFile.Length}");
         }
         // error is gone, so rollback reduction
         else
         {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("> Error is no longer present undo reduction");
            
            // handle odd and even file length
            var amount = testFile.Length % 2 == 0 ? testFile.Length / 2 : (testFile.Length / 2) + 1;

            // try second half of file
            var secondhalf = testFile.Skip(testFile.Length / 2).Take(amount);
            File.WriteAllLines(TEST_FILE_PATH, secondhalf);

            // start parser with second half
            parserProcess = Process.Start(ParserPath, TEST_FILE_PATH);
            parserProcess.WaitForExit();
            programExitCode = parserProcess.ExitCode;
         }

         // persist data for next iteration
         testFile = File.ReadAllLines(TEST_FILE_PATH);
      }

      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine("> Minimal file size reached or could not detect any faulty input");

      Console.ReadKey();
   }
}