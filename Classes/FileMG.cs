using System;
using System.IO;

namespace CSharpGame
{
    /// <summary>
    /// file management class
    /// </summary>
    class FileMG
    {
        /// <summary>
        /// file name property
        /// </summary>
        private readonly string _fileName;

        /// <summary>
        /// create file if doesn't exist
        /// </summary>
        /// <param name="fileName"></param>
        public FileMG(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.OpenOrCreate);
            file.Close();
            _fileName = fileName;
        }

        /// <summary>
        /// write to file
        /// </summary>
        /// <param name="str">string</param>
        public void Write(string str)
        {
            File.WriteAllLines(_fileName, new[] { str });
        }

        /// <summary>
        /// write to file
        /// </summary>
        /// <param name="str">array of strings</param>
        public void Write(string[] str)
        {
            File.WriteAllLines(_fileName, str);
        }

        /// <summary>
        /// append to file
        /// </summary>
        /// <param name="str">string to append</param>
        public void Append(string str)
        {
            using (StreamWriter w = File.AppendText(_fileName))
            {
                w.WriteLine(str);
            }
        }

        /// <summary>
        /// display content od the file
        /// </summary>
        public void Display()
        {
            foreach (string x in File.ReadAllLines(_fileName))
            {
                Console.WriteLine(x);
            }
        }

    }
}
