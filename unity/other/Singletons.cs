using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Singleton<T> where T: new() {
	private static T only;
	static Singleton(){
		only = new T();
	}

	public static T Instance {
		get { return only;}
	}
}

public class SingletonComponentData: Singleton<SingletonComponentData> {
	Dictionary<string, MonoBehaviour> dict = new Dictionary<string, MonoBehaviour>();

	public bool set(string name, MonoBehaviour obj){
		if(dict.ContainsKey(name) == false){
			dict.Add(name, obj);
			return true;
		}
		return false;
	}

	public void release(string name){
		dict.Remove(name);
	}

	public void rootset(string name, MonoBehaviour obj){
		if(dict.ContainsKey(name)){
			GameObject.Destroy(dict[name]);
			dict.Remove(name);
		}
		dict.Add(name, obj);
	}
}

public class SingletonComponent: MonoBehaviour{
	static SingletonComponent(){

	}

	void Awake(){
		if(gameObject.name == "Root"){
			SingletonComponentData.Instance.rootset("SingletonComponent", this);
			return;
		}
		if(SingletonComponentData.Instance.set("SingletonComponent", this)){
			return;
		}
		Destroy(this);
	}

}

public class Root {
	private static GameObject only;
	static Root(){
		only = new GameObject("Root");
		GameObject.DontDestroyOnLoad(only);
		only.AddComponent("ui");
		only.AddComponent("SceneRoot");
	}

	public static GameObject Instance {
		get { return only; }
	}
}
