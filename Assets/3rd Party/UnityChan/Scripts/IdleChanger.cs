using UnityEngine;
using System.Collections;

//
// ↑↓キーでループアニメーションを切り替えるスクリプト（ランダム切り替え付き）Ver.3
// 2014/04/03 N.Kobayashi
//

// Require these components when using this script
[RequireComponent(typeof(Animator))]



public class IdleChanger : MonoBehaviour
{
	
	private Animator anim;						// Animatorへの参照
	private AnimatorStateInfo currentState;		// 現在のステート状態を保存する参照
	private AnimatorStateInfo previousState;	// ひとつ前のステート状態を保存する参照
	public bool _random = false;				// ランダム判定スタートスイッチ
	public float _minInterval = 0f;                // ランダム判定のインターバル
	public float _interval = 2f;				// ランダム判定のインターバル
	//private float _seed = 0.0f;					// ランダム判定用シード
	


	// Use this for initialization
	void Start ()
	{
		// 各参照の初期化
		anim = GetComponent<Animator> ();
		// ランダム判定用関数をスタートする
		StartCoroutine ("RandomChange");
	}

	// ランダム判定用関数
	IEnumerator RandomChange ()
	{
		// 無限ループ開始
		while (true) {
			yield return new WaitForSeconds(Random.Range(_minInterval, _interval));

			anim.SetTrigger("Wait");

			yield return new WaitForSeconds(5);
		}

	}

}
