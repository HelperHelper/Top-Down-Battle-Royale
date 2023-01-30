using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChasePlayerState : AiState
{
    float timer = 0.0f;

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent)
    {
       
    }
    public void Update(AiAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.character.transform.position;
            agent.navMeshAgent.stoppingDistance = 5;
        }

        if (timer < 0.0f)
        {
          
              

                float sqDistance = (agent.character.transform.position - agent.navMeshAgent.destination).sqrMagnitude;
                if (sqDistance > agent.config.minDistance * agent.config.minDistance) // lo que estaba dentro del if antes    direction.sqrMagnitude > agent.config.minDistance * agent.config.minDistance
                {
                  
                    agent.navMeshAgent.destination = agent.character.transform.position;
                    agent.navMeshAgent.stoppingDistance = 5;



                }


            timer = agent.config.maxTime;
        }

     }

    public void Exit(AiAgent agent)
    {
        
    }

  

}
