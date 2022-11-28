using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.Calculus.Equation.DataStructures.LocalObjects;
using BoundaryProblem.DataStructures;
using BoundaryProblem.DataStructures.BoundaryConditions;
using BoundaryProblem.DataStructures.BoundaryConditions.Second;
using BoundaryProblem.DataStructures.BoundaryConditions.Third;
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

        private void GetTemplateMatrices(
            out Matrix xStiffnessMatrix, out Matrix yStiffnessMatrix, 
            out Matrix xMassMatrix, out Matrix yMassMatrix
            )
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

        public void ApplyThirdBoundaryConditions(EquationData equation, ThirdBoundaryProvider condition)
        {
            foreach (var flowExchangeCondition in condition.FlowExchangeConditions)
            {
                var massMatrix = GetMassMatrixForBound(flowExchangeCondition.Bound);
                Element element = _grid.Elements[flowExchangeCondition.ElementId];
                var boundNodeIndexes = element.GetBoundNodeIndexes(flowExchangeCondition.Bound);

                //TODO переименовать маст хэв
                var A_S3 = flowExchangeCondition.Betta * massMatrix;
                var environmentVector = Vector.Create(Element.NodesOnBound, flowExchangeCondition.Environment);
                var b_S3 = A_S3 * environmentVector;

                var localVector = new LocalVector(
                    b_S3,
                    new IndexPermutation(boundNodeIndexes)
                    );
                var localMatrix = new LocalMatrix(
                    A_S3, 
                    new IndexPermutation(boundNodeIndexes)
                    );

                _vectorInserter.Insert(equation.RightSide, localVector);
                _matrixInserter.Insert(equation.Matrix, localMatrix);
                // A_S3 = ...
                // b_S3 = A_S3 * Vector.Create(Element.NodesOnBound, flowCondition.Environment);

                // Create Local Objects (matrix A and vector B)
                // Insert Local Objects

                // have fun!
            }

            throw new NotImplementedException();
        }

        public void ApplySecondBoundaryConditions(EquationData equation, SecondBoundaryProvider condition)
        {
            foreach (var flowCondition in condition.FlowConditions)
            {
                var massMatrix = GetMassMatrixForBound(flowCondition.Bound);

                Element element = _grid.Elements[flowCondition.ElementId];
                var thettaVector = Vector.Create(Element.NodesOnBound, flowCondition.Thetta);

                var boundNodeIndexes = element.GetBoundNodeIndexes(flowCondition.Bound);
                LocalVector localVector = new(
                    thettaVector * massMatrix,
                    new IndexPermutation(boundNodeIndexes)
                    );

                _vectorInserter.Insert(equation.RightSide, localVector);
            }
        }

        private Matrix GetMassMatrixForBound(Bound bound)
        {
            return bound switch
            {
                Bound.Right or Bound.Left => YMassMatrix,
                Bound.Top or Bound.Bottom => XMassMatrix,
                _ => throw new ArgumentException(String.Empty, nameof(bound))
            };
        }

        public void ApplyFirstBoundaryConditions(EquationData equation)
        {
            throw new NotImplementedException();
        }
    }
}
