namespace StairsCalc
{
    public abstract class DBProvider
    {
        public abstract Material GetMainMaterial(string id);
        public abstract Material GetCoverMaterial(string id);
    }
    
    public class SOProvider : DBProvider
    {
        public MaterialDB mainMaterials;
        public MaterialDB coverMaterials;
        
        public SOProvider(MaterialDB mainMats, MaterialDB coverMats)
        {
            mainMaterials = mainMats;
            coverMaterials = coverMats;
        }
        
        public override Material GetMainMaterial(string id)
        {
            foreach (var material in mainMaterials.materials)
            {
                if (material.Id == id)
                {
                    return material;
                }
            }

            return null;
        }

        public override Material GetCoverMaterial(string id)
        {
            foreach (var material in coverMaterials.materials)
            {
                if (material.Id == id)
                {
                    return material;
                }
            }

            return null;
        }
    }
}