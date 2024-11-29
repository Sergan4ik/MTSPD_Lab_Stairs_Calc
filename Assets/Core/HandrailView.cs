using System.Collections.Generic;
using StairsCalc;
using UnityEngine;
using Material = UnityEngine.Material;

public class HandrailView : MonoBehaviour
{
    public Transform handrail;
    public void Show(DBProvider provider, Project project, Handrail prototype, int idx, bool isLeft)
    {
        var depth = project.stepPrototype.length * (project.stepCount - 1);
        var gap = depth / (project.handrailCount - 1);
        var zPos = idx * gap;
        var handrailStep = zPos / project.stepPrototype.length;
        var yPos = (handrailStep + 0.5f) * project.stepPrototype.height + prototype.length / 2;
        var xPos = 0f;
        
        xPos = XPos(project, prototype, isLeft);
        if (prototype is HandrailSquare square)
        {
            handrail.localScale = new Vector3(square.width, square.length, square.height);
        }
        else if (prototype is HandrailCylinder cylinder)
        {
            handrail.localScale = new Vector3(cylinder.radius * 2, cylinder.length / 2, cylinder.radius * 2);
        }
        
        transform.localPosition = new Vector3(xPos, yPos, zPos);

        handrail.GetComponent<MeshRenderer>().SetMaterials(
            new List<Material>() { provider.GetMainMaterial(prototype.materialId).material }
        );
    }
    
    public void ShowMain(DBProvider provider, Project project, bool isLeft)
    {
        var model = project.mainHandrail;
        if (model is HandrailSquare square)
        {
            handrail.localScale = new Vector3(square.width, square.length, square.height);
        }
        else if (model is HandrailCylinder cylinder)
        {
            handrail.localScale = new Vector3(cylinder.radius * 2, cylinder.length / 2, cylinder.radius * 2);
        }

        var tan = (project.stepPrototype.length) / (project.stepPrototype.height);
        var angle = Mathf.Atan(tan);

        var yPos = project.handrailPrototype.length + (project.stepPrototype.height * project.stepCount / 2);
        var xPos = 0f;
        
        xPos = XPos(project, model, isLeft);
        
        transform.localPosition = new Vector3(xPos, yPos, (project.stepCount - 1f) * project.stepPrototype.length / 2);
        transform.localRotation = Quaternion.Euler(angle * Mathf.Rad2Deg, 0, 0);
        
        handrail.GetComponent<MeshRenderer>().SetMaterials(
            new List<Material>() { provider.GetMainMaterial(model.materialId).material }
        );
    }

    private static float XPos(Project project, Handrail model, bool isLeft)
    {
        var xPos = 0f;
        if (model is HandrailSquare square_)
        {
            xPos = square_.width / 2 * (isLeft ? 1 : -1);
        }
        else if (model is HandrailCylinder cylinder)
        {
            xPos = cylinder.radius * (isLeft ? 1 : -1);
        }

        if (isLeft)
        {
            xPos -= project.stepPrototype.width / 2;
        }
        else
        {
            xPos += project.stepPrototype.width / 2;
        }


        return xPos;
    }
}