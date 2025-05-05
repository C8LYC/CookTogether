using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalTable : Block
{
    [SerializeField] Item _itemHold;
    [SerializeField] Transform _itemHoldPivot;
    public override void Interact(Player player)
    {
       
        if (player.itemHold != null )
        {
            if (_itemHold != null)
            { 
                // check the below stament correctness by debug log 
                // print itemhold.converto<ingredient>
                Debug.Log($" has ingredient? : {_itemHold.ConvertTo<Ingredient>()!=null}.");
                Debug.Log($" ingredient data? : {_itemHold.ConvertTo<Ingredient>()?.ingredientData}.");
                Debug.Log($" input item data? : {player .itemHold!=null}.");
                IngredientData ingredientData = _itemHold.ConvertTo<Ingredient>()?.ingredientData.mixedIngredient(player.itemHold.ConvertTo<Ingredient>()?.ingredientData);
                if (ingredientData)
                {
                    // Create enittye of ingredinet data
                    Item newItem = Instantiate(ingredientData.ingredientPrefab);
                    DestroyImmediate(_itemHold.gameObject);
                    _itemHold = newItem;
                    _itemHold.transform.SetParent(_itemHoldPivot);
                    _itemHold.transform.localPosition = Vector3.zero;
                    _itemHold.transform.localRotation = Quaternion.identity;
                    DestroyImmediate(player.itemHold.gameObject);
                    player.SetItemHold(null);
                    
                }
                
                return;
            }
            Debug.Log("Table is empty, placing item.");
            _itemHold = player.itemHold;
            _itemHold.transform.SetParent(_itemHoldPivot);
            _itemHold.transform.localPosition = Vector3.zero;
            _itemHold.transform.localRotation = Quaternion.identity;
            player.SetItemHold(null);


        }
        else if (_itemHold != null && player.itemHold==null)
        {
            player.SetItemHold(_itemHold);
            _itemHold = null;
            Debug.Log($"Item {player.itemHold.name} was picked up from the table.");
        }
        
       
        
    }

    
}
