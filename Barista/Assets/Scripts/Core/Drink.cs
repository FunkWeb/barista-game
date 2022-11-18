using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class Drink : MonoBehaviour, IEventReceiver<MainDispenser.Used>, IEventReceiver<SideDispenser.Used>, IEventReceiver<Customer.Leave>,
                                        IEventReceiver<LidPile.AttemptLidDrink>, IEventReceiver<CupPile.AttemptNewCup>
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        public bool Lidded{get; private set;}

        private bool _hasCup;
        public bool HasCup
        {
            private get{ return _hasCup;} 
            //Only show displayCup if there is a cup to show
            set
            {
                _hasCup = value;
                _displayCupContents.gameObject.SetActive(_hasCup);
            }
        }

        [Header("References"), SerializeField]
        private DisplayCupContents _displayCupContents;

        [SerializeField]
        public DrinkMixture DrinkMixture = new DrinkMixture(); //Holds the actual ingredients and amounts of liquid in the drink.

        private void Awake()
        {
            _displayCupContents.DrinkMixture = DrinkMixture;
        }

        //Subscribe to events
        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        //Receive events from pEventBus
        public void OnEvent(MainDispenser.Used e)
        {
            if (!HasCup)
            {
                TestUI.Log("Cannot fill drink. No cup to fill.");
                return;
            }
                
            AddMainIngredient(e.ingredient, e.amount);
            _displayCupContents.UpdateDisplay();
        }
        public void OnEvent(SideDispenser.Used e)
        {
            if (!HasCup)
            {
                TestUI.Log("Cannot fill drink. No cup to fill.");
                return;
            }
            AddSideIngredient(e.ingredient);
            _displayCupContents.UpdateSideIngredientDisplay();
        }
        public void OnEvent(Customer.Leave e)
        {
            if (e.satisfied)
                Clear();
        }
        public void OnEvent(LidPile.AttemptLidDrink e)
        {
            if (!HasCup)
            {
                TestUI.Log("There is no cup to lid.");
                return;
            }
            if (!Lidded)
                Lidded = true;
            
            //Todo: Assemble drink and store resulting recipe.

            //Todo: Update visual display drink to match assembled drink + lid graphics.

            //Todo: Spawn and transfer drink info to receipt object.

        }
        public void OnEvent(CupPile.AttemptNewCup e)
        {
            if (!HasCup)
                HasCup = true;
        }

        public void AddMainIngredient(MainIngredientData ingredient, float amount)
        {
            //Add non existant ingredient keys before attempting to change any values. Prevents crash from attempting to change value of yet to be added key.
            if (!DrinkMixture.MainIngredients.ContainsKey(ingredient))
                DrinkMixture.MainIngredients.Add(ingredient, 0f);

            //If cup does not overflow if amount is added, allow it
            if (DrinkMixture.GetTotalLiquid + amount < DrinkMixture.MaxTotalLiquid)
            {
                DrinkMixture.MainIngredients[ingredient] += amount;
            }
            else //If it would overflow, fill to the brim instead.
            {
                DrinkMixture.MainIngredients[ingredient] += (DrinkMixture.MaxTotalLiquid - DrinkMixture.GetTotalLiquid);
            }
        }

        public void AddSideIngredient(SideIngredientData ingredient)
        {
            DrinkMixture.SideIngredients.HashSet.Add(ingredient);
        }

        public void Clear()
        {
            DrinkMixture = new DrinkMixture();
            _displayCupContents.DrinkMixture = DrinkMixture;
            _displayCupContents.ResetIngredientDisplay();
            HasCup = false;
            //if (_debugLogsEnabled)
                //TestUI.Log("Drink contents emptied.");
        }


        
    }
}

