using System;
using EFT;
using EFT.Interactive;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public class OldApproachState : MovementState
{
    public OldApproachState(MovementContext context) : base(context) {}

    // Token: 0x060020DE RID: 8414 RVA: 0x0019D3B4 File Offset: 0x0019B5B4
    public override void Enter(bool isFromSameState)
    {
        this.MovementContext.StateLocksInventory = true;
        this.MovementContext.SetTilt(0f, false);
        this.MovementContext.SetBlindFire(0);
        base.Enter(isFromSameState);
        this.bool_3 = false;
        this.MovementContext.IsInPronePose = false;
        this.float_0 = Time.time;
        WorldInteractiveObject.GStruct416 interactionParameters = this.MovementContext.InteractionParameters;
        this.vector3_0 = interactionParameters.InteractionPosition;
        this.vector3_1 = interactionParameters.ViewTarget;
        this.bool_0 = interactionParameters.Snap;
        this.vector2_0 = this.MovementContext.Rotation;
        this.vector3_2 = this.MovementContext.TransformPosition;
        this.float_3 = this.MovementContext.PoseLevel;
        this.float_2 = this.MovementContext.TransformRotation.eulerAngles.y;
        if (interactionParameters.RotationMode == WorldInteractiveObject.ERotationInterpolationMode.ViewTarget)
        {
            this.bool_1 = (this.vector3_1.y > this.MovementContext.RibcagePosition().y);
            if (!this.bool_1)
            {
                float num = (interactionParameters.Grip == null) ? (this.MovementContext.RibcagePosition().y - this.vector3_1.y) : (this.MovementContext.RibcagePosition().y - interactionParameters.Grip.transform.position.y);
                this.bool_2 = (num > 0.65f);
            }
        }
        else
        {
            this.bool_2 = interactionParameters.Sit;
        }
        if (this.bool_0)
        {
            float num2 = Vector3.Distance(this.MovementContext.TransformPosition, this.vector3_0);
            this.float_1 = num2 / this.ApproachSpeed;
            switch (interactionParameters.RotationMode)
            {
                case WorldInteractiveObject.ERotationInterpolationMode.ViewTarget:
                    this.vector2_1 = this.MovementContext.CalculateLookAtDirection(this.vector3_1, this.vector3_0, this.bool_1 ? 1.5f : (this.bool_2 ? 1.2f : 0f));
                    break;
                case WorldInteractiveObject.ERotationInterpolationMode.ViewTargetWithZeroPitch:
                    this.vector2_1 = this.MovementContext.CalculateLookAtDirection(this.vector3_1, this.vector3_0, this.bool_1 ? 1.5f : (this.bool_2 ? 1.2f : 0f));
                    this.vector2_1.y = 0f;
                    break;
                case WorldInteractiveObject.ERotationInterpolationMode.ViewTargetAsOrientation:
                    this.vector2_1 = new Vector2(interactionParameters.ViewTarget.x, interactionParameters.ViewTarget.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.vector2_1 = this.MovementContext.ClampRotation(this.vector2_1);
            float num3 = Mathf.Abs(Mathf.DeltaAngle(this.vector2_1.x, this.MovementContext.Rotation.x));
            float num4 = Mathf.Abs(Mathf.DeltaAngle(this.vector2_1.y, this.MovementContext.Rotation.y));
            if (num3 / this.ApproachAngularSpeed > this.float_1)
            {
                this.float_1 = num3 / this.ApproachAngularSpeed;
            }
            if (num4 / this.ApproachAngularSpeed > this.float_1)
            {
                this.float_1 = num4 / this.ApproachAngularSpeed;
            }
            this.float_1 = Mathf.Min(this.float_1, 1.5f);
        }
        else
        {
            this.MovementContext.PlayerAnimatorSetApproached(true);
            this.bool_3 = true;
        }
        if (this.bool_3)
        {
            return;
        }
        if (this.bool_1 || this.bool_2)
        {
            this.MovementContext.LevelOnApproachStart = this.MovementContext.PoseLevel;
        }
    }

    // Token: 0x060020DF RID: 8415 RVA: 0x0019D73C File Offset: 0x0019B93C
    private void method_0(float stateTime, float dt)
    {
        float t = (this.float_1 > 0f) ? Mathf.Clamp01(this.ApproachCurve.Evaluate(stateTime / this.float_1)) : 1f;
        this.MovementContext.ApplyRotation(Quaternion.Euler(0f, Mathf.LerpAngle(this.float_2, this.vector2_1.x, t), 0f));
        if (this.bool_1)
        {
            this.MovementContext.SetPoseLevel(Mathf.Lerp(this.float_3, 1f, t), true);
        }
        else if (this.bool_2)
        {
            this.MovementContext.SetPoseLevel(Mathf.Lerp(this.float_3, 0f, t), true);
        }
        Vector2 rotation = new Vector2(Mathf.LerpAngle(this.vector2_0.x, this.vector2_1.x, t), Mathf.LerpAngle(this.vector2_0.y, this.vector2_1.y, t));
        this.MovementContext.Rotation = rotation;
        Vector3 motion = Vector3.Lerp(this.vector3_2, this.vector3_0, t) - this.MovementContext.TransformPosition;
        if (motion.sqrMagnitude > 0f)
        {
            this.MovementContext.ApplyApproachMotion(motion, dt);
        }
        this.vector3_3 = Vector3.Lerp(this.vector3_3, this.MovementContext.InverseTransformVector(motion.normalized), 0.1f);
        this.MovementContext.MovementDirection = new Vector2(this.vector3_3.x, this.vector3_3.z);
    }

    // Token: 0x060020E0 RID: 8416 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void Prone()
    {
    }

    // Token: 0x060020E1 RID: 8417 RVA: 0x0019D8CC File Offset: 0x0019BACC
    public override void ManualAnimatorMoveUpdate(float deltaTime)
    {
        float num = Time.time - this.float_0;
        if (this.bool_0)
        {
            this.method_0(num, deltaTime);
        }
        else if (this.bool_1)
        {
            this.MovementContext.SetPoseLevel(1f, false);
        }
        else if (this.bool_2)
        {
            this.MovementContext.SetPoseLevel(0f, false);
        }
        if (num > this.float_1)
        {
            this.MovementContext.PlayerAnimatorSetApproached(true);
            this.bool_3 = true;
        }
    }

    // Token: 0x060020E2 RID: 8418 RVA: 0x000B4841 File Offset: 0x000B2A41
    public override void Exit(bool toSameState)
    {
        base.Exit(toSameState);
        this.MovementContext.StateLocksInventory = false;
        this.bool_3 = false;
        this.MovementContext.PlayerAnimatorSetApproached(false);
    }

    // Token: 0x060020E3 RID: 8419 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void BlindFire(int b)
    {
    }

    // Token: 0x04001DAC RID: 7596
    private float float_0;

    // Token: 0x04001DAD RID: 7597
    private float float_1 = 0.33f;

    // Token: 0x04001DAE RID: 7598
    public float ApproachSpeed = 2.5f;

    // Token: 0x04001DAF RID: 7599
    public float ApproachAngularSpeed = 90f;

    // Token: 0x04001DB0 RID: 7600
    public float TMinus;

    // Token: 0x04001DB1 RID: 7601
    public AnimationCurve ApproachCurve = new AnimationCurve(new Keyframe[]
    {
        new Keyframe(0f, 0f, 0f, 0f),
        new Keyframe(1f, 1f, 0f, 0f)
    });

    // Token: 0x04001DB2 RID: 7602
    private Vector3 vector3_0;

    // Token: 0x04001DB3 RID: 7603
    private Vector3 vector3_1;

    // Token: 0x04001DB4 RID: 7604
    private bool bool_0;

    // Token: 0x04001DB5 RID: 7605
    private Vector2 vector2_0;

    // Token: 0x04001DB6 RID: 7606
    private float float_2;

    // Token: 0x04001DB7 RID: 7607
    private float float_3;

    // Token: 0x04001DB8 RID: 7608
    private Vector3 vector3_2;

    // Token: 0x04001DB9 RID: 7609
    private float float_4;

    // Token: 0x04001DBA RID: 7610
    private bool bool_1;

    // Token: 0x04001DBB RID: 7611
    private bool bool_2;

    // Token: 0x04001DBC RID: 7612
    private bool bool_3;

    // Token: 0x04001DBD RID: 7613
    private Vector2 vector2_1;

    // Token: 0x04001DBE RID: 7614
    private Vector3 vector3_3;
}
