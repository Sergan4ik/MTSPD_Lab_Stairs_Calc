using System.Collections;
using NUnit.Framework;
using StairsCalc;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTests
{
    [Test]
    public void UnitTestsSimplePasses()
    {
    }

    [UnityTest]
    public IEnumerator UnitTestsWithEnumeratorPasses()
    {
        yield return null;
    }
    
    //generate volume calculation test
    [Test]
    public void VolumeCalcTestHandrail()
    {
        var square = new HandrailSquare();
        square.width = 1;
        square.height = 1;
        square.length = 1;
        Assert.AreEqual(1, square.GetVolume());
        
        var cylinder = new HandrailCylinder();
        cylinder.radius = 1;
        cylinder.length = 1;
        Assert.AreEqual(Mathf.PI, cylinder.GetVolume());
    }
    
    [Test]
    public void VolumeCalcTestStep()
    {
        var step = new Step();
        step.width = 1;
        step.height = 1;
        step.length = 1;
        Assert.AreEqual(1, step.GetVolume());
    }
    
    [Test]
    public void CurrencyToolsTest()
    {
        Assert.IsTrue(CurrencyTools.TryGetCurrencySymbol("USD", out var symbol));
        Assert.AreEqual("$", symbol);
        
        Assert.IsTrue(CurrencyTools.TryGetCurrencySymbol("EUR", out symbol));
        Assert.AreEqual("€", symbol);
        
        Assert.IsFalse(CurrencyTools.TryGetCurrencySymbol("XXX", out symbol));
    }
    
    [Test]
    public void GlobalEconomyTest()
    {
        var economy = new GlobalEconomy();
        economy.rates = new System.Collections.Generic.List<CurrencyPairRate>
        {
            new CurrencyPairRate() {from = Currency.USD, to = Currency.EUR, rate = 0.8f},
        };
        
        Assert.AreEqual(0.8f, economy.GetRate(Currency.USD, Currency.EUR));
        Assert.AreEqual(1.25f, economy.GetRate(Currency.EUR, Currency.USD));
    }
    
    [Test]
    public void UsersDBTest()
    {
        var db = ScriptableObject.CreateInstance<UsersDB>();
        db.admins = new System.Collections.Generic.List<AdminUser>();
        db.users = new System.Collections.Generic.List<User>();
        
        db.AddAdmin("admin", "admin");
        Assert.AreEqual(1, db.admins.Count);
        
        db.users.Add(new User());
        Assert.AreEqual(1, db.users.Count);
    }
}
