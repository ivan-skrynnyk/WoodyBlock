using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ShapesData
{
    public static class ShapesData
    {
        private static bool[,] OShape1 = new bool[,]
        {
            { true }
        };

        private static bool[,] OShape2 = new bool[,]
        {
            { true, true },
            { true, true }
        };

        private static bool[,] OShape3 = new bool[,]
        {
            { true, true, true },
            { true, true, true },
            { true, true, true }
        };

        private static bool[,] IShapeVertical2 = new bool[,]
        {
            { true },
            { true }
        };

        private static bool[,] IShapeHorizontal2 = new bool[,]
        {
            { true, true},
        };

        private static bool[,] IShapeVertical3 = new bool[,]
        {
            { true },
            { true },
            { true }
        };

        private static bool[,] IShapeHorizontal3 = new bool[,]
        {
            { true, true, true }
        };

        private static bool[,] LShapeLeft2 = new bool[,]
        {
            { true, false },
            { true, true }
        };

        private static bool[,] LShapeRight2 = new bool[,]
        {
            { false, true },
            { true, true }
        };

        private static bool[,] LShapeLeftInvrted2 = new bool[,]
        {
            { true, true },
            { true, false }
        };

        private static bool[,] LShapeRightInverted2 = new bool[,]
        {
            { true, true },
            { false, true }
        };

        private static bool[,] LShapeLeft3 = new bool[,]
        {
            { true, false, false },
            { true, false, false },
            { true, true, true }
        };

        private static bool[,] LShapeRight3 = new bool[,]
        {
            { false, false, true },
            { false, false, true },
            { true, true, true }
        };

        private static bool[,] LShapeLeftInverted3 = new bool[,]
        {
            { true, true, true },
            { true, false, false },
            { true, false, false }
        };

        private static bool[,] LShapeRightInverted3 = new bool[,]
        {
            { true, true, true },
            { false, false, true },
            { false, false, true }
        };

        public static List<bool[,]> ShapesList = new List<bool[,]> { OShape1, OShape2, OShape3,IShapeHorizontal2, IShapeHorizontal3, IShapeVertical2, IShapeVertical3,
            LShapeLeft2, LShapeLeft3, LShapeLeftInverted3, LShapeLeftInvrted2, LShapeRight2, LShapeRight3, LShapeRightInverted2, LShapeRightInverted3 };
    }
}
