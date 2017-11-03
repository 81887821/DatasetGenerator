using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatasetGenerator
{
    class Program
    {
        public const int NUMBER_OF_THREADS = 4;
        public const string DIRECTORY_TO_SAVE = @"S:\";
        public const string IMAGE_EXTTENSION = ".jpg";

        static void Main(string[] args)
        {
            Random random = new Random();
            Thread[] threads = new Thread[NUMBER_OF_THREADS];
            ImageGenerator[] imageGenerators = new ImageGenerator[NUMBER_OF_THREADS];

            for (int i = 0; i < NUMBER_OF_THREADS; i++)
            {
                imageGenerators[i] = new ImageGenerator(i);
                threads[i] = new Thread(imageGenerators[i].GenerateImages);
                threads[i].Start();
            }

            for (int i = 0; i < NUMBER_OF_THREADS; i++)
            {
                threads[i].Join();
            }

            using (StreamWriter writer = new StreamWriter(DIRECTORY_TO_SAVE + "label.txt"))
            {
                for (int i = 0; i < NUMBER_OF_THREADS; i++)
                {
                    Dictionary<int, int> values = imageGenerators[i].GetValues();
                    foreach (KeyValuePair<int, int> value in values)
                    {
                        writer.WriteLine(value.Key + IMAGE_EXTTENSION + " " + value.Value);
                    }
                }
            }
        }
    }
}
