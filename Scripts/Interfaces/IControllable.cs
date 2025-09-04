namespace Asteroids.Scripts.Interfaces;

public interface IControllable {
    public void Boost(double delta);
    public void Slow(double delta);
    public void Right(double delta);
    public void Left(double delta);
    public void Shoot();
}