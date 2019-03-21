using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoLib
{

public static partial class Cmd
{
    /// <summary>
    /// Pulsates the scale.
    /// </summary>
    /// <param name="amount">The amount to increase the scale by.</param>
    public static CommandDelegate PulsateScale(Ref<Vector3> scale, float amount, double duration) 
    {
        CheckArgumentNonNull(scale, "scale");
        CommandDelegate tweenBack = null;
        return Cmd.Sequence(
            Cmd.Do( () => {
                // Because we don't know what the original scale is at this point,
                // we have to recreate the scale back tween every time.
                tweenBack = Cmd.ChangeTo(scale, scale.Value, duration / 2.0, Ease.Smooth());
            }),
            Cmd.ScaleBy(scale, Vector3.one * (amount + 1.0f), duration / 2.0, Ease.Smooth()),
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
        return Cmd.Sequence(
            Cmd.Do( () => {
                // Because we don't know what the original scale is at this point,
                // we have to recreate the scale back tween every time.
                tweenBack = Cmd.ChangeTo(scale, scale.Value, duration / 2.0, Ease.Smooth());
            }),
            Cmd.ChangeBy(scale, amount, duration / 2.0, Ease.Smooth()),
            Cmd.Defer( () => tweenBack)
        );
    }

    /// <summary>
    /// Oscillates around a value. This will animation from
    ///  startValue > startValue + amount > startValue - amount- > startValue, 
    /// in a smooth circular motion.
    /// </summary>
    /// <param name="amount">
    /// The maximum amount to oscillate away from the default value.
    /// </param>
    public static CommandDelegate Oscillate(Ref<float> single, float amount, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");
        float baseValue = 0f;
        return Cmd.Sequence(
            Cmd.Do( () => baseValue = single.Value),
            Cmd.Duration( t => {
                single.Value = baseValue + Mathf.Sin((float) t * 2f * Mathf.PI) * amount;
            }, duration, ease)
        );
    }

    /// <summary>
    /// Oscilates a quaternion.
    /// </summary>
    /// <param name="amount"> The amount to wiggle by in degrees.</param>
    public static CommandDelegate Wiggle(Ref<Quaternion> rotation, float amount, double duration)
    {
        CheckArgumentNonNull(rotation, "rotation");
        return Wiggle(rotation, amount, duration, Vector3.forward);
    }

    /// <summary>
    /// Oscilates a quaternion.
    /// </summary>
    /// <param name="amount"> The amount to wiggle by in degrees.</param>
    /// <param name="axis"> The axis to wiggle around </param>
     public static CommandDelegate Wiggle(Ref<Quaternion> rotation, float amount, double duration, Vector3 axis)
    {
        CheckArgumentNonNull(rotation, "rotation");

        Quaternion startQuaternion = Quaternion.identity;
        
        float val = 0.0f;
        
        Ref<float> floatRef = new Ref<float>(
            () => val,
            (t) => {
                val = t;
                rotation.Value = startQuaternion * Quaternion.AngleAxis(val, axis);
            }
        );
        
        return Cmd.Sequence(
            Cmd.Do( () => { 
                startQuaternion = rotation.Value; 
                val = 0.0f;
            }),
            
            Cmd.Oscillate(
                floatRef, amount, duration, Ease.Smooth()
            )
        );
    }

    /// <summary>
    /// Wobble a value. This oscilates a value, with a decay.
    /// </summary>
    /// <param name="amplitude">Amplitude.</param>
    /// <param name="duration">Duration.</param>
    public static CommandDelegate Wobble(Ref<float> single, float amount, double duration) 
    {
        CheckArgumentNonNull(single, "val");

        float intervals = 3f; 
        float decay = Mathf.Log(100f * amount);
        
        float baseVal = 0f;
        return Cmd.Sequence(
            Cmd.Do( () => {
                baseVal = single.Value;
            }),
              Cmd.Duration(
                t => {
                    float decayCoeef = Mathf.Exp((float)t * decay);
                    single.Value = baseVal + amount * Mathf.Sin(intervals * (float)t * 2 * Mathf.PI) / decayCoeef;
                },
                duration
            ), 
            Cmd.Do( () => {
                single.Value = baseVal;
            })
        );    
    }

    /// <summary>
    /// Squashes the y axis, while inversely stretching the x axis.
    /// </summary>
    /// <param name="val">The value to animate.</param>
    /// <param name="amplitude">The size of the squash.</param>
    /// <param name="duration">The duration of the squash.</param>
    public static CommandDelegate SquashAndStretch(Ref<Vector3> scale, float amplitude, double duration) 
    {
        CheckArgumentNonNull(scale, "scale");
        return SquashAndStretch(scale, amplitude, duration, Vector3.up, new Vector3(0.5f, 0f, 0.5f).normalized);
    }

    /// <summary>
    /// Squashes the normal axis, while inversely stretching the tangent axis.
    /// </summary>
    /// <param name="val">The value to animate.</param>
    /// <param name="amplitude">The size of the squash.</param>
    /// <param name="duration">The duration of the squash.</param>
    /// <param name="normal"> The normal of the animation. </param>
    /// <param name="tangent"> The tangent of the animation. </param>
    public static CommandDelegate SquashAndStretch(Ref<Vector3> scale, float amplitude, double duration, Vector3 normal, Vector3 tangent) 
    {
        CheckArgumentNonNull(scale, "scale");
        if (normal.magnitude == 0f) {
            throw new InvalidOperationException("Normal must have non-zero magnitude.");
        }
        if (tangent.magnitude == 0f) {
            throw new InvalidOperationException("Tangent must have non-zero magnitude.");
        }
        normal.Normalize();
        tangent.Normalize();
        Vector3 cross = Vector3.Cross(normal, tangent);

        Ref<Vector2> tempRef = new Ref<Vector2>(
            () => new Vector2(Vector3.Dot(scale.Value, tangent), Vector3.Dot(scale.Value, normal)),
            t => scale.Value = t.x * tangent + t.y * normal + Vector3.Dot(scale.Value, cross) * cross
        );
        return SquashAndStretch(tempRef, amplitude, duration);
    }

    /// <summary>
    /// Squashes the y axis, while inversely stretching the x axis.
    /// </summary>
    /// <param name="val">The value to animate.</param>
    /// <param name="amplitude">The size of the squash.</param>
    /// <param name="duration">The duration of the squash.</param>
    public static CommandDelegate SquashAndStretch(Ref<Vector2> scale, float amplitude, double duration) 
    {
        CheckArgumentNonNull(scale, "scale");

        Vector2 startScale = Vector2.zero;
        float area = 0f;
        Ref<float> widthRef = new Ref<float>(
            () => scale.Value.x,
            (t) => {
                Vector2 tempVal = scale.Value;
                tempVal.x = t;
                scale.Value = tempVal;
            }
        );    
        
        return Cmd.Sequence(
            Cmd.Do( () => {
                area = scale.Value.x * scale.Value.y;
                startScale = scale.Value;
            }),
            Cmd.Parallel(
                Wobble(widthRef, amplitude, duration),
                Cmd.Duration((t) => {
                    Vector2 tempVal = scale.Value;
                    if (tempVal.x != 0f) {
                        tempVal.y = area / tempVal.x;
                    }
                    scale.Value = tempVal;
                }, duration)
            ),
            Cmd.Do( () => {
                scale.Value = startScale;
            })
        );
    }

    /// <summary>
    /// Performs a squash and stretch animation, while changing to a target scale.
    /// </summary>
    /// <param name="scale">The value to animate.</param>
    /// <param name="endScale">The final scale.</param>
    /// <param name="amplitude">The amplitude of a squash and strech</param>
    /// <param name="duration">The duration of the animation</param>
    public static CommandDelegate ScaleSquashAndStretchTo(Ref<Vector3> scale, Vector3 endScale, float amplitude, double duration)
    {
        CheckArgumentNonNull(scale, "scale");
        return ScaleSquashAndStretchTo(scale, endScale, amplitude, duration, Vector3.up, new Vector3(0.5f, 0f, 0.5f).normalized);
    }

    /// <summary>
    /// Performs a squash and stretch animation, while changing to a target scale.
    /// </summary>
    /// <param name="scale">The value to animate.</param>
    /// <param name="endScale">The final scale.</param>
    /// <param name="amplitude">The amplitude of a squash and strech</param>
    /// <param name="duration">The duration of the animation</param>
    /// <param name="normal"> The normal of the animation. </param>
    /// <param name="tangent"> The tangent of the animation. </param>
    public static CommandDelegate ScaleSquashAndStretchTo(Ref<Vector3> scale, Vector3 endScale, float amplitude, double duration, Vector3 normal, Vector3 tangent)
    {    
        CheckArgumentNonNull(scale, "scale");
        var squashRef = Ref<Vector3>.Create(Vector3.one);
        var scaleRef = Ref<Vector3>.Create();
        return Cmd.Sequence(
            Cmd.Do( () => scaleRef.Value = scale.Value),
            Cmd.Parallel(
                SquashAndStretch(squashRef, amplitude, duration, normal, tangent),
                Cmd.ChangeTo(scaleRef, endScale, duration / 4, Ease.Smooth()),
                Cmd.Duration(t => scale.Value = Vector3.Scale(squashRef.Value,scaleRef.Value), duration)
            )
        );
    }

    public static CommandDelegate ScaleSquashAndStretchFrom(Ref<Vector3> scale, Vector3 startScale, float amplitude, double duration)
    {
        CheckArgumentNonNull(scale, "scale");
        return ScaleSquashAndStretchFrom(scale, startScale, amplitude, duration, Vector3.up, new Vector3(0.5f, 0f, 0.5f).normalized);
    }

    /// <summary>
    /// Performs a squash and stretch animation, while changing from a target scale.
    /// </summary>
    /// <param name="scale">The value to animate.</param>
    /// <param name="startScale">The scale to animate from.</param>
    /// <param name="amplitude">The amplitude of a squash and strech</param>
    /// <param name="duration">The duration of the animation</param>
    /// <param name="normal"> The normal of the animation. </param>
    /// <param name="tangent"> The tangent of the animation. </param>
    public static CommandDelegate ScaleSquashAndStretchFrom(Ref<Vector3> scale, Vector3 startScale, float amplitude, double duration, Vector3 normal, Vector3 tangent)
    {
        CheckArgumentNonNull(scale, "scale");
        Vector3 targetScale = Vector3.zero;
        return Cmd.Sequence(
            Cmd.Do( () => {
                targetScale = scale.Value;
                scale.Value = startScale;
            }),
            Cmd.Defer( () => Cmd.ScaleSquashAndStretchTo(scale, targetScale, amplitude, duration, normal, tangent))
        );
    }

    /// <summary>
    /// Shakes the vector several times in a random value, up to amount in magnitude.
    /// </summary>
    /// <param name="amount">The maximum displacement of the vector.</param>
    public static CommandDelegate Shake(Ref<Vector2> vector, float amount, double duration)
    {
        return Shake(vector, Vector2.one * amount, duration);
    }

    /// <summary>
    /// Shakes the vector several times in a random value, up to amount in magnitude.
    /// </summary>
    /// <param name="amount">The maximum displacement of the vector, with separate x and y values.</param>
    public static CommandDelegate Shake(Ref<Vector2> single, Vector2 amount, double duration)
    {
        const int ShakesPerSecond = 12;
        int numShakes = Mathf.CeilToInt((float)duration * ShakesPerSecond);
        return Shake(single, amount, numShakes, duration);
    }

    /// <summary>
    /// Shakes the vector several times in a random value, up to amount in magnitude.
    /// </summary>
    /// <param name="amount">The maximum displacement of the vector, with separate x and y values.</param>
    /// <param name="numShakes"> The number of shakes to perform. </param>
    public static CommandDelegate Shake(Ref<Vector2> vector, Vector2 amount, int numShakes, double duration)
    {
        CheckArgumentNonNull(vector);
        if (numShakes <= 0) {
            throw new ArgumentOutOfRangeException("numBounces", "Must be larger than 0.");
        }
        double avgDuration = duration / (numShakes + 1);
        CommandDelegate[] list = new CommandDelegate[numShakes + 1];
        return Cmd.Defer( 
            () => {
                var baseVal = vector.Value;
                for (int i = 0; i < numShakes; ++i) {
                    // Generate an offset within the range -1, 1
                    var offset = new Vector2(
                        UnityEngine.Random.Range(-1f, 1f),
                        UnityEngine.Random.Range(-1f, 1f)
                    );
                    if (offset.magnitude > 1f) {
                        // Randomize the length of the offset if it is too large.
                        offset = offset.normalized * UnityEngine.Random.Range(0f, 1f);    
                    }
                    // Scale the offset, by the amount, and the scale factor.
                    offset = new Vector2(offset.x * amount.x, offset.y * amount.y);
                    list[i] = Cmd.ChangeTo(vector, baseVal + offset, avgDuration);                        
                }
                list[numShakes] = Cmd.ChangeTo(vector, baseVal, avgDuration);
                return Cmd.Sequence(list);
            }
        );
    }

    /// <summary>
    /// Shakes the vector several times in a random value, up to amount in magnitude.
    /// </summary>
    /// <param name="amount">The maximum displacement of the vector.</param>
    public static CommandDelegate Shake(Ref<Vector3> vector, float amount, double duration)
    {
        return Shake(vector, Vector3.one * amount, duration);
    }

    /// <summary>
    /// Shakes the vector several times in a random value, up to amount in magnitude.
    /// </summary>
    /// <param name="amount">The maximum displacement of the vector, with separate x,y and z values.</param>
    public static CommandDelegate Shake(Ref<Vector3> vector, Vector3 amount, double duration)
    {
        const int ShakesPerSecond = 12;
        int numShakes = Mathf.CeilToInt((float)duration * ShakesPerSecond);
        return Shake(vector, amount, numShakes, duration);
    }

    /// <summary>
    /// Shakes the vector several times in a random value, up to amount in magnitude.
    /// </summary>
    /// <param name="amount">The maximum displacement of the vector, with separate x,y and z values.</param>
    /// <param name="numShakes"> The number of shakes to perform. </param>
    public static CommandDelegate Shake(Ref<Vector3> vector, Vector3 amount, int numShakes, double duration)
    {
        CheckArgumentNonNull(vector);
        if (numShakes <= 0) {
            throw new ArgumentOutOfRangeException("numBounces", "Must be larger than 0.");
        }

        double avgDuration = duration / (numShakes + 1);

        CommandDelegate[] list = new CommandDelegate[numShakes + 1];
        return Cmd.Defer( 
            () => {
                var baseVal = vector.Value;
                for (int i = 0; i < numShakes; ++i) {
                    // Generate an offset within the range -1, 1
                    var offset = new Vector3(
                        UnityEngine.Random.Range(-1f, 1f),
                        UnityEngine.Random.Range(-1f, 1f),
                        UnityEngine.Random.Range(-1f, 1f)

                    );
                    if (offset.magnitude > 1f) {
                        // Randomize the length of the offset if it is too large.
                        offset = offset.normalized * UnityEngine.Random.Range(0f, 1f);    
                    }

                    // Scale the offset, by the amount, and the scale factor.
                    offset = new Vector3(offset.x * amount.x, offset.y * amount.y, offset.z * amount.z);
                    list[i] = Cmd.ChangeTo(vector, baseVal + offset, avgDuration);                        
                }
                list[numShakes] = Cmd.ChangeTo(vector, baseVal, avgDuration);
                return Cmd.Sequence(list);
            }
        );
    }

    /// <summary>
    /// Shakes the vector several times in a random value, up to amount in magnitude.
    /// </summary>
    /// <param name="amount">The maximum displacement of the rotation, in degrees.</param>
    public static CommandDelegate Shake(Ref<Quaternion> rotation, float amount, double duration)
    {
        const int ShakesPerSecond = 12;
        int numShakes = Mathf.CeilToInt((float)duration * ShakesPerSecond);
        return Shake(rotation, amount, numShakes, duration);
    }

    /// <summary>
    /// Shakes the vector several times in a random value, up to amount in magnitude.
    /// </summary>
    /// <param name="amount">The maximum displacement of the rotation, in degrees.</param>
    /// <param name="numShakes"> The number of shakes to perform. </param>
    public static CommandDelegate Shake(Ref<Quaternion> rotation, float amount, int numShakes, double duration)
    {
        CheckArgumentNonNull(rotation);
        if (amount <= 0f || amount > 180f) {
            throw new ArgumentOutOfRangeException("amount", "Must be larger than 0.");
        }
        if (numShakes <= 0) {
            throw new ArgumentOutOfRangeException("numBounces", "Must be larger than 0.");
        }


        amount = Mathf.Abs(amount);

        double avgDuration = duration / (numShakes + 1);

        CommandDelegate[] list = new CommandDelegate[numShakes + 1];
        return Cmd.Defer( 
            () => {
                var baseVal = rotation.Value;
                for (int i = 0; i < numShakes; ++i) {
                    // Generate an offset within the range -amount, amount
                    var offset = Quaternion.Euler(
                        UnityEngine.Random.Range(-amount, amount),
                        UnityEngine.Random.Range(-amount, amount),
                        UnityEngine.Random.Range(-amount, amount)
                    );

                    float angle = Quaternion.Angle(Quaternion.identity, offset);
                    // Clamp the offset
                    if (angle > amount) {
                        float t = UnityEngine.Random.Range(0f, angle / amount);
                        offset = Quaternion.LerpUnclamped(Quaternion.identity, offset, t);
                    }

                    list[i] = Cmd.RotateTo(rotation, baseVal * offset, avgDuration);                        
                }
                list[numShakes] = Cmd.RotateTo(rotation, baseVal, avgDuration);
                return Cmd.Sequence(list);
            }
        );
    }
}

}

