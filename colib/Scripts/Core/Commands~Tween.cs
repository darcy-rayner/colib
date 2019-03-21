namespace CoLib
{

public static partial class Cmd
{
    #region ChangeTo
    
    public static CommandDelegate ChangeTo(Ref<float> single, float endSingle, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");
        float startSingle = 0.0f;
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                startSingle = single.Value;
            }),
            Cmd.Duration( delegate(double t) {
                single.Value = (endSingle - startSingle) * (float) t + startSingle;
            }, duration, ease)
        );
    }
    
    public static CommandDelegate ChangeTo(Ref<double> single, double endSingle, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        double startSingle = 0.0;
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                startSingle = single.Value;
            }),
            Cmd.Duration( delegate(double t) {
                single.Value = (endSingle - startSingle) * t + startSingle;
            }, duration, ease)
        );
    }

    public static CommandDelegate ChangeTo(Ref<short> single, short endValue, double duration , CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        var reference = ToDoubleRef(single);
        return Cmd.Sequence (
            Cmd.Do( () => reference.Value = (double) single.Value),
            Cmd.ChangeTo(reference, (double) endValue, duration, ease)
        );
    }

    public static CommandDelegate ChangeTo(Ref<int> single, int endValue, double duration , CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        var reference = ToDoubleRef (single);
        return Cmd.Sequence (
            Cmd.Do( () => reference.Value = (double) single.Value),
            Cmd.ChangeTo(reference, (double) endValue, duration, ease)
        );
    }

    public static CommandDelegate ChangeTo(Ref<long> single, long endValue, double duration , CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        var reference = ToDoubleRef (single);
        return Cmd.Sequence (
            Cmd.Do( () => reference.Value = (double) single.Value),
            Cmd.ChangeTo(reference, (double) endValue, duration, ease)
        );
    }

    public static CommandDelegate ChangeTo<T>(IInterpolatable<T> interpolatable, T endValue, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(interpolatable, "interpolatable");

        T startValue = interpolatable.GetValue();
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                startValue = interpolatable.GetValue();
            }),
            Cmd.Duration( delegate(double t) {
                interpolatable.Interpolate(startValue, endValue, t);
            }, duration, ease)
        );
    }

    public static CommandDelegate ChangeTo<T>(Ref<T> val, T endValue, IInterpolator<T> interpolator, double duration, CommandEase ease = null) where T : struct
    {
        CheckArgumentNonNull(val, "val");
        CheckArgumentNonNull(interpolator, "interpolator");

        T startValue = val.Value;
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                startValue = val.Value;
            }),
            Cmd.Duration( delegate(double t) {
                val.Value = interpolator.Interpolate(startValue, endValue, t);
            }, duration, ease)
        );
    }

    #endregion
    
    #region ChangeBy

    public static CommandDelegate ChangeBy(Ref<float> single, float offset, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");
        double lastT = 0.0;
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                lastT = 0.0;    
            }),
            Cmd.Duration( delegate(double t) {
                single.Value +=  offset * (float) (t - lastT);
                lastT = t;
            }, duration, ease)
        );
    }
    
    public static CommandDelegate ChangeBy(Ref<double> single, double offset, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        double lastT = 0.0;
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                lastT = 0.0;    
            }),
            Cmd.Duration( delegate(double t) {
                single.Value +=  offset * (t - lastT);
                lastT = t;
            }, duration, ease)
        );
    }

    public static CommandDelegate ChangeBy(Ref<short> single, short offset, double duration , CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        var reference = ToDoubleRef(single);
        return Cmd.Sequence (
            Cmd.Do( () => reference.Value = (double) single.Value),
            Cmd.ChangeBy(reference, (double) offset, duration, ease)
        );
    }

    public static CommandDelegate ChangeBy(Ref<int> single, int offset, double duration , CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        var reference = ToDoubleRef(single);
        return Cmd.Sequence (
            Cmd.Do( () => reference.Value = (double) single.Value),
            Cmd.ChangeBy(reference, (double) offset, duration, ease)
        );
    }

    public static CommandDelegate ChangeBy(Ref<long> single, long offset, double duration , CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        var reference = ToDoubleRef(single);
        return Cmd.Sequence (
            Cmd.Do( () => reference.Value = (double) single.Value),
            Cmd.ChangeBy(reference, (double) offset, duration, ease)
        );
    }

    #endregion
    
    #region ChangeFrom
    
    public static CommandDelegate ChangeFrom(Ref<float> single, float startSingle, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        float endSingle = 0.0f;
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                endSingle = single.Value;
            }),
            Cmd.Duration( delegate(double t) {
                single.Value = (endSingle - startSingle) * (float) t + startSingle;    
            }, duration, ease)
        );
    }
    
    public static CommandDelegate ChangeFrom(Ref<double> single, double startSingle, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        double endSingle = 0.0;
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                endSingle = single.Value;
            }),
            Cmd.Duration( delegate(double t) {
                single.Value = (endSingle - startSingle) * t + startSingle;    
            }, duration, ease)
        );
    }

    public static CommandDelegate ChangeFrom(Ref<short> single, short startValue, double duration , CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        var reference = ToDoubleRef(single);
        return Cmd.Sequence (
            Cmd.Do( () => reference.Value = (double) single.Value),
            Cmd.ChangeFrom(reference, (double) startValue, duration, ease)
        );
    }

    public static CommandDelegate ChangeFrom(Ref<int> single, int startValue, double duration , CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        var reference = ToDoubleRef(single);
        return Cmd.Sequence (
            Cmd.Do( () => reference.Value = (double) single.Value),
            Cmd.ChangeFrom(reference, (double) startValue, duration, ease)
        );    }

    public static CommandDelegate ChangeFrom(Ref<long> single, long startValue, double duration , CommandEase ease = null)
    {
        CheckArgumentNonNull(single, "single");

        var reference = ToDoubleRef(single);
        return Cmd.Sequence (
            Cmd.Do( () => reference.Value = (double) single.Value),
            Cmd.ChangeFrom(reference, (double) startValue, duration, ease)
        );
    }

    public static CommandDelegate ChangeFrom<T>(IInterpolatable<T> interpolatable, T startValue, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(interpolatable, "interpolatable");

        T endValue = interpolatable.GetValue();
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                endValue = interpolatable.GetValue();
            }),
            Cmd.Duration( delegate(double t) {
                interpolatable.Interpolate(startValue, endValue, t);
            }, duration, ease)
        );
    }

    public static CommandDelegate ChangeFrom<T>(Ref<T> val, T startValue, IInterpolator<T> interpolator, double duration, CommandEase ease = null) where T : struct
    {
        CheckArgumentNonNull(val, "val");
        CheckArgumentNonNull(interpolator, "interpolator");

        T endValue = val.Value;
        return Cmd.Sequence(
            Cmd.Do(delegate() {
                endValue = val.Value;
            }),
            Cmd.Duration( delegate(double t) {
                val.Value = interpolator.Interpolate(startValue, endValue, t);
            }, duration, ease)
        );
    }

    #endregion
    
    #region ScaleBy
    
    public static CommandDelegate ScaleBy(Ref<float> scale, float scaleFactor, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(scale, "scale");

        float lastScaleFactor = 1.0f;
        return Cmd.Sequence(
            Cmd.Do(delegate(){
                lastScaleFactor = 1.0f;
            }),
            Cmd.Duration( delegate(double t) {
                float newScaleFactor = (float)t * (scaleFactor - 1.0f) + 1.0f;
                scale.Value = scale.Value * newScaleFactor / lastScaleFactor;
                lastScaleFactor = newScaleFactor;
            }, duration, ease)
        );
    }
    
    public static CommandDelegate ScaleBy(Ref<double> scale, double scaleFactor, double duration, CommandEase ease = null)
    {
        CheckArgumentNonNull(scale, "scale");

        double lastScaleFactor = 1.0;
        return Cmd.Sequence(
            Cmd.Do(delegate(){
                lastScaleFactor = 1.0;
            }),
            Cmd.Duration( delegate(double t) {
                double newScaleFactor = t * (scaleFactor - 1.0) + 1.0;
                scale.Value = scale.Value * newScaleFactor / lastScaleFactor;
                lastScaleFactor = newScaleFactor;
            }, duration, ease)
        );
    }

    #endregion

    #region Static private methods

    private static Ref<double> ToDoubleRef(this Ref<short> reference)
    {
        double val = (double) reference.Value;

        Ref<double> newReference = new Ref<double> (
            () => val,
            (t) => {
                val = t;
                reference.Value = System.Convert.ToInt16(
                    System.Math.Round(val, System.MidpointRounding.AwayFromZero)
                );
            }
        );
        return newReference;
    }

    private static Ref<double> ToDoubleRef(this Ref<int> reference)
    {
        double val = (double) reference.Value;
        Ref<double> newReference = new Ref<double> (
            () => val,
            (t) => {
                val = t;
                reference.Value = System.Convert.ToInt32(
                    System.Math.Round(val, System.MidpointRounding.AwayFromZero)
                );
            }
        );
        return newReference;
    }

    private static Ref<double> ToDoubleRef(this Ref<long> reference)
    {
        double val = (double) reference.Value;
        Ref<double> newReference = new Ref<double> (
            () => val,
            (t) => {
                val = t;
                reference.Value = System.Convert.ToInt64(
                    System.Math.Round(val, System.MidpointRounding.AwayFromZero)
                );
            }
        );
        return newReference;
    }

    #endregion
}
    
}
    

