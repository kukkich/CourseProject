using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.Geometry;

namespace BoundaryProblem.Calculus.Equation
{
    public class Assembler
    {
        private readonly Matrix _xStiffnessMatrix;
        private readonly Matrix _yStiffnessMatrix;
        private readonly Matrix _xMassMatrix;
        private readonly Matrix _yMassMatrix;
        private readonly Grid _grid;

        public Assembler(Grid grid)
        {
            _grid = grid;

            double[,] defaultStiffnessMatrix =
            {
                { 148, -189, 54, -13 },
                { -189, 432, -297, 54 },
                { 54, -297, 432, -189 },
                { -13, 54, -189, 148 }
            };
            double[,] defaultMassMatrix =
            {
                { 128, 99, -36, 19 },
                { 99, 648, -81, -36 },
                { -36, -81, 648, 99 },
                { 19, -36, 99, 128 }
            };

            (_xStiffnessMatrix, _yStiffnessMatrix) = (new Matrix(defaultStiffnessMatrix), new Matrix(defaultStiffnessMatrix));
            //!TODO умножать на коэффициент материала лямбду, постоянную на элементе (мб умножать не прям тут)
            _xStiffnessMatrix.Multiply(1 / (40.0d * grid.ElementLength.X));
            _yStiffnessMatrix.Multiply(1 / (40.0d * grid.ElementLength.Y));

            //!TODO аналогично умножать на омегу, не обязательно здесь
            (_xMassMatrix, _yMassMatrix) = (new Matrix(defaultMassMatrix), new Matrix(defaultMassMatrix));
            _xMassMatrix.Multiply(grid.ElementLength.X / 1680.0d);
            _yMassMatrix.Multiply(grid.ElementLength.Y / 1680.0d);
        }


    }
}
