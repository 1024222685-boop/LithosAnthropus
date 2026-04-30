using UnityEngine;

public class Player_SickleThrowState : PlayerState
{
    private Camera mainCamera;

    public Player_SickleThrowState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        skillManager.sickleThrow.EnableDots(true);

        if(mainCamera != Camera.main)
            mainCamera = Camera.main;
    }

    public override void Update()
    {
        base.Update();

        Vector2 dirToMouse = GetStickDirection();
        if (dirToMouse == Vector2.zero)
            dirToMouse = DirectionToMouse();

        player.SetVelocity(0, rb.velocity.y);
        player.HandleFlip(dirToMouse.x);
        skillManager.sickleThrow.PredicTraJectory(dirToMouse);

        if (input.Player.Attack.WasPressedThisFrame())
        {
            anim.SetBool("sickleThrowPerformed", true);

            skillManager.sickleThrow.EnableDots(false);
            skillManager.sickleThrow.ConfirmTraJectory(dirToMouse);
        }

        if (input.Player.RangeAttack.WasPressedThisFrame() || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("sickleThrowPerformed", false);
        skillManager.sickleThrow.EnableDots(false);
    }

    private Vector2 GetStickDirection()
    {
        Vector2 stickInput = input.Player.AimDirection.ReadValue<Vector2>();

        if (stickInput.magnitude > 0.1f)
        {
            return stickInput.normalized;
        }

        return Vector2.zero;
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(player.mousePosition);

        Vector2 direction = worldMousePosition - playerPosition;

        return direction.normalized;
    }
}
