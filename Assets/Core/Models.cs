using System;
using System.IO;
using UnityEngine;

namespace StairsCalc
{
    [Serializable]
    public class Material
    {
        public string Id;
        public float price;
        public float density;
        public UnityEngine.Material material;
        
        public Material(string id, float price, float density)
        {
            Id = id;
            this.price = price;
            this.density = density;
        }

        public override string ToString()
        {
            return $"Material: {Id}, Price: {price}/unit, Density: {density} kg/m^3";
        }
    }
    
    [Serializable]
    public class AdminUser
    {
        public string Id;
        public int passwordHash;
    }
    
    [Serializable]
    public class User
    {
        public string Id;
        public string email;
        public int passwordHash;
    }
    
    public abstract class Handrail : ICloneable
    {
        public float length;
        public string materialId;
        public abstract float GetVolume();
        public abstract object Clone();
    }
    
    public class HandrailCylinder : Handrail
    {
        public float radius;
        public override float GetVolume()
        {
            return Mathf.PI * radius * radius * length;
        }

        public override object Clone()
        {
            return new HandrailCylinder
            {
                length = length,
                materialId = materialId,
                radius = radius
            };
        }
    }
    
    public class HandrailSquare : Handrail
    {
        public float width;
        public float height;
        public override float GetVolume()
        {
            return width * height * length;
        }

        public override object Clone()
        {
            return new HandrailSquare
            {
                length = length,
                materialId = materialId,
                width = width,
                height = height
            };
        }
    }
    
    public class Step
    {
        public float height;
        public float width;
        public float length;

        public string materialId;
        public string coverMaterialId;
        
        public float GetVolume()
        {
            return height * width * length;
        }
    }

    public enum Currency
    {
        USD,
        EUR,
        UAH
    }
    [Serializable]
    public class CurrencyPairRate
    {
        public Currency from;
        public Currency to;
        public float rate;
        
        public CurrencyPairRate()
        {
        }
        public CurrencyPairRate(Currency from, Currency to, float rate)
        {
            this.from = from;
            this.to = to;
            this.rate = rate;
        }
        
        public CurrencyPairRate GetReversed()
        {
            return new CurrencyPairRate(to, from, 1 / rate);
        }
    }
    
    public class Project
    {
        public Project(ref Step stepPrototype, ref Handrail handrailPrototype, int stepCount, int handrailCount, DBProvider dbProvider)
        {
            this.stepPrototype = stepPrototype;
            this.handrailPrototype = handrailPrototype;
            this.stepCount = stepCount;
            this.handrailCount = handrailCount;
            this.dbProvider = dbProvider;
        }
        public struct ConstructionVolumes
        {
            public float stepVolume;
            public float stepCoverSquare;
            public float handrailVerticalVolume;
            public float handrailMainVolume;
        } 
        public struct ConstructionMasses
        {
            public float stepMass;
            public float stepCoverMass;
            public float handrailVerticalMass;
            public float handrailMainMass;
        } 
        public struct ConstructionPrices
        {
            public float stepPrice;
            public float stepCoverPrice;
            public float handrailVerticalPrice;
            public float handrailMainPrice;
            
            public float totalPrice => stepPrice + stepCoverPrice + handrailVerticalPrice + handrailMainPrice;
        }
        
        public Step stepPrototype;
        public Handrail handrailPrototype;
        public int stepCount;
        public int handrailCount;
        public DBProvider dbProvider;

        // public Handrail mainHandrail
        // {
        //     get 
        //     {
        //         var res = handrailPrototype.Clone() as Handrail;
        //         var constructionLen = stepCount * stepPrototype.length;
        //         var constructionHeight = stepCount * stepPrototype.height;
        //         res.length = Mathf.Sqrt(constructionLen * constructionLen + constructionHeight * constructionHeight);
        //         return res;
        //     }
        // }
        private float CalculateMainHandrailLength()
        {
            var constructionLen = stepCount * stepPrototype.length;
            var constructionHeight = stepCount * stepPrototype.height;
            return Mathf.Sqrt(constructionLen * constructionLen + constructionHeight * constructionHeight);
        }

        public Handrail mainHandrail
        {
            get
            {
                var res = handrailPrototype.Clone() as Handrail;
                res.length = CalculateMainHandrailLength();
                return res;
            }
        }

        public ConstructionVolumes CalculateConstructionVolume()
        {
            var stepVolume = stepPrototype.GetVolume() * stepCount;
            var handrailVerticalVolume = handrailPrototype.GetVolume() * handrailCount * 2;
            Handrail mainHandrail = handrailPrototype.Clone() as Handrail;
            var constructionLen = stepCount * stepPrototype.length;
            var constructionHeight = stepCount * stepPrototype.height;
            mainHandrail.length = Mathf.Sqrt(constructionLen * constructionLen + constructionHeight * constructionHeight);
            return new ConstructionVolumes()
            {
                stepVolume = stepVolume,
                stepCoverSquare = stepPrototype.width * stepPrototype.length * stepCount,
                handrailVerticalVolume = handrailVerticalVolume,
                handrailMainVolume = mainHandrail.GetVolume() * 2
            };
        }
        
        public ConstructionMasses CalculateConstructionMass(ConstructionVolumes volumes)
        {
            return new ConstructionMasses()
            {
                stepMass = volumes.stepVolume * dbProvider.GetMainMaterial(stepPrototype.materialId).density,
                stepCoverMass = volumes.stepCoverSquare *
                                dbProvider.GetCoverMaterial(stepPrototype.coverMaterialId).density,
                handrailVerticalMass = volumes.handrailVerticalVolume *
                                       dbProvider.GetMainMaterial(handrailPrototype.materialId).density,
                handrailMainMass = volumes.handrailMainVolume *
                                   dbProvider.GetMainMaterial(handrailPrototype.materialId).density
            };
        }
        
        public ConstructionPrices CalculateConstructionPrice(ConstructionMasses masses)
        {
            return new ConstructionPrices()
            {
                stepPrice = masses.stepMass * dbProvider.GetMainMaterial(stepPrototype.materialId).price,
                stepCoverPrice = masses.stepCoverMass * dbProvider.GetCoverMaterial(stepPrototype.coverMaterialId).price,
                handrailVerticalPrice = masses.handrailVerticalMass * dbProvider.GetMainMaterial(handrailPrototype.materialId).price,
                handrailMainPrice = masses.handrailMainMass * dbProvider.GetMainMaterial(handrailPrototype.materialId).price
            };
        }
        
        public Report GetReport(string name, Currency currency)
        {
            var volumes = CalculateConstructionVolume();
            var masses = CalculateConstructionMass(volumes);
            var prices = CalculateConstructionPrice(masses);
            return new Report(name, this, volumes, masses, prices, currency);
        }
    }
    
    public class Report
    {
        public string name;
        public Currency currency;
        public Project sourceProject;
        public Project.ConstructionVolumes volumes;
        public Project.ConstructionMasses masses;
        public Project.ConstructionPrices prices;
        
        public Report(string name, Project source, Project.ConstructionVolumes volumes, Project.ConstructionMasses masses, Project.ConstructionPrices prices, Currency currency)
        {
            this.name = name;
            this.volumes = volumes;
            this.masses = masses;
            this.prices = prices;
            this.currency = currency;
            sourceProject = source;
        }
        
        public void SaveToFile(string path)
        {
            //clear file
            File.WriteAllText(path, string.Empty);
            
            // Save report to CSV
            string sym = $"$";
            float rate = GlobalEconomy.GetEconomy().GetRate(Currency.USD, currency);
            CurrencyTools.TryGetCurrencySymbol(currency.ToString(), out sym);
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine(";Volumes;Masses;Prices");
                sw.WriteLine($"Step ({sourceProject.stepPrototype.materialId});{volumes.stepVolume} m^3;{masses.stepMass} kg;{prices.stepPrice * rate} {sym}");
                sw.WriteLine($"Step cover ({sourceProject.stepPrototype.coverMaterialId});{volumes.stepCoverSquare} m^2;{masses.stepCoverMass} kg;{prices.stepCoverPrice * rate} {sym}");
                sw.WriteLine($"Handrail vertical ({sourceProject.handrailPrototype.materialId});{volumes.handrailVerticalVolume} m^3;{masses.handrailVerticalMass} kg;{prices.handrailVerticalPrice * rate} {sym}");
                sw.WriteLine($"Handrail main ({sourceProject.handrailPrototype.materialId});{volumes.handrailMainVolume} m^3;{masses.handrailMainMass} kg;{prices.handrailMainPrice * rate} {sym}");
                sw.WriteLine($"Total;;{masses.stepMass + masses.stepCoverMass + masses.handrailVerticalMass + masses.handrailMainMass} kg;{prices.totalPrice * rate} {sym}");
            }
        }
    }
}