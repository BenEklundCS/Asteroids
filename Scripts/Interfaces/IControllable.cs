namespace Asteroids.Scripts.Interfaces;

public interface IControllable {
    public void Boost();
    public void Slow();
    public void Right(double delta);
    public void Left(double delta);
    public void Shoot();
}