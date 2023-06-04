namespace BoundaryProblem.DataStructures;   

public class MaterialProvider : IMaterialProvider
{
    public readonly Material[] Materials;

    public MaterialProvider(Material[] materials)
    {
        Materials = materials;
    }

    public Material GetMaterialById(int id) => Materials[id];

    public static MaterialProvider Deserialize(ProblemFilePathsProvider files)
    {
        using var stream = new StreamReader(files.Material);
            
        var materials = new Material[int.Parse(stream.ReadLine())];

        for (int i = 0; ; i++)
        {
            var line = stream.ReadLine();
            if (String.IsNullOrEmpty(line)) break;

            var values = line.Split(' ');
            materials[i] = 
                new Material(
                    double.Parse(values[0]),
                    double.Parse(values[1]),
                    double.Parse(values[2])
                );
        }

        return new MaterialProvider(materials);
    }
}