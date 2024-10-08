﻿#nullable enable
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
        CollectItem,
        UseItem,
        Count
    }

    public class ActionResult
    {
        public readonly ActionResultType ResultType;
        public readonly Entity? Entity = null;

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
