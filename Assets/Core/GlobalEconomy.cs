using System.Collections.Generic;
using UnityEngine;

namespace StairsCalc
{
    [CreateAssetMenu(fileName = "GlobalEconomy", menuName = "StairsCalc/GlobalEconomy", order = 0)]
    public class GlobalEconomy : ScriptableObject
    {
        public static readonly string GlobalEconomyPath = "GlobalEconomy";
        public List<CurrencyPairRate> rates;
        
        public static GlobalEconomy GetEconomy()
        {
            return Resources.Load<GlobalEconomy>(GlobalEconomyPath);
        }
        
        public float GetRate(Currency from, Currency to)
        {
            foreach (var rate in rates)
            {
                if (rate.from == from && rate.to == to)
                {
                    return rate.rate;
                }
                
                if (rate.from == to && rate.to == from)
                {
                    return 1.0f / rate.rate;
                }
            }

            return 1.0f;
        }
    }
}