#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeGame
{
    public enum ActionResultType
    {
        None,
        Rest,
        Move,
        HitWall,
        HitEntity,
    }

    public class ActionResult
    {
        public readonly ActionResultType ResultType;
        public readonly Entity? Entity;

        public ActionResult(ActionResultType resultType, Entity entity)
        {
            ResultType = resultType;
            if(entity != null)
            {
                Entity = entity;
            }
        }
    }
}
