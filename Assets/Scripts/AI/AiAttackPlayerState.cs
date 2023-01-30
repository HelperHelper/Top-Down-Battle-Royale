using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AiAttackPlayerState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.AttackPlayer;
    }

    public void Enter(AiAgent agent)
    {


        if (PlayerHealth.Instance.playerdeath == true)
        {
            agent.weapons[agent.currentWeapon].triggerDown = false;
        }
        else
        {
           


        }

    }

    public void Update(AiAgent agent)
    {
        //Debug.Log("Entra al debug de seguir y atacar al jugador");
        if (PlayerHealth.Instance.playerdeath == true)
        {
            agent.navMeshAgent.stoppingDistance = 5f;
        }
        else
        {
           if(GetDistanceFromPlayer(agent) >= agent.chasingDistance && GetDistanceFromPlayer(agent) > agent.attackingDistance)
            {
                agent.navMeshAgent.isStopped = false;
                agent.transform.LookAt(agent.character.transform);
                agent.navMeshAgent.SetDestination(agent.character.transform.position);


            } 
            else if(GetDistanceFromPlayer(agent) < agent.attackingDistance)
            {
                agent.navMeshAgent.isStopped = true;
              
                StartAttacking(agent);
            }
           
           if(GetDistanceFromPlayer(agent) > agent.attackingDistance)
            {
                agent.weapons[agent.currentWeapon].triggerDown = false;

            }

        }
        
      
    }

    private void StartAttacking(AiAgent agent)
    {
        agent.weapons[agent.currentWeapon].triggerDown = true;

    }

    float GetDistanceFromPlayer(AiAgent agent)
    {
        return Vector3.Distance(agent.transform.position, agent.character.transform.position);
    }


    public void Exit(AiAgent agent)
    {
       agent.navMeshAgent.stoppingDistance = 5.0f;
    }


}
