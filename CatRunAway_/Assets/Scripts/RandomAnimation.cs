using UnityEngine;

//Script utilizado para alterar as animaçoes Idle do personagem, aleatoriamente
public class RandomAnimation : StateMachineBehaviour
{
	public string parameter;
	public int count;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetInteger(parameter, Random.Range(0, count));
	}
}
