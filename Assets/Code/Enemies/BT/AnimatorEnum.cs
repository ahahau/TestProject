using Unity.Behavior;

namespace Code.Enemies.BT
{
    [BlackboardEnum]
    public enum AnimatorEnum
    {
        IDLE = 0, 
        ATTACK = 1, 
        MOVE = 2,
        DEAD = 3, 
        HIT = 4
    }
}