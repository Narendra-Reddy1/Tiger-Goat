using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SovereignStudios
{
    [Serializable]
    public abstract class VirtualItem
    {
        public delegate void CurrencyBalance();
        public static event CurrencyBalance onCurrencyBalanceChanged;

        public ResourceID itemId;
        public VirtualItemType virtualItemType;

        public VirtualItem(ResourceID itemId, VirtualItemType virtualItemType)
        {
            this.itemId = itemId;
            this.virtualItemType = virtualItemType;
        }

        public virtual int GetBalance()
        {
            return 0;
        }

        public virtual bool Buy()
        {
            return false;
        }

        //public virtual void Give (){}

        public virtual void Give(int quantity)
        {
            Debug.LogWarning("NOT APPLICABLE BUT INVESTIGATE IF HAPENING FOR ANYTHING EXCEPT UPGRADABLE");
        }

        //public virtual void Take (){}

        public virtual void Take(int quantity)
        {
            Debug.LogWarning("NOT APPLICABLE BUT INVESTIGATE IF HAPENING FOR ANYTHING EXCEPT UPGRADABLE");
        }

        public virtual bool CanAfford()
        {
            return false;
        }

        //public virtual CurrencyType GetPurchaseCurrency()
        //{
        //    return PlayerResourcesManager.currencyType;
        //}

        public virtual int GetPurchaseValue()
        {
            return 100;
        }

        //public virtual void DeductCurrencyBalance(CurrencyType purchaseCurrency, int puchasePrice)
        //{
        //    switch (purchaseCurrency)
        //    {
        //        case CurrencyType.KEDOS:

        //            Debug.Log(this.GetType().Name + "  " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        //            PlayerResourcesManager.Take(ResourceID.KEDOS_ID, puchasePrice);
        //            invokeCurrencyBalanceChangedListeners();
        //            break;
        //        default:
        //            SovereignUtils.LogError($"Invalid currency Type: {purchaseCurrency} {this.GetType().Name} : {System.Reflection.MethodBase.GetCurrentMethod().Name}");
        //            break;
        //    }
        //}

        public void invokeCurrencyBalanceChangedListeners()
        {
            if (onCurrencyBalanceChanged != null)
            {
                onCurrencyBalanceChanged();
            }
        }

    }

    public enum VirtualItemType
    {
        CURRENCY,
        CONSUMABLE,
        EQUIPABLE,
        UPGRADABLE,
        POWERUP
    }
}