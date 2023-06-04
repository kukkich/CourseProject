using BoundaryProblem.Calculus.Equation.Assembling;
using BoundaryProblem.Calculus.Equation.Solving;
using BoundaryProblem.Calculus.Equation.Solving.Preconditioner;
using BoundaryProblem.DataStructures.BoundaryConditions.First;
using BoundaryProblem.DataStructures.BoundaryConditions.Second;
using BoundaryProblem.DataStructures.BoundaryConditions.Third;
using BoundaryProblem.DataStructures.DensityFunction;
using BoundaryProblem.DataStructures;
using BoundaryProblem.Geometry;

namespace BoundaryProblem;

public class FiniteElementSolver
{
    public required double SolutionPrecision { get; init; }
    public required int MaxIteration { get; init; }
    public ProblemFilePathsProvider Files { get; init; }
    public Grid Grid { get; set; }
    public IMaterialProvider Materials { get; set; }

    private FirstBoundaryProvider _firstBoundary;
    private SecondBoundaryProvider _secondBoundary;
    private ThirdBoundaryProvider _thirdBoundary;
    private IDensityFunctionProvider _densityFunction;
    private GlobalAssembler _globalAssembler;
    private ConjugateGradientSolver SLAESolver;

    public FiniteElementSolver(ProblemFilePathsProvider files)
    {
        Files = files;
            
        Grid = Grid.Deserialize(Files);

        //_firstBoundary = FirstBoundaryProvider.Deserialize(Files);
        //_secondBoundary = SecondBoundaryProvider.Deserialize(Files);
        //_thirdBoundary = ThirdBoundaryProvider.Deserialize(Files);

        //_densityFunction = FunctionDefinedOnNodes.Deserialize(Files);
        //Materials = MaterialProvider.Deserialize(Files);

        //_globalAssembler = new GlobalAssembler(Grid, Materials, _densityFunction);
    }

    public void ReadData()
    {
        _firstBoundary = FirstBoundaryProvider.Deserialize(Files);
        _secondBoundary = SecondBoundaryProvider.Deserialize(Files);
        _thirdBoundary = ThirdBoundaryProvider.Deserialize(Files);

        _densityFunction = FunctionDefinedOnNodes.Deserialize(Files);
        //Materials = MaterialProvider.Deserialize(Files);

        _globalAssembler = new GlobalAssembler(Grid, Materials, _densityFunction);
    }

    public FiniteElementSolution Solve()
    {
        var equation = _globalAssembler.BuildEquation();

        var a = equation.Matrix;
        
        _globalAssembler.ApplySecondBoundaryConditions(equation, _secondBoundary)
            .ApplyThirdBoundaryConditions(equation, _thirdBoundary)
            .ApplyFirstBoundaryConditions(equation, _firstBoundary);

        IPreconditioner preconditioner = new DiagonalPreconditioner(equation.Matrix.Diagonal);
        SLAESolver = new ConjugateGradientSolver(preconditioner, SolutionPrecision, MaxIteration);
    
        var solution = SLAESolver.Solve(equation);

        return new FiniteElementSolution(Grid, solution);
    }
}