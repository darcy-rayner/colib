<<<<<<< HEAD
namespace CoLib
{

public interface IInterpolatable<T>
{
    T GetValue();

    /// <summary>
    /// Get an interpolated value between startValue and endValue.
    /// When t = 0, startValue should be returned, and when t = 1,
    /// endValue should be returned. T is typically between 0 - 1,
    /// but the implementor should be able to accept values outside
    /// that range.
    /// </summary>
    T Interpolate(T startValue, T endValue, double t);
}

}

=======
namespace CoLib
{

public interface IInterpolatable<T>
{
    T GetValue();

    /// <summary>
    /// Get an interpolated value between startValue and endValue.
    /// When t = 0, startValue should be returned, and when t = 1, 
    /// endValue should be returned. T is typically between 0 - 1,
    /// but the implementor should be able to accept values outside 
    /// that range.
    /// </summary>
    T Interpolate(T startValue, T endValue, double t);
}

}

>>>>>>> 3c368a71062a6e4c49298b44dcdd13b67b1cef69
