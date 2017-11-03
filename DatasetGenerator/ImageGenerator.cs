using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetGenerator
{
    class ImageGenerator
    {
        private const int IMAGE_WIDTH = 120;
        private const int IMAGE_HEIGHT = 32;
        private const int NUMBER_OF_IMAGES_TO_GENERATE = 10000;
        private static readonly string[] BANNED_FONTS = { "HoloLens MDL2 Assets", "Marlett", "NewJumja", "Segoe MDL2 Assets", "Webdings", "Wingdings" };
        private const bool DRAW_LINES = false;
        private const int IMAGES_PER_THREAD = NUMBER_OF_IMAGES_TO_GENERATE / Program.NUMBER_OF_THREADS;

        private readonly int part;
        private Dictionary<int, int> values;

        public ImageGenerator(int part)
        {
            this.part = part;
            values = new Dictionary<int, int>();
        }

        public void GenerateImages()
        {
            FontFamily[] fonts = FontFamily.Families;
            Random random = new Random();

            for (int i = part * NUMBER_OF_IMAGES_TO_GENERATE / Program.NUMBER_OF_THREADS; i < (part + 1) * NUMBER_OF_IMAGES_TO_GENERATE / Program.NUMBER_OF_THREADS; i++)
            {
                int digit = random.Next() % 6 + 1;
                int number = random.Next() % ((int)Math.Pow(10, digit));
                Bitmap bitmap = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT);
                int fontIndex = random.Next() % fonts.Length;

                while (BANNED_FONTS.Contains(fonts[fontIndex].Name))
                    fontIndex = random.Next() % fonts.Length;

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font font = new Font(fonts[fontIndex].Name, 15))
                    {
                        graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, IMAGE_WIDTH, IMAGE_HEIGHT));
                        graphics.DrawString(number.ToString(), font, Brushes.Black, new PointF(10f, 5f));
                        int lines = random.Next() % 5 + 3;
                        if (DRAW_LINES)
                        {
                            for (int j = 0; j < lines; j++)
                                graphics.DrawLine(new Pen(Brushes.Black, 1f), new Point(random.Next() % IMAGE_WIDTH, random.Next() % IMAGE_HEIGHT), new Point(random.Next() % IMAGE_WIDTH, random.Next() % IMAGE_HEIGHT));
                        }
                    }
                }
                bitmap.Save(Program.DIRECTORY_TO_SAVE + i + Program.IMAGE_EXTTENSION);
                values.Add(i, number);

                if (part == 0 && i % 64 == 0)
                {
                    Console.WriteLine((i + 1) + " / " + IMAGES_PER_THREAD);
                }
            }
        }

        public Dictionary<int, int> GetValues()
        {
            return values;
        }
    }
}
