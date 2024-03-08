using System;
using System.Collections.Generic;
using System.Threading;
using MBW.Utilities.DI.Named.Implementation;
using Xunit;

namespace MBW.Utilities.DI.Named.Tests;

public class ParallelTests
{
    [Fact]
    public void TestParallelRegistrations()
    {
        // If a user sets up multiple ServiceCollections with the same names, in parallel, they may need to create the same registration type multiple times.
        // It is an error to have two types of the same name, so they must be singular.
        // This test attempts to highlight these issues

        ManualResetEvent evnt = new ManualResetEvent(false);
        List<Type> createdTypes = new List<Type>();

        void ProcessOne()
        {
            // Synchronize all threads
            evnt.WaitOne();

            var tp = RegistrationTypeManager.GetRegistrationWrapperType(typeof(object), "test", true);

            lock (createdTypes)
                createdTypes.Add(tp);
        }

        Thread[] threads = new Thread[4];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(ProcessOne);
            threads[i].Start();
        }

        // Release them all
        evnt.Set();

        for (int i = 0; i < threads.Length; i++)
            threads[i].Join();

        // Verify
        Assert.Equal(threads.Length, createdTypes.Count);
        Assert.All(createdTypes, x => Assert.Equal(createdTypes[0], x));
    }
}