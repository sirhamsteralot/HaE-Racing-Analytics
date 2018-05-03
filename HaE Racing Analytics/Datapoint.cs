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
        public struct DataPoint
        {
            public Vector3D position;
            public Vector3D forward;
            public Vector3D velocity;

            public TimeSpan time;

            public bool IsValid()
            {
                return position != Vector3D.Zero && forward != Vector3D.Zero && velocity != Vector3D.Zero;
            }
        }
    }
}
