<<<<<<< HEAD
﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{

public static class CanvasGroupExtensions
{
    public static Ref<float> ToAlphaRef(this CanvasGroup group)
    {
        if (group == null) {
            throw new ArgumentNullException("group");
        }

        return new Ref<float>(
            () => group.alpha,
            (t) => group.alpha = t
        );
    }
}

}
=======
﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoLib
{

public static class CanvasGroupExtensions
{
    public static Ref<float> ToAlphaRef(this CanvasGroup group)
    {
        if (group == null) {
            throw new ArgumentNullException("group");
        }

        return new Ref<float>(
            () => group.alpha,
            (t) => group.alpha = t
        );
    }
}

}
>>>>>>> 3c368a71062a6e4c49298b44dcdd13b67b1cef69
