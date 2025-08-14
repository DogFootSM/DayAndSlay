public interface IMonsterState
{
    public abstract void Enter(GeneralAnimator animator);
    public abstract void Update();
    public abstract void Exit();
}
