using System;
public class StateMatchine<T> where T : class
{
    //
    // Fields
    //
    protected long mEnterStateTime;

    protected T mOwner;

    protected State<T> mLastState;

    protected Map<string, State<T>> mStateCache;

    protected State<T> mCurrentState;

    //
    // Constructors
    //
    public StateMatchine (T owner)
    {
        this.mStateCache = new Map<string, State<T>> ();
        this.mOwner = owner;
    }

    //
    // Methods
    //
    public virtual void changeState (int id, object obj = null)
    {
        State<T> state = this.mStateCache.get (id.ToString ());
        if(state == null){
            return;
        }
        this.mLastState = this.mCurrentState;
        this.mCurrentState = state;
        this.mCurrentState.setOwner(this.mOwner);
        this.mLastState.onLeave(state.getStateId);
        this.mCurrentState.onEnter (obj);
        this.mEnterStateTime = TimeUtils.currentMilliseconds (false);
    }

    public void clear ()
    {
        if (this.mCurrentState != null)
        {
            this.mCurrentState.onLeave(0);
            this.mCurrentState = null;
      }
      this.mLastState = null;
      this.mStateCache.Container.Clear ();

    }

    public virtual void fixedUpdate ()
    {
        if (this.mCurrentState != null)
        {
            this.mCurrentState.onFixedUpdate();
        }
    }

    public int getCurrentState ()
    {
        if (this.mCurrentState != null)
        {
            return this.mCurrentState.getStateId ();
        }
        return -1;
    }

    public State<T> getCurrentStateInstance ()
    {
        return this.mCurrentState;
    }

    public bool isExist (ActorState state)
    {
        int num = (int)state;
        return this.mStateCache.ContainsKey (num.ToString ());
    }

    public virtual void lateUpdate ()
    {
        if (this.mCurrentState != null)
        {
          
            this.mCurrentState.onLateUpdate ();
        }
    }

    public void reEnterState (object obj = null)
    {
        if (this.mCurrentState != null)
        {
          
                        this.mCurrentState.onEnter (obj);
        }
    }

    public void registerState (int id, State<T> state)
    {
        if (this.mStateCache.get (id.ToString ()) == null)
        {
             this.mStateCache.add (id.ToString (), state);
        } else {
             this.mStateCache.Container [id.ToString ()] = state;
        }
    }

    public void removeState (int id)
    {
        this.mStateCache.remove (id.ToString ());
    }

    public void rollback (object parameter)
    {
        if (this.mLastState != null)
        {
          
                        this.changeState (this.mLastState.getStateId (), null);
        }
    }

    public void setOwner (T owner)
    {
        this.mOwner = owner;
    }

    public virtual void update ()
    {
        if (this.mCurrentState != null)
        {
            this.mCurrentState.onUpdate ();
        }
    }
}