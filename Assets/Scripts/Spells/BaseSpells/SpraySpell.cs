using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

public class SpraySpell : Spell
{
    public SpraySpell(SpellCaster owner) : base(owner) { }

    /// <summary>
    /// How many projectiles to fire in the spray.
    /// </summary>
    private string N;
    /// <summary>
    /// How wide the spray cone is.
    /// </summary>
    /// <value>0-1: 0-360 degrees centered at angle of mouse click.</value>
    private string spray;

    public override void SetAttributes(JObject attributes)
    {
        base.SetAttributes(attributes);
        if (attributes["N"] != null) N = attributes["N"].ToString();
        if (attributes["spray"] != null) spray = attributes["spray"].ToString();
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Vector3 directionVector = target - where;
        double angleToCast = Math.Atan(directionVector.y / directionVector.x);//radians
        if (directionVector.x < 0) angleToCast += Math.PI;
        if (directionVector.y < 0 && directionVector.x > 0) angleToCast += 2d * Math.PI;
        double sprayAngle = RPNEvaluator.RPNEvaluator.Evaluatef(spray,GetRPNDict()) * 2d*Math.PI;//radians
        double angleToTopOfSpray = angleToCast + sprayAngle/2;//radians
        int N = RPNEvaluator.RPNEvaluator.Evaluate(this.N,GetRPNDict());
        double projectileAngle;//radians
        Vector3 projectileTarget;
        for (int i=0; i<N; i++)
        {
            projectileAngle = angleToTopOfSpray - (i * sprayAngle / (N-1));//radians; N-1 to get full range of spray
            projectileTarget = new Vector3((float)(where.x + Math.Cos(projectileAngle)), (float)(where.y + Math.Sin(projectileAngle)), where.z);
            yield return base.Cast(where, projectileTarget, team);
        }
    }
}