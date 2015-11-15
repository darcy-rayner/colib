using UnityEngine;
using System;

namespace CoLib
{

public static class MaterialExtensions 
{
	#region Extension methods

	public static Ref<int> ToIntPropertyRef(this Material material, string property)
	{
		CheckMaterialNonNull(material);
		CheckPropertyExists(material, property);
		int propertyIndex = Shader.PropertyToID(property);
		return material.ToIntPropertyRef(propertyIndex);
	}

	public static Ref<int> ToIntPropertyRef(this Material material, int property)
	{
		CheckMaterialNonNull(material);
		CheckPropertyExists(material, property);
		return new Ref<int>(
			() => material.GetInt(property),
			t => material.SetInt(property, t)
		);
	}

	public static Ref<float> ToFloatPropertyRef(this Material material, string property)
	{
		CheckMaterialNonNull(material);
		CheckPropertyExists(material, property);
		int propertyIndex = Shader.PropertyToID(property);
		return material.ToFloatPropertyRef(propertyIndex);
	}

	public static Ref<float> ToFloatPropertyRef(this Material material, int property)
	{
		CheckMaterialNonNull(material);
		CheckPropertyExists(material, property);

		return new Ref<float>(
			() => material.GetFloat(property),
			t => material.SetFloat(property, t)
		);
	}

	public static Ref<Vector4> ToVectorPropertyRef(this Material material, string property)
	{
		CheckMaterialNonNull(material);
		CheckPropertyExists(material, property);
		int propertyIndex = Shader.PropertyToID(property);
		return material.ToVectorPropertyRef(propertyIndex);
	}

	public static Ref<Vector4> ToVectorPropertyRef(this Material material, int property)
	{
		CheckMaterialNonNull(material);
		CheckPropertyExists(material, property);
		return new Ref<Vector4>(
			() => material.GetVector(property),
			t => material.SetVector(property, t)
		);
	}

	public static Ref<Color> ToColourPropertyRef(this Material material, string property)
	{
		CheckMaterialNonNull(material);
		CheckPropertyExists(material, property);
		int propertyIndex = Shader.PropertyToID(property);
		return material.ToColourPropertyRef(propertyIndex);
	}

	public static Ref<Color> ToColourPropertyRef(this Material material, int property)
	{
		CheckMaterialNonNull(material);
		CheckPropertyExists(material, property);

		return new Ref<Color>(
			() => material.GetColor(property),
			t => material.SetColor(property, t)
		);
	}

	#endregion

	#region Private methods

	private static void CheckMaterialNonNull(Material material)
	{
		if (material == null) {
			throw new ArgumentNullException("material");
		}
	}

	private static void CheckPropertyExists(Material material, string property)
	{
		if (string.IsNullOrEmpty(property)) {
			throw new ArgumentNullException("property");
		}

		if (!material.HasProperty(property)) {
			throw new InvalidOperationException(string.Format("Material doesn't have property named {0}", property));
		}
	}

	private static void CheckPropertyExists(Material material, int property)
	{
		if (!material.HasProperty(property)) {
			throw new InvalidOperationException(string.Format("Material doesn't have property with ID {0}", property));
		}
	}

	#endregion

}

}
