using UnityEngine;
using System.Collections;
using CoLib;

namespace CoLib.Example
{

public class CommandQueueExample : MonoBehaviour
{
    // Use this for initialization
    private void Start ()
    {
        bool condition = false;

        gameObject.Queue(
            Cmd.ScaleTo(gameObject, 0.05f, 2.0f, Ease.OutQuart()),
            Cmd.ScaleTo(gameObject, 1.0f, 1.0f, Ease.OutBounce()),
            Cmd.RepeatForever(
                Cmd.Parallel(
                    Cmd.Repeat(2,
                        Cmd.ScaleBy(gameObject, 1.5f, 1.0f, Ease.OutBounce())
                    ),
                    Cmd.RotateBy(gameObject, Quaternion.Euler(180.0f,0.0f, 90.0f), 0.25f, Ease.InOutHermite())
                ),
                Cmd.WaitForSeconds(0.25f),
                Cmd.TintTo(gameObject, Color.red, 0.5f, Ease.InBack(0.2f)),
                Cmd.TintBy(gameObject, Color.blue, 0.5f),
                Cmd.Condition(delegate() { condition = !condition; return condition; },
                    Cmd.MoveBy(gameObject, new Vector3(0.0f, 2.0f, 0.0f), 0.25f, Ease.InOutHermite()),
                    Cmd.MoveBy(gameObject, new Vector3(0.0f, -2.0f, 0.0f), 0.25f, Ease.InOutHermite())
                ),
                Cmd.MoveTo(gameObject, new Vector3(0.0f, 0.0f, 0.0f), 0.25f, Ease.InOutHermite()),
                Cmd.Parallel(
                    Cmd.ScaleTo(gameObject, 0.5f, 1.0f, Ease.OutBounce()),
                    Cmd.RotateTo(gameObject, Quaternion.identity, 0.5f, Ease.InOutHermite())
                ),
                Cmd.TintTo(gameObject, Color.white, 0.25f, Ease.InOutSin()),
                Cmd.MoveFrom(gameObject,  new Vector3(0.0f, 0.0f, 0.8f), 0.5f, Ease.OutElastic()),
                Cmd.RotateFrom(gameObject, Quaternion.Euler(0.0f, 45.0f, 45.0f), 0.5f, Ease.InOutExpo()),
                Cmd.ScaleFrom(gameObject, 0.25f, 0.75f, Ease.InOutHermite()),
                Cmd.TintFrom(gameObject, Color.green, 0.25f, Ease.InOutQuint()),
                Cmd.ScaleTo(gameObject, 1.0f, 0.2f, Ease.Smooth()),
                Cmd.Wiggle(gameObject.ToRotationRef(true), 30f, 2.0),
                Cmd.Wobble(transform.ToPositionYRef(true), 3f, 2.0),
                Cmd.Parallel(
                    Cmd.SquashAndStretch(gameObject.ToScaleRef(), 3f, 2.0),
                    Cmd.Shake(transform.ToPositionRef(true), 0.3f, 2.0),
                    Cmd.Shake(transform.ToRotationRef(true), 5f, 2.0)
                )
            )
        );
    }
}

}
