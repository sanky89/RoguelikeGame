using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeGame
{
    public class MapConfiguration
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int MaxRooms { get; set; }
        public int MinRoomSize { get; set; }
        public int MaxRoomSize { get; set; }
        public int MaxMonstersCount{ get; set; }
        public int MaxItemsCount { get; set; }
        public string Layout { get; set; }
    }
}
