using UnityEngine;

public class HomingTrajectory : Trajectory
{
    readonly float turn_rate;
    public HomingTrajectory(float speed) : base(speed)
    {
        turn_rate = 0.25f;//max degrees turn per Update() frame
    }

    public override void Movement(Transform transform)
    {
        
        GameObject closest = GameManager.Instance.GetClosestEnemy(transform.position);
        if (closest == null)
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        }
        else //we have the angle (in the xz plane) of where the projectile is currently heading, and we have the closest enemy
        {
            Quaternion targetRotation = Quaternion.LookRotation(closest.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turn_rate);
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        }
    }
}
