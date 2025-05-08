using Unity.Behavior;

namespace Code.Enemies.BT
{
    [BlackboardEnum]
    public enum EnemyState
    {
        PATROL = 0,
        CHASE = 1,
        ATTACK = 2,
        DEAD = 3,
    }
}