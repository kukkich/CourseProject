using BoundaryProblem.Calculus.Equation.Assembling;
using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.DataStructures;
using CourseProject.Tests.DataStructuresMoq;
using CourseProject.Tests.MoqDataStructures;

namespace CourseProject.Tests.Calculus.Equation.Assembling.LocalAssemblingTests
{
    internal class MatrixPermutationTests
    {
        private LocalMatrixAssembler _assembler;
        private IMaterialProvider _materialProvider;
        [SetUp]
        public void Setup()
        {
            _materialProvider = new IdentityMaterialProvider();

            _assembler = new LocalMatrixAssembler(
                _materialProvider,
                xMassTemplate: new ZeroMatrix(),
                yMassTemplate: new ZeroMatrix(),
                xStiffnessTemplate: new ZeroMatrix(),
                yStiffnessTemplate: new ZeroMatrix()
            );
        }

        [TestCase(1, 2, 3, 4, 5, 6)]
        [TestCase(6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 29)]
        public void CorrectValuesTest(params int[] permutation)
        {
            var expectedPermutation = new IndexPermutation(permutation);

            var assembledMatrix = _assembler.Assemble(new Element(
                default,
                permutation
            ));

            Assert.Multiple(() =>
            {
                for (int i = 0; i < permutation.Length; i++)
                {
                    Assert.That(assembledMatrix.IndexPermutation.ApplyColumnPermutation(i),
                        Is.EqualTo(expectedPermutation.ApplyColumnPermutation(i)));

                    Assert.That(assembledMatrix.IndexPermutation.ApplyRowPermutation(i),
                        Is.EqualTo(expectedPermutation.ApplyRowPermutation(i)));
                }
            });
        }
    }
}
