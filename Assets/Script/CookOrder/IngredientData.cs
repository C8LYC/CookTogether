using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Order/Ingredient Data", order = 1)]
public class IngredientData : ScriptableObject
{
	public string Name;
	[Header("­¹§÷¹Ï¤ù")]
	public Sprite sprite;
}