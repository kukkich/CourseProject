using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.Calculus.Equation.DataStructures.LocalObjects;
using BoundaryProblem.DataStructures;
using BoundaryProblem.DataStructures.BoundaryConditions.Second;
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

        public Matrix XMassMatrix { get; set; }
        public Matrix YMassMatrix { get; set; }

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
            IDensityFunctionProvider functionProvider
        )
        {
            _grid = grid;
            _portraitBuilder = new PortraitBuilder();
            _matrixInserter = new MatrixInserter();
            _vectorInserter = new VectorInserter();

            GetTemplateMatrices(
                out var xStiffnessMatrix,
                out var yStiffnessMatrix,
                out var xMassMatrix,
                out var yMassMatrix
            );
            YMassMatrix = yMassMatrix;
            XMassMatrix = xMassMatrix;

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

        public EquationData BuildEquation()
        {
            SymmetricSparseMatrix globalMatrix = BuildPortrait();
            EquationData equation = new(
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

        public void ApplyThirdBoundaryConditions(EquationData equation)
        {
            throw new NotImplementedException();
        }

        public void ApplySecondBoundaryConditions(EquationData equation, SecondBoundaryProvider secondCondition)
        {
            foreach (var conditionsUnit in secondCondition.FlowConditions)
            {
                var massMatrix = conditionsUnit.Bound switch
                {
                    Bound.Right or Bound.Left => YMassMatrix,
                    Bound.Top or Bound.Bottom => XMassMatrix,
                    _ => throw new ArgumentException(String.Empty, nameof(secondCondition))
                };

                Element element = _grid.Elements[conditionsUnit.ElementId];
                var omegaVector = new Vector(new[]
                {
                    conditionsUnit.Thetta,
                    conditionsUnit.Thetta,
                    conditionsUnit.Thetta,
                    conditionsUnit.Thetta
                });

                var boundNodeIndexes = element.GetBoundNodeIndexes(conditionsUnit.Bound);
                LocalVector localVector = new(
                    omegaVector * massMatrix,
                    new IndexPermutation(boundNodeIndexes)
                    );

                _vectorInserter.Insert(equation.RightSide, localVector);
            }
        }

        public void ApplyFirstBoundaryConditions(EquationData equation)
        {
            throw new NotImplementedException();
        }
    }
}
