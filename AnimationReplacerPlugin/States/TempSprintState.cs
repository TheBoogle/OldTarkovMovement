using EFT;
using System;
using UnityEngine;

// Token: 0x02000EE9 RID: 3817
public class TempSprintState : SprintState
{
    public TempSprintState(MovementContext movementContext) : base(movementContext)
    { }

    // Token: 0x06005C15 RID: 23573 RVA: 0x00286E60 File Offset: 0x00285060
    public override void Enter(bool isFromSameState)
    {
        base.Enter(isFromSameState);
        this.MovementContext.SetPatrol(true);
    }

    // Token: 0x06005C16 RID: 23574 RVA: 0x00286EBC File Offset: 0x002850BC
    public override void Exit(bool toSameState)
    {
        base.Exit(toSameState);
        this.MovementContext.SetPatrol(false);
    }
}
