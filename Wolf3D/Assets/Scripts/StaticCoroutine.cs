using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticCoroutine 
{
	private class CoroutineHolder : MonoBehaviour { }
	
	private static CoroutineHolder _runner;
	private static CoroutineHolder runner {
		get {
			if (_runner == null) {
				_runner = new GameObject("Static Corotuine Runner").AddComponent<CoroutineHolder>();
			}
			return _runner;
		}
	}
 
	public static Coroutine StartCoroutine(IEnumerator corotuine) {
		return runner.StartCoroutine(corotuine);
	}
	
	public static void StopCoroutine(Coroutine corotuine) {
		runner.StopCoroutine(corotuine);
	}
}
