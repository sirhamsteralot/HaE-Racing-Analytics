using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        static Program P;

        List<LapData> laps = new List<LapData>();
        LapData fastestLap;
        LapData currentLap;


        IMyShipController control;

        IngameTime ingameTime = new IngameTime();
        bool stopped = false;

        ProjectorVisualization visualization;

        TimeSpan CurrentLapDiff => ingameTime.Time - currentLapStartTime;

        TimeSpan currentLapStartTime;

        #region blocknames
        public INISerializer nameSerializer = new INISerializer("BlockNames");

        public string ControlName { get { return (string)nameSerializer.GetValue("ControlName"); }}
        public string ProjectorName { get { return (string)nameSerializer.GetValue("ProjectorName"); } }
        #endregion

        public Program()
        {
            nameSerializer.AddValue("ControlName", x => x, "Control");
            nameSerializer.AddValue("ProjectorName", x => x, "Ghost");

            nameSerializer.DeSerialize(Me.CustomData);

            string customData = Me.CustomData;
            nameSerializer.FirstSerialization(ref customData);
            Me.CustomData = customData;

            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            control = GridTerminalSystem.GetBlockWithName(ControlName) as IMyShipController;

            var projector = GridTerminalSystem.GetBlockWithName(ProjectorName) as IMyProjector;

            visualization = new ProjectorVisualization(projector, Vector3I.One);

            P = this;
        }

        public void SubMain(string argument, UpdateType updateSource)
        {
            ProcessArguments(argument);
            RenderGhost();
            LogPoint();

            ingameTime.Tick(Runtime.TimeSinceLastRun.TotalMilliseconds);
        }

        public void Main(string argument, UpdateType updateSource)
        {
            DebugUtils.MainWrapper(SubMain, argument, updateSource, this);
        }


        public void LogPoint()
        {
            if (stopped || currentLap == null)
                return;

            currentLap.LogPoint(CurrentLapDiff);
        }

        public void RenderGhost()
        {
            if (fastestLap == null)
                return;

            DataPoint ghostPoint = fastestLap.GetClosePoint(CurrentLapDiff);

            Echo($"Current time: {CurrentLapDiff}\npointTime: {ghostPoint.time}");

            visualization.UpdatePosition(ghostPoint.position);
        }

        public void CheckStart()
        {

        }

        public void CheckFastestLap(LapData lap)
        {
            if (fastestLap == null)
            {
                fastestLap = lap;
                return;
            }

            if (fastestLap.laptime < lap.laptime)
                fastestLap = lap;
        }

        public void ProcessArguments(string argument)
        {
            switch(argument)
            {
                case "Trigger":
                    if (currentLap != null)
                    {
                        currentLap.laptime = CurrentLapDiff;
                        laps.Add(currentLap);
                        CheckFastestLap(currentLap);
                    }

                    currentLap = new LapData(control, ingameTime);
                    currentLapStartTime = ingameTime.Time;
                    break;

                case "Stop_Resume":
                    stopped = !stopped;
                    break;
            }

        }
    }
}