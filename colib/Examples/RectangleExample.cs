using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CoLib;

namespace CoLib.Example
{

    public class RectangleExample : MonoBehaviour
    {

        CommandQueue _queue = new CommandQueue();
        Ref<Rect> _rectRef;
        Ref<Rect> _secondRectRef;

        void Start()
        {
            Rect rect = new Rect(0, 0, 350.0f, 300.0f);
            _rectRef = new Ref<Rect>(
                () => rect,
                t => rect = t
            );

            Rect secondRect = new Rect(0, 0, 350.0f, 300.0f);
            _secondRectRef = new Ref<Rect>(
                () => secondRect,
                t => secondRect = t
            );

            _queue.Sequence(
                Cmd.RepeatForever(
                    Cmd.Coroutine(() => AnimateRects())
                )
            );

        }

        IEnumerator<CommandDelegate> AnimateRects()
        {
            List<CommandDelegate> commands = new List<CommandDelegate>();
            commands.Add(
                Cmd.Sequence(
                    Cmd.ChangeTo(_rectRef, new Rect(50.0f, 100.0f, 300.0f, 200.0f), 4.0f, new Vector2(1.0f, 1.0f),
                        Ease.OutBack(0.4)),
                    Cmd.WaitForSeconds(1.0f),
                    Cmd.ChangeTo(_rectRef, new Rect(150.0f, 50.0f, 450.0f, 400.0f), 2.0f, new Vector2(1.0f, 1.0f),
                        Ease.InHermite()),
                    Cmd.ChangeTo(_rectRef, new Rect(0.0f, 0.0f, 350.0f, 300.0f), 1.0f, Ease.InCirc())
                )
            );

            commands.Add(
                Cmd.Sequence(
                    Cmd.ChangeTo(_secondRectRef, new Rect(350.0f, 100.0f, 300.0f, 200.0f), 4.0f,
                        new Vector2(0.0f, 0.0f), Ease.OutQuad()),
                    Cmd.ChangeTo(_secondRectRef, new Rect(Screen.width, Screen.height, 0.0f, 0.0f), 3.0f,
                        Ease.OutElastic())
                )
            );

            yield return Cmd.Parallel(commands.ToArray());
        }

        void Update()
        {
            _queue.Update(Time.deltaTime);
        }

        void OnGUI()
        {
            GUI.Box(_rectRef.Value, "One");
            GUI.Box(_secondRectRef.Value, "Two");
        }
    }
}
