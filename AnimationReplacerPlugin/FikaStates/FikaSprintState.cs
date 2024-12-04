using System;
using EFT;
using Fika.Core.Coop.ObservedClasses;
using UnityEngine;

public class FikaSprintState : ObservedSprintState
{
    // Token: 0x06000B69 RID: 2921 RVA: 0x0003F253 File Offset: 0x0003D453
    public FikaSprintState(MovementContext movementContext) : base(movementContext)
    {
        this.MovementContext = movementContext;
    }

    public override void Enter(bool isFromSameState)
    {
        base.Enter(isFromSameState);
        this.MovementContext.SetPatrol(true);
    }

    public override void Exit(bool toSameState)
    {
        base.Exit(toSameState);
        this.MovementContext.SetPatrol(false);
    }
}
