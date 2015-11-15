using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{

public static class RawImageExtensions
{
	public static Ref<Color> ToColorRef(this RawImage image)
	{
		return new Ref<Color>(
			() => image.color,
			t => image.color = t
		);
	}

	public static Ref<float> ToRedRef(this RawImage image)
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

	public static Ref<float> ToGreenRef(this RawImage image)
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

	public static Ref<float> ToBlueRef(this RawImage image)
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

	public static Ref<float> ToAlphaRef(this RawImage image)
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
