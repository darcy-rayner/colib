using System;
using UnityEngine;

namespace CoLib
{

public static partial class Commands
{
	/// <summary>
	/// Pulsates the scale.
	/// </summary>
	/// <param name="amount">The amount to increase the scale by.</param>
	public static CommandDelegate PulsateScale(Ref<Vector3> scale, float amount, double duration) 
	{
		CheckArgumentNonNull(scale, "scale");
		CommandDelegate tweenBack = null;
		return Commands.Sequence(
			Commands.Do( () => {
				// Because we don't know what the original scale is at this point,
				// we have to recreate the scale back tween every time.
				tweenBack = Commands.ChangeTo(scale, scale.Value, duration / 2.0, Ease.Smooth());
			}),
			Commands.ScaleBy(scale, Vector3.one * (amount + 1.0f), duration / 2.0, Ease.Smooth()),
			(ref double deltaTime) => tweenBack(ref deltaTime)
		);
	}

	/// <summary>
	/// Pulsates a value.
	/// </summary>
	/// <param name="amount">The amount to increase the value by.</param>
	public static CommandDelegate PulsateScale(Ref<float> scale, float amount, double duration) 
	{
		CheckArgumentNonNull(scale, "scale");
		CommandDelegate tweenBack = null;
		return Commands.Sequence(
			Commands.Do( () => {
				// Because we don't know what the original scale is at this point,
				// we have to recreate the scale back tween every time.
				tweenBack = Commands.ChangeTo(scale, scale.Value, duration / 2.0, Ease.Smooth());
			}),
			Commands.ChangeBy(scale, amount, duration / 2.0, Ease.Smooth()),
			Commands.Defer( () => tweenBack)
		);
	}

	/// <summary>
	/// Oscillates around a value.
	/// </summary>
	/// <param name="amount">
	/// The maximum amount to oscillate away from the default value.
	/// </param>
	public static CommandDelegate Oscillate(Ref<float> single, float amount, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");
		float baseValue = 0f;
		return Commands.Sequence(
			Commands.Do( () => baseValue = single.Value),
			Commands.Duration( (t) => {
				single.Value = baseValue + Mathf.Sin((float) t * 2f * Mathf.PI) * amount;
			}, duration, ease)
		);
	}

}

}

