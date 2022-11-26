using BoundaryProblem.Calculus.Equation.DataStructures;

namespace BoundaryProblem.Calculus.Equation;

public record EquationData(SymmetricSparseMatrix Matrix, Vector Solution, Vector RightSide);
