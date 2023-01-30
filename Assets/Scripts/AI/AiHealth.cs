using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AiHealth : Health
{
    AiAgent agent;
    public GameObject lifebar;
   
 
    protected override void OnStart()
    {
        agent = GetComponent<AiAgent>();


    }

    protected override void OnDeath(Vector3 direction)
    {

        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateId.Death);
        VictoryGame.Instance.VictoryPlayer();
        Controller.Instance.enabled = false;
    }



    protected override void OnDamage(Vector3 direction)
    {
        lifebar.GetComponent<Slider>().value = currentHealth;

    }
}
