using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Material = StairsCalc.Material;

[CreateAssetMenu(fileName = "MaterialDB", menuName = "StairsCalc/MaterialDB", order = 0)]
public class MaterialDB : ScriptableObject
{
    public List<StairsCalc.Material> materials;
    
    [Button]
    public void MultPrices(float mult)
    {
        foreach (var material in materials)
        {
            material.price *= mult;
        }
    }
    
    [Button]
    public void TestFill()
    {
        materials = new List<Material>()
        {
            new Material("Concrete", 50.0f, 2400.0f),
            new Material("Steel", 100.0f, 7850.0f),
            new Material("Wood (Oak)", 60.0f, 700.0f),
            new Material("Wood (Pine)", 40.0f, 450.0f),
            new Material("Marble", 150.0f, 2700.0f),
            new Material("Granite", 120.0f, 2700.0f),
            new Material("Tile (Ceramic)", 30.0f, 2000.0f),
            new Material("Glass", 80.0f, 2500.0f),
            new Material("Carpet (Covering)", 20.0f, 300.0f),
            new Material("Vinyl (Covering)", 15.0f, 1200.0f),
        };
    }
    
    [Button]
    public void TestCoverFill()
    {
        materials = new List<Material>()
        {
            new Material("Ceramic Tiles (8mm)", 30.0f, 16.0f), // 2000 kg/m³ × 0.008 m
            new Material("Porcelain Tiles (10mm)", 35.0f, 20.0f), // 2000 kg/m³ × 0.010 m
            new Material("Laminate (10mm)", 20.0f, 8.0f), // 800 kg/m³ × 0.010 m
            new Material("Hardwood (20mm Oak)", 60.0f, 14.0f), // 700 kg/m³ × 0.020 m
            new Material("Softwood (20mm Pine)", 40.0f, 9.0f), // 450 kg/m³ × 0.020 m
            new Material("Carpet (5mm)", 25.0f, 1.5f), // 300 kg/m³ × 0.005 m
            new Material("Vinyl (3mm)", 15.0f, 3.6f), // 1200 kg/m³ × 0.003 m
            new Material("Natural Stone (Marble, 15mm)", 150.0f, 40.5f), // 2700 kg/m³ × 0.015 m
            new Material("Granite (15mm)", 120.0f, 40.5f), // 2700 kg/m³ × 0.015 m
            new Material("Glass (12mm)", 80.0f, 30.0f), // 2500 kg/m³ × 0.012 m
        };
    }
}