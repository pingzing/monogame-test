namespace monogame_test.Core.DebugHelpers
{
    public static class DebugConstants
    {
        private static bool _showBoundingBoxes = false;
        public static bool ShowBoundingBoxes
        {
            get
            {
#if DEBUG
                return _showBoundingBoxes;
#else
                return false;
#endif
            }
            set { _showBoundingBoxes = value; }
        }

        private static bool _showCollisionOverlays = false;
        public static bool ShowCollisionOverlays
        {
            get
            {
#if DEBUG
                return _showCollisionOverlays;
#else
                return false;
#endif
            }
            set { _showCollisionOverlays = value; }
        } 
    }
}
