using System;
using System.Collections.Generic;

namespace GameScripts.ConsumeSystem.Module
{
    [Serializable]
    public class PriceTemplate
    {
        public List<PriceData> prices;

        public PriceTemplate(List<PriceData> price)
        {
            prices = price;
        }
    }

}