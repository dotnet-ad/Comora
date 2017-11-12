// INCLUDED FILE, DO NOT MODIFY IT
// (from nuget package 'Transform.Sources')


namespace Transform
{
    using System;
    using Microsoft.Xna.Framework;

    public class Acceleration2D
    {
        public Acceleration2D(Velocity2D velocity)
        {
            this.Velocity = velocity ?? throw new ArgumentException(nameof(velocity));
        }

        public Transform2D Transform => this.Velocity.Transform;

        public Velocity2D Velocity { get; }

        public Vector2 Position { get; set; }

        public Vector2 Scale { get; set; }

        public float Rotation { get; set; }

        public void Update(GameTime time)
        {
            var delta = (float)time.ElapsedGameTime.TotalSeconds;
            this.Velocity.Position += this.Position * delta;
            this.Velocity.Scale += this.Scale * delta;
            this.Velocity.Rotation += this.Rotation * delta;

            this.Velocity.Update(time);
        }
    }
}

namespace Transform
{
    using System;

    public enum Ease
    {
        InOut,
        In,
        Out,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
    }

    public static class EaseFunctions
    {
        private static readonly float PI_2 = (float)(Math.PI / 2);

        public static float In(float t) => t * t;

        public static float Out(float t) => t * (2 - t);

        public static float InOut(float t) => t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;

        public static float ElasticIn(float t) => (float)(Math.Sin(13 * PI_2 * t) * Math.Pow(2, 10 * (t - 1)));

        public static float ElasticOut(float t) => (float)(Math.Sin(-13 * PI_2 * (t + 1)) * Math.Pow(2, -10 * t) + 1);

        public static float ElasticInOut(float t) 
        {
            if (t < 0.5f)
            {
                return (float)(0.5 * Math.Sin(13 * PI_2 * (2 * t)) * Math.Pow(2, 10 * ((2 * t) - 1)));
            }
            return (float)(0.5 * (Math.Sin(-13 * PI_2 * ((2 * t - 1) + 1)) * Math.Pow(2, -10 * (2 * t - 1)) + 2));
        }

        public static Func<float,float> Get(Ease ease)
        {
            switch (ease)
            {
                case Ease.In: return In;
                case Ease.Out: return Out;
                case Ease.InOut: return InOut;
                case Ease.ElasticIn: return ElasticIn;
                case Ease.ElasticOut: return ElasticOut;
                case Ease.ElasticInOut: return ElasticInOut;
            }

            throw new NotSupportedException();
        }
    }
}

namespace Transform
{
    using Microsoft.Xna.Framework;

    public static class Extensions
    {
        public static Transform2D ToTransform(this Vector2 position)
        {
            return new Transform2D()
            {
                Position = position,
            };
        }

        public static Velocity2D WithVelocity(this Transform2D transform)
        {
            return new Velocity2D(transform);
        }

        public static Acceleration2D WithAcceleration(this Velocity2D velocity)
        {
            return new Acceleration2D(velocity);
        }
    }
}

namespace Transform
{
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;

    public class Transform2D
    {
        #region Constructors

        public Transform2D()
        {
            this.Position = Vector2.Zero;
            this.Rotation = 0;
            this.Scale = Vector2.One;
        }

        #endregion

        #region Fields

        private Transform2D parent;

        private List<Transform2D> children = new List<Transform2D>();

        private Matrix absolute, invertAbsolute, local;

        private float localRotation, absoluteRotation;

        private Vector2 localScale, absoluteScale, localPosition, absolutePosition;

        private bool needsAbsoluteUpdate = true, needsLocalUpdate = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent transform.
        /// </summary>
        /// <value>The parent.</value>
        public Transform2D Parent
        {
            get => this.parent;
            set
            {
                if(this.parent != value)
                {
                    if (this.parent != null)
                        this.parent.children.Remove(this);

                    this.parent = value;

                    if (this.parent != null)
                        this.parent.children.Add(this);

                    this.SetNeedsAbsoluteUpdate();
                }
            }
        }

        /// <summary>
        /// Gets all the children transform.
        /// </summary>
        /// <value>The children.</value>
        public IEnumerable<Transform2D> Children => this.children;

        /// <summary>
        /// Gets the absolute rotation.
        /// </summary>
        /// <value>The absolute rotation.</value>
        public float AbsoluteRotation => this.UpdateAbsoluteAndGet(ref this.absoluteRotation);

        /// <summary>
        /// Gets the absolute scale.
        /// </summary>
        /// <value>The absolute scale.</value>
        public Vector2 AbsoluteScale => this.UpdateAbsoluteAndGet(ref this.absoluteScale);

        /// <summary>
        /// Gets the absolute position.
        /// </summary>
        /// <value>The absolute position.</value>
        public Vector2 AbsolutePosition => this.UpdateAbsoluteAndGet(ref this.absolutePosition);

        /// <summary>
        /// Gets or sets the rotation (relative to the parent, absolute if no parent).
        /// </summary>
        /// <value>The rotation.</value>
        public float Rotation
        {
            get => this.localRotation;
            set
            {
                if (this.localRotation != value)
                {
                    this.localRotation = value;
                    this.SetNeedsLocalUpdate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the position (relative to the parent, absolute if no parent).
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position
        {
            get => this.localPosition;
            set
            {
                if(this.localPosition != value)
                {
                    this.localPosition = value;
                    this.SetNeedsLocalUpdate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the scale (relative to the parent, absolute if no parent).
        /// </summary>
        /// <value>The scale.</value>
        public Vector2 Scale
        {
            get => this.localScale;
            set
            {
                if (this.localScale != value)
                {
                    this.localScale = value;
                    this.SetNeedsLocalUpdate();
                }
            }
        }

        /// <summary>
        /// Gets the matrix representing the local transform.
        /// </summary>
        /// <value>The relative matrix.</value>
        public Matrix Local => this.UpdateLocalAndGet(ref this.absolute);

        /// <summary>
        /// Gets the matrix representing the absolute transform.
        /// </summary>
        /// <value>The absolute matrix.</value>
        public Matrix Absolute => this.UpdateAbsoluteAndGet(ref this.absolute);

        /// <summary>
        /// Gets the matrix representing the invert of the absolute transform.
        /// </summary>
        /// <value>The absolute matrix.</value>
        public Matrix InvertAbsolute => this.UpdateAbsoluteAndGet(ref this.invertAbsolute);

        #endregion

        #region Methods

        public void ToLocalPosition(ref Vector2 absolute, out Vector2 local)
        {
            Vector2.Transform(ref absolute, ref this.invertAbsolute, out local);
        }

        public void ToAbsolutePosition(ref Vector2 local, out Vector2 absolute)
        {
            Vector2.Transform(ref local, ref this.absolute, out absolute);
        }

        public Vector2 ToLocalPosition(Vector2 absolute)
        {
            Vector2 result;
            ToLocalPosition(ref absolute, out result);
            return result;
        }

        public Vector2 ToAbsolutePosition(Vector2 local)
        {
            Vector2 result;
            ToAbsolutePosition(ref local, out result);
            return result;
        }

        private void SetNeedsLocalUpdate()
        {
            this.needsLocalUpdate = true;
            this.SetNeedsAbsoluteUpdate();
        }

        private void SetNeedsAbsoluteUpdate()
        {
            this.needsAbsoluteUpdate = true;

            foreach (var child in this.children)
            {
                child.SetNeedsAbsoluteUpdate();
            }
        }

        private void UpdateLocal()
        {
            var result = Matrix.CreateScale(this.Scale.X, this.Scale.Y, 1);
            result *= Matrix.CreateRotationZ(this.Rotation);
            result *= Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0);
            this.local = result;

            this.needsLocalUpdate = false;
        }

        private void UpdateAbsolute()
        {
            if (this.Parent == null)
            {
                this.absolute = this.local;
                this.absoluteScale = this.localScale;
                this.absoluteRotation = this.localRotation;
                this.absolutePosition = this.localPosition;
            }
            else
            {
                var parentAbsolute = this.Parent.Absolute;
                Matrix.Multiply(ref this.local, ref parentAbsolute, out this.absolute);
                this.absoluteScale = this.Parent.AbsoluteScale * this.Scale;
                this.absoluteRotation = this.Parent.AbsoluteRotation + this.Rotation;
                this.absolutePosition = Vector2.Zero;
                this.ToAbsolutePosition(ref this.absolutePosition, out this.absolutePosition);
            }

            Matrix.Invert(ref this.absolute, out this.invertAbsolute);

            this.needsAbsoluteUpdate = false;
        }

        private T UpdateLocalAndGet<T>(ref T field)
        {
            if (this.needsLocalUpdate)
            {
                this.UpdateLocal();
            }

            return field;
        }

        private T UpdateAbsoluteAndGet<T>(ref T field)
        {
            if (this.needsLocalUpdate)
            {
                this.UpdateLocal();
            }

            if (this.needsAbsoluteUpdate)
            {
                this.UpdateAbsolute();
            }

            return field;
        }

        #endregion

    }
}

namespace Transform
{
    using System;
    using Microsoft.Xna.Framework;

    public class Velocity2D
    {
        public Velocity2D(Transform2D transform)
        {
            this.Transform = transform ?? throw new ArgumentException(nameof(transform));
        }

        public Transform2D Transform { get; }

        public Vector2 Position { get; set; }

        public Vector2 Scale { get; set; }

        public float Rotation { get; set; }

        public void Update(GameTime time)
        {
            var delta = (float)time.ElapsedGameTime.TotalSeconds;
            this.Transform.Position += this.Position * delta;
            this.Transform.Scale += this.Scale * delta;
            this.Transform.Rotation += this.Rotation * delta;
        }
    }
}

namespace Transform
{
    using System;
    using Microsoft.Xna.Framework;

    public class Delay : ITween
    {
        public Delay(TimeSpan duration)
        {
            this.Duration = duration;
        }

        public TimeSpan Time { get; private set; }

        public TimeSpan Duration { get; }

        public bool IsFinished { get; private set; }

        public void Reset()
        {
            this.IsFinished = false;
            this.Time = TimeSpan.Zero;
        }

        public bool Update(GameTime time)
        {
            if (!this.IsFinished)
            {
                var delta = (float)time.ElapsedGameTime.TotalSeconds;
                this.Time += time.ElapsedGameTime;
                this.IsFinished = (this.Time >= this.Duration);
            }

            return this.IsFinished;
        }
    }
}

namespace Transform
{
    using System;
    using Microsoft.Xna.Framework;

    public interface ITween
    {
        TimeSpan Time { get; }

        TimeSpan Duration { get; }

        bool IsFinished { get; }

        void Reset();

        bool Update(GameTime time);
    }
}

namespace Transform
{
    using System;
    using System.Linq;
    using Microsoft.Xna.Framework;

    public class Parallel : ITween
    {
        public Parallel(params ITween[] tweens)
        {
            this.tweens = tweens.ToArray();
        }

        private ITween[] tweens;

        public TimeSpan Time => this.tweens.Max(x => x.Time);

        public TimeSpan Duration => this.tweens.Max(x => x.Duration);

        public bool IsFinished => this.tweens.All(x => x.IsFinished);

        public void Reset()
        {
            foreach (var tween in this.tweens)
            {
                tween.Reset();
            }
        }


        public bool Update(GameTime time)
        {
            foreach (var tween in this.tweens)
            {
                if(!tween.IsFinished)
                {
                    tween.Update(time);
                }
            }

            return this.IsFinished;
        }
    }
}

namespace Transform
{
    using System;
    using Microsoft.Xna.Framework;

    public class Repeat : ITween
    {
        public Repeat(ITween tween, int times = -1)
        {
            this.tween = tween;
            this.RepeatTimes = times;
        }

        #region Fields

        private ITween tween;

        private int current;

        #endregion

        public TimeSpan Time { get; set; }

        public int RepeatTimes { get; }

        public TimeSpan Duration => TimeSpan.MaxValue;

        public bool IsFinished => RepeatTimes > 0 && current >= RepeatTimes;

        public void Reset()
        {
            current = 0;
            tween.Reset();
        }

        public bool Update(GameTime time)
        {
            var isFinished = this.IsFinished;
            if (!isFinished)
            {
                if(tween.Update(time))
                {
                    current++;

                    isFinished = this.IsFinished;
                    if(!isFinished)
                    {
                        tween.Reset();
                    }
                }

            }

            return isFinished;
        }
    }
}

namespace Transform
{
    using System;
    using System.Linq;
    using Microsoft.Xna.Framework;

    public class Sequence : ITween
    {
        public Sequence(params ITween[] tweens)
        {
            this.tweens = tweens.ToArray();
        }

        private ITween[] tweens;

        public TimeSpan Time 
        {
            get
            {
                var result = TimeSpan.Zero;

                for (int i = 0; i < this.tweens.Length; i++)
                {
                    var tween = this.tweens[i];

                    if(i == current)
                        return result + tween.Time;
                    
                    result += tween.Duration;
                }

                return result;
            }
        }

        public TimeSpan Duration => new TimeSpan(tweens.Sum(x => x.Duration.Ticks));

        public bool IsFinished { get; private set; }

        public void Reset()
        {
            foreach (var tween in this.tweens)
            {
                tween.Reset();
            }

            this.current = 0;
            this.IsFinished = false;
        }

        private int current;

        public bool Update(GameTime time)
        {
            if (!this.IsFinished)
            {
                var tween = this.tweens[current];

                if(tween.Update(time))
                {
                    current++;
                }
                this.IsFinished = current >= tweens.Length;
            }

            return this.IsFinished;
        }
    }
}

namespace Transform
{
    using System;
    using Microsoft.Xna.Framework;

    public class Tween2D : ITween
    {
        public Tween2D(TimeSpan duration, Transform2D transform, Transform2D to, Ease ease)
            : this(duration, transform, new Transform2D()
        {
            Position = transform.Position,
            Rotation = transform.Rotation,
            Scale = transform.Scale,
        }, to, EaseFunctions.Get(ease))
        {
        }

        public Tween2D(TimeSpan duration, Transform2D transform, Transform2D from, Transform2D to, Ease ease)
            :this(duration, transform, from, to, EaseFunctions.Get(ease))
        {
        }

        public Tween2D(TimeSpan duration, Transform2D transform, Transform2D from, Transform2D to, Func<float, float> ease)
        {
            this.Duration = duration;
            this.Transform = transform;
            this.From = from;
            this.To = to;
            this.Ease = Ease;
            this.easeFunction = ease;
        }

        private Func<float, float> easeFunction;

        public TimeSpan Time { get; private set; }

        public TimeSpan Duration { get; }

        public Transform2D Transform { get; }

        public Transform2D From { get; }

        public Transform2D To { get; }

        public bool IsRevert { get; }

        public Ease Ease { get; }

        public bool IsFinished { get; private set; }

        public void Reset()
        {
            this.IsFinished = false;
            this.Time = TimeSpan.Zero;

            Transform.Position = From.Position;
            Transform.Scale = From.Scale;
            Transform.Rotation = From.Rotation;
        }

        public bool Update(GameTime time)
        {
            if(!this.IsFinished)
            {
                var delta = (float)time.ElapsedGameTime.TotalSeconds;

                this.Time += time.ElapsedGameTime;

                var t = Math.Max(0, Math.Min(1, (float)(this.Time.TotalMilliseconds / this.Duration.TotalMilliseconds)));

                var amount = easeFunction(t);

                Transform.Position = From.Position + (To.Position - From.Position) * amount;
                Transform.Scale = From.Scale + (To.Scale - From.Scale) * amount;
                Transform.Rotation = From.Rotation + (To.Rotation - From.Rotation) * amount;

                this.IsFinished = (t >= 1);
            }

            return this.IsFinished;
        }
    }
}

