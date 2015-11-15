CoLib
=

CoLib is a control flow library designed to simplify writing complex
timed logic in Unity. The library is modular and highly extensible. It's design
is similar to the Cocos2d, or LibGDX Action systems.

Basic Usage
-

Below is a simple tweening example:
    
    void Start()
    {
        CommandQueue queue = new CommandQueue();
        // In sequence, move the current gameObject to (10.0f, 0.0f, 0.0f) over 8 seconds, wait
        // 2 seconds, then translate by -5 units in the x axis over 4 seconds.
        queue.Enqueue(
            Commands.MoveTo(gameObject, new Vector2(10.0f, 0.0f, 0.0f), 8.0f),
            Commands.Wait(2.0f),
            Commands.MoveBy(gameObject, new Vector2(-5.0f, 0.0f, 0.0f), 4.0f, Ease.InOutHermite())
        );
        
        StartCoroutine(queue.WaitTillFinished());
    }

CommandQueue executes commands sequentially. Alternatively, to execute commands in parallel,
you can do the following:

    void Start()
    {
        CommandQueue commandQueue = new CommandQueue();
        commandQueue.Enqueue(
            Commands.Parallel(
                Commands.MoveTo(gameObject, new Vector2(10.0f, 0.0f, 0.0f), 8.0f),
                Commands.TintTo(gameObject, Color.blue, 4.0f)
            )
        );
        StartCoroutine(queue.WaitTillFinished());
    }

These examples all use coroutines to update the CommandQueue. However, it is also possible to update a CommandQueue manually

    private CommandQueue _queue = new CommandQueue();
    
    void Update()
    {
        _queue.Update(Time.deltaTime);
    }
    
    void MoveRight()
    {
        _queue.Enqueue(
            Commands.MoveBy(gameObject, new Vector2(5.0f, 0.0f, 0.0f), 2.0f, Ease.InOutHermite()),
        );
    }
    
    void GoBlue()
    {
        _queue.Enqueue(
            Commands.TintTo(gameObject, Color.blue, 10.0f * 60.0f, Ease.InOutHermite())
        );
    }
    
    void GoGray()
    {
        _queue.Enqueue(
            Commands.TintTo(gameObject, Color.gray, 2.0f)
        );
    }
    
    void Say(string text, float duration = 5.0f)
    {
        _queue.Enqueue(
            Commands.ActionDo( () => {
                guiText.enabled = true;
                guiText.text = text;
            }),
            Commands.Wait(duration),
            Commands.ActionDo( () => {
                guiText.enabled = false;
                guiText.text = "";
            })
        );
    }
    
    void Start()
    {
        MoveRight();
        Say("I can hold my breath for 10 whole minutes!");
        GoBlue();
        GoGray();
        Say("*gasp*");
    }
    
Coroutines
-

Coroutines can be useful for expressing certain kinds of logic which regular Commands can't. They look like this :

    CommandQueue _queue = new CommandQueue();

    IEnumerator<CommandDelegate> CoroutineMethod(int firstVal, int secondVal, int thirdVal)
    {
        Debug.Log(firstVal);
        yield return Commands.WaitForSeconds(1.0f); // You can return any CommandDelegate here.
        Debug.Log(secondVal);
        yield return null; // Wait a single frame.
        Debug.Log(thirdVal);
        
        yield Commands.Coroutine( () => ASecondCoroutine()); // Launch another coroutine
        
        // Execute whatever CommandDelegates we want in parallel, and wait for them to finish
        // before returning.
        yield Commands.Parallel( 
            Commands.Coroutine( () => AThirdCoroutine()),
            Commands.Coroutine( () => AForthCoroutine())
        );
        yield break; // Force exits the coroutine.
     }
        
    void Start() 
    { 
        _queue.Enqueue(
            Commands.Coroutine( () => CoroutineCommand(1,2,3)
        );
    }
     
    void Update()
    {
        _queue.Update(Time.deltaTime);
    }

More complex examples can be found under Assets/Scripts/Tests/Examples

License
-

Modified BSD

*&copy; 2015 Darcy Rayner*
