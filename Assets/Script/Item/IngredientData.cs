using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Order/Ingredient Data", order = 1)]
public class IngredientData : ScriptableObject
{
	[SerializeField] bool throwable = false;
	[SerializeField] string Name;
	[SerializeField] List<conbinedIngredient> combinedIngredient= new List<conbinedIngredient>();
	[Serializable]
	class conbinedIngredient
	{
		public IngredientData result;
		public IngredientData combinedWith;
	}

	
	public Ingredient ingredientPrefab;
	// tempory for replacing object
	public Color color; 
	public Sprite sprite;

	public IngredientData mixedIngredient(IngredientData addedIngredient)
	{
		foreach (var ingredient in combinedIngredient)
		{
			if (ingredient.combinedWith == addedIngredient)
			{
				return ingredient.result;
			}
		}

		return null;
	}
}