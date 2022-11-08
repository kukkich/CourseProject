using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.DataStructures;
using BoundaryProblem.DataStructures.DensityFunction;
using BoundaryProblem.Geometry;

namespace BoundaryProblem.Calculus.Equation.Assembling
{
    // TODO не протестирован
    public class GlobalAssembler
    {
        private static readonly double[,] DefaultStiffnessMatrix;
        private static readonly double[,] DefaultMassMatrix;

        private readonly Grid _grid;
        private readonly MatrixInserter _matrixInserter;
        private readonly LocalMatrixAssembler _localMatrixAssembler;

        static GlobalAssembler()
        {
            DefaultStiffnessMatrix = new double[,]
            {
                { 148, -189, 54, -13 },
                { -189, 432, -297, 54 },
                { 54, -297, 432, -189 },
                { -13, 54, -189, 148 }
            };

            DefaultMassMatrix = new double[,]
            {
                { 128, 99, -36, 19 },
                { 99, 648, -81, -36 },
                { -36, -81, 648, 99 },
                { 19, -36, 99, 128 }
            };
        }
        
        public GlobalAssembler(
            Grid grid,
            IMaterialProvider materialProvider,
            IDensityFunctionProvider functionProvider,
            MatrixInserter matrixInserter)
        {
            _grid = grid;

            _matrixInserter = matrixInserter;

            var xStiffnessMatrix = new Matrix(DefaultStiffnessMatrix) * (1 / (40.0d * _grid.ElementLength.X));
            var yStiffnessMatrix = new Matrix(DefaultStiffnessMatrix) * (1 / (40.0d * _grid.ElementLength.Y));

            var xMassMatrix = new Matrix(DefaultMassMatrix) * (_grid.ElementLength.X / 1680.0d);
            var yMassMatrix = new Matrix(DefaultMassMatrix) * (_grid.ElementLength.Y / 1680.0d);

            _localMatrixAssembler = new LocalMatrixAssembler(materialProvider,
                xMassTemplate: xMassMatrix, 
                yMassTemplate: yMassMatrix, 
                xStiffnessTemplate: xStiffnessMatrix,
                yStiffnessTemplate: yStiffnessMatrix
                );
        }

        public void BuildMatrix()
        {
            foreach (var element in _grid.Elements)
            {

            }

            throw new NotImplementedException();
        }

        public void ApplyBoundaryConditions()
        {
            throw new NotImplementedException();
        }
    }
}
