using System.Collections.Generic;
using StairsCalc;
using UnityEngine;
using Material = UnityEngine.Material;

public class StepView : MonoBehaviour
{
    public Transform step;
    public Transform cover;
    public void Show(DBProvider provider, Step prototype, int idx)
    {
        step.localScale = new Vector3(prototype.width, prototype.height, prototype.length);
        cover.localScale = new Vector3(prototype.width, 0.01f, prototype.length);
        transform.localPosition = new Vector3(0, idx * prototype.height, idx * prototype.length);
        cover.localPosition = new Vector3(0, prototype.height / 2, 0);
        
        step.GetComponent<MeshRenderer>().SetMaterials(
            new List<Material>() { provider.GetMainMaterial(prototype.materialId).material }
        );
        cover.GetComponent<MeshRenderer>().SetMaterials(
            new List<Material>() { provider.GetCoverMaterial(prototype.coverMaterialId).material }
        );
    }
}