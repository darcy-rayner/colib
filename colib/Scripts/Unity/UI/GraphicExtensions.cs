<<<<<<< HEAD
﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{

public static class GraphicExtensions
{
    #region Extension methods

    public static Ref<Color> ToColorRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);
        return new Ref<Color>(
            () => graphic.color,
            (t) => graphic.color = t
        );
    }

    public static Ref<float> ToRedRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);

        return new Ref<float>(
            () => graphic.color.r,
            t => {
                var color = graphic.color;
                color.r = t;
                graphic.color = color;
            }
        );
    }

    public static Ref<float> ToGreenRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);

        return new Ref<float>(
            () => graphic.color.g,
            t => {
                var color = graphic.color;
                color.g = t;
                graphic.color = color;
            }
        );
    }

    public static Ref<float> ToBlueRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);

        return new Ref<float>(
            () => graphic.color.b,
            t => {
                var color = graphic.color;
                color.b = t;
                graphic.color = color;
            }
        );
    }

    public static Ref<float> ToAlphaRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);

        return new Ref<float>(
            () => graphic.color.a,
            t => {
                var color = graphic.color;
                color.a = t;
                graphic.color = color;
            }
        );
    }

    #endregion

    #region Private methods

    private static void CheckGraphicNonNull(Graphic graphic)
    {
        if (graphic == null) {
            throw new ArgumentNullException("graphic");
        }
    }

    #endregion
}

}
=======
﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{

public static class GraphicExtensions
{
    #region Extension methods

    public static Ref<Color> ToColorRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);
        return new Ref<Color>(
            () => graphic.color,
            (t) => graphic.color = t
        );
    }

    public static Ref<float> ToRedRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);

        return new Ref<float>(
            () => graphic.color.r,
            t => {
                var color = graphic.color;
                color.r = t;
                graphic.color = color;
            }
        );
    }

    public static Ref<float> ToGreenRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);

        return new Ref<float>(
            () => graphic.color.g,
            t => {
                var color = graphic.color;
                color.g = t;
                graphic.color = color;
            }
        );
    }

    public static Ref<float> ToBlueRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);

        return new Ref<float>(
            () => graphic.color.b,
            t => {
                var color = graphic.color;
                color.b = t;
                graphic.color = color;
            }
        );
    }

    public static Ref<float> ToAlphaRef(this Graphic graphic)
    {
        CheckGraphicNonNull(graphic);

        return new Ref<float>(
            () => graphic.color.a,
            t => {
                var color = graphic.color;
                color.a = t;
                graphic.color = color;
            }
        );
    }

    #endregion

    #region Private methods

    private static void CheckGraphicNonNull(Graphic graphic)
    {
        if (graphic == null) {
            throw new ArgumentNullException("graphic");
        }
    }

    #endregion
}

}
>>>>>>> 3c368a71062a6e4c49298b44dcdd13b67b1cef69
