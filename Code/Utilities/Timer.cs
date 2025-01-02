using System;
using Sandbox;

namespace Utilities
{
    /// <summary>
    /// A reusable Timer component for use in S&box projects.
    /// Handles countdowns, looping, and optional callbacks upon completion.
    /// </summary>
    public class Timer : Component
    {
		[Property] 
		bool Logging { get; set; } = true;

        /// <summary>
        /// Duration of the timer in seconds.
        /// </summary>
        [Property]
        private float Duration { get; set; } = 1.0f;

        /// <summary>
        /// Automatically starts the timer when initialized.
        /// </summary>
        [Property]
        public bool AutoStart { get; set; } = false;

        /// <summary>
        /// Indicates whether the timer is currently running.
        /// </summary>
        [Property]
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Number of times the timer has looped.
        /// </summary>
        public int LoopCount { get; private set; } = 0;

        /// <summary>
        /// Number of loops after which the timer will stop. Set to 0 for unlimited loops.
        /// </summary>
        [Property]
        public int LoopStopCount { get; set; } = 0;

        /// <summary>
        /// If true, the timer will not automatically restart after completion.
        /// </summary>
        [Property]
        public bool OneShot { get; set; } = true;

        /// <summary>
        /// Callback action executed when the timer completes.
        /// </summary>
        public Action OnComplete { get; set; }

        private float elapsedTime; // Internal elapsed time tracker

        /// <summary>
        /// Initializes the timer with default settings.
        /// </summary>
        public Timer()
        {
            if (AutoStart) Start();
        }

        /// <summary>
        /// Configures the timer with specific parameters.
        /// </summary>
        /// <param name="duration">The timer duration in seconds.</param>
        /// <param name="onComplete">Callback action to execute on completion.</param>
        /// <param name="oneShot">Whether the timer runs only once or loops.</param>
        /// <param name="autoStart">Whether the timer should start automatically.</param>
        /// <param name="loopStopCount">Number of loops before the timer stops.</param>
        public void Set(float duration, Action onComplete = null, bool oneShot = true, bool autoStart = false, int loopStopCount = 0)
        {
            if (duration <= 0)
            {
                throw new ArgumentException("Duration must be greater than 0.");
            }

            if (oneShot && loopStopCount > 0)
            {
                throw new ArgumentException("OneShot cannot be true while LoopStopCount is greater than 0.");
            }

            Duration = duration;
            OnComplete = onComplete;
            OneShot = oneShot;
            AutoStart = autoStart;
            LoopStopCount = loopStopCount;
            ResetTimer();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;

			if(LoopStopCount > 0) OneShot = false;
        }

        /// <summary>
        /// Stops the timer and resets the loop count.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
            LoopCount = 0;
        }

        /// <summary>
        /// Resets the timer's elapsed time and stops it.
        /// </summary>
        public void ResetTimer()
        {
            elapsedTime = 0f;
            IsRunning = false;
            if (Logging) Log.Info("Timer reset");
        }

        /// <summary>
        /// Adjusts the elapsed time by a given amount.
        /// </summary>
        /// <param name="adjustment">The amount to adjust the elapsed time by.</param>
        public void AdjustElapsedTime(float adjustment)
        {
            elapsedTime = Math.Clamp(elapsedTime + adjustment, 0, Duration);
        }

        /// <summary>
        /// Indicates whether the timer has completed.
        /// </summary>
        public bool IsFinished => OneShot && elapsedTime >= Duration;

        /// <summary>
        /// The remaining time before the timer completes.
        /// </summary>
        public float RemainingTime => Math.Max(0, Duration - elapsedTime);

        /// <summary>
        /// The elapsed time since the timer started.
        /// </summary>
        public float ElapsedTime => elapsedTime;

        /// <summary>
        /// Handles the initialization logic when the component starts.
        /// </summary>
        protected override void OnStart()
        {
            if (AutoStart) Start();
        }

        /// <summary>
        /// Updates the timer logic every frame.
        /// </summary>
        protected override void OnUpdate()
        {
            if (!IsRunning) return;

            elapsedTime += Time.Delta;

            // Handle multiple potential completions in a single frame
            while (elapsedTime >= Duration)
            {
                elapsedTime -= Duration;

                // Execute the callback
                OnComplete?.Invoke();

                if (OneShot)
                {
                    Stop();
                    return;
                }

                LoopCount++;

                if (LoopStopCount > 0 && LoopCount >= LoopStopCount)
                {
                    Stop();
                    return;
                }
            }
        }
    }
}
