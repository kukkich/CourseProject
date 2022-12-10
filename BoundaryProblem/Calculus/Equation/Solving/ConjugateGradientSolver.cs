using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.Calculus.Equation.Solving.Preconditioner;

namespace BoundaryProblem.Calculus.Equation.Solving;

public class ConjugateGradientSolver
{
    private readonly IPreconditioner _preconditioner;
    private readonly double _precision;
    private readonly int _maxIteration;

    private EquationData _equation;
    private Vector r;
    private Vector z;
    private Vector _additionalMemory;
    private Vector _additionalMemory2;

    public ConjugateGradientSolver(IPreconditioner preconditioner, double precision, int maxIteration)
    {
        _preconditioner = preconditioner;
        _precision = precision;
        _maxIteration = maxIteration;
    }

    public Vector Solve(EquationData equation)
    {
        InitializeStartValues(equation);

        IterationProcess();

        return equation.Solution;
    }

    private void IterationProcess()
    {
        var fNorm = _equation.RightSide.Norm;
        
        for (var i = 1; i < _maxIteration && (r.Norm / fNorm) >= _precision; i++)
        {
            var scalarProduct = Vector.ScalarProduct(
                _preconditioner.MultiplyOn(r),
                r
            );

            var AzProduct = LinearAlgebra.Multiply(_equation.Matrix, z);
            
            var zScalarProduct = Vector.ScalarProduct(
                AzProduct,
                z
            );

            var alpha = scalarProduct / zScalarProduct;

            _equation.Solution = LinearAlgebra.LinearCombination(
                _equation.Solution, z,
                1d, alpha
            );

            var rNext = LinearAlgebra.LinearCombination(
                r, AzProduct,
                1d, -alpha,
                _additionalMemory
            );

            var betta = Vector.ScalarProduct(_preconditioner.MultiplyOn(rNext), rNext) /
                        scalarProduct;

            z = LinearAlgebra.LinearCombination(
                _preconditioner.MultiplyOn(rNext), z,
                1d, betta
            );

            r = rNext;
        }
    }

    private void InitializeStartValues(EquationData equation)
    {
        _equation = equation;
        var AxProduct = LinearAlgebra.Multiply(equation.Matrix, equation.Solution);
        r = LinearAlgebra.Subtract(
            equation.RightSide,
            AxProduct
        );
        z = _preconditioner.MultiplyOn(r);

        _additionalMemory = Vector.Create(equation.RightSide.Length);
        _additionalMemory2 = Vector.Create(equation.RightSide.Length);
    }
}