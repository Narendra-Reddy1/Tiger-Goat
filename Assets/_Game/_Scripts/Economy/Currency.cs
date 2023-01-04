using SovereignStudios.Enums;
using UnityEngine;

namespace SovereignStudios
{
    [System.Serializable]
    public class Currency : VirtualItem
    {

        public int balance;
        public int collectedBalance;
        public int maxBalance;
        public int MaxBalance
        {
            get
            {
                return this.maxBalance;
            }
        }

        public Currency(ResourceID itemId)
            : base(itemId, VirtualItemType.CURRENCY)
        {
            this.balance = 0;
            this.maxBalance = 9999999;
        }

        public Currency(ResourceID itemId, int maxBalance)
            : base(itemId, VirtualItemType.CURRENCY)
        {
            this.balance = 0;
            this.maxBalance = maxBalance;
        }

        public Currency(ResourceID itemId, int startBalance, int maxBalance)
            : base(itemId, VirtualItemType.CURRENCY)
        {
            this.balance = startBalance;
            this.maxBalance = maxBalance;
        }

        //This method will return the total balance
        public override int GetBalance()
        {
            return balance + GetCollectedBalance();
        }

        //This method will return the non collected balance
        public int GetNonCollectedBalance()
        {
            return balance;
        }

        //This method will return the collected balance
        public int GetCollectedBalance()
        {
            return collectedBalance;
        }

        //BUY NOT APPLICABLE

        public override void Give(int quantity)
        {
            //Give will update the collected balance, actual balance will get updated 
            //after the HUD animation in home screen

            collectedBalance += quantity;

            //Debug.LogError("balance : " + balance + " quantity : " + quantity + " maxBalance : " + maxBalance);

            //balance += quantity;
            //balance = Mathf.Clamp(balance + quantity, 0, maxBalance);
            //base.invokeCurrencyBalanceChangedListeners();
        }

        //This method updates the actual balance and clears the collected balance
        public void UpdateBalance()
        {
            balance = Mathf.Clamp(balance + GetCollectedBalance(), 0, maxBalance);
            base.invokeCurrencyBalanceChangedListeners();
            collectedBalance = 0;
        }

        /// <summary>
        /// Takes from balance.
        /// </summary>
        public override void Take(int quantity)
        {
            if (quantity <= GetNonCollectedBalance() + GetCollectedBalance())
            {
                if (quantity <= GetNonCollectedBalance())
                {
                    balance -= quantity;
                }
                else
                {
                    collectedBalance -= quantity - balance;
                    balance = 0;
                }
            }
            else
            {
                balance = 0;
                collectedBalance = 0;
            }
            base.invokeCurrencyBalanceChangedListeners();
        }

        // CAN AFFORD, PURCHASE CURRENCY, PURCHASE VALUE NOT APPLICABLE
        /*public virtual string GetPurchaseCurrency (){
            return "";
        }

        public virtual int GetPurchaseValue (){
            return 0;
        }*/


        /// <summary>
        /// Resets the balance.
        /// </summary>
        public void ResetBalance()
        {
            balance = 0;
        }

    }
}