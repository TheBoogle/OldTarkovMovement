using System;
using EFT;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000ECF RID: 3791
public class OldIdleState : MovementState
{
    public OldIdleState(MovementContext movementContext) : base(movementContext)
    {
        if (!this.MovementContext.IsAI)
        {
            this.gclass744_0 = new GClass744();
            this.gclass744_0.Init(movementContext);
        }
    }

    // Token: 0x060059F4 RID: 23028 RVA: 0x000D87AF File Offset: 0x000D69AF
    private static bool smethod_0(Vector2 direction)
    {
        return direction.x > 1E-05f || direction.y > 1E-05f || direction.x < -1E-05f || direction.y < -1E-05f;
    }

    // Token: 0x060059F5 RID: 23029 RVA: 0x000D87E7 File Offset: 0x000D69E7
    public override void Exit(bool toSameState)
    {
        base.Exit(toSameState);
        this.MovementContext.HoldBreath(false);
        this.bool_1 = false;
        this.bool_0 = false;
    }

    // Token: 0x060059F6 RID: 23030 RVA: 0x000D880A File Offset: 0x000D6A0A
    public override void Enter(bool isFromSameState)
    {
        base.Enter(isFromSameState);
        this.MovementContext.EnableSprint(false);
        this.MovementContext.LeftStanceController.SetAnimatorLeftStanceToCacheFromBodyAction(false);
        GClass744 gclass = this.gclass744_0;
        if (gclass == null)
        {
            return;
        }
        gclass.Enter();
    }

    // Token: 0x060059F7 RID: 23031 RVA: 0x000D881F File Offset: 0x000D6A1F
    public override void BlindFire(int b)
    {
        this.MovementContext.SetBlindFire(b);
    }
    public override void BlendMotion(ref Vector3 motion, float deltaTime)
    {
        motion = Vector3.Lerp(motion, this.MovementContext.LastBlendMotionDelta * deltaTime, EFTHardSettings.Instance.IdleStateMotionPreservation);
    }
    public override void Vaulting()
    {
        this.MovementContext.TryVaulting();
    }

    // Token: 0x060059F8 RID: 23032 RVA: 0x000D882D File Offset: 0x000D6A2D
    public override void Pickup(bool enabled, [CanBeNull] Action action)
    {
        this.MovementContext.OverrideState(this.MovementContext.PickUpState);
        this.MovementContext.PickupAction = action;
    }

    public override void Plant(bool enabled, bool multitool, float plantTime, Action<bool> action)
    {
        GClass1867 gclass;
        if ((gclass = (this.MovementContext.PlantState as GClass1867)) != null)
        {
            gclass.PlantMultitool = multitool;
            gclass.PlantTime = plantTime;
        }
        this.MovementContext.OverrideState(this.MovementContext.PlantState);
        this.MovementContext.PlantAction = action;
    }
    
    // Token: 0x060059FA RID: 23034 RVA: 0x000D882D File Offset: 0x000D6A2D
    public override void Examine(bool enabled, [CanBeNull] Action action)
    {
        this.MovementContext.OverrideState(this.MovementContext.PickUpState);
        this.MovementContext.PickupAction = action;
    }

    // Token: 0x060059FB RID: 23035 RVA: 0x00280DD4 File Offset: 0x0027EFD4
    public override void Move(Vector2 direction)
    {
        if (OldIdleState.smethod_0(direction) && this.MovementContext.CanWalk)
        {
            direction.x = (float)Math.Sign(direction.x);
            direction.y = (float)Math.Sign(direction.y);
            this.MovementContext.MovementDirection = direction;
            this.MovementContext.EnableSprint(this.bool_0 && direction.y > 0.1f);
            if (this.MovementContext.IsSprintEnabled)
            {
                this.MovementContext.SetPoseLevel(1f, false);
                if (this.MovementContext.PoseLevel > 0.9f && this.MovementContext.SmoothedCharacterMovementSpeed >= 1f)
                {
                    this.MovementContext.PlayerAnimatorEnableSprint(true);
                }
            }
            this.MovementContext.PlayerAnimatorEnableInert(true);
        }
        else
        {
            this.MovementContext.MovementDirection = Vector2.zero;
        }
    }

    // Token: 0x060059FC RID: 23036 RVA: 0x00280EAC File Offset: 0x0027F0AC
    public override void Jump()
    {
        if (this.MovementContext.PoseLevel > 0.6f && this.MovementContext.IsGrounded)
        {
            this.MovementContext.TryJump();
            return;
        }
        this.ChangePose(1f - this.MovementContext.PoseLevel);
    }

    // Token: 0x060059FD RID: 23037 RVA: 0x000D8851 File Offset: 0x000D6A51
    public override void EnableSprint(bool enable, bool isToggle = false)
    {
        if (!isToggle)
        {
            this.bool_0 = (enable && this.MovementContext.CanSprint);
        }
    }

    // Token: 0x060059FE RID: 23038 RVA: 0x000D886D File Offset: 0x000D6A6D
    public override void EnableBreath(bool enable)
    {
        this.MovementContext.HoldBreath(enable);
    }

    // Token: 0x060059FF RID: 23039 RVA: 0x000D887B File Offset: 0x000D6A7B
    public override void Kick()
    {
        this.MovementContext.PlayerAnimatorEnableKick(true);
    }

    // Token: 0x06005A00 RID: 23040 RVA: 0x00280EFC File Offset: 0x0027F0FC
    public override void SetStep(int step)
    {
        if (Mathf.Abs(step) > 0)
        {
            Vector3 a = new Vector3(this.MovementContext.TransformForwardVector.z, 0f, -this.MovementContext.TransformForwardVector.x);
            if (this.MovementContext.OverlapOrHasNoGround(0.3f, new Vector3?(a * Mathf.Sign((float)step)), 0.2f, 3f, 0f))
            {
                this.MovementContext.Step = 0;
                return;
            }
        }
        this.MovementContext.Step = step;
    }

    public void method_0(float deltaTime)
    {
        GClass744 gclass = this.gclass744_0;
        if (gclass == null)
        {
            return;
        }
        gclass.ProcessAnimatorStep(deltaTime, this.Type);
    }

    // Token: 0x04005683 RID: 22147
    private bool bool_0;

    // Token: 0x04005684 RID: 22148
    private bool bool_1;

    // Token: 0x04005685 RID: 22149
    private const float float_0 = 1E-05f;

    private GClass744 gclass744_0;
}
