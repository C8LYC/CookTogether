using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewMealData", menuName = "Order/Meal Data", order = 1)]
public class MealData : ScriptableObject
{
	public string Name;
	[Header("Error?")]
	public Sprite sprite;
	public List<IngredientData> NeedIngredients;
}
