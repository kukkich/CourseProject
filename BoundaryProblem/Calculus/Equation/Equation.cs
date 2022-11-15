using BoundaryProblem.Calculus.Equation.DataStructures;

namespace BoundaryProblem.Calculus.Equation;

public record Equation(SymmetricSparseMatrix Matrix, Vector Solution, Vector RightSide);
