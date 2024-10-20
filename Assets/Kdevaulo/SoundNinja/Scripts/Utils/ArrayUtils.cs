using UnityEngine.Assertions;

namespace Kdevaulo.SoundNinja.Utils
{
    public static class ArrayUtils
    {
        private static readonly string ErrorString =
            $"{nameof(ArrayUtils)} {nameof(CheckingForEqualArraysLengths)} Array lengths are not equal";

        public static void CheckingForEqualArraysLengths(int value0, int value1)
        {
            Assert.IsTrue(value0 == value1, ErrorString);
        }

        public static void CheckingForEqualArraysLengths(int value0, int value1, int value2)
        {
            Assert.IsTrue(value0 == value1 && value1 == value2, ErrorString);
        }

        public static void CheckingForEqualArraysLengths(int value0, int value1, int value2, int value3)
        {
            Assert.IsTrue(value0 == value1 && value1 == value2 && value2 == value3, ErrorString);
        }
    }
}