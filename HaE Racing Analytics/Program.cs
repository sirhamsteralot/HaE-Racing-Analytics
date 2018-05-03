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
        public List<LapData> laps = new List<LapData>();

        public IMyShipController control;

        IngameTime ingameTime = new IngameTime();
        bool stopped = false;

        TimeSpan lapStartTime;

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
        }

        public void Main(string argument, UpdateType updateSource)
        {
            ProcessArguments(argument);

            ingameTime.Tick(Runtime.TimeSinceLastRun.TotalMilliseconds);
        }

        public void LogPoint()
        {
            if (laps.Count == 0 || stopped)
                return;

            laps[laps.Count - 1].LogPoint(ingameTime.Time - lapStartTime);
        }

        public void RenderGhost()
        {

        }

        public void CheckStart()
        {

        }

        public void ProcessArguments(string argument)
        {
            switch(argument)
            {
                case "Trigger":
                    laps.Add(new LapData(control, ingameTime));
                    lapStartTime = ingameTime.Time;
                    break;

                case "Stop_Resume":
                    stopped = !stopped;
                    break;
            }

        }
    }
}