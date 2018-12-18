namespace ecs{
public interface ISystem{
    void Init();
    void PreUpdate(float dt);
    void Update(float dt);
    void LateUpdate(float dt);
    void Tick(float dt);
    void Execute();
}
}