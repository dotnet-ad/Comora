namespace Comora
{
	public interface ICameraAnimation
	{
		double Time { get; }

		double Duration { get; }

		bool Update(Camera camera, double timeMs);
	}
}

