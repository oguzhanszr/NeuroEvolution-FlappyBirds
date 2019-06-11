using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FlappyBirds
{
    public class CompareBird : IComparer<Bird>
    {
        public int Compare(Bird x, Bird y)
        {
            int result;
            if (Bird.ReferenceEquals(x, y))
            {
                result = 0;
            }
            else
            {
                if (x == null)
                {
                    result = 1;
                }
                else if (y == null)
                {
                    result = -1;
                }
                else
                {
                    result = NumberCompare(x.fitness, y.fitness);
                }
            }
            return result;
        }

        int NumberCompare(double number1, double number2)
        {
            int result;
            if (number1 < number2)
            {
                result = 1;
            }
            else if (number1 > number2)
            {
                result = -1;
            }
            else
            {
                result = 0;
            }
            return result;
        }
    }
}