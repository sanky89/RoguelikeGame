using System;

namespace RoguelikeGame
{
    public static class ActionHelper
    {
        public static bool IsMovementAction(InputAction inputAction)
        {
            return inputAction is InputAction.MOVE_LEFT 
                            or InputAction.MOVE_RIGHT
                            or InputAction.MOVE_UP
                            or InputAction.MOVE_DOWN
                            or InputAction.MOVE_NW
                            or InputAction.MOVE_NE
                            or InputAction.MOVE_SW
                            or InputAction.MOVE_SE;
        }

        public static bool IsUseItemAction(InputAction inputAction)
        {
            return inputAction is InputAction.USE_ITEM_1
                            or InputAction.USE_ITEM_2
                            or InputAction.USE_ITEM_3
                            or InputAction.USE_ITEM_4
                            or InputAction.USE_ITEM_5;
        }
    }
}