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
    partial class Program
    {
        public class LapData
        {
            public TimeSpan laptime;
            

            IngameTime ingameTime;

            List<DataPoint> datapoints = new List<DataPoint>();

            IMyShipController controller;

            public LapData(IMyShipController controller, IngameTime ingameTime)
            {
                this.controller = controller;
                this.ingameTime = ingameTime;
            }

            public DataPoint GetClosePoint(TimeSpan currentTime)
            {
                DataPoint closestPoint = datapoints[0];
                double smallestDiff = double.MaxValue;

                foreach (DataPoint point in datapoints)
                {
                    var diff = Math.Abs((point.time - currentTime).TotalSeconds);


                    if (diff < smallestDiff)
                    {
                        closestPoint = point;
                        smallestDiff = diff;
                    }
                }

                return closestPoint;
            }


            public void LogPoint(TimeSpan deltaTime)
            {
                DataPoint point = new DataPoint
                {
                    position = controller.CenterOfMass,
                    forward = controller.WorldMatrix.Forward,
                    velocity = controller.GetShipVelocities().LinearVelocity,
                    time = deltaTime
                };

                datapoints.Add(point);
            }
        }
    }
}
