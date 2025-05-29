using System;

namespace _01.Script.Core.GameEventSystem
{
    public static class CameraEvents
    {
        public static CameraChangeEvent CameraChangeEvent = new CameraChangeEvent();
        public static PlayerCamSelectEvent PlayerCamSelectEvent = new PlayerCamSelectEvent();
        public static PlayerCamUnSelectedEvent PlayerCamUnSelectedEvent = new PlayerCamUnSelectedEvent();
    }

    public class CameraChangeEvent : GameEvent
    {
        public Type nextCam;
    }

    public class PlayerCamSelectEvent : GameEvent
    {

    }

    public class PlayerCamUnSelectedEvent : GameEvent
    {

    }
}
