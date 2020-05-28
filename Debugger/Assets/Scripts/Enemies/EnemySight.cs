using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    private Enemy parent;
    private float updateTargetTimer = 2f;
    // Start is called before the first frame update
    void Start()
    {
        parent = this.GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        // UpdateTarget if it's time to refresh, if you're only targeting yourself,
        //   or if you're a friend targeting the player
        if (updateTargetTimer <= 0 || parent.targets.Count == 1 || parent.IsFriendTargetingPlayer())
        {
            UpdateTarget();
            updateTargetTimer = 2f;
        }
        else
        {
            updateTargetTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        LivingEntity le = collision.gameObject.GetComponent<LivingEntity>();
        if (le != null && le != this.parent && le.gameObject != this.parent.gameObject)
        {
            if (IsFriend(le))
            {
                parent.friendlyTargets.Add(le);
            }
            else
            {
                parent.targets.Add(le);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LivingEntity le = collision.gameObject.GetComponent<LivingEntity>();
        if (le != null)
        {
            parent.targets.Remove(le);
        }
    }

    private bool IsFriend(LivingEntity le)
    {
        if (le == null) return false;
        return parent.alignment == le.alignment &&
               parent.alignment != Alignment.NEUTRAL &&
               le.alignment != Alignment.NEUTRAL;
    }

    private bool IsEnemy(LivingEntity le)
    {
        return !IsFriend(le);
    }

    /**
     * Updates the parent Enemy's target as such:
     *   - If the parent's target list has entries,
     *     the parent should target one of those since it
     *     is an enemy
     *   - If The parent's target list is empty, if the
     *     parent is FRIEND (i.e. Friend to the player) then
     *     it will target the player IF it is within the player's
     *     range, otherwise it will target the next closest friend
     *   - If the parent's target list is empty and its
     *     friendly target list is empty, the parent will target itself
     *     thus not move
     */
    private void UpdateTarget()
    {
        LivingEntity closestTarget = null;

        if (parent.targets.Count > 0)
        {
            closestTarget = GetClosestTarget(parent.targets);
        }
        else if (parent.targets.Count == 0 && parent.friendlyTargets.Count > 0 && IsFriend(parent.player))
        {
            closestTarget = parent.friendlyTargets.Contains(parent.player) ? parent.player : GetClosestTarget(parent.friendlyTargets);
        }
        else if (IsEnemy(parent.player))
        {
            closestTarget = null;
        }
        parent.target = closestTarget;
    }

    private LivingEntity GetClosestTarget(ISet<LivingEntity> targetList)
    {
        LivingEntity closestTarget = null;
        float minDist = float.MaxValue;
        foreach (LivingEntity le in targetList)
        {
            float dist = Vector3.Distance(this.parent.transform.position, le.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestTarget = le;
            }
        }
        return closestTarget;
    }
}
