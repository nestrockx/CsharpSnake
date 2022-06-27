using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpGame {

    /// <summary>
    /// class for storing data from file
    /// </summary>
    class Result
    {
        public string Name { get; set; }
        public int Res { get; set; }
    }

    /// <summary>
    /// rank file manager inheriting from file managment class
    /// </summary>
    class RankFileMG : FileMG
    {
        /// <summary>
        /// file name property
        /// </summary>
        private readonly string _fileName;

        /// <summary>
        /// contructor with inherited parameter
        /// </summary>
        /// <param name="fileName">file name</param>
        public RankFileMG(string fileName) : base(fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// sort rank
        /// </summary>
        public void SortRank()
        {
            var resultsList = new List<Result>();
            foreach (string x in File.ReadAllLines(_fileName))
            { 
                string[] str = x.Split(' ');
                Result res = new Result
                {
                    Name = str[0],
                    Res = int.Parse(str[1])
                };
                resultsList.Add(res);
            }
            var results = resultsList.ToArray();
            Array.Sort(results, (x, y) => y.Res.CompareTo(x.Res));
            File.WriteAllText(_fileName, string.Empty);
            foreach (var x in results)
            {
                using (StreamWriter w = File.AppendText(_fileName))
                {
                    w.WriteLine(x.Name + " " + x.Res);
                }
            }
        }

        /// <summary>
        /// display rank
        /// </summary>
        /// <param name="n"></param>
        public void DisplayRank(int n)
        {
            int i = 0;
            Console.Clear();
            Console.WriteLine("BEST SCORES:");
            foreach (string x in File.ReadAllLines(_fileName))
            {
                Console.WriteLine(x);
                i++;
                if (i == n)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// is score eligible to be ranked
        /// </summary>
        /// <param name="n">amount of rankables</param>
        /// <param name="points">points gathered</param>
        /// <returns>bool</returns>
        public bool IsScoreRankable(int n, int points)
        {
            int i = 0;
            foreach (string x in File.ReadAllLines(_fileName))
            {
                i++;
                if (i == n)
                {
                    string[] str = x.Split(' ');
                    if (str.Count() == 2)
                    {
                        if (points > int.Parse(str[1]))
                        {
                            return true;
                        }
                    }
                }
            }
            if (i < 10)
            {
                return true;
            }
            return false;
        }

    }
}
