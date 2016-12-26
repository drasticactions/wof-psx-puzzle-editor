using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using WheelFun.Shared;

namespace WheelFun
{
    class MainClass
    {
        static void Main()
        {
            var puzzleBytes = File.ReadAllBytes("PUZZLES.BIN");
            var testQuestion = Helpers.CreateQuestionByteArray("  SLOWBEEF   AND DIABETUS    YAOI FAN      FICTION  ");
            var gameList = Helpers.GetDefaultGameLists();

            byte[] newQuestions = puzzleBytes;
           
            foreach(var game in gameList) {
                if (game.GameType == GameType.Clue) {
                    // We're skipping "Clue" games for now.
                    continue;
                }
                var startIndex = game.Start;
                while (true) {
                    startIndex = Helpers.FindIndex(newQuestions, game.StartByte, startIndex);
                    if (startIndex >= game.End) {
                        // We're out of the byte range for this category, break.
                        break;
                    }
                    // Each question is 52 bytes, so we're taking the question itself,
                    // And leaving the control bytes alone.
                    var endIndex = startIndex + 54;
                    var destArray = newQuestions.Skip(startIndex + 2).Take((endIndex - startIndex) - 2).ToArray();
                    newQuestions = Helpers.ReplaceBytes(newQuestions, destArray, testQuestion);
                    Console.WriteLine($"{startIndex} : {endIndex} : { endIndex - startIndex } : {newQuestions.Length} Written!");

                    // Start the next loop with the next control byte.
                    startIndex = startIndex + 1;
                }
            }

            File.WriteAllBytes("PUZZLES.BIN", newQuestions);
        }


    }
}
