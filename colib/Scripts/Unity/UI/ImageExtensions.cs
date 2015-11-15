using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{

public static class ImageExtensions
{
	public static Ref<Color> ToColorRef(this Image image)
	{
		return new Ref<Color>(
			() => image.color,
			(t) => image.color = t
		);
	}

	public static Ref<float> ToRedRef(this Image image)
	{
		return new Ref<float>(
			() => image.color.r,
			t => {
				var color = image.color;
				color.r = t;
				image.color = color;
			}
		);
	}

	public static Ref<float> ToGreenRef(this Image image)
	{
		return new Ref<float>(
			() => image.color.g,
			t => {
				var color = image.color;
				color.g = t;
				image.color = color;
			}
		);
	}

	public static Ref<float> ToBlueRef(this Image image)
	{
		return new Ref<float>(
			() => image.color.b,
			t => {
				var color = image.color;
				color.b = t;
				image.color = color;
			}
		);
	}

	public static Ref<float> ToAlphaRef(this Image image)
	{
		return new Ref<float>(
			() => image.color.a,
			t => {
				var color = image.color;
				color.a = t;
				image.color = color;
			}
		);
	}
}

}