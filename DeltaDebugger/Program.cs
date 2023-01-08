using System.Diagnostics;

namespace DeltaDebugger;

public class Program
{
   private const string PARSER_PATH = "../../../WCNFParser.exe";
   private const string INPUT_FILE_PATH = "../../../TestInput.txt";
   private const string OUTPUT_FILE_PATH = "../../../TestOutput.txt";

   public static void Main(string[] args)
   {
      // save file for rollback purposes
      var testFile = File.ReadAllLines(INPUT_FILE_PATH);

      // start first iteration with the whole file
      var parserProcess = Process.Start(PARSER_PATH, INPUT_FILE_PATH);
      parserProcess.WaitForExit();
      var programExitCode = parserProcess.ExitCode;

      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("---------------------------------------------------------------");
      Console.WriteLine("                    Delta debugger v0.0.1                      ");
      Console.WriteLine("---------------------------------------------------------------");

      // execute parser as long as errors occurres
      while (programExitCode != 0)
      {
         // minimal file reached
         if (testFile.Length == 1)
         {
            break;
         }

         // try first half of file
         var firstHalf = testFile.Take(testFile.Length / 2);

         // save to disk for the parser to read
         File.WriteAllLines(OUTPUT_FILE_PATH, firstHalf);

         Console.ForegroundColor = ConsoleColor.White;
         Console.WriteLine($"> Trying to reduce input file with the first half...");

         // run parser
         parserProcess = Process.Start(PARSER_PATH, OUTPUT_FILE_PATH);
         parserProcess.WaitForExit();
         programExitCode = parserProcess.ExitCode;

         // error still occurres, so reduction is successful
         if (programExitCode != 0)
         {            
            // update backup file with first half
            testFile = firstHalf.ToArray();
         }
         // error is gone, so rollback reduction
         else
         {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("> Reduction of file was not successful!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("> Trying to reduce input file with the second half...");
            
            // handle odd and even file length
            var amount = testFile.Length % 2 == 0 ? testFile.Length / 2 : (testFile.Length / 2) + 1;

            // try second half of file
            var secondhalf = testFile.Skip(testFile.Length / 2).Take(amount);

            // update backup file with second half
            testFile = secondhalf.ToArray();

            // save to disk for the parser to read
            File.WriteAllLines(OUTPUT_FILE_PATH, secondhalf);

            // retry execution with second half
            programExitCode = 1;
         }

         Console.ForegroundColor = ConsoleColor.Green;
         Console.WriteLine($"> File successfully reduced. New file length: {testFile.Length}");
      }

      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine(">");
      Console.WriteLine("> Minimal file size reached or could not detect any faulty input");
      Console.WriteLine(">");

      Console.ReadKey();
   }
}