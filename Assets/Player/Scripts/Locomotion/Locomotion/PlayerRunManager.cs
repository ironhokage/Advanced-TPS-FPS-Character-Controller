namespace Player.Locomotion.Locomotion
{
    public class PlayerRunManager
    {
        public bool IsRunning { get; private set; }
        public bool ContinuousRun { get; set; } // Could be a setting players toggle

        public void HandleRunPressed(float direction) 
        {
            if (direction < 1)
            {
                IsRunning = false;
                return;
            }
            
            if (ContinuousRun)
                IsRunning = !IsRunning; // Toggle
            else
                IsRunning = true; // Hold to run
        }

        public void HandleRunReleased() 
        { 
            if (!ContinuousRun)
                IsRunning = false;
        }
    }
}
