using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{
	
public static partial class Commands
{
	#region Image

	public static CommandDelegate TintTo(Image image, Color endColour, double duration, CommandEase ease = null)
	{
		return TintTo(image.ToColorRef(), endColour, duration, ease);
	}

	public static CommandDelegate TintFrom(Image image, Color startColour, double duration, CommandEase ease = null)
	{
		return TintFrom(image.ToColorRef(), startColour, duration, ease);
	}

	public static CommandDelegate TintBy(Image image, Color offset, double duration, CommandEase ease = null)
	{
		return TintBy(image.ToColorRef(), offset, duration, ease);
	}

	public static CommandDelegate AlphaTo(Image image, float alpha, double duration, CommandEase ease = null)
	{
		return AlphaTo(image.ToColorRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaFrom(Image image, float alpha, double duration, CommandEase ease = null)
	{
		return AlphaFrom(image.ToColorRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaBy(Image image, float alpha, double duration, CommandEase ease = null)
	{
		return AlphaBy(image.ToColorRef(), alpha, duration, ease);
	}
	
	#endregion

	#region RawImage

	public static CommandDelegate TintTo(RawImage image, Color endColour, double duration, CommandEase ease = null)
	{
		return TintTo(image.ToColorRef(), endColour, duration, ease);
	}

	public static CommandDelegate TintFrom(RawImage image, Color startColour, double duration, CommandEase ease = null)
	{
		return TintFrom(image.ToColorRef(), startColour, duration, ease);
	}

	public static CommandDelegate TintBy(RawImage image, Color offset, double duration, CommandEase ease = null)
	{
		return TintBy(image.ToColorRef(), offset, duration, ease);
	}

	public static CommandDelegate AlphaTo(RawImage image, float alpha, double duration, CommandEase ease = null)
	{
		return AlphaTo(image.ToColorRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaFrom(RawImage image, float alpha, double duration, CommandEase ease = null)
	{
		return AlphaFrom(image.ToColorRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaBy(RawImage image, float alpha, double duration, CommandEase ease = null)
	{
		return AlphaBy(image.ToColorRef(), alpha, duration, ease);
	}

	#endregion

	#region CanvasGroup

	public static CommandDelegate AlphaTo(CanvasGroup group, float alpha, double duration, CommandEase ease = null)
	{
		return ChangeTo(group.ToAlphaRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaFrom(CanvasGroup group, float alpha, double duration, CommandEase ease = null)
	{
		return ChangeFrom(group.ToAlphaRef(), alpha, duration, ease);
	}

	public static CommandDelegate AlphaBy(CanvasGroup group, float alpha, double duration, CommandEase ease = null)
	{
		return ChangeBy(group.ToAlphaRef(), alpha, duration, ease);
	}

	#endregion

	#region RectTransform

	public static CommandDelegate ScaleTo(RectTransform transform, Vector2 endScale, double duration, CommandEase ease = null)
	{
		return ChangeTo(transform.ToScaleRef(), endScale, duration, ease);
	}

	public static CommandDelegate ScaleFrom(RectTransform transform, Vector2 startScale, double duration, CommandEase ease = null)
	{
		return ChangeTo(transform.ToScaleRef(), startScale, duration, ease);
	}

	public static CommandDelegate ScaleBy(RectTransform transform, Vector2 scaleFactor, double duration, CommandEase ease = null)
	{
		return ScaleBy(transform.ToScaleRef(), scaleFactor, duration, ease);
	}

	#endregion
}

}
