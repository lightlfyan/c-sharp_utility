using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IState
{
	void Action ();
	void OnEntry ();
	void OnLeave ();
	List<ITransition> GetTransitions ();
}

public interface ITransition
{
	bool IsTriggered ();
	IState GetTargetState ();
	void Action ();
}
	
public class BattleStateMachine
{

	private IState currentSt;

	public BattleStateMachine (IState state)
	{
		this.currentSt = state;
	}
	
	public void ChangeST (IState State)
	{
		this.currentSt.OnLeave ();
		State.OnEntry ();
		this.currentSt = State;
	}

	public void Update ()
	{
		ITransition ts = null;

		foreach (var s in currentSt.GetTransitions()) {
			if (s.IsTriggered ()) {
				ts = s;
				break;
			}
		}

		if (ts != null) {
			var newst = ts.GetTargetState ();
			this.currentSt.OnLeave ();
			ts.Action ();
			newst.OnEntry ();
			this.currentSt = newst;
		} else {
			this.currentSt.Action ();
		}
	}
}






