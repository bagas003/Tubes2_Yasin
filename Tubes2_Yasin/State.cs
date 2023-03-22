using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes2_Yasin
{
    public class State
    {
        public Tuple<int, int> start = null, current = null;
        public int TCounts = 0, TFounds = 0;
        public char[,] map = null;
        public bool[,] visited = null;
        public Tuple<int, int> [,] parent = null;
        public List<Tuple<int, int>> path = new List<Tuple<int, int>>();
        public string route = "";

        public int[] dx = { 1, 0, -1, 0 };
        public int[] dy = { 0, 1, 0, -1 };

    }
}
