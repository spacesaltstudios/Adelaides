using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Moves the actor towards the center of the screen, while still staying on its current side (LEFT or RIGHT)
public class MoveActorInwardsNode : Node
{
    public string actor_name;
    public bool wait_until_actor_has_stopped = false;


    public override void Run_Node()
    {
        // Check if the Actor is on the scene
        if (!ActorManager.Is_Actor_On_Scene(actor_name))
        {
            Debug.LogError(actor_name + " actor is not on the scene. Please use an Enter Actor Node for " + actor_name);
            Finish_Node();
            return;
        }

        ActorManager.Move_Actor_Inwards(ActorManager.Get_Actor(actor_name));

        if (wait_until_actor_has_stopped)
            StartCoroutine(Wait_Until_Actor_Is_Stopped());
        else
            Finish_Node();
    }

    IEnumerator Wait_Until_Actor_Is_Stopped()
    {
        yield return 0;

        Actor actor = ActorManager.Get_Actor(actor_name);
        if (actor == null)
        {
            Debug.Log("Could not find actor");
            Finish_Node();
        }

        while (actor.is_moving)
            yield return 0;

        Finish_Node();
    }


    public override void Finish_Node()
    {
        base.Finish_Node();
    }
}
