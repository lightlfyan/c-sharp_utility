using UnityEngine;
using System.Collections;


public abstract class Decision
{
    public virtual void MakeDecision ()
    {
        GetBranch ().MakeDecision ();
    }
	
    public virtual Decision GetBranch ()
    {
        throw new UnityException ("");
    }
}

public abstract class Action: Decision
{
    public override void MakeDecision ()
    {
        Debug.Log ("do action");
    }
}


// e.g

public class Skill1: Action
{
    public override void MakeDecision ()
    {
        Debug.Log ("Skill1");
    }
}

public class Skill2: Action
{
    public override void MakeDecision ()
    {
        Debug.Log ("Skill2");
    }
}


public class Skill3: Action
{
    public override void MakeDecision ()
    {
        Debug.Log ("Skill3");
    }
}

public class SkillAI: Decision
{
    private int i;
    

    public SkillAI ()
    {
        i = Random.Range (1, 4);
    }

    public override Decision GetBranch ()
    {
        //logic
        i += 1;
        if (i > 3) {
            i = 1;
        }

        switch (i) {
        case 1:
            return new Skill1 ();
        case 2:
            return new Skill2 ();
        default:
            return new Skill3 ();
        }
    }
}


public class UseDecision
{
    public static void test ()
    {
        var a = new SkillAI ();
        a.MakeDecision ();
    }
}
