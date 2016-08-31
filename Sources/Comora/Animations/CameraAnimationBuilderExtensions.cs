namespace Comora
{
	using System;
	using Microsoft.Xna.Framework;

	public static class CameraAnimationBuilderExtensions
	{
		#region Camera

		public static ICameraAnimationBuilder Wait(this ICamera camera, TimeSpan duration)
		{
			return camera.StartAnimation().ThenWait(duration);
		}

		public static ICameraAnimationBuilder Shake(this ICamera camera, TimeSpan duration, float intensity = 60)
		{
			return camera.StartAnimation().ThenShake(duration,intensity);
		}

		public static ICameraAnimationBuilder Move(this ICamera camera, TimeSpan duration,Vector2 to, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return camera.StartAnimation().ThenMove(duration, camera.Position , to, easing);
		}

		public static ICameraAnimationBuilder Move(this ICamera camera, TimeSpan duration, Vector2 from, Vector2 to, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return camera.StartAnimation().ThenMove(duration, from,to,easing);
		}

		public static ICameraAnimationBuilder Rotate(this ICamera camera, TimeSpan duration, float to, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return camera.StartAnimation().ThenRotate(duration, camera.Angle, to, easing);
		}

		public static ICameraAnimationBuilder Rotate(this ICamera camera, TimeSpan duration, float from, float to, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return camera.StartAnimation().ThenRotate(duration, from, to, easing);
		}

		public static ICameraAnimationBuilder Zoom(this ICamera camera, TimeSpan duration, float to, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return camera.StartAnimation().ThenZoom(duration, camera.Scale, to, easing);
		}

		public static ICameraAnimationBuilder Zoom(this ICamera camera, TimeSpan duration, float from, float to, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return camera.StartAnimation().ThenZoom(duration, from, to, easing);
		}

		public static ICameraAnimationBuilder MoveAndZoom(this ICamera camera, TimeSpan duration, Vector2 from, Vector2 to, float fromZ, float toZ, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return camera.StartAnimation().ThenMoveAndZoom(duration, from, to, fromZ, toZ, easing);
		}

		#endregion

		#region Builder

		public static ICameraAnimationBuilder Then(this ICameraAnimationBuilder builder, TimeSpan duration, Action<Camera,double> animation, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return builder.Then(new RelayCameraAnimation(easing,duration,animation));
		}

		public static ICameraAnimationBuilder ThenWait(this ICameraAnimationBuilder builder, TimeSpan duration)
		{
			return builder.Then(new WaitCameraAnimation(duration));
		}

		public static ICameraAnimationBuilder ThenShake(this ICameraAnimationBuilder builder, TimeSpan duration, float intensity = 60)
		{
			return builder.Then(new ShakeCameraAnimation(duration, intensity));
		}

		public static ICameraAnimationBuilder ThenMove(this ICameraAnimationBuilder builder, TimeSpan duration, Vector2 from, Vector2 to, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return builder.Then(new MoveCameraAnimation(easing, duration,from,to));
		}

		public static ICameraAnimationBuilder ThenRotate(this ICameraAnimationBuilder builder, TimeSpan duration, float from, float to, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return builder.Then(new RotateCameraAnimation(easing, duration, from, to));
		}

		public static ICameraAnimationBuilder ThenZoom(this ICameraAnimationBuilder builder, TimeSpan duration, float from, float to, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return builder.Then(new ZoomCameraAnimation(easing, duration, from, to));
		}

		public static ICameraAnimationBuilder ThenMoveAndZoom(this ICameraAnimationBuilder builder, TimeSpan duration, Vector2 from, Vector2 to, float fromZ, float toZ, Easing.Mode easing = Easing.Mode.EaseBoth)
		{
			return builder.Then(new MoveCameraAnimation(easing, duration, from, to), new ZoomCameraAnimation(easing, duration, fromZ, toZ));
		}

		#endregion

	}
}

