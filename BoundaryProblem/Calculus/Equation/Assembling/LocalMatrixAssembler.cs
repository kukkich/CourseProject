using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.Calculus.Equation.DataStructures.LocalObjects;
using BoundaryProblem.DataStructures;

namespace BoundaryProblem.Calculus.Equation.Assembling
{
    public class LocalMatrixAssembler
    {
        private const int LocalMatrixSize = (Element.STEPS_INSIDE_ELEMENT + 1) * (Element.STEPS_INSIDE_ELEMENT + 1);
        private readonly IMaterialProvider _materialProvider;
        private readonly Matrix _xMassTemplate;
        private readonly Matrix _yMassTemplate;
        private readonly Matrix _xStiffnessTemplate;
        private readonly Matrix _yStiffnessTemplate;

        public LocalMatrixAssembler(
            IMaterialProvider materialProvider,
            Matrix xMassTemplate, Matrix yMassTemplate,
            Matrix xStiffnessTemplate, Matrix yStiffnessTemplate
        )
        {
            _materialProvider = materialProvider;
            _xMassTemplate = xMassTemplate;
            _yMassTemplate = yMassTemplate;
            _xStiffnessTemplate = xStiffnessTemplate;
            _yStiffnessTemplate = yStiffnessTemplate;
        }

        public LocalMatrix Assemble(Element element)
        {
            var material = _materialProvider.GetMaterialById(element.MaterialId);

            var (stiffness, masses) = GetMassesAndStiffnessMatrix(material);

            return AttachIndexes(element.NodeIndexes, stiffness + masses);
        }

        private static LocalMatrix AttachIndexes(int[] indexes, Matrix matrix)
        {
            return new LocalMatrix(
                matrix,
                new IndexPermutation(indexes)
            );
        }

        private (Matrix stiffness, Matrix masses) GetMassesAndStiffnessMatrix(Material material)
        {
            var stiffness = new double[LocalMatrixSize, LocalMatrixSize];
            var masses = new double[LocalMatrixSize, LocalMatrixSize];

            for (int i = 0; i < LocalMatrixSize; i++)
            {
                for (int j = 0; j < LocalMatrixSize; j++)
                {
                    stiffness[i, j] =
                        material.Lambda * (
                            _xStiffnessTemplate[IndexFromX(i), IndexFromX(j)] *
                            _yMassTemplate[IndexFromY(i), IndexFromY(j)]
                            +
                            _xMassTemplate[IndexFromX(i), IndexFromX(j)] *
                            _yStiffnessTemplate[IndexFromY(i), IndexFromY(j)]
                        );

                    masses[i, j] =
                        material.Gamma *
                        _xMassTemplate[IndexFromX(i), IndexFromX(j)] *
                        _yMassTemplate[IndexFromY(i), IndexFromY(j)];
                }
            }

            return (new Matrix(stiffness), new Matrix(masses));
        }

        private static int IndexFromX(int i) => i % 4;

        private static int IndexFromY(int i) => i / 4;
    }
}
