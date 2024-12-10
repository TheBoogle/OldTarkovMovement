using System;
using System.Runtime.CompilerServices;
using EFT;
using EFT.Interactive;
using UnityEngine;
using static RootMotion.BipedReferences;

// Token: 0x02000EEB RID: 3819
public class OldStationaryState : StationaryState
{

    public OldStationaryState(MovementContext movementContext) : base(movementContext)
    { }

    // Token: 0x17000B43 RID: 2883
    // (get) Token: 0x06005C21 RID: 23585 RVA: 0x000DA268 File Offset: 0x000D8468
    public virtual bool OutOfOperationRange
    {
        get
        {
            return Vector3.Distance(this.StationaryWeapon.OperatorPosition, this.MovementContext.TransformPosition) > 0.1f;
        }
    }

    // Token: 0x06005C22 RID: 23586 RVA: 0x0028707C File Offset: 0x0028527C
    public override void Enter(bool isFromSameState)
    {
        base.Enter(isFromSameState);
        this.StationaryWeapon = this.MovementContext.StationaryWeapon;
        if (this.OutOfOperationRange)
        {
            this.MovementContext.DropStationary(GStruct177.EStationaryCommand.Denied);
            return;
        }
        this.bool_2 = false;
        this.vector2_0 = new Vector2(this.StationaryWeapon.Yaw, this.StationaryWeapon.Pitch);
        this.float_5 = 0.5f;
        if (isFromSameState)
        {
            return;
        }
        this.Stage = EStationaryStage.In;
        this.MovementContext.StateLocksInventory = true;
        this.MovementContext.SetRotationLimit(this.MovementContext.StationaryWeapon.YawLimit, this.MovementContext.StationaryWeapon.PitchLimit);
        this.MovementContext.SetStationaryWeapon(new Action<Player.AbstractHandsController, Player.AbstractHandsController>(this.method_0));
    }

    // Token: 0x06005C23 RID: 23587 RVA: 0x00287144 File Offset: 0x00285344
    private void method_0(Player.AbstractHandsController arg1, Player.AbstractHandsController newContoller)
    {
        OldStationaryState.Class698 @class = new OldStationaryState.Class698();
        @class.OldStationaryState = this;
        this.MovementContext.StateLocksInventory = false;
        this.transform_0 = newContoller.HandsHierarchy.GetTransform(ECharacterWeaponBones.weapon);
        this.StationaryWeapon.SetPivots(newContoller.HandsHierarchy);
        if ((@class.firearm = (newContoller as Player.FirearmController)) != null)
        {
            @class.firearm.OnShot += this.StationaryWeapon.Shot;
            this.action_1 = new Action(@class.method_0);
        }
        this.StationaryWeapon.Hide(this.MovementContext.IsAI);
        this.MovementContext.RotationAction = ((this.StationaryWeapon.Animation == StationaryWeapon.EStationaryAnimationType.AGS_17) ? MovementContext.AGSRotationFunction : MovementContext.UtesRotationFunction);
        this.Stage = EStationaryStage.Main;
        this.MovementContext.OnHandsControllerChanged += this.method_2;
        this.MovementContext.HandsChangingEvent += this.method_1;
        this.action_0 = new Action(@class.method_1);
    }

    // Token: 0x06005C24 RID: 23588 RVA: 0x000DA28C File Offset: 0x000D848C
    public override void OnInteraction()
    {
        if (this.Stage != EStationaryStage.Main)
        {
            return;
        }
        this.Stage = EStationaryStage.Out;
        this.MovementContext.DropStationary(GStruct177.EStationaryCommand.Leave);
    }

    // Token: 0x06005C25 RID: 23589 RVA: 0x000DA2AB File Offset: 0x000D84AB
    private void method_1()
    {
        this.Stage = EStationaryStage.Out;
        Action action = this.action_0;
        if (action == null)
        {
            return;
        }
        action();
    }

    // Token: 0x06005C26 RID: 23590 RVA: 0x00287250 File Offset: 0x00285450
    private void method_2(Player.AbstractHandsController arg1, Player.AbstractHandsController arg2)
    {
        if (arg2 != null && arg2.Item == this.StationaryWeapon.Item)
        {
            return;
        }
        this.transform_0 = null;
        Action action = this.action_1;
        if (action != null)
        {
            action();
        }
        this.StationaryWeapon.Show();
        this.MovementContext.OnHandsControllerChanged -= this.method_2;
        this.MovementContext.PlayerAnimatorSetStationary(false);
        this.MovementContext.RotationAction = MovementContext.DefaultRotationFunction;
    }

    // Token: 0x06005C27 RID: 23591 RVA: 0x000DA2C4 File Offset: 0x000D84C4
    public override void DropStationary()
    {
        this.bool_2 = true;
        if (this.bool_2 && this.Stage == EStationaryStage.Main)
        {
            this.MovementContext.DropStationary(GStruct177.EStationaryCommand.Denied);
        }
    }

    // Token: 0x06005C28 RID: 23592 RVA: 0x002872D0 File Offset: 0x002854D0
    public override void Rotate(Vector2 deltaRotation, bool ignoreClamp = false)
    {
        EStationaryStage stage = this.Stage;
        if (stage != EStationaryStage.Main)
        {
            if (stage == EStationaryStage.Out)
            {
                if (Vector2.Distance(this.vector2_0, this.MovementContext.Rotation) >= 0.1f)
                {
                    this.float_5 -= Time.deltaTime;
                    Vector2 rotation = new Vector2(Mathf.SmoothDampAngle(this.MovementContext.Rotation.x, this.vector2_0.x, ref this.vector2_1.x, this.float_5), Mathf.SmoothDampAngle(this.MovementContext.Rotation.y, this.vector2_0.y, ref this.vector2_1.y, this.float_5));
                    this.MovementContext.Rotation = rotation;
                }
                this.MovementContext.UpdateStationaryDeltaAngle();
            }
        }
        else
        {
            base.Rotate(deltaRotation, false);
            this.MovementContext.UpdateStationaryDeltaAngle();
        }
        if (this.StationaryWeapon != null && this.transform_0 != null)
        {
            this.StationaryWeapon.ManualUpdate(this.transform_0.position, this.transform_0.rotation);
        }
    }

    // Token: 0x06005C29 RID: 23593 RVA: 0x002873F8 File Offset: 0x002855F8
    public override void ManualAnimatorMoveUpdate(float deltaTime)
    {
        if (this.bool_2 && this.Stage == EStationaryStage.Main)
        {
            this.DropStationary();
        }
        Vector3 playerAnimatorDeltaPosition = this.MovementContext.PlayerAnimatorDeltaPosition;
        this.MovementContext.ApplyApproachMotion(playerAnimatorDeltaPosition, deltaTime);
    }

    // Token: 0x06005C2A RID: 23594 RVA: 0x00287438 File Offset: 0x00285638
    public override void Exit(bool toSameState)
    {
        base.Exit(toSameState);
        if (toSameState)
        {
            return;
        }
        this.transform_0 = null;
        this.MovementContext.SetRotationLimit(Player.GClass1738.FULL_YAW_RANGE, Player.GClass1738.STAND_POSE_ROTATION_PITCH_RANGE);
        this.StationaryWeapon.Unlock(string.Empty);
        this.MovementContext.StationaryWeapon = null;
        this.MovementContext.RotationAction = MovementContext.DefaultRotationFunction;
        Action action = this.action_0;
        if (action != null)
        {
            action();
        }
        Action action2 = this.action_1;
        if (action2 == null)
        {
            return;
        }
        action2();
    }

    // Token: 0x06005C2B RID: 23595 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void Jump()
    {
    }

    // Token: 0x06005C2C RID: 23596 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void EnableBreath(bool enable)
    {
    }

    // Token: 0x06005C2D RID: 23597 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void EnableSprint(bool enable, bool isToggle = false)
    {
    }

    // Token: 0x06005C2E RID: 23598 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void Prone()
    {
    }

    // Token: 0x06005C2F RID: 23599 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void BlindFire(int b)
    {
    }

    // Token: 0x06005C30 RID: 23600 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void ChangeSpeed(float speedDelta)
    {
    }

    // Token: 0x06005C31 RID: 23601 RVA: 0x000A295B File Offset: 0x000A0B5B
    public override void ChangePose(float poseDelta)
    {
    }

    // Token: 0x040057BB RID: 22459
    private const float float_1 = 0.5f;

    // Token: 0x040057BC RID: 22460
    private const float float_2 = 0.1f;

    // Token: 0x040057BD RID: 22461
    public EStationaryStage Stage;

    public enum EStationaryStage
    {
        // Token: 0x04008B7C RID: 35708
        In,
        // Token: 0x04008B7D RID: 35709
        Main,
        // Token: 0x04008B7E RID: 35710
        Out
    }

    // Token: 0x040057BE RID: 22462
    public StationaryWeapon StationaryWeapon;

    // Token: 0x040057BF RID: 22463
    private Vector2 vector2_0;

    // Token: 0x040057C0 RID: 22464
    private Vector2 vector2_1;

    // Token: 0x040057C1 RID: 22465
    private float float_3;

    // Token: 0x040057C2 RID: 22466
    private float float_4;

    // Token: 0x040057C3 RID: 22467
    private float float_5;

    // Token: 0x040057C4 RID: 22468
    private Transform transform_0;

    // Token: 0x040057C5 RID: 22469
    private bool bool_2;

    // Token: 0x040057C6 RID: 22470
    private Action action_0;

    // Token: 0x040057C7 RID: 22471
    private Action action_1;

    // Token: 0x02000EED RID: 3821
    [CompilerGenerated]
    private sealed class Class698
    {
        // Token: 0x06005C34 RID: 23604 RVA: 0x000DA2EA File Offset: 0x000D84EA
        internal void method_0()
        {
            this.firearm.OnShot -= this.OldStationaryState.StationaryWeapon.Shot;
            this.OldStationaryState.action_1 = null;
        }

        // Token: 0x06005C35 RID: 23605 RVA: 0x000DA319 File Offset: 0x000D8519
        internal void method_1()
        {
            this.OldStationaryState.MovementContext.HandsChangingEvent -= this.OldStationaryState.method_1;
            this.OldStationaryState.action_0 = null;
        }

        // Token: 0x040057CC RID: 22476
        public Player.FirearmController firearm;

        // Token: 0x040057CD RID: 22477
        public OldStationaryState OldStationaryState;
    }
}
