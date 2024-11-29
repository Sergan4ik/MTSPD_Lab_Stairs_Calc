using NUnit.Framework;
using StairsCalc;
using UnityEngine;
using Material = StairsCalc.Material;

public class IntegrationTests
{
    [Test]
    public void TestUsersDB()
    {
        var usersDB = ScriptableObject.CreateInstance<UsersDB>();
        usersDB.AddAdmin("admin", "admin");
        Assert.AreEqual(usersDB.admins.Count, 1);
    }
    
    [Test]
    public void TestGlobalEconomy()
    {
        var economy = ScriptableObject.CreateInstance<GlobalEconomy>();
        var rate = new CurrencyPairRate
        {
            from = Currency.USD,
            to = Currency.EUR,
            rate = 1.0f
        };
        economy.rates.Add(rate);
        Assert.AreEqual(economy.GetRate(Currency.USD, Currency.EUR), 1.0f);
    }
    
    [Test]
    public void TestCurrencyTools()
    {
        string symbol;
        Assert.IsTrue(CurrencyTools.TryGetCurrencySymbol("USD", out symbol));
        Assert.AreEqual(symbol, "$");
    }
    
    [Test]
    public void TestDBProvider()
    {
        var mainMaterials = ScriptableObject.CreateInstance<MaterialDB>();
        var coverMaterials = ScriptableObject.CreateInstance<MaterialDB>();
        var provider = new SOProvider(mainMaterials, coverMaterials);
        var material = new Material("1", 1.0f, 1.0f);
        mainMaterials.materials.Add(material);
        Assert.AreEqual(provider.GetMainMaterial("1"), material);
    }
    
    [Test]
    public void TestModels()
    {
        var handrail = new HandrailCylinder
        {
            length = 1.0f,
            materialId = "1",
            radius = 0.5f
        };
        Assert.AreEqual(handrail.GetVolume(), Mathf.PI * 0.5f * 0.5f * 1.0f);
    }
}