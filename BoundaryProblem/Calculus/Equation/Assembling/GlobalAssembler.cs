using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.Calculus.Equation.DataStructures.LocalObjects;
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
        private readonly PortraitBuilder _portraitBuilder;
        private readonly LocalMatrixAssembler _localMatrixAssembler;
        private readonly LocalRightSideAssembler _localRightSideAssembler;
        private readonly VectorInserter _vectorInserter;
        private readonly MatrixInserter _matrixInserter;

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
            MatrixInserter matrixInserter,
            VectorInserter vectorInserter
        )
        {
            _grid = grid;
            _portraitBuilder = new PortraitBuilder();

            _matrixInserter = matrixInserter;
            _vectorInserter = vectorInserter;

            GetTemplateMatrices(
                out var xStiffnessMatrix, 
                out var yStiffnessMatrix, 
                out var xMassMatrix,
                out var yMassMatrix
            );

            _localMatrixAssembler = new LocalMatrixAssembler(
                materialProvider,
                xMassTemplate: xMassMatrix, 
                yMassTemplate: yMassMatrix, 
                xStiffnessTemplate: xStiffnessMatrix,
                yStiffnessTemplate: yStiffnessMatrix
            );

            _localRightSideAssembler = new LocalRightSideAssembler(
                functionProvider,
                xMassTemplate: xMassMatrix,
                yMassTemplate: yMassMatrix
            );
        }

        private void GetTemplateMatrices(out Matrix xStiffnessMatrix, out Matrix yStiffnessMatrix, out Matrix xMassMatrix, out Matrix yMassMatrix)
        {
            xStiffnessMatrix = new Matrix(DefaultStiffnessMatrix) * (1 / (40.0d * _grid.ElementLength.X));
            yStiffnessMatrix = new Matrix(DefaultStiffnessMatrix) * (1 / (40.0d * _grid.ElementLength.Y));

            xMassMatrix = new Matrix(DefaultMassMatrix) * (_grid.ElementLength.X / 1680.0d);
            yMassMatrix = new Matrix(DefaultMassMatrix) * (_grid.ElementLength.Y / 1680.0d);
        }

        public Equation BuildEquation()
        {
            SymmetricSparseMatrix globalMatrix = BuildPortrait();
            Vector rightSide = new (new double[globalMatrix.RowIndexes.Length]);
            Equation equation = new (
                Matrix: globalMatrix,
                Solution: new Vector(new double[globalMatrix.RowIndexes.Length]),
                RightSide: new Vector(new double[globalMatrix.RowIndexes.Length])
            );

            foreach (var element in _grid.Elements)
            {
                var localMatrix = _localMatrixAssembler.Assemble(element);
                var localRightSide = _localRightSideAssembler.Assemble(element);
                
                _matrixInserter.Insert(equation.Matrix, localMatrix);
                _vectorInserter.Insert(equation.RightSide, localRightSide);
            }

            return equation;
        }

        private SymmetricSparseMatrix BuildPortrait() => _portraitBuilder.Build(_grid);

        public void ApplyBoundaryConditions(Equation equation)
        {
            throw new NotImplementedException();
        }
    }
}
