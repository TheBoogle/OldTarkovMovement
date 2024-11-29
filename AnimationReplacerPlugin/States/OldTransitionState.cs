using System;
using EFT;
using UnityEngine;

// Token: 0x02000EEA RID: 3818
public class OldTransitionState: MovementState
{
    public OldTransitionState(MovementContext movementContext) : base(movementContext)
    { }
    // Token: 0x06005C1D RID: 23581 RVA: 0x000DA242 File Offset: 0x000D8442
    public override void Enter(bool isFromSameState)
    {
        base.Enter(isFromSameState);
        this.MovementContext.PlayerAnimatorEnableSprint(false);
        this.MovementContext.SetTilt(0f, false);
    }

    // Token: 0x06005C1E RID: 23582 RVA: 0x0028703C File Offset: 0x0028523C
    public override void ManualAnimatorMoveUpdate(float deltaTime)
    {
        base.UpdateRotationSpeed(deltaTime);
        Vector3 playerAnimatorDeltaPosition = this.MovementContext.PlayerAnimatorDeltaPosition;
        this.ApplyGravity(ref playerAnimatorDeltaPosition, deltaTime);
        this.LimitMotion(ref playerAnimatorDeltaPosition, deltaTime);
        this.MovementContext.ApplyMotion(playerAnimatorDeltaPosition, deltaTime);
    }

    // Token: 0x06005C1F RID: 23583 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void SetTilt(float tilt)
    {
    }
}
