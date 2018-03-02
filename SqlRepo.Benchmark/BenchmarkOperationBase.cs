﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SqlRepo.Benchmark
{
    public abstract class BenchmarkOperationBase : IBenchmarkOperation
    {
        public IBenchmarkHelpers BenchmarkHelpers { get; }

        protected BenchmarkOperationBase(IBenchmarkHelpers benchmarkHelpers, Component component)
        {
            Component = component;
            BenchmarkHelpers = benchmarkHelpers;
        }

        public BenchmarkResult Run()
        {
            var notes = GetNotes();

            Console.WriteLine($"Running {GetType().Name}");

            if(!string.IsNullOrEmpty(notes))
                Console.WriteLine(notes);

            var result = new BenchmarkResult();
            result.TestName = GetType().Name;

            Setup();
            Thread.Sleep(2000);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Execute();

            sw.Stop();

            result.TimeTaken = Math.Round(sw.Elapsed.TotalMilliseconds, 2);
            result.Notes = GetNotes();
            result.Component = Component.ToString();

            return result;
        }

        public virtual string GetNotes()
        {
            return null;
        }

        public Component Component { get; set; }

        public virtual void Setup()
        {
            //BenchmarkHelpers.ClearRecords();
            //BenchmarkHelpers.ClearBufferPool();
            //BenchmarkHelpers.InsertRecords(50000);
        }

        public abstract void Execute();
    }
}