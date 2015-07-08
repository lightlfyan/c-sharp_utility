public abstract class State<T> where T : class
{
    protected T mOwner;

    public T getOwner ()
    {
        return this.mOwner;
    }

    public abstract int getStateId ();

    public abstract void onEnter (object obj = null);

    public abstract void onFixedUpdate ();

    public abstract void onLateUpdate ();

    public abstract void onLeave (int stateID);

    public abstract void onUpdate ();

    public void setOwner (T owner)
    {
        this.mOwner = (T)((object)owner);
    }
}
