using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelFun.Shared
{
    public class StartEndPoints 
    {
        public int Start { get; set; }

        public int End { get; set; }

        public byte[] StartByte { get; set; } = new byte[] { 0x0D, 0x0A };

        public string Category { get; set; }

        public GameType GameType { get; set; }
    }

    public enum GameType {
        Puzzle,
        Clue
    }

    public class Helpers
    {
        public static List<StartEndPoints> GetDefaultGameLists() {
            var list = new List<StartEndPoints>();
            list.Add(new StartEndPoints { Category = "Artist & Song", Start = 40, End = 4470, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Author & Title", Start = 4590, End = 6045, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Before & After", Start = 6165, End = 47107, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Classic TV", Start = 47290, End = 55570, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Clue", Start = 55865, End = 58053, GameType = GameType.Clue});
            list.Add(new StartEndPoints { Category = "Event", Start = 58140, End = 78670, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Fictional Character", Start = 78790, End = 86020, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Fictional Characters", Start = 86140, End = 92495, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Fill In The Blank", Start = 92615, End = 103010, GameType = GameType.Clue});
            list.Add(new StartEndPoints { Category = "Foreign Words", Start = 102940, End = 108087, GameType = GameType.Clue});
            list.Add(new StartEndPoints { Category = "Husband & Wife", Start = 108190, End = 109820, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Landmark", Start = 110115, End = 112270, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Next Line Please", Start = 112390, End = 114127, GameType = GameType.Clue});
            list.Add(new StartEndPoints { Category = "Nickname", Start = 114140, End = 115945, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Occupation", Start = 116065, End = 125395, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "People", Start = 125515, End = 132920, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Person", Start = 133040, End = 144120, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Phrase", Start = 144240, End = 195745, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Place", Start = 195865, End = 213595, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Places", Start = 213715, End = 214470, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Proper Name", Start = 214590, End = 226895, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Quotation", Start = 227015, End = 232495, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Same Name", Start = 232615, End = 255770, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Show Biz", Start = 255890, End = 266620, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Slogan", Start = 266740, End = 272078, GameType = GameType.Clue});
            list.Add(new StartEndPoints { Category = "Song/Artist", Start = 272165, End = 272920, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Star and Role", Start = 273040, End = 276770, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "The 60's", Start = 276890, End = 278170, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "The 70's", Start = 278290, End = 279920, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "The 80's", Start = 280040, End = 281670, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "The 90's", Start = 281790, End = 282370, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Thing", Start = 282490, End = 309145, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Things", Start = 309265, End = 330845, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Title", Start = 330965, End = 338895, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Title/Author", Start = 339015, End = 341520, GameType = GameType.Puzzle});
            list.Add(new StartEndPoints { Category = "Where Are We", Start = 341640, End = 345401, GameType = GameType.Clue});
            list.Add(new StartEndPoints { Category = "Who Is It?", Start = 345490, End = 347863, GameType = GameType.Clue});
            list.Add(new StartEndPoints { Category = "Who Said it?", Start = 347940, End = 349775, GameType = GameType.Clue});
            return list;
        }

        public static byte[] ReplaceByteWithByte(byte[] src, byte search, byte repl) {
            var byteList = new List<byte>();
            foreach(var oldByte in src) {
                if (oldByte == search) {
                    byteList.Add(repl);
                } else {
                    byteList.Add(oldByte);
                }
            }
            return byteList.ToArray();
        }

        public static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index>=0)
            {
                dst = new byte[src.Length - search.Length + repl.Length];
                // before found array
                Buffer.BlockCopy(src,0,dst,0, index);
                // repl copy
                Buffer.BlockCopy(repl,0,dst,index,repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src, 
                    index+search.Length , 
                    dst, 
                    index+repl.Length, 
                    src.Length-(index+search.Length));
            }
            return dst;
        }

        public static int FindIndex(byte[] src, byte[] find, int startFrom) {
            var test = src.Skip(startFrom).ToArray();
            return FindBytes(test, find) + startFrom;
        }

        public static byte[] CreateQuestionByteArray(string question) {
            var oldTestQuestion = Encoding.ASCII.GetBytes(question);
            return ReplaceByteWithByte(oldTestQuestion, 32, 0);
        }

        public static string ConvertToString(byte[] question) {
            var questionNew = ReplaceByteWithByte(question, 0, 32);
            return Encoding.ASCII.GetString(questionNew);
        }

        public static int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for(int i=0; i<src.Length; i++)
            {
                if(src[i] == find[matchIndex])
                {
                    if (matchIndex==(find.Length-1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }
    }
}
