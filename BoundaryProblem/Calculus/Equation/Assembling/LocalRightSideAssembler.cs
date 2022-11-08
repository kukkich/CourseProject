using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.Calculus.Equation.DataStructures.LocalObjects;
using BoundaryProblem.DataStructures;
using BoundaryProblem.DataStructures.DensityFunction;

namespace BoundaryProblem.Calculus.Equation.Assembling
{
    public class LocalRightSideAssembler
    {
        private const int NodesInElement = (Element.STEPS_INSIDE_ELEMENT + 1) * (Element.STEPS_INSIDE_ELEMENT + 1);
        private readonly IDensityFunctionProvider _functionProvider;
        private readonly Matrix _xMassTemplate;
        private readonly Matrix _yMassTemplate;

        public LocalRightSideAssembler(
            IDensityFunctionProvider functionProvider,
            Matrix xMassTemplate, Matrix yMassTemplate
            )
        {
            _functionProvider = functionProvider;
            _xMassTemplate = xMassTemplate;
            _yMassTemplate = yMassTemplate;
        }

        public LocalVector Assemble(Element element)
        {
            
            var localVector = GetFunctionVector(element.NodeIndexes);
            var masses = GetDefaultMasses();

            return AttachIndexes(element.NodeIndexes, localVector * masses);
        }

        private Vector GetFunctionVector(int[] nodeIndexes)
        {
            var result = new double[NodesInElement];
            
            for (var i = 0; i < NodesInElement; i++)
                result[i] = _functionProvider.Calc(nodeIndexes[i]);

            return new Vector(result);
        }

        private Matrix GetDefaultMasses()
        {
            var masses = new double[NodesInElement, NodesInElement];

            for (int i = 0; i < NodesInElement; i++)
            {
                for (int j = 0; j < NodesInElement; j++)
                {
                    masses[i, j] =
                        _xMassTemplate[IndexFromX(i), IndexFromX(j)] *
                        _yMassTemplate[IndexFromY(i), IndexFromY(j)];
                }
            }

            return new Matrix(masses);
        }

        private static LocalVector AttachIndexes(int[] indexes, Vector vector)
        {
            return new LocalVector(vector, new IndexPermutation(indexes));
        }

        private static int IndexFromX(int i) => i % 4;

        private static int IndexFromY(int i) => i / 4;
    }
}
