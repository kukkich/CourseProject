namespace BoundaryProblem.DataStructures
{
    public class MaterialProvider : IMaterialProvider
    {
        private readonly Material[] _materials;

        public MaterialProvider(Material[] materials)
        {
            _materials = materials;
        }

        public Material GetMaterialById(int id) => _materials[id];
    }
}
