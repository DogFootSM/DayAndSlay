public interface IBossMonsterState
{
    public abstract void Enter(BossAnimator animator);
    public abstract void Update();
    public abstract void Exit();
}
