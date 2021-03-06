﻿using System;
using System.Drawing;
using System.Linq;
using Hqub.Speckle.Core.BitmapExtensions;

namespace Hqub.Speckle.Core.Correlation
{
    public class SpegoCorrelationEngine : BaseCorrelationEngine
    {
        public override double Compare(string pathA, string pathB)
        {
            var image1 = new Bitmap(pathA);
            var image2 = new Bitmap(pathB);

            return Compare(image1, image2);
        }

        public override double Compare(string pathA, string pathB, Rectangle bound)
        {
//            bound = new Rectangle(437, 391, 634-437, 527-391);
            var image1 = BitmapTools.CropImage(new Bitmap(pathA), bound);
            var image2 = BitmapTools.CropImage(new Bitmap(pathB), bound);

            return Compare(image1, image2);
        }

        public override double Compare(Bitmap imageA, Bitmap imageB)
        {
            // Получаем одноканальное изображение:
            var grayA = imageA; //BitmapTools.ConvertBitmapToGrayScale(image1);
            var grayB = imageB; //BitmapTools.ConvertBitmapToGrayScale(image2);

            // Запоминаем ширину и высоту изображения
            var w = Math.Max(grayA.Width, grayB.Width);
            var h = Math.Max(grayA.Height, grayB.Height);

            // Кол-во пикселей:
            var amountA = grayA.Width * grayA.Height;
            var amountB = grayB.Width * grayB.Height;
            var amount = w * h;


            // Расскалдываем картинки в одномерный массив
            var scaffA = ScaffBitmap(grayA);
            var scaffB = ScaffBitmap(grayB);

            // Получаем среднее значение цвета
            var averageA = scaffA.Sum() / amountA;
            var averageB = scaffB.Sum() / amountB;

            var numenator = 0;
            double denumerator = 0;
            double dA = 0;
            double dB = 0;

            for (var i = 0; i < amount; i++)
            {
                // Вычисляем числитель:
                numenator += (scaffA[i] - averageA) * (scaffB[i] - averageB);

                // Вычисляем часть знаменателя:
                dA += Math.Pow(scaffA[i] - averageA, 2);
                dB += Math.Pow(scaffB[i] - averageB, 2);
            }

            numenator /= amount;
            denumerator = Math.Sqrt(dA / amount) * Math.Sqrt(dB / amount);

            return numenator / denumerator;
        }

        public new ILogger Logger { get; set; }

    }
}
