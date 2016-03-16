using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 task 任务类

 多重执行模式(对任务列表的操作)
 1 选择执行 (一系列任务中, 执行任意一个)
 2 顺序执行(安顺序执行task,一个失败,则不继续进行)
 3 一直执行,直到成功
 4 随机执行
 5 反转任务顺序
*/


public interface ITask
{
    BTaskStatus Update ();
}

public enum BTaskStatus
{
    Success,
    Failure,
    Running
}

public class Selector: ITask
{
    private List<ITask> ts;
	
    public Selector (List<ITask> _ts)
    {
        this.ts = _ts;
    }

    public BTaskStatus Update ()
    {
        if (ts.Any (o => o.Update () == BTaskStatus.Success)) {
            return BTaskStatus.Success;
        }
        
        return BTaskStatus.Failure;
    }
}

public class Sequence: ITask
{
    private List<ITask> ts;
	
    public Sequence (List<ITask> _ts)
    {
        this.ts = _ts;
    }
    
    
    public BTaskStatus Update ()
    {
        if (ts.All (o => o.Update () == BTaskStatus.Success)) {
            return BTaskStatus.Success;
        }
        return BTaskStatus.Failure;
    }
}


public class RandomSelector: ITask
{
    private List<ITask> ts;
	
    public RandomSelector (List<ITask> _ts)
    {
        this.ts = _ts;
    }

    public BTaskStatus Update ()
    {
        var i = Random.Range (0, ts.Count ());
        return ts [i].Update ();
    }
}

public class UntilFail: ITask
{
	
    protected ITask wrapper;
	
    public UntilFail (ITask t)
    {
        this.wrapper = t;
    }
    
    public BTaskStatus Update ()
    {
        if (this.wrapper.Update () != BTaskStatus.Failure) {
            return BTaskStatus.Running;
        }
        
        return BTaskStatus.Failure;
    }
}


public class InRange: ITask
{
    public int i = 0;
    
    public BTaskStatus Update ()
    {
        if (i < 3) {
            i += 1;
            return BTaskStatus.Success;
        }
        
        return BTaskStatus.Failure;
    }
}

public class CanUseSkill: ITask
{
    public BTaskStatus Update ()
    {
        return BTaskStatus.Success;
    }
}

public class Attack: ITask
{
    public BTaskStatus Update ()
    {
        Debug.Log ("do attack");
        return BTaskStatus.Success;
    }
}

public class BehaviourTest
{
    // shared dict for pass message
    
    public static ITask task;
    
    public static void test ()
    {
        var inrange = new InRange ();
        var canuseskill = new CanUseSkill ();
        var attack = new Attack ();
        
        var listtask = new List<ITask> (){
            inrange, 
            canuseskill, 
            attack
        };
        
        var seqtask = new Sequence (listtask);
        var untilfailltask = new UntilFail (seqtask);
        
        task = untilfailltask;
    
//        task = new UntilFail (
//            new Sequence (new List<ITask> (){
//            new InRange(),
//            new CanUseSkill(),
//            new Attack()
//            }));
    }
}