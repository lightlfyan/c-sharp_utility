using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ActorEvent
{
    OnStuck,
    OnKilled,
    OnSound
    //...
}



public interface IEventHandler
{
    void Hanlde (ActorEvent e);
}

public class L4dAction
{
    public virtual void OnStart ()
    {
    }
    public virtual void Update ()
    {
    }
    public virtual void OnEnd ()
    {
    }
    public virtual void OnSuspend ()
    {
    }
    public virtual void OnResume ()
    {
    }
    public virtual bool OnBreak ()
    {
        return false;
    }
    
    
    // transitionst
    public virtual void Continue ()
    {
    }
    public virtual void ChangeTo (L4dAction next, string reason)
    {
    }
    public virtual void SuspendFor (L4dAction next, string reason)
    {
    }
    public virtual void Done (string reason)
    {
    }
    
    // send event to ai tree
    
}

public class model
{
    public int hp;
    public int mp;
    public int[] skill;
}


public class Actor: IEventHandler
{
    private L4dAction currentAction;
    private Queue<ActorEvent> eventQueue;
    
    public void Update ()
    {
        var e = eventQueue.Dequeue ();
        Debug.Log ("handle event: " + e.ToString ());
    
        currentAction.Update ();
    }
    
    public void Hanlde (ActorEvent e)
    {
        eventQueue.Enqueue (e);
    }
    
    private void passiveSkillCheck ()
    {
    
    }
}




// queary system
// e.g should i pick up this object
//     should i attack enemy

public class Queary
{
    public virtual bool ShouldPickUP (Actor a)
    {
        return false;
    }
    
    public virtual bool ShouldHurry ()
    {
        return false;
    }
}