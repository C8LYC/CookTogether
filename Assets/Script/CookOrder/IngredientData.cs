using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Order/Ingredient Data", order = 1)]
public class IngredientData : ScriptableObject
{
	public string Name;
	public Color color;
	[Header("�����Ϥ�")]
	public Sprite sprite;
}