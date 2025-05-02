using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewRecipeData", menuName = "Order/Recipe Data", order = 1)]

public class RecipeData : ScriptableObject
{
	public string Name;
	public MealData TargetMeals;
	public int Fee;
	public float TImeLimit;
}
