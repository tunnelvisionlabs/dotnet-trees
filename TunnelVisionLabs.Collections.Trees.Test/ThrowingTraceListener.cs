// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    // To enable this for a process, add the following to the app.config for the project:
    //
    // <configuration>
    //  <system.diagnostics>
    //    <trace>
    //      <listeners>
    //        <remove name="Default" />
    //        <add name="ThrowingTraceListener" type="PerfView.TestUtilities.ThrowingTraceListener, PerfView.TestUtilities" />
    //      </listeners>
    //    </trace>
    //  </system.diagnostics>
    // </configuration>
    [ExcludeFromCodeCoverage]
    public sealed class ThrowingTraceListener : TraceListener
    {
        public override void Fail(string message, string detailMessage)
        {
            throw new DebugAssertFailureException(message + Environment.NewLine + detailMessage);
        }

        public override void Write(object o)
        {
            if (Debugger.IsLogging())
            {
                Debugger.Log(0, null, o?.ToString());
            }
        }

        public override void Write(object o, string category)
        {
            if (Debugger.IsLogging())
            {
                Debugger.Log(0, category, o?.ToString());
            }
        }

        public override void Write(string message)
        {
            if (Debugger.IsLogging())
            {
                Debugger.Log(0, null, message);
            }
        }

        public override void Write(string message, string category)
        {
            if (Debugger.IsLogging())
            {
                Debugger.Log(0, category, message);
            }
        }

        public override void WriteLine(object o)
        {
            if (Debugger.IsLogging())
            {
                Debugger.Log(0, null, o?.ToString() + Environment.NewLine);
            }
        }

        public override void WriteLine(object o, string category)
        {
            if (Debugger.IsLogging())
            {
                Debugger.Log(0, category, o?.ToString() + Environment.NewLine);
            }
        }

        public override void WriteLine(string message)
        {
            if (Debugger.IsLogging())
            {
                Debugger.Log(0, null, message + Environment.NewLine);
            }
        }

        public override void WriteLine(string message, string category)
        {
            if (Debugger.IsLogging())
            {
                Debugger.Log(0, category, message + Environment.NewLine);
            }
        }

        [Serializable]
        public class DebugAssertFailureException : Exception
        {
            public DebugAssertFailureException()
            {
            }

            public DebugAssertFailureException(string message)
                : base(message)
            {
            }

            public DebugAssertFailureException(string message, Exception inner)
                : base(message, inner)
            {
            }

            protected DebugAssertFailureException(
                SerializationInfo info,
                StreamingContext context)
                : base(info, context)
            {
            }
        }
    }
}
